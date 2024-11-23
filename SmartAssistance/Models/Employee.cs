namespace SmartAssistance.Models
{
    public class Employee
    {
        public string Id { get; set; } = null!;
        public int RolesId { get; set; }
        public int SpecialtiesId { get; set; }
        public DateOnly DateEntry { get; set; }
        public string TypeDocument { get; set; } = null!;
        public string UrlImage { get; set; } = null!;
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public DateOnly Birthdate { get; set; }
        public string Nationality { get; set; } = null!;
        public string Genre { get; set; } = null!;
        public int Phone { get; set; }
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? ZoneAccess { get; set; }
        public string State { get; set; } = null!;

        public virtual EmployeeCredential? EmployeeCredential { get; }
        public virtual Role Role { get; } = null!;
        public virtual Specialty Specialty { get; } = null!;

        public virtual ICollection<Assign> Assigns { get; } = [];
        public virtual ICollection<Assist> Assists { get; } = [];
    }
}