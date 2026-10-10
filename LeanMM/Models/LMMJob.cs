namespace LeanMM.Models
{
    public class LMMJob
    {
        public int? ID { get; set; }
        public int? TASK_ID { get; set; }
        public string? PHYS_NAME { get; set; }
        public string? FILE_NAME { get; set; }
        public bool? Finished { get; set; }
    }
}
