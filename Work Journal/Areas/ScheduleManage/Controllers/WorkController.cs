using Microsoft.AspNetCore.Mvc;

namespace Work_Journal.Areas.ScheduleManage.Controllers
{
    public class WorkController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail()
        {
            return View();
        }
    }
}
