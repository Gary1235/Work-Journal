﻿using Logic;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModel;
using Models.ViewModel.ScheduleManage;
using Services;

namespace Work_Journal.Areas.ScheduleManage.Controllers
{
    [Area("ScheduleManage")]
    public class WorkScheduleController : Controller
    {
        private readonly IWorkScheduleService _workScheduleService;

        public WorkScheduleController(IWorkScheduleService workScheduleService)
        {
            _workScheduleService = workScheduleService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SwitchPage(SearchModel search)
        {
            var list = _workScheduleService.GetList(search);

            return Json(list);
        }

        [HttpGet]
        public IActionResult Input(WorkScheduleInputPage model)
        {
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetWorkScheduleItems(Guid scheduleId)
        {
            var list = _workScheduleService.GetWorkSchedule(scheduleId);

            return Json(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Insert(WorkScheduleViewModel model)
        {
            var result = _workScheduleService.AddWorkSchedule(model);
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(WorkScheduleViewModel model)
        {
            var result = _workScheduleService.UpdateWorkSchedule(model);

            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExportScheduleDaily(Guid scheduleId, bool needYesterday)
        {
            var exportData = _workScheduleService.ExportScheduleDaily(scheduleId, needYesterday);

            return Json(exportData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExportSchedulePeriod(string dtSString, string dtEString)
        {
            var dtStart = dtSString.RocShortToDateTime();
            var dtEnd = dtEString.RocShortToDateTime();
            var exportData = _workScheduleService.ExportSchedulePeriod(dtStart, dtEnd);

            return Json(exportData);
        }
    }
}
