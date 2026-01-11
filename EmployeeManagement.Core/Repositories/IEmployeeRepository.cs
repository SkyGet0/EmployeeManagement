using System;
using System.Collections.Generic;
using System.Text;

using EmployeeManagement.Core.Models;

namespace EmployeeManagement.Core.Repositories
{
    public interface IEmployeeRepository
    {
        // Read
        Task<Employee?> GetByIdAsync(int id);
        Task<List<Employee>> GetAllAsync();
        Task<List<Employee>> GetByDepartmentAsync(string department);
        Task<Employee?> GetByEmailAsync(string email);

        // Search and Filter
        Task<List<Employee>> SearchAsync(string searchTerm);
        Task<List<Employee>> GetByActivStatusAsync(bool isActive);

        // Create
        Task<Employee> AddAsync(Employee employee);

        // Update
        Task UpdateAsync(Employee employee);

        // Delete
        Task DeleteAsync(int id);

        // Pagination
        Task<(List<Employee> items, int totalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? department = null,
            string? searchTerm = null);

        // Existence checks
        Task<bool> ExistsAsync(int id);
        Task<bool> EmailExistsAsync(string email, int? excludeId = null);
    }
}
