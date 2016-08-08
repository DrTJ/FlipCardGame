//-----------------------------------------------------------------------
// <copyright file="ProfileController.cs" company="Processa"> 
// Copyright (c) 2016 Todos los derechos reservados.
// </copyright>
// <author>mtaghi</author>
// <date>3/31/2016 9:44:00 AM</date>
//-----------------------------------------------------------------------

using FlipCardGame.Models.Repositories;

namespace FlipCardGame.Controllers
{
    #region using
    using RoyaMVC_EN.AccountManagement;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    #endregion

    [Authorize]
    [Filters.InitializeSimpleMembership()]
    public class ProfileController : BaseController
    {
        private UsersRepository repoUsers;

        public ProfileController() : base()
        {
            this.repoUsers = new UsersRepository();
        }

        [AllowAnonymous]
        public ActionResult Index(int id)
        {
            var user = this.repoUsers.GetUser(id);            
            ViewBag.UserProfile = user;

            return View();
        }

        public ActionResult Panel()
        {
            var idUser = CurrentUser.UserID;
            return View();
        }

    }
}
