using System.Web.Mvc;

namespace HojasDeVida.Web.Controllers
{
    public class AboutController : HojasDeVidaControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}