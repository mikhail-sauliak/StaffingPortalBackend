using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Models;
using StaffingPortalBackend.Services;

namespace StaffingPortalBackend.Tests.Unit.Services
{
    [TestFixture]
    public class PersonServiceTests
    {
        private AppDbContext _context = null!;
        private IMapper _mapper = null!;
        private PersonService _service = null!;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
            _service = new PersonService(_context, _mapper);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetPersonTitleHistory_ReturnsFallbackEntry_WhenNoHistoryExists()
        {
            var availableFrom = new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc);

            var person = new Person
            {
                FirstName = "Jane",
                LastName = "Doe",
                Location = "Remote",
                AvailableFrom = availableFrom,
                Comments = string.Empty,
                TechStack = "QA",
                Stream = "SDET",
                TMAware = "Not",
                PlannedAssignment = null,
                Level = "Junior QA Engineer",
                AssignmentExistsInGCP = false,
                UserId = null,
                User = null!,
                SalaryReviews = new List<SalaryReview>(),
                TitleHistories = new List<TitleHistory>(),
                DivisionManagerId = null,
                DivisionManager = null!,
                TalentManagerId = null,
                TalentManager = null!,
                HeadId = null,
                Head = null!,
                ManagedPersonsAsHead = new List<Person>(),
                DeputyHeadId = null,
                DeputyHead = null!,
                ManagedPersonsAsDeputyHead = new List<Person>(),
                ManagedPersonsAsDivisionManager = new List<Person>(),
                ManagedPersonsAsTalentManager = new List<Person>(),
                HierarchyNodes = new List<ManagementHierarchy>()
            };

            _context.People.Add(person);
            await _context.SaveChangesAsync();

            var history = await _service.GetPersonTitleHistory(person.Id);

            Assert.That(history, Is.Not.Null);
            var historyList = history!.ToList();
            Assert.That(historyList, Has.Count.EqualTo(1));

            var entry = historyList.Single();
            Assert.Multiple(() =>
            {
                Assert.That(entry.PersonId, Is.EqualTo(person.Id));
                Assert.That(entry.NewTitle, Is.EqualTo(person.Level));
                Assert.That(entry.OldTitle, Is.EqualTo(person.Level));
                Assert.That(entry.Justification, Is.EqualTo("Current title"));
                Assert.That(entry.DateChanged, Is.EqualTo(availableFrom));
            });
        }

        [Test]
        public async Task GetPersonTitleHistory_ReturnsExistingHistory_WhenRecordsArePresent()
        {
            var person = new Person
            {
                FirstName = "John",
                LastName = "Smith",
                Location = "Warsaw",
                AvailableFrom = new DateTime(2023, 5, 1, 0, 0, 0, DateTimeKind.Utc),
                Comments = string.Empty,
                TechStack = "QA",
                Stream = "SDET",
                TMAware = "Not",
                PlannedAssignment = null,
                Level = "Senior QA",
                AssignmentExistsInGCP = false,
                UserId = null,
                User = null!,
                SalaryReviews = new List<SalaryReview>(),
                TitleHistories = new List<TitleHistory>(),
                DivisionManagerId = null,
                DivisionManager = null!,
                TalentManagerId = null,
                TalentManager = null!,
                HeadId = null,
                Head = null!,
                ManagedPersonsAsHead = new List<Person>(),
                DeputyHeadId = null,
                DeputyHead = null!,
                ManagedPersonsAsDeputyHead = new List<Person>(),
                ManagedPersonsAsDivisionManager = new List<Person>(),
                ManagedPersonsAsTalentManager = new List<Person>(),
                HierarchyNodes = new List<ManagementHierarchy>()
            };

            _context.People.Add(person);
            await _context.SaveChangesAsync();

            var historyEntity = new TitleHistory
            {
                PersonId = person.Id,
                OldTitle = "Middle QA",
                NewTitle = "Senior QA",
                DateChanged = new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                Justification = "Promotion"
            };

            _context.TitleHistories.Add(historyEntity);
            await _context.SaveChangesAsync();

            var history = await _service.GetPersonTitleHistory(person.Id);

            Assert.That(history, Is.Not.Null);
            var historyList = history!.ToList();
            Assert.That(historyList, Has.Count.EqualTo(1));

            var entry = historyList.Single();
            Assert.Multiple(() =>
            {
                Assert.That(entry.Id, Is.Not.EqualTo(0));
                Assert.That(entry.OldTitle, Is.EqualTo("Middle QA"));
                Assert.That(entry.NewTitle, Is.EqualTo("Senior QA"));
                Assert.That(entry.Justification, Is.EqualTo("Promotion"));
            });
        }
    }
}
