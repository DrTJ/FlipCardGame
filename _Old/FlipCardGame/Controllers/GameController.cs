using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FlipCardGame.Controllers
{
    public class GameController : Controller
    {
        public ActionResult Index() {

            return View();
        }

        public ActionResult Play()
        {

            return View();
        }
    }
}