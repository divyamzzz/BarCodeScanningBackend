namespace SuperCircle.Models
{
    public class ProductionOrderWrapper
    {
        public List<ProductionOrderDto> SuperCircle { get; set; } = new();
    }
    public class ProductionOrderDto
    {

        public byte Id { get; set; }
        public DateTime DocDate { get; set; }
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public string HeaderItemCode { get; set; }
        public short HeaderPlannedQty { get; set; }
        public string HeaderProdName { get; set; }
        public string HeaderWarehouse { get; set; }
        public double IssuedQty { get; set; }
        public byte LineNum { get; set; }
        public string RowItemCode { get; set; }
        public double RowPlannedQty { get; set; }
        public string RowWarehouse { get; set; }
        public string SeriesName { get; set; }
        public string Status { get; set; }
    }

}
