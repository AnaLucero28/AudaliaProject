using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataHUBWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {        
        public ActionResult Index()
        {
            return View();                        
        }
    }
}