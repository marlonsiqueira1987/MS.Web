using MS.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MS.Web.UI.Controllers
{
    public class HomeController: BaseController
    {

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ViewResult Sobre()
        {
            return View();
        }
    }
}
