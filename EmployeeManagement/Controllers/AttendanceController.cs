using EmployeeManagement.Dto;
using EmployeeManagement.Service;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [ApiController]
    [Route("api/punch")]
    public class AttendanceController:ControllerBase
    {
        private readonly IAttendanceService attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            this.attendanceService = attendanceService;
        }

        [HttpPost("{employeeId}")]
        public async Task<IActionResult>Punch(int employeeId)
        {
            try
            {
                var result = await attendanceService.PunchAsync(employeeId);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("today/{employeeId}")]
        public async Task<IActionResult> GetTodayAttendance(int employeeId)
        {
            var attendance = await attendanceService.GetTodayAttendanceAsync(employeeId);
            if (attendance == null)
                return NotFound("No attendance record found for today.");
            return Ok(attendance);
        }

        [HttpGet("monthly/{employeeId}")]
        public async Task<IActionResult> GetMonthlyAttendance(int employeeId, [FromQuery] int month, [FromQuery] int year)
        {
            var records = await attendanceService.GetMonthlyAttendanceAsync(employeeId, month, year);
            if (records == null || records.Count == 0)
                return NotFound("No attendance records found for the specified month.");

            return Ok(records);
        }
        [HttpPut("manual-entry")]
        public async Task<IActionResult> ManualEntry([FromBody] AttendanceManualEntryDTO dto)
        {
            var attendance = await attendanceService.ManualEntryAsync(dto);
            return Ok(attendance);
        }

        [HttpGet("absent")]
        public async Task<IActionResult> GetAbsentEmployees([FromQuery] DateTime date)
        {
            var absentEmployees = await attendanceService.GetAbsentEmployeesAsync(date);
            return Ok(absentEmployees);
        }

        [HttpGet("summary/{employeeId}")]
        public async Task<IActionResult> GetMonthlySummary(int employeeId, [FromQuery] int month, [FromQuery] int year)
        {
            var result = await attendanceService.GetMonthlySummaryAsync(employeeId, month, year);
            return Ok(result);
        }



    }
}
