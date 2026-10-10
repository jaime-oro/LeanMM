namespace LeanMM.Models
{
    public class MMElement
    {
        public string? CLASS_NAME { get; set; }
        public string? INFOS { get; set; }
        public string? ELID { get; set; }
        public string? ALID { get; set; }

        public string? UNIQUE_ID_STRING { get; set; }

        public string Name 
        { 
            get
            {
                if (string.IsNullOrEmpty(UNIQUE_ID_STRING)) return string.Empty;
                return UNIQUE_ID_STRING.Split('|')[0];
            }
        }
        public int Version
        {
            get
            {
                int version = 0;
                if (string.IsNullOrEmpty(UNIQUE_ID_STRING)) return 0;
                string[] parts = UNIQUE_ID_STRING.Split('|');

                if (parts.Length > 1)
                {
                    int.TryParse(parts[1].Trim(), out version);
                }

                return version;
            }
        }
        public decimal Quantity { get; set; } = 0;
        public bool IsLatestVersion { get; set; } = true;
        public bool HasChildren { get; set; } = false;
        public int Level { get; set; } = 0;
        public bool IsExpanded { get; set; } = false;

        public string ClassName { get => CLASS_NAME?.Trim() ?? string.Empty; }

        public List<MMElement> Children { get; set; } = new();

    }
}
