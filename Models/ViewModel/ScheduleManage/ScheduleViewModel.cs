using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel.ScheduleManage
{
    public class WorkScheduleModel : PageListViewModel
    {
        public WorkScheduleModel()
        {
            this.List = new List<ScheduleViewModel>();
        }

        public List<ScheduleViewModel> List { get; set; }
    }

    public class ScheduleViewModel
    {
        public Guid Id { get; set; }

        public string? Subject { get; set; }

        public string? WorkDateTime { get; set; }

        public string? CreateDateTime { get; set; }

        public string? UpdateDateTime { get; set; }
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
