using Microsoft.AspNetCore.Mvc;
using StaffingPortalBackend.Services;
using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Models;
using Microsoft.AspNetCore.Authorization;
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
            if (!_authorizationHelper.IsAuthorizedForAdminOperations(User))
            {
                return Forbid();
            }

            var people = await _administrationService.GetPeopleAsync();
            return Ok(people);
        }

        [HttpPost]
        public async Task<ActionResult<PersonReadDto>> CreatePerson([FromBody] PersonCreateDto personCreateDto)
        {
            if (!_authorizationHelper.IsAuthorizedForAdminOperations(User))
            {
                return Forbid();
            }

            var createdPerson = await _administrationService.CreatePersonAsync(personCreateDto);
            return CreatedAtAction(nameof(GetPeople), new { id = createdPerson.Id }, createdPerson);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(int id, [FromBody] PersonUpdateDto personUpdateDto)
        {
            if (!_authorizationHelper.IsAuthorizedForAdminOperations(User))
            {
                return Forbid();
            }

            await _administrationService.UpdatePersonAsync(id, personUpdateDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
           if (!_authorizationHelper.IsAuthorizedForAdminOperations(User))
            {
                return Forbid();
            }

            await _administrationService.DeletePersonAsync(id);
            return NoContent();
        }

        [HttpGet("{id}/details")]
        public async Task<ActionResult<PersonAdminDto>> GetPersonDetails(int id)
        {
            try
            {
                var personDetails = await _administrationService.GetPersonAdminDetailsAsync(id);
                return Ok(personDetails);
            }
            catch (KeyNotFoundException knfException)
            {
                return NotFound(knfException.Message);
            }
            catch (Exception exception)
            {
                return StatusCode(500, "Internal server error: " + exception.Message);
            }
        }

        [HttpGet("manager/{managerId}/subordinates")]
        public async Task<ActionResult<IEnumerable<PersonAdminDto>>> GetSubordinates(int managerId)
        {
            if (!_authorizationHelper.IsAuthorizedForAdminOperations(User))
            {
                return Forbid();
            }

            var subordinates = await _administrationService.GetSubordinatesAsync(managerId);
            return Ok(subordinates);
        }

        [HttpGet("stream/{stream}")]
        public async Task<ActionResult<IEnumerable<PersonAdminDto>>> GetPeopleByStream(string stream)
        {
            if (!_authorizationHelper.IsAuthorizedForAdminOperations(User))
            {
                return Forbid();
            }

            var people = await _administrationService.GetPeopleByStreamAsync(stream);
            return Ok(people);
        }

        [HttpGet("division-managers")]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetDivisionManagers()
        {
            if (!_authorizationHelper.IsAuthorizedForAdminOperations(User))
            {
                return Forbid();
            }

            var divisionManagers = await _administrationService.GetDivisionManagersAsync();
            return Ok(divisionManagers);
        }

        [HttpGet("division-manager/{divisionManagerId}/talent-managers")]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetTalentManagersByDivisionManager(int divisionManagerId)
        {
            if (!_authorizationHelper.IsAuthorizedForAdminOperations(User))
            {
                return Forbid();
            }

            var talentManagers = await _administrationService.GetTalentManagersByDivisionManagerAsync(divisionManagerId);
            if (talentManagers == null)
            {
                return NotFound();
            }

            return Ok(talentManagers);
        }

        [HttpGet("talent-managers")]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAllTalentManagers()
        {
            if (!_authorizationHelper.IsAuthorizedForAdminOperations(User))
            {
                return Forbid();
            }

            var talentManagers = await _administrationService.GetAllTalentManagersAsync();
            return Ok(talentManagers);
        }
    }
}
