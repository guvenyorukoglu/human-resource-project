using humanResourceProject.Application.Services.Abstract.ICompanyServices;
using Microsoft.AspNetCore.Mvc;

namespace humanResourceProject.Presentation.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyWriteService _writeCompanyService;
        private readonly ICompanyReadService _readService;
        public IActionResult Index()
        {
            return View();
        }
    }
}
