using System;
using System.Collections.Generic;

namespace SuperCircle.Models;

public partial class SuperCircle
{
    public byte Id { get; set; }

    public DateTime DocDate { get; set; }

    public int DocEntry { get; set; }

    public int DocNum { get; set; }

    public string HeaderItemCode { get; set; } = null!;

    public short HeaderPlannedQty { get; set; }

    public string HeaderProdName { get; set; } = null!;

    public string HeaderWarehouse { get; set; } = null!;

    public double IssuedQty { get; set; }

    public byte LineNum { get; set; }

    public string RowItemCode { get; set; } = null!;

    public double RowPlannedQty { get; set; }

    public string RowWarehouse { get; set; } = null!;

    public string SeriesName { get; set; } = null!;

    public string Status { get; set; } = null!;
}
