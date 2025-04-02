using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Yanyana.BackEnd.Api;
using Yanyana.BackEnd.Business.Dtos;
using Yanyana.BackEnd.Core.Entities;
using Yanyana.BackEnd.Core.Enums;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Yanyana.BackEnd.Data.Context;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using static Yanyana.BackEnd.Core.Enums.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;

namespace Yanyana.BackEnd.Tests
{
    public class UserControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly YanDbContext _context;

        public UserControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            var scope = factory.Services.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<YanDbContext>();
        }

        [Fact]
        public async Task GetAllUsers_ReturnsOkWithUsers()
        {
            // Arrange
            var adminClaim = new Claim(ClaimTypes.Role, RoleEnum.Admin.ToString());
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[] { adminClaim }, "test"));
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer test");

            // Seed test data
            var testUser = new User { Email = "test@example.com" };
            _context.Users.Add(testUser);
            _context.SaveChanges();

            // Act
            var response = await _client.GetAsync("/api/user");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<IEnumerable<UserResponse>>(responseContent);
            Assert.NotNull(users);
            Assert.Contains(users, u => u.Email == "test@example.com");
        }

        [Fact]
        public async Task GetUserById_ReturnsNotFoundForInvalidId()
        {
            // Act
            var response = await _client.GetAsync("/api/user/999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task AssignRole_ReturnsNoContentForValidRequest()
        {
            // Arrange
            var adminClaim = new Claim(ClaimTypes.Role, RoleEnum.Admin.ToString());
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[] { adminClaim }, "test"));
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer test");

            var assignRoleRequest = new AssignRoleRequest { Role = RoleEnum.Admin };
            var content = new StringContent(JsonConvert.SerializeObject(assignRoleRequest), Encoding.UTF8, "application/json");

            // Seed test user
            var testUser = new User { Email = "test@example.com" };
            _context.Users.Add(testUser);
            _context.SaveChanges();

            // Act
            var response = await _client.PutAsync($"/api/user/{testUser.UserId}/role", content);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Verify role was assigned
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.UserId == testUser.UserId);
            Assert.Contains(user.UserRoles, ur => ur.Role.Name == RoleEnum.Admin.ToString());
        }

        [Fact]
        public async Task UpdateUser_ReturnsForbidForNonAdminUpdatingOtherUser()
        {
            // Arrange
            var userClaim = new Claim(ClaimTypes.Role, RoleEnum.User.ToString());
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[] { userClaim }, "test"));
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer test");

            var updateUserRequest = new UpdateUserRequest { FirstName = "NewName" };
            var content = new StringContent(JsonConvert.SerializeObject(updateUserRequest), Encoding.UTF8, "application/json");

            // Seed test user
            var testUser = new User { Email = "test@example.com" };
            _context.Users.Add(testUser);
            _context.SaveChanges();

            // Act
            var response = await _client.PutAsync($"/api/user/{testUser.UserId}", content);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task UpdateUser_ReturnsOkForAdminOrSelfUpdate()
        {
            // Arrange for admin update
            var adminClaim = new Claim(ClaimTypes.Role, RoleEnum.Admin.ToString());
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[] { adminClaim }, "test"));
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer test");

            var updateUserRequest = new UpdateUserRequest { FirstName = "NewName" };
            var content = new StringContent(JsonConvert.SerializeObject(updateUserRequest), Encoding.UTF8, "application/json");

            // Seed test user
            var testUser = new User { Email = "test@example.com" };
            _context.Users.Add(testUser);
            _context.SaveChanges();

            // Act
            var response = await _client.PutAsync($"/api/user/{testUser.UserId}", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Verify update
            var updatedUser = await _context.Users.FindAsync(testUser.UserId);
            Assert.Equal("NewName", updatedUser.FirstName);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }

    // Custom Web Application Factory for testing
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Instead of using UseSolutionRelativeContentRoot, set an absolute content root.
            // Adjust the relative path to point to your API project folder.
            var projectDir = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\Yanyana.BackEnd");
            builder.UseContentRoot(projectDir);

            // Configure services (e.g., override the DB context)
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<YanDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<YanDbContext>(options =>
                    options.UseInMemoryDatabase("TestDb"));
            });
        }
    }


}