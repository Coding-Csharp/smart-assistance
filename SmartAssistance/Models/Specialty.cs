﻿namespace SmartAssistance.Models
{
    public class Specialty
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Employee> Employees { get; } = [];
    }
}