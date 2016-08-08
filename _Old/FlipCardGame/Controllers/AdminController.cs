using FlipCardGame.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FreeMarket.Controllers
{
    //[Authorize(Roles = "Admin, Operator, Demo")]
    [FlipCardGame.Filters.InitializeSimpleMembership]
    public class AdminController : CustomControllerBase
    {
        FlipCardGame.Models.Repositories.AdminRepository repoAdmin;

        public AdminController()
            : base() {
            this.repoAdmin = new FlipCardGame.Models.Repositories.AdminRepository();
        }

        public ActionResult Panel() {
            return View();
        }

    }
}
