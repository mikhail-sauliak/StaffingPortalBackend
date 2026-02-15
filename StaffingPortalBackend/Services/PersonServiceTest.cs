// using NUnit.Framework;
// using Moq;
// using AutoMapper;
// using Microsoft.EntityFrameworkCore;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using StaffingPortalBackend.Services;
// using StaffingPortalBackend.Models;
// using StaffingPortalBackend.DTO;

// namespace StaffingPortalBackend.Tests.Services
// {
//     [TestFixture]
//     public class PersonServiceTests
//     {
//         private PersonService _personService;
//         private Mock<AppDbContext> _mockDbContext;
//         private Mock<IMapper> _mockMapper;

//         [SetUp]
//         public void Setup()
//         {
//             _mockDbContext = new Mock<AppDbContext>();
//             _mockMapper = new Mock<IMapper>();
//             _personService = new PersonService(_mockDbContext.Object, _mockMapper.Object);
//         }

//         [Test]
//         public async Task GetAllPersons_ShouldReturnAllPersons()
//         {
//             // Arrange
//             var persons = new List<Person>
//             {
//                 new Person { Id = 1, FirstName = "John", LastName = "Doe" },
//                 new Person { Id = 2, FirstName = "Jane", LastName = "Smith" }
//             };
//             var personReadDtos = new List<PersonReadDto>
//             {
//                 new PersonReadDto { Id = 1, FirstName = "John", LastName = "Doe" },
//                 new PersonReadDto { Id = 2, FirstName = "Jane", LastName = "Smith" }
//             };

//             _mockDbContext.Setup(db => db.People).Returns(MockDbSet(persons));
//             _mockMapper.Setup(mapper => mapper.Map<IEnumerable<PersonReadDto>>(persons)).Returns(personReadDtos);

//             // Act
//             var result = await _personService.GetAllPersons();

//             // Assert
//             Assert.AreEqual(personReadDtos, result);
//         }

//         [Test]
//         public async Task GetAllActivePersons_ShouldReturnActivePersons()
//         {
//             // Arrange
//             var currentDate = DateTime.Today;
//             var persons = new List<Person>
//             {
//                 new Person { Id = 1, FirstName = "John", LastName = "Doe", IsDismissed = false, EndDate = null },
//                 new Person { Id = 2, FirstName = "Jane", LastName = "Smith", IsDismissed = false, EndDate = currentDate.AddDays(1) },
//                 new Person { Id = 3, FirstName = "Bob", LastName = "Johnson", IsDismissed = true, EndDate = currentDate.AddDays(-1) }
//             };
//             var activePersons = persons.Where(p => !p.IsDismissed && (p.EndDate == null || p.EndDate >= currentDate));
//             var activePersonReadDtos = new List<PersonReadDto>
//             {
//                 new PersonReadDto { Id = 1, FirstName = "John", LastName = "Doe" },
//                 new PersonReadDto { Id = 2, FirstName = "Jane", LastName = "Smith" }
//             };

//             _mockDbContext.Setup(db => db.People).Returns(MockDbSet(persons));
//             _mockMapper.Setup(mapper => mapper.Map<IEnumerable<PersonReadDto>>(activePersons)).Returns(activePersonReadDtos);

//             // Act
//             var result = await _personService.GetAllActivePersons();

//             // Assert
//             Assert.AreEqual(activePersonReadDtos, result);
//         }


//         [Test]
//         public async Task GetPersonById_ExistingId_ShouldReturnPerson()
//         {
//             // Arrange
//             int existingId = 1;
//             var existingPerson = new Person { Id = existingId, FirstName = "John", LastName = "Doe" };
//             var personReadDto = new PersonReadDto { Id = existingId, FirstName = "John", LastName = "Doe" };

//             _mockDbContext.Setup(db => db.People.FindAsync(existingId)).ReturnsAsync(existingPerson);
//             _mockMapper.Setup(mapper => mapper.Map<PersonReadDto>(existingPerson)).Returns(personReadDto);

//             // Act
//             var result = await _personService.GetPersonById(existingId);

//             // Assert
//             Assert.AreEqual(personReadDto, result);
//         }

//         [Test]
//         public async Task GetPersonById_NonExistingId_ShouldReturnNull()
//         {
//             // Arrange
//             int nonExistingId = 100;

//             _mockDbContext.Setup(db => db.People.FindAsync(nonExistingId)).ReturnsAsync((Person)null);

