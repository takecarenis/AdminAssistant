using Microsoft.AspNetCore.Mvc;

namespace AdminAssistant.Blog.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Posts()
        {
            return View();
        }
    }
}