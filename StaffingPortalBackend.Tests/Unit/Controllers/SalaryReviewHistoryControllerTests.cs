using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using StaffingPortalBackend.Controllers;
using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Models;
using StaffingPortalBackend.Services;

namespace StaffingPortalBackend.Tests.Unit.Controllers
{
    [TestFixture]
    public class SalaryReviewHistoryControllerTests
    {
        private Mock<ISalaryReviewHistoryService> _serviceMock = null!;
        private SalaryReviewHistoryController _controller = null!;

        [SetUp]
        public void SetUp()
        {
            _serviceMock = new Mock<ISalaryReviewHistoryService>();
            _controller = new SalaryReviewHistoryController(_serviceMock.Object);
        }

        [Test]
        public async Task GetAllHistory_ReturnsOkWithServiceResponse()
        {
            var history = new List<SalaryReviewHistoryReadDto>
            {
                new SalaryReviewHistoryReadDto { HistoryId = 1, PersonId = 5 }
            };
            _serviceMock.Setup(s => s.GetAllHistoryAsync()).ReturnsAsync(history);

            var result = await _controller.GetAllHistory();

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult!.Value as ServiceResponse<IEnumerable<SalaryReviewHistoryReadDto>>;
            Assert.That(response, Is.Not.Null);
            Assert.That(response!.Data, Is.EqualTo(history));
        }

        [Test]
        public async Task GetHistoryByPersonId_ReturnsOkWithEmptyCollection_WhenNoRecords()
        {
            _serviceMock.Setup(s => s.GetHistoryByPersonIdAsync(It.IsAny<int>()))
                .ReturnsAsync((IEnumerable<SalaryReviewHistoryReadDto>)new List<SalaryReviewHistoryReadDto>());

            var result = await _controller.GetHistoryByPersonId(10);

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var response = okResult!.Value as ServiceResponse<IEnumerable<SalaryReviewHistoryReadDto>>;
            Assert.That(response, Is.Not.Null);
            Assert.That(response!.Data, Is.Empty);
        }

        [Test]
        public async Task CreateHistory_ReturnsBadRequest_WhenModelInvalid()
        {
            _controller.ModelState.AddModelError("PersonId", "Required");

            var result = await _controller.CreateHistory(new SalaryReviewHistoryCreateDto());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task CreateHistory_ReturnsCreated_WhenModelValid()
        {
            var dto = new SalaryReviewHistoryCreateDto
            {
                PersonId = 1,
                SalaryReviewId = 2,
                ReviewDate = DateTime.UtcNow,
                OldSalary = 1000,
                ProposedSalary = 1200,
                Currency = "PLN",
                ContractType = "B2B"
            };

            var result = await _controller.CreateHistory(dto);

            var createdResult = result as ObjectResult;
            Assert.That(createdResult, Is.Not.Null);
            Assert.That(createdResult!.StatusCode, Is.EqualTo(201));
            var response = createdResult.Value as ServiceResponse<SalaryReviewHistoryCreateDto>;
            Assert.That(response, Is.Not.Null);
            Assert.That(response!.Data, Is.EqualTo(dto));
        }

        [Test]
        public async Task UpdateHistory_ReturnsBadRequest_WhenIdsMismatch()
        {
            var dto = new SalaryReviewHistoryUpdateDto { HistoryId = 2 };

            var result = await _controller.UpdateHistory(1, dto);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task DeleteHistory_ReturnsNoContent()
        {
            var result = await _controller.DeleteHistory(5);

            Assert.That(result, Is.TypeOf<NoContentResult>());
            _serviceMock.Verify(s => s.DeleteHistoryAsync(5), Times.Once);
        }
    }
}
