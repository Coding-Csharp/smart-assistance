namespace SmartAssistance.Models
{
    public class EmployeeCredential
    {
        public string EmployeesId { get; set; } = null!;
        public string Code { get; set; } = null!;

        public virtual Employee Employee { get; set; } = null!;

        public EmployeeCredential()
        {
            this.EmployeesId = string.Empty;
            this.Code = string.Empty;
        }
        public EmployeeCredential
            (string employeeId, string code)
        {
            this.EmployeesId = employeeId;
            this.Code = code;
        }
    }
}