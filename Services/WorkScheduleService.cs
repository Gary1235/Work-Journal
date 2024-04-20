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
        WorkSchedulePageList GetList(ScheduleSearchViewModel search);
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
        /// 取得 有日誌的年份
        /// </summary>
        /// <returns></returns>
        List<int> GetYearOfHasWork();
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
        /// 刪除 工作行程
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        SaveChangesResult DeleteWorkSchedule(Guid id);
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
        ExportWorkSchedule ExportSchedulePeriod(DateTime? dateTimeStart, DateTime? dateTimeEnd);
    }


    public class WorkScheduleService : BaseService, IWorkScheduleService
    {
        public WorkScheduleService(WorkJournalContext dbContext) : base(dbContext)
        {

        }

        public WorkSchedulePageList GetList(ScheduleSearchViewModel search)
        {
            var model = new WorkSchedulePageList();
            IPagedList<Schedule> pageList;

            var query = db.Schedules.Where(x => !x.IsDelete).AsNoTracking();

            if (!string.IsNullOrEmpty(search.Keyword))
            {
                query = query.Where(x => x.Subject.Contains(search.Keyword));
            }
            if (search.Year > 0)
            {
                query = query.Where(x => x.WorkDateTime.Year == search.Year);
            }
            if (search.Month > 0)
            {
                query = query.Where(x => x.WorkDateTime.Month == search.Month);
            }

            pageList = query
                .OrderByDescending(x => x.WorkDateTime)
                .ToPagedList(search.CurrentPage, search.PageSize);

            model.List = MappingList(pageList.ToList(), new List<ScheduleViewModel>());
            model.CurrentPage = search.CurrentPage;
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
                WorkDateTime = viewModel.Schedule.WorkDateTimeString.ToDateTime(),
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

            var result = Save();
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
                schedule.WorkDateTime = viewModel.Schedule.WorkDateTimeString.ToDateTime();
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

            var result = Save();
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
                schedule.WorkDateTimeString = schedule.WorkDateTime.ToString("yyyy-MM-dd");
                schedule.CreateDateTimeString = schedule.CreateDateTime.ToString("yyyy-MM-dd");
                schedule.UpdateDateTimeString = schedule.UpdateDateTime?.ToString("yyyy-MM-dd");
            }

            var scheduleItems = db.ScheduleItems
                .OrderBy(x => x.WorkItem)
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
                schedule.WorkDateTimeString = schedule.WorkDateTime.ToString("yyyy-MM-dd");
                schedule.CreateDateTimeString = schedule.CreateDateTime.ToString("yyyy-MM-dd");
                schedule.UpdateDateTimeString = schedule.UpdateDateTime?.ToString("yyyy-MM-dd");

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

            data.Schedule = schedule;
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
                viewModle.CreateDateTimeString = model.CreateDateTime.ToString("yyyy-MM-dd");
                viewModle.UpdateDateTimeString = model.UpdateDateTime?.ToString("yyyy-MM-dd");
                viewModle.WorkDateTimeString = model.WorkDateTime.ToString("yyyy-MM-dd");
            }
            else if (obj1 is ScheduleViewModel viewModel2 && obj2 is Schedule model2)
            {
                model2.Id = viewModel2.Id;
                model2.Subject = viewModel2.Subject;
                model2.CreateDateTime = viewModel2.CreateDateTimeString.ToDateTime();
                model2.UpdateDateTime = viewModel2.UpdateDateTimeString.ToDateTime();
                model2.WorkDateTime = viewModel2.WorkDateTimeString.ToDateTime();
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

        public ExportWorkSchedule ExportSchedulePeriod(DateTime? dateTimeStart, DateTime? dateTimeEnd)
        {
            var exportData = new ExportWorkSchedule();

            if (dateTimeStart != null && dateTimeEnd != null)
            {
                var currentDateTime = dateTimeStart.Value;
                while (DateTime.Compare(currentDateTime, dateTimeEnd.Value) <= 0)
                {
                    var currentSchedule = GetWorkSchedule(currentDateTime);
                    if (currentSchedule != null && currentSchedule.Schedule != null)
                    {
                        exportData.WorkSchedules.Add(currentSchedule);
                    }
                    currentDateTime = currentDateTime.AddDays(1);
                }
            }

            return exportData;
        }

        public List<int> GetYearOfHasWork()
        {
            var list = db.Schedules.Where(x => !x.IsDelete).Select(x => x.WorkDateTime.Year).Distinct().ToList();

            return list;
        }

        public SaveChangesResult DeleteWorkSchedule(Guid id)
        {
            var data = db.Schedules.Where(x => x.Id == id);
            db.Schedules.RemoveRange(data);

            return Save();
        }
    }
}
