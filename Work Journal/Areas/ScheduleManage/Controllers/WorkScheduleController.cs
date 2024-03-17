using Microsoft.AspNetCore.Mvc;
using Models.ViewModel;
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

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update()
        {
            return View();
        }
    }
}
