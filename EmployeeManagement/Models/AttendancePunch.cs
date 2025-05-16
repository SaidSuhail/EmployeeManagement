using System.Text.Json.Serialization;

namespace EmployeeManagement.Models
{
    public class AttendancePunch
    {
        public int AttendanceId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime PunchDate { get; set; }
        public DateTime PunchInTime { get; set; }
        public DateTime PunchOutTime { get; set; }
      public AttendanceStatus Status { get; set; }
        [JsonIgnore]
        public Employee Employee { get; set; }
    } 
    public enum AttendanceStatus
        {
            Abscent = 2,Present = 1,Late = 3, EarlyLeave = 4,LateAndEarlyLeave = 5
        }
}
