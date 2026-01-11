using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Core.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<Employee> Employees { get; set; } = new();
    }
}
