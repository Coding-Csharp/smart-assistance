namespace SmartAssistance.Models
{
    public class Assign
    {
        public int Id { get; set; }
        public string EmployeesId { get; set; } = null!;
        public int PositionsId { get; set; }

        public virtual Employee Employee { get; } = null!;
        public virtual Position Position { get; } = null!;

        public Assign()
        {
            this.EmployeesId = string.Empty;
            this.PositionsId = 0;
        }
        public Assign
            (string employeeId, int positionId)
        {
            this.EmployeesId = employeeId;
            this.PositionsId = positionId;
        }
    }
}