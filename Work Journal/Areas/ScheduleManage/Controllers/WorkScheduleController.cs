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
        public IActionResult Input(Guid? scheduleId)
        {
            var model = new WorkScheduleInputPage() { ScheduleId = scheduleId };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetWorkScheduleItems(Guid scheduleId)
        {
            var list = _workScheduleService.GetWorkScheduleItems(scheduleId);

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
    }
}
