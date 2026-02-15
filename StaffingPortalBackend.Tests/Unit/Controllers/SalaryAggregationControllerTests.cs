using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using StaffingPortalBackend.Controllers;
using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Models;
using StaffingPortalBackend.Services;

namespace StaffingPortalBackend.Tests.Unit.Controllers
{
    [TestFixture]
    public class SalaryAggregationControllerTests
    {
    private Mock<ISalaryReviewHistoryService> _salaryReviewHistoryServiceMock = null!;
    private Mock<IManagementHierarchyService> _hierarchyServiceMock = null!;
    private AppDbContext _context = null!;
    private SalaryAggregationController _controller = null!;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _salaryReviewHistoryServiceMock = new Mock<ISalaryReviewHistoryService>();
            _hierarchyServiceMock = new Mock<IManagementHierarchyService>();

            var authorizationHelper = new AuthorizationHelper(_context, _hierarchyServiceMock.Object);

            _controller = new SalaryAggregationController(_salaryReviewHistoryServiceMock.Object, authorizationHelper)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Name, "1"),
                            new Claim(ClaimTypes.Role, "Head"),
                            new Claim("Stream", "Engineering"),
                            new Claim("PersonId", "1")
                        }, "TestAuth"))
                    }
                }
            };
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetAggregatedMonthlySalariesByYear_ReturnsBadRequest_WhenYearIsInvalid()
        {
            var result = await _controller.GetAggregatedMonthlySalariesByYear(0);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task GetAggregatedMonthlySalariesByYear_ReturnsEmptyCollection_WhenNoData()
        {
            _salaryReviewHistoryServiceMock
                .Setup(s => s.GetAggregatedMonthlySalariesByYear(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(new List<MonthlySalaryAggregationDto>());

            var result = await _controller.GetAggregatedMonthlySalariesByYear(2024);

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.Value, Is.InstanceOf<IEnumerable<MonthlySalaryAggregationDto>>());
            Assert.That(((IEnumerable<MonthlySalaryAggregationDto>)okResult.Value!).Any(), Is.False);
        }

        [Test]
        public async Task GetAggregatedMonthlySalariesByYear_ReturnsOkWithData_WhenDataExists()
        {
            var data = new List<MonthlySalaryAggregationDto>
            {
                new MonthlySalaryAggregationDto
                {
                    PersonId = 1,
                    FullName = "John Doe",
                    Salaries = new List<SalaryMonthPeriodDto>()
                }
            };

            _salaryReviewHistoryServiceMock
                .Setup(s => s.GetAggregatedMonthlySalariesByYear(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(data);

            var result = await _controller.GetAggregatedMonthlySalariesByYear(2024);

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.Value, Is.EqualTo(data));
        }

        [Test]
        public async Task GetAggregatedMonthlySalariesByYear_ReturnsServerError_WhenServiceThrows()
        {
            _salaryReviewHistoryServiceMock
                .Setup(s => s.GetAggregatedMonthlySalariesByYear(It.IsAny<int>(), It.IsAny<User>()))
                .ThrowsAsync(new InvalidOperationException("boom"));

            var result = await _controller.GetAggregatedMonthlySalariesByYear(2024);

            var objectResult = result as ObjectResult;
            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult!.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }

        [Test]
        public async Task GetAggregatedSalaries_ReturnsOk_WithEnumerable()
        {
            var data = new List<AggregatedSalaryDto>
            {
                new AggregatedSalaryDto { PersonId = 1, FullName = "Jane" }
            };

            _salaryReviewHistoryServiceMock
                .Setup(s => s.GetAggregatedSalariesForAllTime(It.IsAny<User>()))
                .ReturnsAsync(data);

            var result = await _controller.GetAggregatedSalaries();

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.Value, Is.EqualTo(data));
        }

        [Test]
        public async Task GetAggregatedSalariesByYear_ReturnsOk_WithEmptyEnumerable()
        {
            _salaryReviewHistoryServiceMock
                .Setup(s => s.GetAggregatedSalariesByYear(It.IsAny<int>()))
                .ReturnsAsync(new List<AggregatedSalaryDto>());

            var result = await _controller.GetAggregatedSalariesByYear(2024);

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.Value, Is.InstanceOf<IEnumerable<AggregatedSalaryDto>>());
        }
    }
}
