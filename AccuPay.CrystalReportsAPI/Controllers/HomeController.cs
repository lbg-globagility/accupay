using System.Web.Mvc;

namespace AccuPay.CrystalReportsAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectPermanent("/Help");
        }
    }
}