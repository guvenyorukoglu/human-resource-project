using Microsoft.AspNetCore.Mvc;

namespace humanResourceProject.Presentation.Controllers
{
    public class PersonnelManagerController : Controller
    {
        public async Task<IActionResult> Home()
        {
            return View();
        }

        public async Task<IActionResult> Leaves(Guid id)
        {
            return View();
        }


    }
}
