using System;
using System.Collections.Generic;

namespace Models.Model;

public partial class ScheduleItem
{
    public Guid Id { get; set; }

    public Guid ScheduleId { get; set; }

    public string? WorkItem { get; set; }

    public int WorkDuration { get; set; }
}
