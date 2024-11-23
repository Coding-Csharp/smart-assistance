namespace SmartAssistance.Models
{
    public class Area
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Position> Positions { get; } = [];
    }
}