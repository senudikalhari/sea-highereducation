using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlindMatchPAS_Final.Controllers
{
    [Authorize] 
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}