using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperCircle.Models;

public partial class ProductionOrders
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

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
    public double ActualQuantity { get; set; }
}
