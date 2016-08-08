using FlipCardGame.Models;
using RoyaMVC_EN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FlipCardGame.Controllers
{
    //[Authorize(Roles = "DemoUser, Admin, Operator")]
    [Filters.InitializeSimpleMembership]
    public class UsersController : CustomControllerBase
    {
        Models.Repositories.UsersRepository repoUsers;

        public UsersController()
            : base() {
            this.repoUsers = new Models.Repositories.UsersRepository();
        }

        public ActionResult Index() {
            var res = this.repoUsers.GetUsersList();
            ViewBag.RolesList = this.repoUsers.GetRolesList();

            return View(res);
        }

        [HttpPost]
        public ActionResult AddUser(DAL.Users model) {
            /****************************************************/
            var userID = this.repoUsers.GetNewUserID();

            try {
                var extraFields = new {
                    IDUser = userID,
                    UserFullName = model.UserFullName,
                    UserName = model.UserName,
                    Password = model.Password,
                    EMail = model.EMail,

                    RegisterationDateTime = DateTime.Now,
                    Mobile = model.Mobile,

                    IsActive = true,
                    IsApproved = false,
                    IsEmployee = false
                };

                string confirmationToken = "";
                confirmationToken = WebMatrix.WebData.WebSecurity.CreateUserAndAccount(model.UserName, model.Password, extraFields, true);
                WebMatrix.WebData.WebSecurity.ConfirmAccount(confirmationToken);

                System.Web.Security.Roles.AddUserToRole(model.UserName, UserRoles.Customer);
                WebMatrix.WebData.WebSecurity.Login(model.UserName, model.Password);
                repoUsers.SetCurrentUser(model.UserName);

                return RedirectToAction("Index");
            }
            catch (System.Web.Security.MembershipCreateUserException e) {
                ModelState.AddModelError("", AccountController.ErrorCodeToString(e.StatusCode));
            }

            /****************************************************/
            return RedirectToAction("Index");
        }

        //public ActionResult ReSendVerification(int id) {
        //    this.repoUsers.Delete(id);
        //    return RedirectToAction("Index");
        //}

        public ActionResult Delete(int id) {
            if (User.IsInRole(UserRoles.DemoUser)) {
                var msgs = new List<RoyaMVC_EN.Models.AppMessage>();
                msgs.Add(new RoyaMVC_EN.Models.AppMessage(AppMessages.NoPermission, RoyaMVC_EN.Models.AppMessageTypes.ExclamationMessage));
                ViewBag.Messages = msgs;
            }
            else {
                this.repoUsers.Delete(id);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Details(int id) {
            var resUser = this.repoUsers.GetUser(id);
            return View(resUser);
        }

        public ActionResult Activate(int id, bool activate) {
            if (User.IsInRole(UserRoles.DemoUser)) {
                var msgs = new List<RoyaMVC_EN.Models.AppMessage>();
                msgs.Add(new RoyaMVC_EN.Models.AppMessage(AppMessages.NoPermission, RoyaMVC_EN.Models.AppMessageTypes.ExclamationMessage));
                ViewBag.Messages = msgs;
            }
            else {
                this.repoUsers.ActivateUser(id, activate);
            }

            return RedirectToAction("Index");
        }

        public ActionResult AddRole(int id, int role) {
            if (User.IsInRole(UserRoles.DemoUser)) {
                var msgs = new List<RoyaMVC_EN.Models.AppMessage>();
                msgs.Add(new RoyaMVC_EN.Models.AppMessage(AppMessages.NoPermission, RoyaMVC_EN.Models.AppMessageTypes.ExclamationMessage));
                ViewBag.Messages = msgs;
            }
            else {
                this.repoUsers.AddRemoveRole(id, role);
            }

            return RedirectToAction("Index");
        }

    }
}
