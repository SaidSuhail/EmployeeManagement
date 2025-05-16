using EmployeeManagement.Data;
using EmployeeManagement.Dto;
using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Service
{
    public class AttendanceService:IAttendanceService
    {
        private readonly AppdbContext _context;
        public AttendanceService(AppdbContext context)
        {
            _context = context;
        }
        public async Task<AttendancePunch>PunchAsync(int employeeId)
        {
            var today = DateTime.Today;

            var attendance = await _context.AttendancePunches.FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.PunchDate == today);
            var now = DateTime.Now;
            if(attendance == null)
            {
                attendance = new AttendancePunch
                {
                    EmployeeId = employeeId,
                    PunchDate = today,
                    PunchInTime = now,
                    Status = AttendanceStatus.Present
                };

                if (now.TimeOfDay > new TimeSpan(9, 15, 0))
                    attendance.Status = AttendanceStatus.Late;

                _context.AttendancePunches.Add(attendance);
                await _context.SaveChangesAsync();
                return attendance;
                
            }
            else
            {
                if(attendance.PunchOutTime == default)
                {
                    attendance.PunchOutTime = now;

                    if(now.TimeOfDay<new TimeSpan(17,0,0))
                    {
                        if (attendance.Status == AttendanceStatus.Late)
                            attendance.Status = AttendanceStatus.LateAndEarlyLeave;
                        else
                            attendance.Status = AttendanceStatus.EarlyLeave;
                    }
                    await _context.SaveChangesAsync();
                    return attendance;
                }
                else
                {
                    throw new InvalidOperationException("Already punched");
                }
            }
        }

        public async Task<AttendancePunch?> GetTodayAttendanceAsync(int employeeId)
        {
            var today = DateTime.Today;
            return await _context.AttendancePunches
                .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.PunchDate == today);
        }

        public async Task<List<AttendancePunch>> GetMonthlyAttendanceAsync(int employeeId, int month, int year)
        {
            return await _context.AttendancePunches
                .Where(a => a.EmployeeId == employeeId
                    && a.PunchDate.Month == month
                    && a.PunchDate.Year == year)
            .ToListAsync();
        }

        public async Task<AttendancePunch> ManualEntryAsync(AttendanceManualEntryDTO dto)
        {
            var attendance = await _context.AttendancePunches
                .Include(a=>a.Employee)
                .FirstOrDefaultAsync(a => a.EmployeeId == dto.EmployeeId && a.PunchDate.Date == dto.PunchDate.Date);

            if (attendance == null)
            {
                throw new InvalidOperationException("Attendance record not found for the given date.");
            }

            attendance.PunchInTime = dto.PunchInTime;
            attendance.PunchOutTime = dto.PunchOutTime;
            attendance.Status = dto.Status;

            await _context.SaveChangesAsync();
            return attendance;
        }


        public async Task<List<Employee>> GetAbsentEmployeesAsync(DateTime date)
        {
            var absentEmployeeIds = await _context.AttendancePunches
                .Where(a => a.PunchDate == date && a.Status == AttendanceStatus.Abscent)
                .Select(a => a.EmployeeId)
                .ToListAsync();

            var absentEmployees = await _context.Employees
                .Where(e => absentEmployeeIds.Contains(e.EmployeeId))
                .ToListAsync();

            return absentEmployees;
        }

        public async Task<object> GetMonthlySummaryAsync(int employeeId, int month, int year)
        {
            var attendanceRecords = await _context.AttendancePunches
                .Where(a => a.EmployeeId == employeeId &&
                            a.PunchDate.Month == month &&
                            a.PunchDate.Year == year)
                .ToListAsync();

            var summary = new
            {
                TotalPresent = attendanceRecords.Count(a => a.Status == AttendanceStatus.Present),
                TotalAbsent = attendanceRecords.Count(a => a.Status == AttendanceStatus.Abscent),
                TotalLate = attendanceRecords.Count(a => a.Status == AttendanceStatus.Late),
                TotalEarlyLeave = attendanceRecords.Count(a => a.Status == AttendanceStatus.EarlyLeave),
                TotalLateAndEarlyLeave = attendanceRecords.Count(a => a.Status == AttendanceStatus.LateAndEarlyLeave)
            };

            return summary;
        }


    }
}
