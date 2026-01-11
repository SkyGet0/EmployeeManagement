using System;
using System.Collections.Generic;
using System.Text;

using AutoMapper;
using EmployeeManagement.Core.DTOs;
using EmployeeManagement.Core.Models;
using EmployeeManagement.Core.Repositories;
using EmployeeManagement.Core.Services;

namespace EmployeeManagement.Api.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<EmployeeDto?> GetByIdAsync(int id)
        {
            var employee = await _repository.GetByIdAsync(id);
            return employee == null ? null : _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<List<EmployeeListDto>> GetAllAsync()
        {
            var employees = await _repository.GetAllAsync();
            return _mapper.Map<List<EmployeeListDto>>(employees);
        }

        public async Task<PagedResponseDto<EmployeeListDto>> GetPagedAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string? department = null,
            string? searchTerm = null)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var (items, totalCount) = await _repository.GetPagedAsync(pageNumber, pageSize, department, searchTerm);

            return new PagedResponseDto<EmployeeListDto>
            {
                Data = _mapper.Map<List<EmployeeListDto>>(items),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto)
        {
            // Проверка дублирования email
            if (await _repository.EmailExistsAsync(dto.Email))
                throw new InvalidOperationException($"Email {dto.Email} already exists");

            var employee = _mapper.Map<Employee>(dto);
            employee.HireDate = DateTime.UtcNow;

            var createdEmployee = await _repository.AddAsync(employee);
            return _mapper.Map<EmployeeDto>(createdEmployee);
        }

        public async Task UpdateAsync(int id, UpdateEmployeeDto dto)
        {
            var employee = await _repository.GetByIdAsync(id);
            if (employee == null)
                throw new KeyNotFoundException($"Employee with id {id} not found");

            // Проверка дублирования email
            if (await _repository.EmailExistsAsync(dto.Email, id))
                throw new InvalidOperationException($"Email {dto.Email} already exists");

            _mapper.Map(dto, employee);
            await _repository.UpdateAsync(employee);
        }

        public async Task DeleteAsync(int id)
        {
            if (!await _repository.ExistsAsync(id))
                throw new KeyNotFoundException($"Employee with id {id} not found");

            await _repository.DeleteAsync(id);
        }

        public async Task<List<EmployeeListDto>> GetByDepartmentAsync(string department)
        {
            var employees = await _repository.GetByDepartmentAsync(department);
            return _mapper.Map<List<EmployeeListDto>>(employees);
        }
    }
}