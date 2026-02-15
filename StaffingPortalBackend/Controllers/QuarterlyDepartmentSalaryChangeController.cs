using Microsoft.AspNetCore.Mvc;
using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace StaffingPortalBackend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class QuarterlyDepartmentSalaryChangeController : ControllerBase
    {
        private readonly IQuarterlyDepartmentSalaryChangeService _service;
        private readonly AuthorizationHelper _authorizationHelper;

        public QuarterlyDepartmentSalaryChangeController(IQuarterlyDepartmentSalaryChangeService service, AuthorizationHelper authorizationHelper)
        {
            _service = service;
            _authorizationHelper = authorizationHelper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuarterlyDepartmentSalaryChangeDto>>> GetAll()
        {
            var changes = await _service.GetAllQuarterlyChanges();
            return Ok(changes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuarterlyDepartmentSalaryChangeDto>> GetById(int id)
        {
            var change = await _service.GetQuarterlyChangeById(id);
            if (change == null)
            {
                return NotFound();
            }
            return Ok(change);
        }

        [HttpPost]
        public async Task<ActionResult<QuarterlyDepartmentSalaryChangeDto>> Create(QuarterlyDepartmentSalaryChangeDto dto)
        {
            try
            {
                var newChange = await _service.CreateQuarterlyChange(dto);
                return CreatedAtAction(nameof(GetById), new { id = newChange.Id }, newChange);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, QuarterlyDepartmentSalaryChangeDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            await _service.UpdateQuarterlyChange(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var change = await _service.GetQuarterlyChangeById(id);
            if (change == null)
            {
                return NotFound();
            }

            await _service.DeleteQuarterlyChange(id);
            return NoContent();
        }
    }
}
