using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Services;
using System.Security.Claims;
using StaffingPortalBackend.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace StaffingPortalBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryReviewController : ControllerBase
    {
        private readonly ISalaryReviewService _salaryReviewService;
        private readonly ISalaryReviewHistoryService _salaryReviewHistoryService;
        private readonly AuthorizationHelper _authorizationHelper;

        public SalaryReviewController(
            ISalaryReviewService salaryReviewService,
            ISalaryReviewHistoryService salaryReviewHistoryService,
            AuthorizationHelper authorizationHelper)
        {
            _salaryReviewService = salaryReviewService;
            _salaryReviewHistoryService = salaryReviewHistoryService;
            _authorizationHelper = authorizationHelper;
        }

        private User CurrentUser => new User
        {
            Id = int.Parse(User.FindFirst(ClaimTypes.Name)?.Value ?? "0"),
            Role = User.FindFirst(ClaimTypes.Role)?.Value,
            Stream = User.FindFirst("Stream")?.Value,
            PersonId = int.TryParse(User.FindFirst("PersonId")?.Value, out var personId) ? personId : null
        };

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalaryReviewReadDto>>> GetAllSalaryReviews()
        {
            var isTalentManager = User.FindFirst(ClaimTypes.Role)?.Value == "TalentManager";
            var salaryReviews = await _salaryReviewService.GetAllSalaryReviews(isTalentManager);
            var authorizedReviews = new List<SalaryReviewReadDto>();

            foreach (var review in salaryReviews)
            {
                if (await _authorizationHelper.CanUserAccessDataAsync(CurrentUser, review.PersonId))
                {
                    authorizedReviews.Add(review);
                }
            }

            return Ok(authorizedReviews);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SalaryReviewReadDto>> GetSalaryReviewById(int id)
        {
            var salaryReview = await _salaryReviewService.GetSalaryReviewById(id);
            if (salaryReview == null) return NotFound();
            if (!await _authorizationHelper.CanUserAccessDataAsync(CurrentUser, salaryReview.PersonId)) return Forbid();
            return Ok(salaryReview);
        }

        [HttpGet("current/{personId}")]
        public async Task<ActionResult<SalaryReviewReadDto>> GetCurrentSalaryByPersonId(int personId)
        {
            if (!await _authorizationHelper.CanUserAccessDataAsync(CurrentUser, personId)) return Forbid();

            var currentSalary = await _salaryReviewService.GetCurrentSalaryByPersonId(personId);
            if (currentSalary == null) return NotFound();
            return Ok(currentSalary);
        }

        [HttpPost]
        public async Task<ActionResult<SalaryReviewReadDto>> CreateSalaryReview([FromBody] SalaryReviewCreateDto salaryReviewCreateDto)
        {
            var salaryReview = await _salaryReviewService.CreateSalaryReview(salaryReviewCreateDto);
            return CreatedAtAction(nameof(GetSalaryReviewById), new { id = salaryReview.Id }, salaryReview);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSalaryReview(int id, [FromBody] SalaryReviewUpdateDto salaryReviewUpdateDto)
        {
            var salaryReview = await _salaryReviewService.GetSalaryReviewById(id);
            if (salaryReview == null) return NotFound();
            if (!await _authorizationHelper.CanUserAccessDataAsync(CurrentUser, salaryReview.PersonId)) return Forbid();

            await _salaryReviewService.UpdateSalaryReview(id, salaryReviewUpdateDto);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchSalaryReview(int id, [FromBody] SalaryReviewPatchDto salaryReviewPatchDto)
        {           
            var personId = await _salaryReviewService.GetPersonIdBySalaryReviewId(id);
            if (!personId.HasValue)
            {
                return NotFound("Salary review not found.");
            }
            
            if (!await _authorizationHelper.CanUserAccessDataAsync(CurrentUser, personId.Value))
            {
                return Forbid();
            }

            try
            {
                await _salaryReviewService.UpdatePartialSalaryReview(id, salaryReviewPatchDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the salary review: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalaryReview(int id)
        {
            var salaryReview = await _salaryReviewService.GetSalaryReviewById(id);
            if (salaryReview == null) return NotFound();
            if (!await _authorizationHelper.CanUserAccessDataAsync(CurrentUser, salaryReview.PersonId)) return Forbid();

            await _salaryReviewService.DeleteSalaryReview(id);
            return NoContent();
        }

        [HttpPost("archive/{id}")]
        public async Task<IActionResult> ArchiveSalaryReview(int id)
        {
            await _salaryReviewHistoryService.ArchiveSalaryReview(id);
            return NoContent();
        }

        [HttpGet("history/{personId}")]
        public async Task<ActionResult<IEnumerable<SalaryReviewHistoryReadDto>>> GetSalaryReviewHistoryByPersonId(int personId)
        {
            if (!await _authorizationHelper.CanUserAccessDataAsync(CurrentUser, personId)) return Forbid();

            var history = await _salaryReviewHistoryService.GetHistoryByPersonIdAsync(personId);
            if (history == null || !history.Any()) return NotFound();
            return Ok(history);
        }
    }
}
