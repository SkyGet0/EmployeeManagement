using AutoMapper;
using EmployeeManagement.Api.Services;
using EmployeeManagement.Core.DTOs;
using EmployeeManagement.Core.Models;
using EmployeeManagement.Tests.Mocks;
using Xunit;

namespace EmployeeManagement.Tests.Services
{
    public class EmployeeServiceTests
    {
        private readonly IMapper _mapper;
        private readonly MockEmployeeRepository _repository;
        private readonly EmployeeService _service;

        public EmployeeServiceTests()
        {
            // Setup AutoMapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeDto>();
                cfg.CreateMap<Employee, EmployeeListDto>()
                    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
                cfg.CreateMap<CreateEmployeeDto, Employee>();
                cfg.CreateMap<UpdateEmployeeDto, Employee>();
            });

            _mapper = mapperConfig.CreateMapper();

            // Setup Repository and Service
            _repository = new MockEmployeeRepository();
            _service = new EmployeeService(_repository, _mapper);
        }

        #region Create Tests

        [Fact]
        public async Task CreateAsync_WithValidData_ReturnsEmployeeDto()
        {
            // Arrange
            var dto = new CreateEmployeeDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "+1-555-0100",
                Salary = 75000,
                Department = "IT"
            };

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dto.FirstName, result.FirstName);
            Assert.Equal(dto.Email, result.Email);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task CreateAsync_WithDuplicateEmail_ThrowsException()
        {
            // Arrange
            var dto1 = new CreateEmployeeDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Salary = 75000,
                Department = "IT"
            };

            var dto2 = new CreateEmployeeDto
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "john@example.com", // Same email!
                Salary = 80000,
                Department = "HR"
            };

            // Act & Assert
            await _service.CreateAsync(dto1);
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.CreateAsync(dto2));
            Assert.Contains("already exists", exception.Message);
        }

        #endregion

        #region Read Tests

        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsEmployee()
        {
            // Arrange
            var createDto = new CreateEmployeeDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Salary = 75000,
                Department = "IT"
            };
            var created = await _service.CreateAsync(createDto);

            // Act
            var result = await _service.GetByIdAsync(created.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(created.Id, result.Id);
            Assert.Equal("John", result.FirstName);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllEmployees()
        {
            // Arrange
            await _service.CreateAsync(new CreateEmployeeDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Salary = 75000,
                Department = "IT"
            });

            await _service.CreateAsync(new CreateEmployeeDto
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@example.com",
                Salary = 80000,
                Department = "HR"
            });

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetPagedAsync_WithValidParameters_ReturnsPaginatedData()
        {
            // Arrange
            for (int i = 0; i < 15; i++)
            {
                await _service.CreateAsync(new CreateEmployeeDto
                {
                    FirstName = $"Employee{i}",
                    LastName = "Test",
                    Email = $"emp{i}@example.com",
                    Salary = 50000 + (i * 1000),
                    Department = i % 2 == 0 ? "IT" : "HR"
                });
            }

            // Act
            var result = await _service.GetPagedAsync(pageNumber: 1, pageSize: 10);

            // Assert
            Assert.Equal(10, result.Data.Count);
            Assert.Equal(15, result.TotalCount);
            Assert.Equal(2, result.TotalPages);
        }

        [Fact]
        public async Task GetByDepartmentAsync_ReturnsDepartmentEmployees()
        {
            // Arrange
            await _service.CreateAsync(new CreateEmployeeDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Salary = 75000,
                Department = "IT"
            });

            await _service.CreateAsync(new CreateEmployeeDto
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@example.com",
                Salary = 80000,
                Department = "IT"
            });

            await _service.CreateAsync(new CreateEmployeeDto
            {
                FirstName = "Bob",
                LastName = "Johnson",
                Email = "bob@example.com",
                Salary = 70000,
                Department = "HR"
            });

            // Act
            var result = await _service.GetByDepartmentAsync("IT");

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, emp => Assert.Equal("IT", emp.Department));
        }

        #endregion

        #region Update Tests

        [Fact]
        public async Task UpdateAsync_WithValidData_UpdatesEmployee()
        {
            // Arrange
            var createDto = new CreateEmployeeDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Salary = 75000,
                Department = "IT"
            };
            var created = await _service.CreateAsync(createDto);

            var updateDto = new UpdateEmployeeDto
            {
                FirstName = "John",
                LastName = "Doe Updated",
                Email = "john.updated@example.com",
                Salary = 80000,
                Department = "IT",
                IsActive = true
            };

            // Act
            await _service.UpdateAsync(created.Id, updateDto);

            // Assert
            var updated = await _service.GetByIdAsync(created.Id);
            Assert.NotNull(updated);
            Assert.Equal("Doe Updated", updated.LastName);
            Assert.Equal(80000, updated.Salary);
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidId_ThrowsException()
        {
            // Arrange
            var updateDto = new UpdateEmployeeDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Salary = 75000
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.UpdateAsync(999, updateDto));
            Assert.Contains("not found", exception.Message);
        }

        #endregion

        #region Delete Tests

        [Fact]
        public async Task DeleteAsync_WithValidId_RemovesEmployee()
        {
            // Arrange
            var createDto = new CreateEmployeeDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Salary = 75000,
                Department = "IT"
            };
            var created = await _service.CreateAsync(createDto);

            // Act
            await _service.DeleteAsync(created.Id);

            // Assert
            var deleted = await _service.GetByIdAsync(created.Id);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task DeleteAsync_WithInvalidId_ThrowsException()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.DeleteAsync(999));
            Assert.Contains("not found", exception.Message);
        }

        #endregion

        #region Form Integration Tests

        [Fact]
        public async Task CreateAsync_FromForm_WithAllFields_SuccessfullyCreates()
        {
            // Arrange - эмулируем CreateEmployeeForm
            var createDto = new CreateEmployeeDto
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@example.com",
                PhoneNumber = "+1-555-0102",
                Salary = 85000,
                Department = "HR"
            };

            // Act
            var result = await _service.CreateAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Jane", result.FirstName);
            Assert.Equal(85000, result.Salary);
        }

        [Fact]
        public async Task UpdateAsync_FromForm_UpdatesAllFields()
        {
            // Arrange
            var createDto = new CreateEmployeeDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "+1-555-0100",
                Salary = 75000,
                Department = "IT"
            };
            var created = await _service.CreateAsync(createDto);

            var updateDto = new UpdateEmployeeDto
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@example.com",
                PhoneNumber = "+1-555-0102",
                Salary = 85000,
                Department = "HR",
                IsActive = true
            };

            // Act
            await _service.UpdateAsync(created.Id, updateDto);
            var updated = await _service.GetByIdAsync(created.Id);

            // Assert
            Assert.NotNull(updated);
            Assert.Equal("Jane", updated.FirstName);
            Assert.Equal("HR", updated.Department);
            Assert.Equal(85000, updated.Salary);
            Assert.NotNull(updated.UpdatedAt);
        }

        #endregion
    }
}
