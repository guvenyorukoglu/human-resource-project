using Microsoft.AspNetCore.Mvc;

namespace humanResourceProject.Presentation.Controllers
{
    public class ManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Employees()
        {

            return View();
        }
    }
}
