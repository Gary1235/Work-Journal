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

        private T2 Mapping<T1, T2>(T1 obj1, T2 obj2)
        {
            if (obj1 is Schedule model && obj2 is ScheduleViewModel viewModle)
            {
                viewModle.Id = model.Id;
                viewModle.Subject = model.Subject;
                viewModle.CreateDateTime = model.CreateDateTime.ToRocShortDataTime();
                viewModle.UpdateDateTime = model.UpdateDateTime?.ToRocShortDataTime();
                viewModle.WorkDateTime = model.WorkDateTime?.ToRocShortDataTime();
            }
            else if (obj1 is ScheduleViewModel viewModel2 && obj2 is Schedule model2)
            {
                model2.Id = viewModel2.Id;
                model2.Subject = viewModel2.Subject;
                model2.CreateDateTime = DateTime.TryParse(viewModel2.CreateDateTime, out DateTime cd) ? cd : DateTime.Now;
                model2.UpdateDateTime = DateTime.TryParse(viewModel2.UpdateDateTime, out DateTime ud) ? ud : null;
                model2.WorkDateTime = DateTime.TryParse(viewModel2.WorkDateTime, out DateTime wd) ? wd : null;
            }

            return obj2;

        }
    }
}
