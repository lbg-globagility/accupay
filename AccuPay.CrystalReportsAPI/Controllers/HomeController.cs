using System.Web.Mvc;

namespace AccuPay.CrystalReportsWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectPermanent("/Help");
        }
    }
}