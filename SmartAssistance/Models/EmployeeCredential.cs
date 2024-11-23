namespace SmartAssistance.Models
{
    public class EmployeeCredential
    {
        public string EmployeesId { get; set; } = null!;
        public string Code { get; set; } = null!;

        public virtual Employee Employee { get; set; } = null!;
    }
}