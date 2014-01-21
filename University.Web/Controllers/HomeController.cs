using System.Web.Mvc;

namespace University.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to Bates University Homepage";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "About Bates University";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact Us";

            return View();
        }
    }
}