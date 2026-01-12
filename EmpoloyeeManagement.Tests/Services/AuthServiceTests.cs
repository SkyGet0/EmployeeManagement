using EmployeeManagement.Api.Services;
using EmployeeManagement.Core.DTOs;
using EmployeeManagement.Core.Models;
using EmployeeManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace EmployeeManagement.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthServiceTests()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            _context = new ApplicationDbContext(options);

            // Mock configuration
            var configBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "JwtSettings:Secret", "your-super-secret-key-min-32-characters-long-1234567890" },
                    { "JwtSettings:Issuer", "EmployeeManagementApp" },
                    { "JwtSettings:Audience", "EmployeeManagementUsers" }
                });
            _configuration = configBuilder.Build();

            _authService = new AuthService(_context, _configuration);
        }

        #region Register Tests

        [Fact]
        public async Task RegisterAsync_WithValidData_ReturnsSuccessWithToken()
        {
            // Arrange
            var registerDto = new RegisterRequestDto
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "Password123",
                ConfirmPassword = "Password123"
            };

            // Act
            var result = await _authService.RegisterAsync(registerDto);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Token);
            Assert.NotNull(result.User);
            Assert.Equal("testuser", result.User.Username);
        }

        [Fact]
        public async Task RegisterAsync_WithMismatchedPasswords_ReturnsFail()
        {
            // Arrange
            var registerDto = new RegisterRequestDto
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "Password123",
                ConfirmPassword = "Password456" // not matching
            };

            // Act
            var result = await _authService.RegisterAsync(registerDto);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("do not match", result.Message);
        }

        [Fact]
        public async Task RegisterAsync_WithDuplicateUsername_ReturnsFail()
        {
            // Arrange
            var registerDto1 = new RegisterRequestDto
            {
                Username = "testuser",
                Email = "test1@example.com",
                Password = "Password123",
                ConfirmPassword = "Password123"
            };

            var registerDto2 = new RegisterRequestDto
            {
                Username = "testuser", // existing username
                Email = "test2@example.com",
                Password = "Password123",
                ConfirmPassword = "Password123"
            };

            // Act
            await _authService.RegisterAsync(registerDto1);
            var result = await _authService.RegisterAsync(registerDto2);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("already exists", result.Message);
        }

        [Fact]
        public async Task RegisterAsync_WithDuplicateEmail_ReturnsFail()
        {
            // Arrange
            var registerDto1 = new RegisterRequestDto
            {
                Username = "user1",
                Email = "test@example.com",
                Password = "Password123",
                ConfirmPassword = "Password123"
            };

            var registerDto2 = new RegisterRequestDto
            {
                Username = "user2",
                Email = "test@example.com", // existing email
                Password = "Password123",
                ConfirmPassword = "Password123"
            };

            // Act
            await _authService.RegisterAsync(registerDto1);
            var result = await _authService.RegisterAsync(registerDto2);

            // Assert
            Assert.False(result.Success);
        }

        #endregion

        #region Login Tests

        [Fact]
        public async Task LoginAsync_WithValidCredentials_ReturnsSuccessWithToken()
        {
            // Arrange
            var registerDto = new RegisterRequestDto
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "Password123",
                ConfirmPassword = "Password123"
            };
            await _authService.RegisterAsync(registerDto);

            var loginDto = new LoginRequestDto
            {
                Username = "testuser",
                Password = "Password123"
            };

            // Act
            var result = await _authService.LoginAsync(loginDto);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Token);
            Assert.NotNull(result.User);
        }

        [Fact]
        public async Task LoginAsync_WithInvalidPassword_ReturnsFail()
        {
            // Arrange
            var registerDto = new RegisterRequestDto
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "Password123",
                ConfirmPassword = "Password123"
            };
            await _authService.RegisterAsync(registerDto);

            var loginDto = new LoginRequestDto
            {
                Username = "testuser",
                Password = "WrongPassword" // wrong password
            };

            // Act
            var result = await _authService.LoginAsync(loginDto);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task LoginAsync_WithNonexistentUser_ReturnsFail()
        {
            // Arrange
            var loginDto = new LoginRequestDto
            {
                Username = "nonexistent",
                Password = "Password123"
            };

            // Act
            var result = await _authService.LoginAsync(loginDto);

            // Assert
            Assert.False(result.Success);
        }

        #endregion
    }
}
