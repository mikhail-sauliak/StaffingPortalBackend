using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Services;
using System.Threading.Tasks;
using StaffingPortalBackend.Models;
using System.Collections.Generic;
using System.Linq;

namespace StaffingPortalBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryReviewHistoryController : ControllerBase
    {
        private readonly ISalaryReviewHistoryService _salaryReviewHistoryService;

        public SalaryReviewHistoryController(ISalaryReviewHistoryService salaryReviewHistoryService)
        {
            _salaryReviewHistoryService = salaryReviewHistoryService;
        }

        // GET: api/SalaryReviewHistory
        [HttpGet]
        public async Task<IActionResult> GetAllHistory()
        {
            var historyDto = await _salaryReviewHistoryService.GetAllHistoryAsync();
            var response = new ServiceResponse<IEnumerable<SalaryReviewHistoryReadDto>>
            {
                Data = historyDto ?? Enumerable.Empty<SalaryReviewHistoryReadDto>()
            };

            return Ok(response);
        }

        // GET: api/SalaryReviewHistory/5
        [HttpGet("{personId:int}")]
        [HttpGet("person/{personId:int}")]
        public async Task<IActionResult> GetHistoryByPersonId(int personId)
        {
            var historyDto = await _salaryReviewHistoryService.GetHistoryByPersonIdAsync(personId);
            var historyList = historyDto ?? Enumerable.Empty<SalaryReviewHistoryReadDto>();

            var response = new ServiceResponse<IEnumerable<SalaryReviewHistoryReadDto>>
            {
                Data = historyList
            };

            return Ok(response);
        }

        // POST: api/SalaryReviewHistory
        [HttpPost]
        public async Task<IActionResult> CreateHistory([FromBody] SalaryReviewHistoryCreateDto historyDto)
        {
            if (!ModelState.IsValid) 
                return BadRequest(new ServiceResponse<SalaryReviewHistoryCreateDto> { Success = false, Message = "Invalid input" });

            await _salaryReviewHistoryService.CreateHistoryAsync(historyDto);
            return StatusCode(201, new ServiceResponse<SalaryReviewHistoryCreateDto> { Data = historyDto });
        }

        // PUT: api/SalaryReviewHistory/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHistory(int id, [FromBody] SalaryReviewHistoryUpdateDto historyDto)
        {
            if (id != historyDto.HistoryId) 
                return BadRequest(new ServiceResponse<SalaryReviewHistoryUpdateDto> { Success = false, Message = "ID mismatch" });
            if (!ModelState.IsValid) 
                return BadRequest(new ServiceResponse<SalaryReviewHistoryUpdateDto> { Success = false, Message = "Invalid input" });

            await _salaryReviewHistoryService.UpdateHistoryAsync(historyDto);
            return NoContent();
        }

        // DELETE: api/SalaryReviewHistory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHistory(int id)
        {
            await _salaryReviewHistoryService.DeleteHistoryAsync(id);
            return NoContent();
        }
    }
}
