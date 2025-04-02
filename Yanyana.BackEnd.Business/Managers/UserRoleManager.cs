using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Yanyana.BackEnd.Business.Dtos;
using Yanyana.BackEnd.Core.Entities;
using Yanyana.BackEnd.Core.Enums;
using Yanyana.BackEnd.Data.Context;
using static Yanyana.BackEnd.Core.Enums.Enums;

namespace Yanyana.BackEnd.Business.Managers
{
    public interface IUserRoleManager
    {
        Task<User> Authenticate(string email, string password);
        Task<UserResponse> Register(RegisterRequest request);
        Task<UserResponse> UpdateUser(int userId, UpdateUserRequest request);
        Task DeleteUser(int userId);
        Task<UserResponse> GetUserById(int userId);
        Task<IEnumerable<UserResponse>> GetAllUsers();
        Task AssignRole(int userId, RoleEnum role);
    }

    public class UserRoleManager : IUserRoleManager
    {
        private readonly YanDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserRoleManager(YanDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> Authenticate(string email, string password)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);

            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password) == PasswordVerificationResult.Failed)
                return null;

            var claims = user.UserRoles.Select(ur => new Claim(ClaimTypes.Role, ur.Role.Name));

            user.Claims = claims.ToList();

            return user;
        }

        public async Task<UserResponse> Register(RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(x => x.Email == request.Email))
                throw new ApplicationException("Email already exists");

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                DateOfBirth = request.DateOfBirth,
                PasswordHash = _passwordHasher.HashPassword(null, request.Password),
                UserRoles = new List<UserRole>(),
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            };

            var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == RoleEnum.User.ToString());
            if (defaultRole != null)
            {
                user.UserRoles.Add(new UserRole { Role = defaultRole });
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return MapToUserResponse(user);
        }

        public async Task<UserResponse> UpdateUser(int userId, UpdateUserRequest request)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found");

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.DateOfBirth = request.DateOfBirth;
            user.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return MapToUserResponse(user);
        }

        public async Task DeleteUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found");

            user.IsDeleted = true;
            user.ModifiedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task<UserResponse> GetUserById(int userId)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.UserId == userId && !u.IsDeleted);

            return user == null ? null : MapToUserResponse(user);
        }

        public async Task<IEnumerable<UserResponse>> GetAllUsers()
        {
            var users = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Where(u => !u.IsDeleted)
                .ToListAsync();

            return users.Select(MapToUserResponse);
        }

        public async Task AssignRole(int userId, RoleEnum role)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null) throw new KeyNotFoundException("User not found");

            var roleEntity = await _context.Roles.FirstOrDefaultAsync(r => r.Name == role.ToString());
            if (roleEntity == null) throw new KeyNotFoundException("Role not found");

            // Check if user already has the role
            if (!user.UserRoles.Any(ur => ur.RoleId == roleEntity.RoleId))
            {
                user.UserRoles.Add(new UserRole { User = user, Role = roleEntity });
                await _context.SaveChangesAsync();
            }
        }

        private UserResponse MapToUserResponse(User user)
        {
            return new UserResponse
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList(),
                CreatedDate = user.CreatedDate,
                ModifiedDate = user.ModifiedDate
            };
        }
    }
}