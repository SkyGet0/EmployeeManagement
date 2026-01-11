using EmployeeManagement.Core.Models;
using EmployeeManagement.Core.Repositories;

namespace EmployeeManagement.Tests.Mocks
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employees = new();
        private int _nextId = 1;

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await Task.FromResult(_employees.FirstOrDefault(e => e.Id == id));
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await Task.FromResult(_employees);
        }

        public async Task<List<Employee>> GetByDepartmentAsync(string department)
        {
            return await Task.FromResult(
                _employees.Where(e => e.Department == department).ToList());
        }

        public async Task<Employee?> GetByEmailAsync(string email)
        {
            return await Task.FromResult(
                _employees.FirstOrDefault(e => e.Email == email));
        }

        public async Task<List<Employee>> SearchAsync(string searchTerm)
        {
            return await Task.FromResult(
                _employees.Where(e =>
                    e.FirstName.Contains(searchTerm) ||
                    e.LastName.Contains(searchTerm) ||
                    e.Email.Contains(searchTerm))
                .ToList());
        }

        public async Task<List<Employee>> GetByActivStatusAsync(bool isActive)
        {
            return await Task.FromResult(
                _employees.Where(e => e.IsActive == isActive).ToList());
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            employee.Id = _nextId++;
            employee.CreatedAt = DateTime.UtcNow;
            _employees.Add(employee);
            return await Task.FromResult(employee);
        }

        public async Task UpdateAsync(Employee employee)
        {
            var existing = _employees.FirstOrDefault(e => e.Id == employee.Id);
            if (existing != null)
            {
                var index = _employees.IndexOf(existing);
                employee.UpdatedAt = DateTime.UtcNow;
                _employees[index] = employee;
            }
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == id);
            if (employee != null)
                _employees.Remove(employee);
            await Task.CompletedTask;
        }

        public async Task<(List<Employee> items, int totalCount)> GetPagedAsync(
            int pageNumber, int pageSize, string? department = null, string? searchTerm = null)
        {
            var query = _employees.AsQueryable();

            if (!string.IsNullOrEmpty(department))
                query = query.Where(e => e.Department == department);

            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(e =>
                    e.FirstName.Contains(searchTerm) ||
                    e.LastName.Contains(searchTerm) ||
                    e.Email.Contains(searchTerm));

            var totalCount = query.Count();
            var items = query
                .OrderByDescending(e => e.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return await Task.FromResult((items, totalCount));
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await Task.FromResult(_employees.Any(e => e.Id == id));
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            if (excludeId.HasValue)
                return await Task.FromResult(
                    _employees.Any(e => e.Email == email && e.Id != excludeId));

            return await Task.FromResult(_employees.Any(e => e.Email == email));
        }
    }
}
