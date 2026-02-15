using NUnit.Framework;
using StaffingPortalBackend.Services;
using StaffingPortalBackend.Models;
using System;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;

namespace StaffingPortalBackend.Tests.Unit.Services
{
    public class SalaryReviewServiceTests
    {
        private AppDbContext _context;
        private SalaryReviewService _service;
        private IMapper _mapper;

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

            _service = new SalaryReviewService(_context, _mapper);

            SalaryReviewBuilder.ResetCounter();
        }

        [Test]
        public async Task GetAllSalaryReviews_WhenCalled_ShouldReturnAllReviews()
        {
            // Arrange
            var review1 = new SalaryReviewBuilder().WithDefaults().Build();
            var review2 = new SalaryReviewBuilder().WithDefaults().Build();
            await _context.SalaryReviews.AddRangeAsync(review1, review2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetAllSalaryReviews(false);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task CreateSalaryReview_ValidData_ShouldCreateReview()
        {
            // Arrange
            var reviewDto = new SalaryReviewCreateDtoBuilder().WithDefaults().Build();

            // Act
            var result = await _service.CreateSalaryReview(reviewDto);

            // Assert
            var createdReview = await _context.SalaryReviews.FirstOrDefaultAsync(r => r.PersonId == result.PersonId);
            Assert.That(createdReview, Is.Not.Null);
            Assert.That(createdReview.PersonId, Is.EqualTo(reviewDto.PersonId));
        }

        [Test]
        public async Task GetSalaryReviewById_ExistingId_ShouldReturnReview()
        {
            // Arrange
            var review = new SalaryReviewBuilder().WithDefaults().Build();
            await _context.SalaryReviews.AddAsync(review);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetSalaryReviewById(review.Id);

            Console.WriteLine("Review Object:");
            Console.WriteLine($"Id: {review.Id}");
            Console.WriteLine($"PersonId: {review.PersonId}");
            Console.WriteLine($"ReviewDate: {review.ReviewDate}");
            Console.WriteLine($"Justification: {review.Justification}");
            Console.WriteLine($"OldSalary: {review.OldSalary}");
            Console.WriteLine($"ProposedSalary: {review.ProposedSalary}");
            Console.WriteLine($"IsApproved: {review.IsApproved}");
            Console.WriteLine($"Currency: {review.Currency}");
            Console.WriteLine($"ContractType: {review.ContractType}");
            Console.WriteLine($"NewSalaryStartDate: {review.NewSalaryStartDate}");
            Console.WriteLine($"SalaryReceivedDate: {review.SalaryReceivedDate}");
            Console.WriteLine($"DivisionManagerComments: {review.DivisionManagerComments}");
            Console.WriteLine($"TalentManagerComments: {review.TalentManagerComments}");
            Console.WriteLine($"ReviewResult: {review.ReviewResult}");
            Console.WriteLine($"ApprovedByDM: {review.ApprovedByDM}");
           
            Console.WriteLine("\nResult Object:");
            Console.WriteLine($"Id: {result.Id}");
            Console.WriteLine($"PersonId: {result.PersonId}");
            Console.WriteLine($"ReviewDate: {result.ReviewDate}");
            Console.WriteLine($"Justification: {result.Justification}");
            Console.WriteLine($"OldSalary: {result.OldSalary}");
            Console.WriteLine($"ProposedSalary: {result.ProposedSalary}");
            Console.WriteLine($"IsApproved: {result.IsApproved}");
            Console.WriteLine($"Currency: {result.Currency}");
            Console.WriteLine($"ContractType: {result.ContractType}");
            Console.WriteLine($"NewSalaryStartDate: {result.NewSalaryStartDate}");
            Console.WriteLine($"SalaryReceivedDate: {result.SalaryReceivedDate}");
            Console.WriteLine($"DivisionManagerComments: {result.DivisionManagerComments}");
            Console.WriteLine($"TalentManagerComments: {result.TalentManagerComments}");
            Console.WriteLine($"ReviewResult: {result.ReviewResult}");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(review.Id)); 
        }


        [Test]
        public async Task UpdateSalaryReview_ExistingId_ShouldUpdateReview()
        {
            // Arrange
            var review = new SalaryReviewBuilder().WithDefaults().Build();
            await _context.SalaryReviews.AddAsync(review);
            await _context.SaveChangesAsync();

            var updateDto = new SalaryReviewUpdateDtoBuilder().WithDefaults().WithPersonId(2).Build();

            // Act
            await _service.UpdateSalaryReview(review.Id, updateDto);

            // Assert
            var updatedReview = await _context.SalaryReviews.FindAsync(review.Id);
            Assert.That(updatedReview, Is.Not.Null);
            Assert.That(updatedReview.PersonId, Is.EqualTo(updateDto.PersonId));
        }

        [Test]
        public async Task DeleteSalaryReview_ExistingId_ShouldRemoveReview()
        {
            // Arrange
            var review = new SalaryReviewBuilder().WithDefaults().Build();
            await _context.SalaryReviews.AddAsync(review);
            await _context.SaveChangesAsync();

            // Act
            await _service.DeleteSalaryReview(review.Id);

            // Assert
            var deletedReview = await _context.SalaryReviews.FindAsync(review.Id);
            Assert.That(deletedReview, Is.Null);
        }

        [TearDown]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
