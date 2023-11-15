using Microsoft.AspNetCore.Mvc;
using StaffingPortalBackend.Services;
using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;

namespace StaffingPortalBackend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ManagementHierarchyController : ControllerBase
    {
        private readonly IManagementHierarchyService _hierarchyService;

        public ManagementHierarchyController(IManagementHierarchyService hierarchyService)
        {
            _hierarchyService = hierarchyService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddSubordinate([FromBody] HierarchyModificationDto dto)
        {
            if (dto.ManagerId == dto.SubordinateId)
            {
                return BadRequest("Manager and subordinate cannot be the same.");
            }

            try
            {
                await _hierarchyService.AddSubordinate(dto.ManagerId, dto.SubordinateId);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveSubordinate([FromBody] HierarchyModificationDto dto)
        {
            try
            {
                await _hierarchyService.RemoveSubordinate(dto.ManagerId, dto.SubordinateId);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

       [HttpGet("all")]
        public async Task<IActionResult> GetAllHierarchies()
        {
            var hierarchies = await _hierarchyService.GetAllHierarchies();
            return Ok(hierarchies);
        }

        [HttpGet("{managerId}")]
        public async Task<IActionResult> GetHierarchyByManagerId(int managerId)
        {
            var hierarchy = await _hierarchyService.GetHierarchyByManagerId(managerId);
            if (hierarchy == null)
            {
                return NotFound();
            }
            return Ok(hierarchy);
        }
        
        [HttpPut("update")]
        public async Task<IActionResult> UpdateHierarchy([FromBody] ManagementHierarchy hierarchy)
        {
            try
            {
                await _hierarchyService.UpdateHierarchy(hierarchy);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}