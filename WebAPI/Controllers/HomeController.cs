using System.Web.Mvc;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Controller to retrieve MVC views
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Home page view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        /// <summary>
        /// Upload PDF view
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadPdf()
        {
            ViewBag.Title = "Upload PDF";

            return View();
        }
    }
}
