using EmployeeManagement.Core.DTOs;
using EmployeeManagement.Core.Models;
using EmployeeManagement.Core.Services;
using EmployeeManagement.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace EmployeeManagement.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(dto.Username) ||
                    string.IsNullOrWhiteSpace(dto.Email) ||
                    string.IsNullOrWhiteSpace(dto.Password))
                {
                    return new AuthResponseDto { Success = false, Message = "All fields are required" };
                }

                if (dto.Password != dto.ConfirmPassword)
                {
                    return new AuthResponseDto { Success = false, Message = "Passwords do not match" };
                }

                // Check if user exists
                var existingUser = _context.Users.FirstOrDefault(u =>
                    u.Username == dto.Username || u.Email == dto.Email);

                if (existingUser != null)
                {
                    return new AuthResponseDto { Success = false, Message = "User already exists" };
                }

                // Create user
                var user = new User
                {
                    Username = dto.Username,
                    Email = dto.Email,
                    PasswordHash = HashPassword(dto.Password),
                    Role = "Employee"
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var token = GenerateJwtToken(user);

                return new AuthResponseDto
                {
                    Success = true,
                    Message = "Registration successful",
                    Token = token,
                    User = new UserDto
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Email = user.Email,
                        Role = user.Role
                    }
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDto { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
        {
            try
            {
                // Find user
                var user = _context.Users.FirstOrDefault(u => u.Username == dto.Username);
                if (user == null || !VerifyPassword(dto.Password, user.PasswordHash))
                {
                    return new AuthResponseDto { Success = false, Message = "Invalid username or password" };
                }

                if (!user.IsActive)
                {
                    return new AuthResponseDto { Success = false, Message = "User is inactive" };
                }

                var token = GenerateJwtToken(user);

                return new AuthResponseDto
                {
                    Success = true,
                    Message = "Login successful",
                    Token = token,
                    User = new UserDto
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Email = user.Email,
                        Role = user.Role
                    }
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDto { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var jwtSecret = jwtSettings["Secret"];
            var jwtIssuer = jwtSettings["Issuer"];
            var jwtAudience = jwtSettings["Audience"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: new[]
                {
                    new System.Security.Claims.Claim("sub", user.Id.ToString()),
                    new System.Security.Claims.Claim("username", user.Username),
                    new System.Security.Claims.Claim("role", user.Role)
                },
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput.Equals(hash);
        }
    }
}
