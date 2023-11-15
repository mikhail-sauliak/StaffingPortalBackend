using Microsoft.AspNetCore.Mvc;
using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Models;
using StaffingPortalBackend.Services;
using AutoMapper;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace StaffingPortalBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryReviewController : ControllerBase
    {
        private readonly ISalaryReviewService _salaryReviewService;
        private readonly ISalaryReviewHistoryService _salaryReviewHistoryService;
        private readonly IMapper _mapper;
        private readonly AuthorizationHelper _authorizationHelper;

        public SalaryReviewController(
            ISalaryReviewService salaryReviewService,
            ISalaryReviewHistoryService salaryReviewHistoryService,
            IMapper mapper,
            AuthorizationHelper authorizationHelper)
        {
            _salaryReviewService = salaryReviewService;
            _salaryReviewHistoryService = salaryReviewHistoryService;
            _mapper = mapper;
            _authorizationHelper = authorizationHelper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalaryReviewReadDto>>> GetAllSalaryReviews()
        {
            var isTalentManager = User.FindFirst(ClaimTypes.Role)?.Value == "TalentManager";
            var userId = int.Parse(User.FindFirst(ClaimTypes.Name)?.Value ?? "0");
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userStream = User.FindFirst("Stream")?.Value;

            Console.WriteLine($"User Id: {userId}, Role: {userRole}, Stream: {userStream}");

            var currentUser = new User
            {
                Id = userId,
                Role = userRole,
                Stream = userStream
            };

            var salaryReviews = await _salaryReviewService.GetAllSalaryReviews(isTalentManager);
            var authorizedReviews = new List<SalaryReviewReadDto>();

            foreach (var review in salaryReviews)
            {
                Console.WriteLine($"Review PersonId: {review.PersonId}");
                if (_authorizationHelper.CanUserAccessData(currentUser, review.PersonId))
                {
                    authorizedReviews.Add(review);
                }
            }

            return Ok(_mapper.Map<IEnumerable<SalaryReviewReadDto>>(authorizedReviews));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SalaryReviewReadDto>> GetSalaryReviewById(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.Name)?.Value ?? "0");
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userStream = User.FindFirst("Stream")?.Value;

            var currentUser = new User
            {
                Id = userId,
                Role = userRole,
                Stream = userStream
            };

            var salaryReview = await _salaryReviewService.GetSalaryReviewById(id);
            if (salaryReview == null)
            {
                return NotFound();
            }

            Console.WriteLine($"Access check: {_authorizationHelper.CanUserAccessData(currentUser, salaryReview.PersonId)}");

            if (!_authorizationHelper.CanUserAccessData(currentUser, salaryReview.PersonId))
                return Forbid();

            return Ok(_mapper.Map<SalaryReviewReadDto>(salaryReview));
        }

        [HttpPost]
        public async Task<ActionResult<SalaryReviewReadDto>> CreateSalaryReview(SalaryReviewCreateDto salaryReviewCreateDto)
        {
            var salaryReviewReadDto = await _salaryReviewService.CreateSalaryReview(salaryReviewCreateDto);
            return CreatedAtAction(nameof(GetSalaryReviewById), new { Id = salaryReviewReadDto.Id }, salaryReviewReadDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSalaryReview(int id, SalaryReviewUpdateDto salaryReviewUpdateDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.Name)?.Value ?? "0");
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userStream = User.FindFirst("Stream")?.Value;

            var currentUser = new User
            {
                Id = userId,
                Role = userRole,
                Stream = userStream
            };

            var salaryReview = await _salaryReviewService.GetSalaryReviewById(id);
            if (salaryReview == null)
            {
                return NotFound();
            }

            if (!_authorizationHelper.CanUserAccessData(currentUser, salaryReview.PersonId))
                return Forbid();

            await _salaryReviewService.UpdateSalaryReview(id, salaryReviewUpdateDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSalaryReview(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.Name)?.Value ?? "0");
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userStream = User.FindFirst("Stream")?.Value;

            var currentUser = new User
            {
                Id = userId,
                Role = userRole,
                Stream = userStream
            };

            var salaryReview = await _salaryReviewService.GetSalaryReviewById(id);
            if (salaryReview == null)
            {
                return NotFound();
            }

            if (!_authorizationHelper.CanUserAccessData(currentUser, salaryReview.PersonId))
                return Forbid();

            await _salaryReviewService.DeleteSalaryReview(id);
            return NoContent();
        }

        [HttpPost("archive/{id}")]
        public async Task<ActionResult> ArchiveSalaryReview(int id)
        {
            await _salaryReviewHistoryService.ArchiveSalaryReview(id);
            return NoContent();
        }

        [HttpGet("history/{personId}")]
        public async Task<ActionResult<IEnumerable<SalaryReviewHistoryReadDto>>> GetSalaryReviewHistoryByPersonId(int personId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.Name)?.Value ?? "0");
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userStream = User.FindFirst("Stream")?.Value;

            var currentUser = new User
            {
                Id = userId,
                Role = userRole,
                Stream = userStream
            };
            if (!_authorizationHelper.CanUserAccessData(currentUser, personId))
                return Forbid();

            var history = await _salaryReviewHistoryService.GetSalaryReviewHistoryByPersonId(personId);
            return Ok(history);
        }
    }
}
