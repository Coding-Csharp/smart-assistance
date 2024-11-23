namespace SmartAssistance.Models
{
    public class Position
    {
        public int Id { get; set; }
        public int AreasId { get; set; }
        public string Name { get; set; } = null!;

        public virtual Area Area { get; } = null!;

        public virtual ICollection<Assign> Assigns { get; } = [];
    }
}