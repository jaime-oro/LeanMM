namespace LeanMM.Models
{
    public class LMMTask
    {
        public int? ID { get; set; }
        public string? USER { get; set; }
        public string? NAME { get; set; }
        public DateTime? DATE_ADDED { get; set; } = DateTime.Now;
        public DateTime? DATE_FINISHED { get; set; }
        public bool? FINISHED { get; set; } = false;
        public bool? CANCELLED { get; set; } = false;

        public List<LMMJob>? Jobs { get; set; } = new();
    }
}
