using EmployeeManagement.Core.Models;
using System.Security.Cryptography;
using System.Text;

namespace EmployeeManagement.Data.Seeders
{
    public static class DataSeeder
    {
        /// <summary>
        /// initializes database with ready data set
        /// </summary>
        public static void SeedData(ApplicationDbContext context)
        {
            // if database exists, then dont create
            if (context.Users.Any())
            {
                return;
            }

            // creating admin
            var adminUser = new User
            {
                Username = "admin",
                Email = "admin@example.com",
                PasswordHash = HashPassword("AdminPassword123"),
                Role = "Admin",
                CreatedAt = DateTime.UtcNow
            };

            // creating a new employee
            var employeeUser = new User
            {
                Username = "employee",
                Email = "employee@example.com",
                PasswordHash = HashPassword("EmployeePassword123"),
                Role = "Employee",
                CreatedAt = DateTime.UtcNow
            };

            context.Users.Add(adminUser);
            context.Users.Add(employeeUser);

            // creating test employees
            var employees = new List<Employee>
            {
                new Employee
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    PhoneNumber = "+1-555-0100",
                    Salary = 80000,
                    Department = "IT",
                    IsActive = true,
                    HireDate = DateTime.UtcNow.AddMonths(-12),
                    CreatedAt = DateTime.UtcNow
                },
                new Employee
                {
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    PhoneNumber = "+1-555-0101",
                    Salary = 75000,
                    Department = "HR",
                    IsActive = true,
                    HireDate = DateTime.UtcNow.AddMonths(-6),
                    CreatedAt = DateTime.UtcNow
                },
                new Employee
                {
                    FirstName = "Bob",
                    LastName = "Johnson",
                    Email = "bob.johnson@example.com",
                    PhoneNumber = "+1-555-0102",
                    Salary = 70000,
                    Department = "Sales",
                    IsActive = true,
                    HireDate = DateTime.UtcNow.AddMonths(-3),
                    CreatedAt = DateTime.UtcNow
                }
            };

            context.Employees.AddRange(employees);
            context.SaveChanges();
        }

        /// <summary>
        /// password hash through sha256
        /// </summary>
        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
