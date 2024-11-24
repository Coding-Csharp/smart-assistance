namespace SmartAssistance.Models
{
    public class Assist
    {
        public int Id { get; set; }
        public string EmployeesId { get; set; } = null!;
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public int? MinuteLate { get; set; }
        public string EmotionalState { get; set; } = null!;

        public virtual Employee Employee { get; } = null!;

        public Assist()
        {
            this.EmployeesId = string.Empty;
            this.CheckIn = null;
            this.CheckOut = null;
            this.MinuteLate = null;
            this.EmotionalState = string.Empty;
        }
        public Assist
            (string employeeId, DateTime? checkIn,
            DateTime? checkOut, int? minuteLate,
            string emotionalState)
        {
            this.EmployeesId = employeeId;
            this.CheckIn = checkIn;
            this.CheckOut = checkOut;
            this.MinuteLate = minuteLate;
            this.EmotionalState = emotionalState;
        }
    }
}