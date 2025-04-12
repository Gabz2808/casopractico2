using System;
using System.Web.Mvc;

namespace CAAP2.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}

