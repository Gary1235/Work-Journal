using System;
using System.Collections.Generic;

namespace Work_Journal;

public partial class ScheduleItem
{
    public Guid Id { get; set; }

    public Guid ScheduleId { get; set; }

    public string? WorkItem { get; set; }

    public int WorkDuration { get; set; }
}
