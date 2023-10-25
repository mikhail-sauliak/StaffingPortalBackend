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
    public class SalaryReviewRequestsController : ControllerBase
    {
        private readonly ISalaryReviewRequestService _service;
        private readonly AuthorizationHelper _authHelper;
        private readonly AppDbContext _context;

        public SalaryReviewRequestsController(ISalaryReviewRequestService service, AuthorizationHelper authHelper, AppDbContext context)
        {
            _service = service;
            _authHelper = authHelper;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<SalaryReviewRequest>> CreateReviewRequest(SalaryReviewRequestDto requestDto)
        {
            var request = await _service.CreateReviewRequest(requestDto);
            return CreatedAtAction(nameof(GetReviewRequest), new { id = request.Id }, request);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalaryReviewRequestDto>>> GetPendingRequests()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.Name)?.Value ?? "0");
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userStream = User.FindFirst("Stream")?.Value;
            
            var user = await _context.Users.FindAsync(userId);
            if (!_authHelper.CanUserAccessSalaryReviewPage(user))
            {
                return Unauthorized();
            }

            var requestsWithReviews = await _service.GetPendingRequests(userId);
            return Ok(requestsWithReviews);
        }

        [HttpPatch("{id}/approve")]
        public async Task<ActionResult<SalaryReviewRequest>> ApproveRequest(int id, [FromBody] ManagerCommentsDto managerCommentsDto)
        {
            var request = await _service.ApproveRequest(id, managerCommentsDto.Comments);
            if (request == null)
            {
                return NotFound();
            }
            return Ok(request);
        }

        [HttpPatch("{id}/decline")]
        public async Task<ActionResult<SalaryReviewRequest>> DeclineRequest(int id, [FromBody] ManagerCommentsDto managerCommentsDto)
        {
            var request = await _service.DeclineRequest(id, managerCommentsDto.Comments);
            if (request == null)
            {
                return NotFound();
            }
            return Ok(request);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SalaryReviewRequest>> GetReviewRequest(int id)
        {
            var request = await _service.GetReviewRequest(id);
            if (request == null)
            {
                return NotFound();
            }
            return request;
        }

        [HttpPatch("{id}/updateFields")]
        public async Task<ActionResult<SalaryReviewRequest>> UpdateFields(int id, [FromBody] UpdateFieldsDto updateFieldsDto)
        {
            var request = await _service.UpdateFields(id, updateFieldsDto);
            if (request == null)
            {
                return NotFound();
            }
            return Ok(request);
        }

        [HttpPatch("{id}/approveByDM")]
        public async Task<ActionResult<SalaryReviewRequest>> ApproveByDM(int id)
        {
            var request = await _service.ApproveByDM(id);
            if (request == null)
            {
                return NotFound();
            }
            return Ok(request);
        }
    }
}
