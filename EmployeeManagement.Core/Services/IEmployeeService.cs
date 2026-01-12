using EmployeeManagement.Core.DTOs;

namespace EmployeeManagement.Core.Services
{
    public interface IEmployeeService
    {
        Task<EmployeeDto?> GetByIdAsync(int id);
        Task<List<EmployeeListDto>> GetAllAsync();
        Task<PagedResponseDto<EmployeeListDto>> GetPagedAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string? department = null,
            string? searchTerm = null);

        Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto);
        Task UpdateAsync(int id, UpdateEmployeeDto dto);
        Task DeleteAsync(int id);
        Task<List<EmployeeListDto>> GetByDepartmentAsync(string department);
    }
}
