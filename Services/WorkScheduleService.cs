using Logic;
using Models.Models;
using Models.ViewModel.ScheduleManage;
using PagedList;

namespace Services
{
    public interface IWrokScheduleService
    {
        WorkScheduleList GetList();

        ScheduleItemViewModel GetData(Guid id);
    }


    public class WorkScheduleService : IWrokScheduleService
    {
        public WorkScheduleList GetList()
        {
            var list = new WorkScheduleList();

            // todo 實作邏輯
            using (var db = new WorkJournalContext())
            {
                var query = db.Schedules.Where(x => !x.IsDelete);
                var test = query.ToPagedList(1, 10);
            }

            return list;
        }

        public ScheduleItemViewModel GetData(Guid id)
        {
            var data = new ScheduleItemViewModel();

            // todo 實作邏輯

            return data;
        }

        private ScheduleViewModel Mapping(Schedule model)
        {

            var viewModel = new ScheduleViewModel
            {
                Id = model.Id,
                Subject = model.Subject,
                CreateDateTime = model.CreateDateTime.ToRocShortDataTime(),
                UpdateDateTime = model.UpdateDateTime?.ToRocShortDataTime(),
                WorkDateTime = model.WorkDateTime?.ToRocShortDataTime(),
            };

            return viewModel;

        }
    }
}
