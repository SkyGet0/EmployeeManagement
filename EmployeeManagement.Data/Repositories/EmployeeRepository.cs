using System;
using System.Collections.Generic;
using System.Text;

using EmployeeManagement.Core.Models;
using EmployeeManagement.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Data.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<List<Employee>> GetByDepartmentAsync(string department)
        {
            return await _context.Employees
                .Where(e => e.Department == department)
                .ToListAsync();
        }

        public async Task<Employee?> GetByEmailAsync(string email)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<List<Employee>> SearchAsync(string searchTerm)
        {
            return await _context.Employees
                .Where(e => e.FirstName.Contains(searchTerm) ||
                           e.LastName.Contains(searchTerm) ||
                           e.Email.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<List<Employee>> GetByActivStatusAsync(bool isActive)
        {
            return await _context.Employees
                .Where(e => e.IsActive == isActive)
                .ToListAsync();
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            employee.CreatedAt = DateTime.UtcNow;
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task UpdateAsync(Employee employee)
        {
            employee.UpdatedAt = DateTime.UtcNow;
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var employee = await GetByIdAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<(List<Employee> items, int totalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? department = null,
            string? searchTerm = null)
        {
            var query = _context.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(department))
                query = query.Where(e => e.Department == department);

            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(e => e.FirstName.Contains(searchTerm) ||
                                        e.LastName.Contains(searchTerm) ||
                                        e.Email.Contains(searchTerm));

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(e => e.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Employees.AnyAsync(e => e.Id == id);
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            if (excludeId.HasValue)
                return await _context.Employees.AnyAsync(e => e.Email == email && e.Id != excludeId);

            return await _context.Employees.AnyAsync(e => e.Email == email);
        }
    }
}