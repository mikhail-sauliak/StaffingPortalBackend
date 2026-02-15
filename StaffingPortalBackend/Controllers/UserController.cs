using Microsoft.AspNetCore.Mvc;
using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Services;
using StaffingPortalBackend.Models;
using AutoMapper;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace StaffingPortalBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly AuthorizationHelper _authorizationHelper;

        public UserController(IUserService userService, IMapper mapper, AuthorizationHelper authorizationHelper)
        {
            _userService = userService;
            _mapper = mapper;
            _authorizationHelper = authorizationHelper;
        }

        private User GetCurrentUser()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.Name)?.Value ?? "0");
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userStream = User.FindFirst("Stream")?.Value;
            var personIdClaim = User.FindFirst("PersonId")?.Value;

            return new User
            {
                Id = userId,
                Role = userRole,
                Stream = userStream,
                PersonId = int.TryParse(personIdClaim, out var personId) ? personId : null
            };
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAllUsers()
        {
            var currentUser = GetCurrentUser();

            var users = await _userService.GetAllUsers();
            var authorizedUsers = new List<UserReadDto>();

            foreach (var user in users)
            {
                if (user.PersonId.HasValue && await _authorizationHelper.CanUserAccessDataAsync(currentUser, user.PersonId.Value))
                {
                    authorizedUsers.Add(user);
                }
            }

            return Ok(authorizedUsers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadDto>> GetUserById(int id)
        {
            var currentUser = GetCurrentUser();

            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            var targetPersonId = user.PersonId ?? 0;

            if (!await _authorizationHelper.CanUserAccessDataAsync(currentUser, targetPersonId))
                return Forbid();

            return Ok(user);
        }

        [HttpPost("create")]
        public async Task<ActionResult<UserReadDto>> CreateUser(UserCreateDto userDto)
        {
            var serviceResponse = await _userService.CreateUser(userDto);

            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse.Message);
            }

            return Ok(serviceResponse.Data);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var currentUser = GetCurrentUser();

            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            var targetPersonId = user.PersonId ?? 0;

            if (!await _authorizationHelper.CanUserAccessDataAsync(currentUser, targetPersonId))
                return Forbid();
            await _userService.DeleteUser(id);
            return NoContent();
        }

        [HttpGet("division-managers/{divisionManagerId}/talent-managers")]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetTalentManagersByDivisionManager(int divisionManagerId)
        {
            var currentUser = GetCurrentUser();

            if (!await _authorizationHelper.CanUserAccessDataAsync(currentUser, divisionManagerId))
            {
                return Forbid();
            }

            var talentManagers = await _userService.GetTalentManagersByDivisionManager(divisionManagerId);
            if (talentManagers == null || !talentManagers.Any())
            {
                var divisionManager = await _userService.GetUserById(divisionManagerId);
                if (divisionManager == null)
                {
                    return NotFound(new { Message = "Division Manager not found." });
                }

                return Accepted(new { Message = "No Talent Managers found. Returning Division Manager.", DivisionManager = divisionManager });
            }

            return Ok(talentManagers);
        }
    }
}
