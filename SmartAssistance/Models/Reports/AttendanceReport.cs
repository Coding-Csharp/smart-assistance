namespace SmartAssistance.Models.Reports
{
    public class AttendanceReport
    {
        public int id { get; set; }
        public string date { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string checkIn { get; set; }
        public string checkOut { get; set; }
        public int minuteLate { get; set; }
        public string workedTime { get; set; }
        public string emotionalState { get; set; }
    }
}