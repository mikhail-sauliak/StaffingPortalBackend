using NUnit.Framework;
using StaffingPortalBackend.Models;
using StaffingPortalBackend.Services;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Moq;

namespace StaffingPortalBackend.Tests.Unit.Services
{
    [TestFixture]
    public class SalaryReviewHistoryServiceTests
    {
        private AppDbContext _context;
        private IMapper _mapper;
        private SalaryReviewHistoryService _service;
        private Mock<IManagementHierarchyService> _hierarchyServiceMock;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new AppDbContext(options);
            _mapper = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            }).CreateMapper();
            _hierarchyServiceMock = new Mock<IManagementHierarchyService>();
            _hierarchyServiceMock
                .Setup(s => s.GetSubordinatePersonIdsAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(System.Array.Empty<int>());
            var authorizationHelper = new AuthorizationHelper(_context, _hierarchyServiceMock.Object);
            _service = new SalaryReviewHistoryService(_context, _mapper, authorizationHelper);
        }

        [Test]
        public async Task GetSalaryReviewHistoryByPersonId_ReturnsCorrectData()
        {
            // Arrange
            int testPersonId = 1;

            var salaryReviews = new[]
            {
                new SalaryReviewBuilder().WithDefaults().WithPersonId(testPersonId).WithReviewDate(DateTime.UtcNow.AddDays(-100)).Build(),
                new SalaryReviewBuilder().WithDefaults().WithPersonId(testPersonId).WithReviewDate(DateTime.UtcNow.AddDays(-200)).Build()
            };

            await _context.AddRangeAsync(salaryReviews);
            await _context.SaveChangesAsync();

            var salaryReviewHistories = salaryReviews.Select(sr => new SalaryReviewHistory
            {
                PersonId = sr.PersonId,
                ReviewDate = sr.ReviewDate,
                Justification = sr.Justification,
                OldSalary = sr.OldSalary,
                ProposedSalary = sr.ProposedSalary,
                IsApproved = sr.IsApproved,
                Currency = sr.Currency,
                ContractType = sr.ContractType,
                NewSalaryStartDate = sr.NewSalaryStartDate,
                SalaryReceivedDate = sr.SalaryReceivedDate,
                DivisionManagerComments = sr.DivisionManagerComments,
                TalentManagerComments = sr.TalentManagerComments,
                ReviewResult = sr.ReviewResult,
                ArchivedDate = DateTime.UtcNow 
            }).ToArray();

            await _context.AddRangeAsync(salaryReviewHistories);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetSalaryReviewHistoryByPersonId(testPersonId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(salaryReviewHistories.Length));
        }

        [Test]
        public async Task ArchiveSalaryReview_ArchivesExistingReview()
        {
            // Arrange
            var salaryReview = new SalaryReviewBuilder().WithDefaults().Build();
            await _context.AddAsync(salaryReview);
            await _context.SaveChangesAsync();

            // Act
            await _service.ArchiveSalaryReview(salaryReview.Id);

            // Assert
            var archivedReview = await _context.SalaryReviewHistories.FirstOrDefaultAsync(r => r.SalaryReviewId == salaryReview.Id);
            Assert.That(archivedReview, Is.Not.Null);
        }

        [Test]
        public async Task ArchivePreviousSalaryReview_ArchivesReviewIfExists()
        {
            // Arrange
            int testPersonId = 1;
            var salaryReview = new SalaryReviewBuilder().WithDefaults().WithPersonId(testPersonId).Build();
            await _context.AddAsync(salaryReview);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.ArchivePreviousSalaryReview(testPersonId);

            // Assert
            Assert.That(result, Is.True);
            var archivedReview = await _context.SalaryReviewHistories.FirstOrDefaultAsync(r => r.PersonId == testPersonId);
            Assert.That(archivedReview, Is.Not.Null);
        }

        [Test]
        public async Task ArchivePreviousSalaryReview_DoesNotArchiveWhenNoReviewExists()
        {
            // Arrange
            int testPersonId = 2;

            // Act
            var result = await _service.ArchivePreviousSalaryReview(testPersonId);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task GetSalaryReviewHistoryByPersonId_WithNoHistory_ReturnsEmpty()
        {
            // Arrange
            int testPersonId = 3; 

            // Act
            var result = await _service.GetSalaryReviewHistoryByPersonId(testPersonId);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task ArchiveSalaryReview_WithNonExistingReview_DoesNotCreateArchive()
        {
            // Arrange
            int nonExistingReviewId = -1;

            // Act
            await _service.ArchiveSalaryReview(nonExistingReviewId);

            // Assert
            var archivedReview = await _context.SalaryReviewHistories.FirstOrDefaultAsync(r => r.SalaryReviewId == nonExistingReviewId);
            Assert.That(archivedReview, Is.Null);
        }

        [Test]
        public async Task ArchivePreviousSalaryReview_WithMultipleReviews_ArchivesMostRecent()
        {
            // Arrange
            int testPersonId = 4;
            var salaryReviews = new[]
            {
                new SalaryReviewBuilder().WithDefaults().WithPersonId(testPersonId).WithReviewDate(DateTime.UtcNow.AddDays(-60)).Build(),
                new SalaryReviewBuilder().WithDefaults().WithPersonId(testPersonId).WithReviewDate(DateTime.UtcNow.AddDays(-30)).Build()
            };

            await _context.AddRangeAsync(salaryReviews);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.ArchivePreviousSalaryReview(testPersonId);

            // Assert
            Assert.That(result, Is.True);
            var archivedReview = await _context.SalaryReviewHistories.OrderByDescending(r => r.ReviewDate).FirstOrDefaultAsync(r => r.PersonId == testPersonId);
            Assert.That(archivedReview, Is.Not.Null);
            Assert.That(archivedReview.ReviewDate, Is.EqualTo(salaryReviews.Last().ReviewDate));
        }

        [Test]
        public async Task ArchivePreviousSalaryReview_ThrowsException_ReturnsFalse()
        {
            // Arrange
            int testPersonId = 1;
            var salaryReview = new SalaryReviewBuilder().WithDefaults().WithPersonId(testPersonId).Build();
            await _context.AddAsync(salaryReview);
            await _context.SaveChangesAsync();
            
            _context.Database.EnsureDeleted(); 

            // Act
            var result = await _service.ArchivePreviousSalaryReview(testPersonId);

            // Assert
            Assert.That(result, Is.False);
        }  
        
        [TearDown]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
