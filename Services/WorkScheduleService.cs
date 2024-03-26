using Logic;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Models.ViewModel;
using Models.ViewModel.ScheduleManage;
using PagedList;

namespace Services
{
    public interface IWorkScheduleService
    {
        /// <summary>
        /// 取得 列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        WorkSchedulePageList GetList(SearchModel search);
        /// <summary>
        /// 取得 資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        WorkScheduleViewModel GetWorkScheduleItems(Guid id);
        /// <summary>
        /// 新增 工作行程
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        SaveChangesResult AddWorkSchedule(WorkScheduleViewModel viewModel);
        /// <summary>
        /// 更新 工作行程
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        SaveChangesResult UpdateWorkSchedule(WorkScheduleViewModel viewModel);
    }


    public class WorkScheduleService : BaseService, IWorkScheduleService
    {
        public WorkScheduleService(WorkJournalContext dbContext) : base(dbContext)
        {

        }

        public WorkSchedulePageList GetList(SearchModel search)
        {
            var model = new WorkSchedulePageList();
            IPagedList<Schedule> pageList;

            var query = db.Schedules.Where(x => !x.IsDelete).AsNoTracking();

            if (!string.IsNullOrEmpty(search.Keyword))
            {
                query = query.Where(x => x.Subject.Contains(search.Keyword));
            }

            pageList = query.ToPagedList(search.CurrentPage, search.PageSize);

            model.List = MappingList(pageList.ToList(), new List<ScheduleViewModel>());
            model.PageCount = pageList.PageCount;
            model.PageSize = pageList.PageSize;
            model.IsFirstPage = pageList.IsFirstPage;
            model.IsLastPage = pageList.IsLastPage;
            model.HasNextPage = pageList.HasNextPage;
            model.HasPreviousPage = pageList.HasPreviousPage;

            return model;
        }

        public SaveChangesResult AddWorkSchedule(WorkScheduleViewModel viewModel)
        {
            var newScheduleId = Guid.NewGuid();
            var dateTimeNow = DateTime.Now;

            // 建立Schedule
            var model = new Schedule
            {
                Id = newScheduleId,
                Subject = viewModel.Schedule?.Subject,
                CreateDateTime = dateTimeNow,
                WorkDateTime = DateTime.TryParse(viewModel.Schedule?.WorkDateTimeString, out DateTime outputTime) ? outputTime : null,
                IsDelete = false,
            };
            db.Schedules.Add(model);

            // 建立ScheduleItem
            foreach (var item in viewModel.ScheduleItems)
            {
                var detail = new ScheduleItem
                {
                    Id = Guid.NewGuid(),
                    WorkDuration = item.WorkDuration,
                    ScheduleId = newScheduleId,
                    WorkItem = item.WorkItem,
                };
                db.ScheduleItems.Add(detail);
            }

            var result = DbSaveChanges();
            return result;
        }

        public SaveChangesResult UpdateWorkSchedule(WorkScheduleViewModel viewModel)
        {
            var dateTimeNow = DateTime.Now;

            // Update Schedule
            var schedule = db.Schedules.Where(x => x.Id == viewModel.Schedule.Id).FirstOrDefault();
            if (schedule != null)
            {
                schedule.Subject = viewModel.Schedule?.Subject;
                schedule.UpdateDateTime = dateTimeNow;
                schedule.WorkDateTime = DateTime.TryParse(viewModel.Schedule?.WorkDateTimeString, out DateTime outputDate) ? outputDate : null;
            }

            // Delete old ScheduleItem
            var oldScheduleItems = db.ScheduleItems.Where(x => x.ScheduleId == viewModel.Schedule.Id);
            db.ScheduleItems.RemoveRange(oldScheduleItems);

            // Insert new ScheduleItem
            foreach (var item in viewModel.ScheduleItems)
            {
                var detail = new ScheduleItem
                {
                    Id = Guid.NewGuid(),
                    WorkDuration = item.WorkDuration,
                    ScheduleId = item.Id,
                    WorkItem = item.WorkItem,
                };
                db.ScheduleItems.Add(detail);
            }

            var result = DbSaveChanges();
            return result;
        }

        public WorkScheduleViewModel GetWorkScheduleItems(Guid id)
        {
            var data = new WorkScheduleViewModel();
            var schedules = db.Schedules
                .Where(x => x.Id == id && !x.IsDelete)
                .Select(x => new ScheduleViewModel
                {
                    Id = x.Id,
                    Subject = x.Subject,
                    WorkDateTime = x.WorkDateTime,
                    CreateDateTime = x.CreateDateTime,
                    UpdateDateTime = x.UpdateDateTime,
                })
                .FirstOrDefault();

            var scheduleItems = db.ScheduleItems
                .Where(x => x.ScheduleId == id)
                .Select(x => new ScheduleItemViewModel
                {
                    Id = x.Id,
                    ScheduleId = x.ScheduleId,
                    WorkDuration = x.WorkDuration,
                    WorkItem = x.WorkItem,
                })
                .ToList();

            data.Schedule = schedules ?? new ScheduleViewModel();
            data.ScheduleItems = scheduleItems;

            return data;
        }

        private T2 MappingList<T1, T2>(T1 obj1, T2 obj2)
        {
            if (obj1 is List<Schedule> list && obj2 is List<ScheduleViewModel> result)
            {
                foreach (var model in list)
                {
                    result.Add(Mapping(model, new ScheduleViewModel()));
                }
            }

            if (obj1 is List<ScheduleViewModel> list2 && obj2 is List<Schedule> result2)
            {
                foreach (var model in list2)
                {
                    result2.Add(Mapping(model, new Schedule()));
                }
            }

            return obj2;
        }

        private T2 Mapping<T1, T2>(T1 obj1, T2 obj2)
        {
            if (obj1 is Schedule model && obj2 is ScheduleViewModel viewModle)
            {
                viewModle.Id = model.Id;
                viewModle.Subject = model.Subject;
                viewModle.CreateDateTimeString = model.CreateDateTime.ToRocShortDataTime();
                viewModle.UpdateDateTimeString = model.UpdateDateTime?.ToRocShortDataTime();
                viewModle.WorkDateTimeString = model.WorkDateTime?.ToRocShortDataTime();
            }
            else if (obj1 is ScheduleViewModel viewModel2 && obj2 is Schedule model2)
            {
                model2.Id = viewModel2.Id;
                model2.Subject = viewModel2.Subject;
                model2.CreateDateTime = DateTime.TryParse(viewModel2.CreateDateTimeString, out DateTime cd) ? cd : DateTime.Now;
                model2.UpdateDateTime = DateTime.TryParse(viewModel2.UpdateDateTimeString, out DateTime ud) ? ud : null;
                model2.WorkDateTime = DateTime.TryParse(viewModel2.WorkDateTimeString, out DateTime wd) ? wd : null;
            }
            
            return obj2;
        }
    }
}
