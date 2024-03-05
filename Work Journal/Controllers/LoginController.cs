using Microsoft.AspNetCore.Mvc;

namespace Work_Journal.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
