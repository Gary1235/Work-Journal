using Logic.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel.ScheduleManage
{
    public class WorkSchedulePageList : PageListViewModel
    {
        public WorkSchedulePageList()
        {
            this.List = new List<ScheduleViewModel>();
        }

        public List<ScheduleViewModel> List { get; set; }
    }

    public class WorkScheduleViewModel
    {
        public WorkScheduleViewModel()
        {
            this.Schedule = new ScheduleViewModel();
            this.ScheduleItems = new List<ScheduleItemViewModel>();
        }

        public ScheduleViewModel? Schedule { get; set; }

        public List<ScheduleItemViewModel> ScheduleItems { get; set; }
    }

    public class WorkScheduleInputPage
    {
        public Guid? ScheduleId { get; set; }

        public ActionType ActionType { get; set; }
    }

    public class ExportWorkSchedule
    {
        public ExportWorkSchedule()
        {
            WorkScheduleDic = new Dictionary<DateTime, WorkScheduleViewModel>();
        }

        public Dictionary<DateTime, WorkScheduleViewModel> WorkScheduleDic { get; set; }
    }

    public class ScheduleViewModel
    {
        public Guid Id { get; set; }

        public string? Subject { get; set; }

        public string? WorkDateTimeString { get; set; }
        public DateTime WorkDateTime { get; set; }

        public string? CreateDateTimeString { get; set; }
        public DateTime CreateDateTime { get; set; }

        public string? UpdateDateTimeString { get; set; }
        public DateTime? UpdateDateTime { get; set; }
    }

    public class ScheduleItemViewModel
    {
        public Guid Id { get; set; }

        public Guid ScheduleId { get; set; }

        public string? WorkItem { get; set; }

        /// <summary>
        /// 工作時長(小時)
        /// </summary>
        public int WorkDuration { get; set; }
    }
}
