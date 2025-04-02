using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Yanyana.BackEnd.Business.Dtos;
using Yanyana.BackEnd.Business.Managers;
using Yanyana.BackEnd.Core.Entities;
using Yanyana.BackEnd.Core.Enums;

namespace Yanyana.BackEnd.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRoleManager _userRoleManager;

        public AuthController(IUserRoleManager userRoleManager)
        {
            _userRoleManager = userRoleManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            var user = await _userRoleManager.Authenticate(request.Email, request.Password);
            if (user == null) return Unauthorized();

            return new AuthResponse
            {
                UserId = user.UserId,
                Email = user.Email,
                Role = user.Role,
                Token = GenerateJwtToken(user)
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponse>> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var userResponse = await _userRoleManager.Register(request);
                return CreatedAtAction(nameof(UserController.GetUserById),
                    new { id = userResponse.UserId }, userResponse);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string GenerateJwtToken(User user)
        {
            // Implementation with role claim
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            // Add token generation logic
            return "generated-jwt-token";
        }
    }
}