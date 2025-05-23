﻿using System.Text.Json.Serialization;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
      
        public string Name { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public ICollection<AttendancePunch> AttendancePunches { get; set; }

    }
}
