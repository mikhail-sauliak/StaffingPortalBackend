using NUnit.Framework;
using StaffingPortalBackend.Models;
using StaffingPortalBackend.Services;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Threading.Tasks;
using StaffingPortalBackend.Tests.Unit.Mocks;
using StaffingPortalBackend.DTO;
using System.Linq;
using System;
using System.Collections.Generic;
using Moq;

namespace StaffingPortalBackend.Tests.Unit.Services
{
    public class SalaryReviewRequestServiceTests
    {
        private AppDbContext _context;
        private SalaryReviewRequestService _service;
        private IMapper _mapper;
        private ISalaryReviewHistoryService _mockSalaryReviewHistoryService;
        private Mock<IManagementHierarchyService> _hierarchyServiceMock;
        private int _managerPersonId;

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

            _mockSalaryReviewHistoryService = new MockSalaryReviewHistoryService(); 

            _hierarchyServiceMock = new Mock<IManagementHierarchyService>();
            _hierarchyServiceMock
                .Setup(s => s.GetSubordinatePersonIdsAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(System.Array.Empty<int>());

            _service = new SalaryReviewRequestService(_context, _mapper, _mockSalaryReviewHistoryService, _hierarchyServiceMock.Object);
            var user = new UserBuilder()
                .WithDefaults()
                .WithId(1)
                .WithRole("DivisionManager")
                .Build();

            _context.Users.Add(user);                

            var person = new PersonBuilder()
                .WithFirstName("Vasya")
                .WithLastName("Pupkin")
                .WithUserId(user.Id)
                .Build();

            _context.People.Add(person);
            _context.SaveChanges();

            user.PersonId = person.Id;
            _context.Users.Update(user);
            _context.SaveChanges();

            _managerPersonId = person.Id;
        }

        [Test]
        public async Task CreateReviewRequest_ShouldCreateRequest()
        {
            var requestDto = new SalaryReviewRequestDtoBuilder().WithDefaults().Build();

            var result = await _service.CreateReviewRequest(requestDto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.PersonId, Is.EqualTo(requestDto.PersonId));            
        }

        [Test]
        public async Task ApproveRequest_ShouldChangeStatusToApproved()
        {
            var requestDto = new SalaryReviewRequestDtoBuilder()
                .WithDefaults()
                .WithStatus("Pending")
                .Build();
            var request = _mapper.Map<SalaryReviewRequest>(requestDto);
            _context.SalaryReviewRequests.Add(request);
            await _context.SaveChangesAsync();
            
            var mockSalaryReview = new SalaryReviewReadDto
            {
                Id = 1,
                PersonId = requestDto.PersonId,               
            };
            
            var mockService = _mockSalaryReviewHistoryService as MockSalaryReviewHistoryService;
            mockService?.AddSalaryReview(mockSalaryReview);
            
            var existingRequest = await _context.SalaryReviewRequests.FindAsync(request.Id);
            Assert.That(existingRequest, Is.Not.Null);
            
            var result = await _service.ApproveRequest(existingRequest.Id, requestDto);

            Assert.That(result.Status, Is.EqualTo("Approved"));            
        }

        [Test]
        public async Task DeclineRequest_ShouldChangeStatusToDeclined()
        {
            var requestDto = new SalaryReviewRequestDtoBuilder()
                .WithDefaults()
                .WithStatus("Pending") 
                .Build();

            var request = _mapper.Map<SalaryReviewRequest>(requestDto);
            _context.SalaryReviewRequests.Add(request);
            await _context.SaveChangesAsync();

            var result = await _service.DeclineRequest(request.Id, "Not justified");

            Assert.That(result.Status, Is.EqualTo("Declined"));
            Assert.That(result.DivisionManagerComments, Is.EqualTo("Not justified"));
        }

