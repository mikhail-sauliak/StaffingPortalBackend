using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using StaffingPortalBackend.Models;

namespace StaffingPortalBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            try
            {
                var user = await _userService.Authenticate(userLoginDto.Username, userLoginDto.Password);

                if (user == null)
                {
                    Console.WriteLine($"Login failed for user {userLoginDto.Username}"); 
                    return BadRequest(new { message = "Username or password is incorrect" });
                }

                var token = GenerateJwtToken(user);
                Console.WriteLine($"User {userLoginDto.Username} logged in successfully");

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, "Exception occurred during login");
                throw;
            }
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("Stream", user.Stream)
            };

            if (user.PersonId.HasValue)
            {
                claims.Add(new Claim("PersonId", user.PersonId.Value.ToString()));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);  // user id from JWT token
                await _userService.ChangePassword(userId, changePasswordDto.OldPassword, changePasswordDto.NewPassword);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
