using Microsoft.AspNetCore.Mvc;
using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Services;
using StaffingPortalBackend.Models;
using AutoMapper;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAllUsers()
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

            var users = await _userService.GetAllUsers();
            var authorizedUsers = new List<UserReadDto>();

            foreach (var user in users)
            {
                if (_authorizationHelper.CanUserAccessData(currentUser, user.Id))
                {
                    authorizedUsers.Add(user);
                }
            }

            return Ok(authorizedUsers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadDto>> GetUserById(int id)
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

            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            if (!_authorizationHelper.CanUserAccessData(currentUser, id))
                return Forbid();

            return Ok(_mapper.Map<UserReadDto>(user));
        }
        
        [HttpPost("create")]
        public async Task<ActionResult<UserReadDto>> CreateUser(UserCreateDto userDto)
        {
            var salt = _userService.GenerateSalt();
            var hashedPassword = _userService.HashPassword(userDto.Password, salt);

            var userToCreate = new User
            {
                Username = userDto.Username,
                Role = userDto.Role,
                Stream = userDto.Stream,
                PersonId = userDto.PersonId,
                PasswordHash = Convert.FromBase64String(hashedPassword),
                PasswordSalt = salt
            };

            var createdUser = await _userService.CreateUser(userToCreate);
            return Ok(createdUser);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
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

            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            if (!_authorizationHelper.CanUserAccessData(currentUser, id))
                return Forbid();

            // Проверка на наличие связанных записей
            // if (_userService.HasRelatedRecords(user.Id))
            // {
            //     return BadRequest("Ошибка: Невозможно удалить пользователя, так как на него ссылаются другие записи.");
            // }

            await _userService.DeleteUser(id);
            return NoContent();
        }
    }
}
