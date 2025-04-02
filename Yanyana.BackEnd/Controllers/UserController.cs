using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Yanyana.BackEnd.Business.Dtos;
using Yanyana.BackEnd.Business.Managers;
using Yanyana.BackEnd.Core.Enums;
using static Yanyana.BackEnd.Core.Enums.Enums;

namespace Yanyana.BackEnd.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRoleManager _userRoleManager;

        public UserController(IUserRoleManager userRoleManager)
        {
            _userRoleManager = userRoleManager;
        }

        [Authorize(Roles = nameof(RoleEnum.Admin))]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetAllUsers()
        {
            return Ok(await _userRoleManager.GetAllUsers());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetUserById(int id)
        {
            var user = await _userRoleManager.GetUserById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [Authorize(Roles = nameof(RoleEnum.Admin))]
        [HttpPut("{id}/role")]
        public async Task<IActionResult> AssignRole(int id, [FromBody] AssignRoleRequest request)
        {
            await _userRoleManager.AssignRole(id, request.Role);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponse>> UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            if (id != GetCurrentUserId() && !User.IsInRole(nameof(RoleEnum.Admin)))
                return Forbid();

            return Ok(await _userRoleManager.UpdateUser(id, request));
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}