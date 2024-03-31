using System;
using System.Collections.Generic;

namespace Models.Models;

public partial class Schedule
{
    public Guid Id { get; set; }

    public string? Subject { get; set; }

    public DateTime WorkDateTime { get; set; }

    public DateTime CreateDateTime { get; set; }

    public DateTime? UpdateDateTime { get; set; }

    public bool IsDelete { get; set; }
}
