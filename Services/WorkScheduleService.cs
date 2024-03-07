using Logic;
using Models.Models;
using Models.ViewModel;
using Models.ViewModel.ScheduleManage;
using PagedList;

namespace Services
{
    public interface IWrokScheduleService
    {
        WorkScheduleModel GetList(SearchModel search);

        ScheduleItemViewModel GetData(Guid id);

        SaveChangesResult AddWorkSchedule(ScheduleItemViewModel viewModel);
    }


    public class WorkScheduleService : IWrokScheduleService
    {
        public WorkScheduleModel GetList(SearchModel search)
        {
            var model = new WorkScheduleModel();
            IPagedList<Schedule> pageList;

            // todo 實作邏輯
            using (var db = new WorkJournalContext())
            {
                var query = db.Schedules.Where(x => !x.IsDelete);

                if (!string.IsNullOrEmpty(search.Keyword))
                {
                    query = query.Where(x => x.Subject.Contains(search.Keyword));
                }

                pageList = query.ToPagedList(search.CurrentPage, search.PageSize);
            }

            model.List = Mapping(pageList.ToList(), new List<ScheduleViewModel>());
            model.PageCount = pageList.PageCount;
            model.PageSize = pageList.PageSize;
            model.IsFirstPage = pageList.IsFirstPage;
            model.IsLastPage = pageList.IsLastPage;
            model.HasNextPage = pageList.HasNextPage;
            model.HasPreviousPage = pageList.HasPreviousPage;

            return model;
        }

        public ScheduleItemViewModel GetData(Guid id)
        {
            var data = new ScheduleItemViewModel();

            // todo 實作邏輯

            return data;
        }

        public SaveChangesResult AddWorkSchedule(ScheduleItemViewModel viewModel)
        {
            var result = new SaveChangesResult(false, "更新失敗");

            // todo 實作邏輯

            return result;
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
