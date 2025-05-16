using EmployeeManagement.Data;
using EmployeeManagement.Dto;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController:ControllerBase
    {
        private readonly AppdbContext _context;

        public EmployeeController(AppdbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeDto dto)
        {
            var employee = new Employee
            {
                Name = dto.Name,
                Department = dto.Department,
                Email = dto.Email
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return Ok(employee);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Employees.ToList());
        }
    }
}
