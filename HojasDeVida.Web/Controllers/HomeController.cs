using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;

namespace HojasDeVida.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : HojasDeVidaControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}