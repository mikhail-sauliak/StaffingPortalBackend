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
    [Route("api/[controller]")]
    public class AdministrationController : ControllerBase
    {
        private readonly IAdministrationService _administrationService;
        private readonly AuthorizationHelper _authorizationHelper;

        public AdministrationController(
            IAdministrationService administrationService,
            AuthorizationHelper authorizationHelper)
        {
            _administrationService = administrationService;
            _authorizationHelper = authorizationHelper;
        }

        private User GetCurrentUser()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.Name)?.Value ?? "0");
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            return new User
            {
                Id = userId,
                Role = userRole
            };
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonAdminDto>>> GetPeople()
        {
            var currentUser = GetCurrentUser();

            if (!_authorizationHelper.CanUserAccessSalaryReviewPage(currentUser))
            {
                return Forbid();
            }

            var people = await _administrationService.GetPeopleAsync();
            return Ok(people);
        }

        [HttpPost]
        public async Task<ActionResult<PersonReadDto>> CreatePerson([FromBody] PersonCreateDto personCreateDto)
        {
            var currentUser = GetCurrentUser();

            if (!_authorizationHelper.CanUserAccessSalaryReviewPage(currentUser))
            {
                return Forbid();
            }

            var createdPerson = await _administrationService.CreatePersonAsync(personCreateDto);
            return CreatedAtAction(nameof(GetPeople), new { id = createdPerson.Id }, createdPerson);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(int id, [FromBody] PersonUpdateDto personUpdateDto)
        {
            var currentUser = GetCurrentUser();

            if (!_authorizationHelper.CanUserAccessData(currentUser, id))
            {
                return Forbid();
            }

            await _administrationService.UpdatePersonAsync(id, personUpdateDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var currentUser = GetCurrentUser();

            if (!_authorizationHelper.CanUserAccessData(currentUser, id))
            {
                return Forbid();
            }

            await _administrationService.DeletePersonAsync(id);
            return NoContent();
        }
    }
}
