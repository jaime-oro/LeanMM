namespace LeanMM.Models
{
    public class MMMasterData
    {
        public string? NAME { get; set; }
        public string? TYPE { get; set; }
        public string? OSM_VERSION_ID { get; set; }
        public string? INFOS { get; set; }
        public string? DESCRIPTION { get; set; }
        public string? DESCRIPTION2 { get; set; }
        public string? DESCRIPCION3 { get; set; }


        public decimal? Quantity { get; set; }
        public int Level { get; set; } = 0;
        public string? ParentInfos { get; set; }
        public List<MMMasterData> Children { get; set; } = new();

        public bool HasChildren { get; set; } = false;
        public bool IsExpanded { get; set; } = false;

        public string GetQttyString()
        {
            if (Quantity.HasValue)
            {
                return Quantity.Value.ToString("0.##");
            }
            return "-";
        }



        // DETAILS
        public string? CREATED_BY { get; set; }
        public DateOnly? CREATED_AT { get; set; }
        public string? APPROVED_BY { get; set; }
        public DateOnly? APPROVED_AT { get; set; }
        public string? RELEASED_BY { get; set; }
        public DateOnly? RELEASED_AT { get; set; }



        public string? PROCESO_BAJA { get; set; }
        public string? AFECTA_STOCK { get; set; }
        public string? REL_STOCK_COMPRA { get; set; }
        public string? AGRUP_ESTADIST { get; set; }
        public string? COSTE_PREV { get; set; }

        public string? DESC_MP { get; set; }
        public string? MATERIAL { get; set; }
        public string? DIMENSION1 { get; set; }
        public string? DIMENSION2 { get; set; }
        public string? DIMENSION3 { get; set; }
        public string? PESO { get; set; }
        public string? PESO_BRUTO { get; set; }



        public string? DESC_INGLES { get; set; }
        public string? DESC_FRANCES { get; set; }
        public string? DESC_ALEMAN { get; set; }
        public string? OBSERVACIONES1 { get; set; }
        public string? OBSERVACIONES2 { get; set; }
    }
}
