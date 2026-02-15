using Microsoft.AspNetCore.Mvc;
using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Services;
using StaffingPortalBackend.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Security.Claims;
using StaffingPortalBackend.DTO.Onboarding;

namespace StaffingPortalBackend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly IPersonService _personService;
        private readonly IMapper _mapper;
        private readonly AuthorizationHelper _authorizationHelper;
        private readonly IOnboardingService _onboardingService;

        public PeopleController(IPersonService personService, IMapper mapper, AuthorizationHelper authorizationHelper, IOnboardingService onboardingService)
        {
            _personService = personService;
            _mapper = mapper;
            _authorizationHelper = authorizationHelper;
            _onboardingService = onboardingService;
        }

        private User CurrentUser
        {
            get
            {
                var currentUser = new User
                {
                    Username = string.Empty,
                    PasswordHash = Array.Empty<byte>(),
                    PasswordSalt = Array.Empty<byte>(),
                    Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
                    Stream = User.FindFirst("Stream")?.Value ?? string.Empty,
                    ManagedUsers = new List<User>()
                };

                if (int.TryParse(User.FindFirst(ClaimTypes.Name)?.Value, out var userId))
                {
                    currentUser.Id = userId;
                }

                if (int.TryParse(User.FindFirst("PersonId")?.Value, out var personId))
                {
                    currentUser.PersonId = personId;
                }

                return currentUser;
            }
        }

        private static bool HasFullAccess(User user) => string.Equals(user.Role, "Head", StringComparison.OrdinalIgnoreCase);

        private Task<bool> HasAccessToPersonAsync(int personId) => _authorizationHelper.CanUserAccessDataAsync(CurrentUser, personId);

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonReadDto>>> GetPersons()
        {
            try
            {
                var persons = await _personService.GetAllPersons();
                var currentUser = CurrentUser;

                if (HasFullAccess(currentUser))
                {
                    return Ok(persons);
                }

                var accessibleIds = await _authorizationHelper.GetAccessiblePersonIdsAsync(currentUser);

                if (!accessibleIds.Any())
                {
                    return Ok(Array.Empty<PersonReadDto>());
                }

                var filteredPersons = persons.Where(person => accessibleIds.Contains(person.Id)).ToList();

                return Ok(filteredPersons);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error" + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonReadDto>> GetPerson(int id)
        {
            try
            {
                if (!await HasAccessToPersonAsync(id))
                {
                    return Forbid();
                }

                var person = await _personService.GetPersonById(id);
                if (person == null)
                {
                    return NotFound();
                }

                return Ok(person);
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<PersonReadDto>> CreatePerson([FromBody] PersonCreateDto personCreateDto)
        {
            if (personCreateDto == null)
            {
                return BadRequest("Person object is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var personEntity = _mapper.Map<Person>(personCreateDto);
            var serviceResponse = await _personService.CreatePerson(personEntity);

            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse.Message);
            }

            return CreatedAtAction(nameof(GetPerson), new { id = serviceResponse.Data.Id }, serviceResponse.Data);
        }

        [HttpPost("onboard")]
        public async Task<ActionResult<PersonOnboardingResponseDto>> OnboardPerson([FromBody] PersonOnboardingRequestDto onboardingRequest)
        {
            if (!_authorizationHelper.IsAuthorizedForAdminOperations(User))
            {
                return Forbid();
            }

            if (onboardingRequest == null)
            {
                return BadRequest("Onboarding request is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _onboardingService.OnboardPersonAsync(onboardingRequest);
                return CreatedAtAction(nameof(GetPerson), new { id = result.Person.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(int id, [FromBody] PersonUpdateDto personUpdateDto)
        {
            if (personUpdateDto == null)
            {
                return BadRequest("Person object is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (!await HasAccessToPersonAsync(id))
                {
                    return Forbid();
                }

                var existingPerson = await _personService.GetPersonById(id);
                if (existingPerson == null)
                {
                    return NotFound();
                }

                await _personService.UpdatePerson(personUpdateDto);
                return NoContent();
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            try
            {
                if (!await HasAccessToPersonAsync(id))
                {
                    return Forbid();
                }

                var person = await _personService.GetPersonById(id);
                if (person == null)
                {
                    return NotFound();
                }

                await _personService.DeletePerson(id);

                return NoContent();
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}/dismiss")]
        public async Task<IActionResult> DismissPerson(int id, [FromBody] PersonDismissDto dismissDto)
        {
            try
            {
                if (!await HasAccessToPersonAsync(id))
                {
                    return Forbid();
                }

                var person = await _personService.GetPersonById(id);
                if (person == null)
                {
                    return NotFound();
                }

                await _personService.DismissPerson(id, dismissDto.EndDate);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}/updateLevel")]
        public async Task<IActionResult> UpdatePersonLevel(int id, [FromBody] UpdateLevelDto updateLevelDto)
        {
            if (updateLevelDto == null || string.IsNullOrEmpty(updateLevelDto.NewLevel) || string.IsNullOrEmpty(updateLevelDto.Justification))
            {
                return BadRequest("Invalid input");
            }

            try
            {
                if (!await HasAccessToPersonAsync(id))
                {
                    return Forbid();
                }

                var response = await _personService.UpdatePersonLevel(id, updateLevelDto.NewLevel, updateLevelDto.Justification);

                if (!response.Success)
                {
                    return BadRequest(response.Message);
                }

                return Ok(response.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}/titleHistory")]
        public async Task<ActionResult<IEnumerable<TitleHistoryDto>>> GetPersonTitleHistory(int id)
        {
            try
            {
                if (!await HasAccessToPersonAsync(id))
                {
                    return Forbid();
                }

                var titleHistory = await _personService.GetPersonTitleHistory(id);

                if (titleHistory == null)
                {
                    return NotFound("Person not found.");
                }

                return Ok(titleHistory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("{id}/titleHistory")]
        public async Task<ActionResult<TitleHistoryDto>> CreateTitleHistory(int id, [FromBody] TitleHistoryDto titleHistoryDto)
        {
            if (titleHistoryDto == null || string.IsNullOrEmpty(titleHistoryDto.NewTitle) || string.IsNullOrEmpty(titleHistoryDto.Justification))
            {
                return BadRequest("Invalid input");
            }

            try
            {
                if (!await HasAccessToPersonAsync(id))
                {
                    return Forbid();
                }

                var response = await _personService.CreateTitleHistory(id, titleHistoryDto);

                if (!response.Success)
                {
                    return BadRequest(response.Message);
                }

                return CreatedAtAction(nameof(GetPersonTitleHistory), new { id = response.Data.PersonId }, response.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{personId}/titleHistory/{titleHistoryId}")]
        public async Task<IActionResult> UpdateTitleHistory(int personId, int titleHistoryId, [FromBody] TitleHistoryDto titleHistoryDto)
        {
            if (titleHistoryDto == null || string.IsNullOrEmpty(titleHistoryDto.NewTitle) || string.IsNullOrEmpty(titleHistoryDto.Justification))
            {
                return BadRequest("Invalid input");
            }

            try
            {
                if (!await HasAccessToPersonAsync(personId))
                {
                    return Forbid();
                }

                var response = await _personService.UpdateTitleHistory(personId, titleHistoryId, titleHistoryDto);

                if (!response.Success)
                {
                    return BadRequest(response.Message);
                }

                return Ok(response.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{personId}/titleHistory/{titleHistoryId}")]
        public async Task<IActionResult> DeleteTitleHistory(int personId, int titleHistoryId)
        {
            try
            {
                if (!await HasAccessToPersonAsync(personId))
                {
                    return Forbid();
                }

                var response = await _personService.DeleteTitleHistory(personId, titleHistoryId);

                if (!response.Success)
                {
                    return BadRequest(response.Message);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
