using Logic;
using Microsoft.EntityFrameworkCore;
using Models;
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
        WorkScheduleViewModel GetWorkSchedule(Guid id);
        /// <summary>
        /// 取得 資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        WorkScheduleViewModel GetWorkSchedule(DateTime workDateTime);
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
        /// <summary>
        /// 匯出 行程(當日)
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <param name="needYesterday"></param>
        /// <returns></returns>
        ExportWorkSchedule ExportScheduleDaily(Guid scheduleId, bool needYesterday = false);
        /// <summary>
        /// 匯出 行程(選擇區間)
        /// </summary>
        /// <param name="dateTimeStart"></param>
        /// <param name="dateTimeEnd"></param>
        /// <returns></returns>
        ExportWorkSchedule ExportSchedulePeriod(DateTime dateTimeStart, DateTime dateTimeEnd);
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
                WorkDateTime = viewModel.Schedule.WorkDateTimeString.RocShortToDateTime(),
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
                schedule.WorkDateTime = viewModel.Schedule.WorkDateTimeString.RocShortToDateTime();
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
                    ScheduleId = viewModel.Schedule.Id,
                    WorkItem = item.WorkItem,
                };
                db.ScheduleItems.Add(detail);
            }

            var result = DbSaveChanges();
            return result;
        }

        public WorkScheduleViewModel GetWorkSchedule(Guid id)
        {
            var data = new WorkScheduleViewModel();
            var schedule = db.Schedules
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
            if (schedule != null)
            {
                schedule.WorkDateTimeString = schedule.WorkDateTime.ToRocShortDataTime();
                schedule.CreateDateTimeString = schedule.CreateDateTime.ToRocShortDataTime();
                schedule.UpdateDateTimeString = schedule.UpdateDateTime.ToRocShortDataTime();
            }

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

            data.Schedule = schedule ?? new ScheduleViewModel();
            data.ScheduleItems = scheduleItems;

            return data;
        }

        public WorkScheduleViewModel GetWorkSchedule(DateTime workDateTime)
        {
            var data = new WorkScheduleViewModel();
            var schedule = new ScheduleViewModel();
            var scheduleItems = new List<ScheduleItemViewModel>();

            schedule = db.Schedules
                .Where(x => x.WorkDateTime == workDateTime && !x.IsDelete)
                .Select(x => new ScheduleViewModel
                {
                    Id = x.Id,
                    Subject = x.Subject,
                    WorkDateTime = x.WorkDateTime,
                    CreateDateTime = x.CreateDateTime,
                    UpdateDateTime = x.UpdateDateTime,
                })
                .FirstOrDefault();

            if (schedule != null)
            {
                schedule.WorkDateTimeString = schedule.WorkDateTime.ToRocShortDataTime();
                schedule.CreateDateTimeString = schedule.CreateDateTime.ToRocShortDataTime();
                schedule.UpdateDateTimeString = schedule.UpdateDateTime.ToRocShortDataTime();

                scheduleItems = db.ScheduleItems
                .Where(x => x.ScheduleId == schedule.Id)
                .Select(x => new ScheduleItemViewModel
                {
                    Id = x.Id,
                    ScheduleId = x.ScheduleId,
                    WorkDuration = x.WorkDuration,
                    WorkItem = x.WorkItem,
                })
                .ToList();
            }

            data.Schedule = schedule ?? new ScheduleViewModel();
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
                viewModle.UpdateDateTimeString = model.UpdateDateTime.ToRocShortDataTime();
                viewModle.WorkDateTimeString = model.WorkDateTime.ToRocShortDataTime();
            }
            else if (obj1 is ScheduleViewModel viewModel2 && obj2 is Schedule model2)
            {
                model2.Id = viewModel2.Id;
                model2.Subject = viewModel2.Subject;
                model2.CreateDateTime = viewModel2.CreateDateTimeString.RocShortToDateTime();
                model2.UpdateDateTime = viewModel2.UpdateDateTimeString.RocShortToDateTime();
                model2.WorkDateTime = viewModel2.WorkDateTimeString.RocShortToDateTime();
            }

            return obj2;
        }

        public ExportWorkSchedule ExportScheduleDaily(Guid scheduleId, bool needYesterday = false)
        {
            var exportData = new ExportWorkSchedule();

            var currentSchedule = db.Schedules.FirstOrDefault(x => x.Id == scheduleId && !x.IsDelete);
            if (currentSchedule != null)
            {
                // 今日行程 + 昨日行程
                if (needYesterday)
                {
                    exportData = ExportSchedulePeriod(currentSchedule.WorkDateTime.AddDays(-1), currentSchedule.WorkDateTime);
                }
                // 今日行程
                else
                {
                    exportData = ExportSchedulePeriod(currentSchedule.WorkDateTime, currentSchedule.WorkDateTime);
                }
            }


            return exportData;
        }

        public ExportWorkSchedule ExportSchedulePeriod(DateTime dateTimeStart, DateTime dateTimeEnd)
        {
            var exportData = new ExportWorkSchedule();

            var currentDateTime = dateTimeStart;
            while (DateTime.Compare(currentDateTime, dateTimeEnd) <= 0)
            {
                var currentSchedule = GetWorkSchedule(currentDateTime);
                if (currentSchedule != null && currentSchedule.Schedule != null)
                {
                    exportData.WorkSchedules.Add(currentSchedule);
                }
            }

            return exportData;
        }
    }
}
