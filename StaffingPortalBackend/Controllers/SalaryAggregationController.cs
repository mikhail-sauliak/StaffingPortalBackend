using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Services;
using System.Security.Claims;
using StaffingPortalBackend.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace StaffingPortalBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryAggregationController : ControllerBase
    {
        private readonly ISalaryReviewHistoryService _salaryReviewHistoryService;
        private readonly AuthorizationHelper _authorizationHelper;

        public SalaryAggregationController(ISalaryReviewHistoryService salaryReviewHistoryService, AuthorizationHelper authorizationHelper)
        {
            _salaryReviewHistoryService = salaryReviewHistoryService;
            _authorizationHelper = authorizationHelper;
        }

        private User CurrentUser => new User
        {
            Id = int.Parse(User.FindFirst(ClaimTypes.Name)?.Value ?? "0"),
            Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
            Stream = User.FindFirst("Stream")?.Value ?? string.Empty,
            PersonId = int.TryParse(User.FindFirst("PersonId")?.Value, out var personId) ? personId : null
        };

        // GET: api/SalaryAggregation/AggregatedMonthlySalaries/2022
        // [HttpGet("AggregatedMonthlySalaries/{year}")]
        // public async Task<IActionResult> GetAggregatedMonthlySalariesByYear(int year)
        // {
        //     if (year < 1)
        //     {
        //         return BadRequest("Invalid year.");
        //     }

        //     var currentUser = new User
        //     {
        //         Id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0"),
        //         Role = User.FindFirst(ClaimTypes.Role)?.Value
        //     };

        //     try
        //     {
        //         var aggregatedSalaries = await _salaryReviewHistoryService.GetAggregatedMonthlySalariesByYear(year);

        //         if (aggregatedSalaries == null || !aggregatedSalaries.Any())
        //         {
        //             return NotFound($"No salary data found for the year {year}.");
        //         }

        //         Console.WriteLine("Aggregated Salaries:");
        //         foreach (var salary in aggregatedSalaries)
        //         {
        //             Console.WriteLine($"PersonId: {salary.PersonId}, FullName: {salary.FullName}, Total Salaries: {salary.Salaries.Count}");
        //             foreach (var detail in salary.Salaries)
        //             {
        //                 Console.WriteLine($" - Month: {detail.Month}, Salary: {detail.Salary}, Currency: {detail.Currency}");
        //             }
        //         }
        //         var authorizedReviews = aggregatedSalaries.Where(review => _authorizationHelper.CanUserAccessData(currentUser, review.PersonId)).ToList();

        //         if (aggregatedSalaries.Count == 0)
        //         {
        //             return NotFound("No accessible salary data found for the year.");
        //         }

        //         return Ok(aggregatedSalaries);
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, $"Internal server error: {ex.Message}");
        //     }
        // }
        [HttpGet("AggregatedMonthlySalaries/{year}")]
        public async Task<IActionResult> GetAggregatedMonthlySalariesByYear(int year)
        {
            if (year < 1)
            {
                return BadRequest("Invalid year.");
            }

            var currentUser = CurrentUser; // Используйте свойство для получения текущего пользователя

            try
            {
                var aggregatedSalaries = await _salaryReviewHistoryService.GetAggregatedMonthlySalariesByYear(year, currentUser)
                    ?? new List<MonthlySalaryAggregationDto>();

                if (!aggregatedSalaries.Any())
                {
                    return Ok(Array.Empty<MonthlySalaryAggregationDto>());
                }

                return Ok(aggregatedSalaries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("AggregatedSalaries")]
        public async Task<IActionResult> GetAggregatedSalaries()
        {
            try
            {
                var currentUser = CurrentUser;
                var aggregatedSalaries = await _salaryReviewHistoryService.GetAggregatedSalariesForAllTime(currentUser)
                    ?? Enumerable.Empty<AggregatedSalaryDto>();

                return Ok(aggregatedSalaries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("aggregated-salaries/{year}")]
        public async Task<ActionResult<IEnumerable<AggregatedSalaryDto>>> GetAggregatedSalariesByYear(int year)
        {
            var aggregatedSalaries = await _salaryReviewHistoryService.GetAggregatedSalariesByYear(year)
                ?? Enumerable.Empty<AggregatedSalaryDto>();

            return Ok(aggregatedSalaries);
        }
    }
}
