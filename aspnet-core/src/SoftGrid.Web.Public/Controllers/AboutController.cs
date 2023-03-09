using Microsoft.AspNetCore.Mvc;
using SoftGrid.Web.Controllers;

namespace SoftGrid.Web.Public.Controllers
{
    public class AboutController : SoftGridControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}