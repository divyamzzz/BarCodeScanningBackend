using System;
using System.Collections.Generic;

namespace SuperCircle.Models;

public partial class ProductionOrderHeader
{
    public int Id { get; set; }

    public string RouteCardNo { get; set; } = null!;

    public string? RefNo { get; set; }

    public string? CustomerPartNo { get; set; }

    public DateOnly IssueDate { get; set; }

    public string? BackPlateNo { get; set; }

    public string? Formulation { get; set; }

    public decimal OrderQty { get; set; }

    public decimal? PlanningQty { get; set; }

    public DateOnly? ClosingDate { get; set; }
    public string? HeaderWareHouse { get; set; }

    public string? headerItem { get; set; }
}
