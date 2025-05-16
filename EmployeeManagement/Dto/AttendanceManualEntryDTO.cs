using EmployeeManagement.Models;

namespace EmployeeManagement.Dto
{
    public class AttendanceManualEntryDTO
    {
        public int EmployeeId { get; set; }
        public DateTime PunchDate { get; set; }
        public DateTime PunchInTime { get; set; }
        public DateTime PunchOutTime { get; set; }
        public AttendanceStatus Status { get; set; }
    }
}
