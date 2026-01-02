using System;
using System.Collections.Generic;

namespace SuperCircle.Models;

public partial class InventoryStatusMaster
{
    public int Id { get; set; }

    public string? ItemCode { get; set; }

    public decimal? ProducedQuantity { get; set; }

    public decimal? OkQuantity { get; set; }

    public decimal? NotOkQuantity { get; set; }

    public decimal? QualityQuantity { get; set; }

    public decimal? ConsumedQuantity { get; set; }

    public string ? rawItemCode { get; set; }
}
