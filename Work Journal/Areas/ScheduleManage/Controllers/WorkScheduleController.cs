using Microsoft.AspNetCore.Mvc;

namespace Work_Journal.Areas.ScheduleManage.Controllers
{
    [Area("ScheduleManage")]
    public class WorkScheduleController : Controller
    {
        public IActionResult Index()
        {
            return View();
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
