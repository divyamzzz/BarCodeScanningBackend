using System;
using System.Collections.Generic;

namespace SuperCircle.Models;

public partial class ProcessMaster
{
    public int ProcessId { get; set; }

    public string ProcessName { get; set; } = null!;

    public string ProcessCode { get; set; } = null!;

    public int NumberOfStages { get; set; }
    public bool? isActive { get; set; }
}