//             // Act
//             var result = await _personService.GetPersonById(nonExistingId);

//             // Assert
//             Assert.IsNull(result);
//         }

//         [Test]
//         public async Task CreatePerson_ValidPerson_ShouldReturnCreatedPerson()
//         {
//             // Arrange
//             var personToCreate = new Person { FirstName = "John", LastName = "Doe" };
//             var createdPerson = new Person { Id = 1, FirstName = "John", LastName = "Doe" };
//             var personReadDto = new PersonReadDto { Id = 1, FirstName = "John", LastName = "Doe" };

//             _mockDbContext.Setup(db => db.People.AddAsync(personToCreate)).ReturnsAsync(createdPerson);
//             _mockMapper.Setup(mapper => mapper.Map<PersonReadDto>(createdPerson)).Returns(personReadDto);

//             // Act
//             var result = await _personService.CreatePerson(personToCreate);

//             // Assert
//             Assert.AreEqual(personReadDto, result.Data);
//             Assert.IsTrue(result.Success);
//         }

//         [Test]
//         public async Task GetAllPersons_ShouldReturnAllPersons()
//         {
//             // Arrange
//             var persons = new List<Person>
//             {
//                 new Person { Id = 1, FirstName = "John", LastName = "Doe" },
//                 new Person { Id = 2, FirstName = "Jane", LastName = "Smith" }
//             };
//             var personReadDtos = persons.Select(p => new PersonReadDto { Id = p.Id, FirstName = p.FirstName, LastName = p.LastName });

//             _mockDbContext.Setup(db => db.People).Returns(MockDbSet(persons));
//             _mockMapper.Setup(mapper => mapper.Map<IEnumerable<PersonReadDto>>(persons)).Returns(personReadDtos);

//             // Act
//             var result = await _personService.GetAllPersons();

//             // Assert
//             Assert.AreEqual(personReadDtos, result);
//         }

//         [Test]
//         public async Task GetAllActivePersons_ShouldReturnActivePersons()
//         {
//             // Arrange
//             var activePersons = new List<Person>
//             {
//                 new Person { Id = 1, FirstName = "John", LastName = "Doe", IsActive = true },
//                 new Person { Id = 2, FirstName = "Jane", LastName = "Smith", IsActive = true },
//                 new Person { Id = 3, FirstName = "Bob", LastName = "Johnson", IsActive = false }
//             };
//             var activePersonReadDtos = activePersons
//                 .Where(p => p.IsActive)
//                 .Select(p => new PersonReadDto { Id = p.Id, FirstName = p.FirstName, LastName = p.LastName });

//             _mockDbContext.Setup(db => db.People).Returns(MockDbSet(activePersons));
//             _mockMapper.Setup(mapper => mapper.Map<IEnumerable<PersonReadDto>>(activePersons)).Returns(activePersonReadDtos);

//             // Act
//             var result = await _personService.GetAllActivePersons();

//             // Assert
//             Assert.AreEqual(activePersonReadDtos, result);
//         }

//         [Test]
//         public async Task UpdatePerson_ValidPerson_ShouldReturnUpdatedPerson()
//         {
//             // Arrange
//             var personToUpdate = new Person { Id = 1, FirstName = "John", LastName = "Doe" };
//             var updatedPerson = new Person { Id = 1, FirstName = "John", LastName = "Smith" };
//             var personReadDto = new PersonReadDto { Id = 1, FirstName = "John", LastName = "Smith" };

//             _mockDbContext.Setup(db => db.People.FindAsync(personToUpdate.Id)).ReturnsAsync(personToUpdate);
//             _mockMapper.Setup(mapper => mapper.Map<PersonReadDto>(updatedPerson)).Returns(personReadDto);

//             // Act
//             await _personService.UpdatePerson(new PersonUpdateDto { Id = personToUpdate.Id, FirstName = "John", LastName = "Smith" });
//             var result = await _personService.GetPersonById(personToUpdate.Id);

//             // Assert
//             Assert.AreEqual(personReadDto, result);
//         }

//         private DbSet<T> MockDbSet<T>(IEnumerable<T> data) where T : class
//         {
//             var queryableData = data.AsQueryable();
//             var mockDbSet = new Mock<DbSet<T>>();
//             mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableData.Provider);
//             mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
//             mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
//             mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryableData.GetEnumerator());
//             return mockDbSet.Object;
//         }
//     }
// }