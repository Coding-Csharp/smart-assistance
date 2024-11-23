namespace SmartAssistance.Models
{
    public class Assign
    {
        public int Id { get; set; }
        public string EmployeesId { get; set; } = null!;
        public int PositionsId { get; set; }

        public virtual Employee Employee { get; } = null!;
        public virtual Position Position { get; } = null!;
    }
}