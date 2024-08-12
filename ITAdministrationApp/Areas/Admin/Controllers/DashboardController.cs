using ITAdministrationApp.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITAdministrationApp.Areas.Dashboard.Controllers
{
    [Area("admin")]
    [Authorize]
    public class DashboardController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
