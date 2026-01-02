using System;
using System.Collections.Generic;

namespace SuperCircle.Models;

public partial class ProductionOrderStageMaster
{
    public int ?Id { get; set; }

    public string? ProductionOrderStageId { get; set; }

    public int? StageNumber { get; set; }

    public string? ProductionOrderNumber { get; set; } = null!;

    public string? ProcessOperation { get; set; } = null!;

    public DateOnly? OperationDate { get; set; }

    public string? Shift { get; set; } = null!;

    public string? MachineNo { get; set; }

    public decimal? ProducedQty { get; set; }

    public decimal ?OkQty { get; set; }

    public decimal? NotOkQty { get; set; }

    public string? DefectReason { get; set; }

    public string? ProductionOperatorName { get; set; }

    public string? ProductionSupervisorName { get; set; }

    public string? Remarks { get; set; }
    public string? StageType { get; set; }
    public string? RouteCardNo { get; set; }

    public decimal? QualityQuantity { get; set; }
}
