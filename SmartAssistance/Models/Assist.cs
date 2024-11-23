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
    }
}