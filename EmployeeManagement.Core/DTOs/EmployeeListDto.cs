using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Core.DTOs
{
    public class EmployeeListDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public string? Department { get; set; }
        public bool IsActive { get; set; }
        public DateTime HireDate { get; set; }
    }
}

