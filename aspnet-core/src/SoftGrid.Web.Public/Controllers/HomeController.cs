using Microsoft.AspNetCore.Mvc;
using SoftGrid.Web.Controllers;

namespace SoftGrid.Web.Public.Controllers
{
    public class HomeController : SoftGridControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}