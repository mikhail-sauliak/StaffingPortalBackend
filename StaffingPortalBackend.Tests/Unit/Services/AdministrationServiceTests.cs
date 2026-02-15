using NUnit.Framework;
using AutoMapper;
using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Models;
using StaffingPortalBackend.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;

namespace StaffingPortalBackend.Tests.Unit.Services
{
    public class AdministrationServiceTests
    {
        private AppDbContext _context;
        private AdministrationService _service;
        private IMapper _mapper;
        private Mock<IManagementHierarchyService> _hierarchyServiceMock;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new AppDbContext(options);

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
            _hierarchyServiceMock = new Mock<IManagementHierarchyService>();
            _hierarchyServiceMock
                .Setup(s => s.GetSubordinatePersonIdsAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(System.Array.Empty<int>());
            _service = new AdministrationService(_context, _mapper, _hierarchyServiceMock.Object);
        }

        [Test]
        public async Task GetPeopleAsync_ShouldReturnPeople()
        {
            // Arrange
            var person1 = new PersonBuilder().WithFirstName("Alice").WithLastName("Smith").Build();
            var person2 = new PersonBuilder().WithFirstName("Bob").WithLastName("Johnson").Build();
            await _context.People.AddRangeAsync(person1, person2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetPeopleAsync();

            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
        }

        [Test]
        public async Task CreatePersonAsync_ShouldCreatePerson()
        {
            // Arrange
            var personCreateDtoBuilder = new PersonCreateDtoBuilder();
            var personCreateDto = personCreateDtoBuilder
                .WithFirstName("Charlie")
                .WithLastName("Brown")
                .Build();

            // Act
            var result = await _service.CreatePersonAsync(personCreateDto);

            // Assert
            var createdPerson = await _context.People.FirstOrDefaultAsync(p => p.Id == result.Id);
            Assert.That(createdPerson, Is.Not.Null);
            Assert.That(createdPerson.FirstName, Is.EqualTo("Charlie"));
            Assert.That(createdPerson.LastName, Is.EqualTo("Brown"));
        }

        [Test]
        public async Task UpdatePersonAsync_PersonExists_ShouldUpdatePerson()
        {
            // Arrange
            var existingPerson = new PersonBuilder().WithFirstName("Existing").WithLastName("Person").Build();
            await _context.People.AddAsync(existingPerson);
            await _context.SaveChangesAsync();

            var personUpdateDto = new PersonUpdateDtoBuilder()
                .WithFirstName("Updated")
                .WithLastName("Name")
                .Build();

            // Act
            await _service.UpdatePersonAsync(existingPerson.Id, personUpdateDto);

            // Assert
            var updatedPerson = await _context.People.FindAsync(existingPerson.Id);
            Assert.That(updatedPerson.FirstName, Is.EqualTo("Updated"));
            Assert.That(updatedPerson.LastName, Is.EqualTo("Name"));
        }

        [Test]
        public void UpdatePersonAsync_PersonDoesNotExist_ShouldThrowException()
        {
            // Arrange
            var nonExistentId = -1;
            var personUpdateDto = new PersonUpdateDtoBuilder().Build();

            // Act & Assert
            Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdatePersonAsync(nonExistentId, personUpdateDto));
        }

        [Test]
        public async Task DeletePersonAsync_PersonExists_ShouldDeletePerson()
        {
            // Arrange
            var personToDelete = new PersonBuilder().WithFirstName("Delete").WithLastName("Me").Build();
            await _context.People.AddAsync(personToDelete);
            await _context.SaveChangesAsync();

            // Act
            await _service.DeletePersonAsync(personToDelete.Id);

            // Assert
            var deletedPerson = await _context.People.FindAsync(personToDelete.Id);
            Assert.That(deletedPerson, Is.Null);
        }

        [Test]
        public void DeletePersonAsync_PersonDoesNotExist_ShouldThrowException()
        {
            // Arrange
            var nonExistentId = -1;

            // Act & Assert
            Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeletePersonAsync(nonExistentId));
        }

        [Test]
        public async Task GetPersonAdminDetailsAsync_PersonExists_ShouldReturnPersonAdminDto()
        {
            // Arrange
            var person = new PersonBuilder().WithFirstName("John").WithLastName("Doe").Build();
            await _context.People.AddAsync(person);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetPersonAdminDetailsAsync(person.Id);

            // Assert
            Assert.That(result, Is.TypeOf<PersonAdminDto>());
            Assert.That(result.FullName, Is.EqualTo("John Doe"));
        }

        [Test]
        public void GetPersonAdminDetailsAsync_PersonDoesNotExist_ShouldThrowException()
        {
            // Arrange
            var nonExistentId = -1;

            // Act & Assert
            Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetPersonAdminDetailsAsync(nonExistentId));
        }

        [Test]
        public async Task GetSubordinatesAsync_ManagerExists_ShouldReturnSubordinates()
        {
            // Arrange
            var manager = new PersonBuilder().WithId(1).Build();
            var subordinate1 = new PersonBuilder().WithDivisionManagerId(1).Build(); 
            var subordinate2 = new PersonBuilder().WithTalentManagerId(1).Build(); 

            await _context.People.AddAsync(manager);
            await _context.SaveChangesAsync(); 

            subordinate1.DivisionManagerId = manager.Id;
            subordinate2.TalentManagerId = manager.Id;

            await _context.People.AddRangeAsync(subordinate1, subordinate2);
            await _context.SaveChangesAsync(); 

            _hierarchyServiceMock
                .Setup(s => s.GetSubordinatePersonIdsAsync(manager.Id, false, It.IsAny<bool>()))
                .ReturnsAsync(new List<int> { subordinate1.Id, subordinate2.Id });

            // Act
            var result = await _service.GetSubordinatesAsync(manager.Id);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
        }


        [Test]
        public void GetSubordinatesAsync_ManagerDoesNotExist_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var nonExistentManagerId = -1;

            // Act & Assert
            Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetSubordinatesAsync(nonExistentManagerId));
        }

        [Test]
        public async Task GetPeopleByStreamAsync_StreamExists_ShouldReturnPeople()
        {
            // Arrange
            var personInStream1 = new PersonBuilder().WithStream("Backend").Build();
            var personInStream2 = new PersonBuilder().WithStream("Backend").Build();
            await _context.People.AddRangeAsync(personInStream1, personInStream2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetPeopleByStreamAsync("Backend");

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetPeopleByStreamAsync_StreamDoesNotExist_ShouldReturnEmpty()
        {
            // Arrange
            var nonExistentStream = "NonExistent";

            // Act
            var result = await _service.GetPeopleByStreamAsync(nonExistentStream);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetDivisionManagersAsync_ManagersExist_ShouldReturnManagers()
        {
            // Arrange
            var divisionManager1 = new UserBuilder().WithRole("DivisionManager").Build();
            var divisionManager2 = new UserBuilder().WithRole("DivisionManager").Build();
            await _context.Users.AddRangeAsync(divisionManager1, divisionManager2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetDivisionManagersAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetDivisionManagersAsync_NoManagers_ShouldReturnEmpty()
        {
            // Act
            var result = await _service.GetDivisionManagersAsync();

            // Assert
            Assert.That(result, Is.Empty);
        }


        [TearDown]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