        [Test]
        public async Task GetPendingRequests_ShouldReturnPendingRequests()
        {
            var requestDto1 = new SalaryReviewRequestDtoBuilder().WithDefaults().WithStatus("Pending").Build();
            var requestDto2 = new SalaryReviewRequestDtoBuilder().WithDefaults().WithStatus("Pending").Build();
            var request1 = _mapper.Map<SalaryReviewRequest>(requestDto1);
            var request2 = _mapper.Map<SalaryReviewRequest>(requestDto2);
            _context.SalaryReviewRequests.AddRange(request1, request2);
            await _context.SaveChangesAsync();

            var permittedPersonIds = new List<int> { request1.PersonId, request2.PersonId };
            _hierarchyServiceMock
                .Setup(s => s.GetSubordinatePersonIdsAsync(_managerPersonId, true, It.IsAny<bool>()))
                .ReturnsAsync(permittedPersonIds);
            
            var savedRequests = await _context.SalaryReviewRequests.ToListAsync();
            Console.WriteLine($"Saved requests in DB: {savedRequests.Count}");
            foreach (var req in savedRequests)
            {
                Console.WriteLine($"Saved Request ID: {req.Id}, Status: {req.Status}, PersonID: {req.PersonId}");
            }
            var result = await _service.GetPendingRequests(1);
            foreach (var req in result)
            {
                Console.WriteLine($"Result Request ID: {req.Id}, Status: {req.Status}, PersonID: {req.PersonId}");
            }

            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetReviewRequest_ShouldReturnSpecificRequest()
        {
            var requestDto = new SalaryReviewRequestDtoBuilder()
                .WithDefaults()
                .WithStatus("Pending")
                .Build();
            var request = _mapper.Map<SalaryReviewRequest>(requestDto);
            _context.SalaryReviewRequests.Add(request);
            await _context.SaveChangesAsync();

            var result = await _service.GetReviewRequest(request.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(request.Id));
        }

        [Test]
        public async Task GetPendingRequests_UserNotFound_ThrowsInvalidOperationException()
        {
            var nonExistingUserId = -1;

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => _service.GetPendingRequests(nonExistingUserId));
        }

        [Test]
        public async Task GetPendingRequests_InvalidUserRole_ThrowsUnauthorizedAccessException()
        {
            var user = new UserBuilder()
                .WithDefaults()
                .WithRole("InvalidRole")
                .Build();
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act & Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.GetPendingRequests(user.Id));
        }

        [Test]
        public async Task ApproveRequest_RequestNotFound_ThrowsKeyNotFoundException()
        {
            var nonExistingRequestId = -1;

            // Act & Assert
            Assert.ThrowsAsync<KeyNotFoundException>(() => _service.ApproveRequest(nonExistingRequestId, new SalaryReviewRequestDtoBuilder().WithDefaults().Build()));
        }

        [Test]
        public async Task UpdateFields_ReviewNotFound_ThrowsInvalidOperationException()
        {
            var nonExistingReviewId = -1;
            var updateFieldsDto = new UpdateFieldsDto();

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateFields(nonExistingReviewId, updateFieldsDto));
        }

        [Test]
        public async Task ApproveByDM_ReviewNotFound_ThrowsInvalidOperationException()
        {
            var nonExistingReviewId = -1;

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => _service.ApproveByDM(nonExistingReviewId));
        }

        [Test]
        public async Task GetRequestsWithReviews_ReturnsPendingRequestsWithReviews()        {
            
            var person = new PersonBuilder()
                .WithDefaults()
                .WithDivisionManagerId(1)
                .Build();

            _context.People.Add(person);
            await _context.SaveChangesAsync();

            var review = new SalaryReviewBuilder()
                .WithDefaults()
                .WithPersonId(person.Id)
                .Build();

            _context.SalaryReviews.Add(review);
            await _context.SaveChangesAsync();

            _hierarchyServiceMock
                .Setup(s => s.GetSubordinatePersonIdsAsync(_managerPersonId, true, It.IsAny<bool>()))
                .ReturnsAsync(new List<int> { person.Id });

            var request1 = new SalaryReviewRequestBuilder()
                .WithDefaults()
                .WithPersonId(person.Id)
                .WithStatus("Pending")
                .Build();

            var request2 = new SalaryReviewRequestBuilder()
                .WithDefaults()
                .WithPersonId(person.Id)
                .WithStatus("Pending")
                .Build();

            _context.SalaryReviewRequests.AddRange(request1, request2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetRequestsWithReviews();

            // Assert
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count(), Is.EqualTo(2));
            foreach (var requestDto in result)
            {
                Assert.That(requestDto.Status, Is.EqualTo("Pending"));
                Assert.That(requestDto.PersonId, Is.EqualTo(person.Id));                
            }
        }

        [TearDown]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
