using EmployeeManagement.Dto;
using EmployeeManagement.Models;

namespace EmployeeManagement.Service
{
    public interface IAttendanceService
    {
        Task<AttendancePunch> PunchAsync(int employeeId);
        Task<AttendancePunch?> GetTodayAttendanceAsync(int employeeId);
        Task<List<AttendancePunch>> GetMonthlyAttendanceAsync(int employeeId, int month, int year);
        Task<AttendancePunch> ManualEntryAsync(AttendanceManualEntryDTO dto);
        Task<List<Employee>> GetAbsentEmployeesAsync(DateTime date);
        Task<object> GetMonthlySummaryAsync(int employeeId, int month, int year);

    }
}
