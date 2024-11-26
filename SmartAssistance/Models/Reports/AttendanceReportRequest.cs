namespace SmartAssistance.Models.Reports
{
    public class AttendanceReportRequest
    {
        public List<AttendanceReport> attendances { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }

    }
}