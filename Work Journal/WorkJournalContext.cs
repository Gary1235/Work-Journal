using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Work_Journal;

public partial class WorkJournalContext : DbContext
{
    public WorkJournalContext()
    {
    }

    public WorkJournalContext(DbContextOptions<WorkJournalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<ScheduleItem> ScheduleItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DB");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Schedule__3214EC07B3416E9C");

            entity.ToTable("Schedule");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDateTime).HasColumnType("datetime");
            entity.Property(e => e.Subject).HasMaxLength(20);
            entity.Property(e => e.UpdateDateTime).HasColumnType("datetime");
            entity.Property(e => e.WorkDateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<ScheduleItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Schedule__3214EC07C29AFA91");

            entity.ToTable("ScheduleItem");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.WorkItem).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
