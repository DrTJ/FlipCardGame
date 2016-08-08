using System;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using FlipCardGame.Models;
using RoyaMVC_EN.AccountManagement;
using RoyaMVC_EN.Models;

namespace FlipCardGame.Controllers
{
    [Authorize]
    [Filters.InitializeSimpleMembership]
    public class AccountController : CustomControllerBase {
        Models.Repositories.UsersRepository repoUsers;

        public AccountController() : base() { this.repoUsers = new Models.Repositories.UsersRepository(); }
        
        [AllowAnonymous]
        public ActionResult Login(string returnUrl) {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(FlipCardGame.Models.LoginModel model, string returnUrl) {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe)) {
                if (!repoUsers.IsActive(model.UserName)) {
                    ModelState.AddModelError("locked", "Your account has been locked! Contact support assistant.");
                    WebSecurity.Logout();
                }
                else {
                    repoUsers.SetCurrentUser(model.UserName);

                    Session["CurrentUser"] = CurrentUser.Clone();

                    if (string.IsNullOrWhiteSpace(returnUrl) || (returnUrl == "/"))
                        return RedirectToAction("Index", "Home");
                    else
                        return RedirectToLocal(returnUrl);
                }
            }
            else {
                // If we got this far, something failed, redisplay form
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }
            
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff() {
            WebSecurity.Logout();
            Session.Abandon();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register() {
            var res = new RoyaMVC_EN.AccountManagement.RegisterModel();
            return View(res);
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RoyaMVC_EN.AccountManagement.RegisterModel model) {
            if (ModelState.IsValid) {
                var userID = repoUsers.GetNewUserID();
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

                    return RedirectToAction("Index", "Home");
                }
                catch (System.Web.Security.MembershipCreateUserException e) {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            return View(model);
        }
        //
        // POST: /Account/ExternalLogin

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult ExternalLogin(string provider, string returnUrl) {
        //    return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        //}

        //
        // GET: /Account/ExternalLoginCallback

        //[AllowAnonymous]
        //public ActionResult ExternalLoginCallback(string returnUrl) {
        //    AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        //    if (!result.IsSuccessful) {
        //        return RedirectToAction("ExternalLoginFailure");
        //    }

        //    if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false)) {
        //        return RedirectToLocal(returnUrl);
        //    }

        //    if (User.Identity.IsAuthenticated) {
        //        // If the current user is logged in add the new account
        //        OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
        //        return RedirectToLocal(returnUrl);
        //    }
        //    else {
        //        // User is new, ask for their desired membership name
        //        string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
        //        ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
        //        ViewBag.ReturnUrl = returnUrl;
        //        return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
        //    }
        //}

        //
        // POST: /Account/ExternalLoginConfirmation

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl) {
        //    string provider = null;
        //    string providerUserId = null;

        //    if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId)) {
        //        return RedirectToAction("Manage");
        //    }

        //    if (ModelState.IsValid) {
        //        // Insert a new user into the database
        //        using (UsersContext db = new UsersContext()) {
        //            UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
        //            // Check if user already exists
        //            if (user == null) {
        //                // Insert name into the profile table
        //                db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
        //                db.SaveChanges();

        //                OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
        //                OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

        //                return RedirectToLocal(returnUrl);
        //            }
        //            else {
        //                ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
        //            }
        //        }
        //    }

        //    ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
        //    ViewBag.ReturnUrl = returnUrl;
        //    return View(model);
        //}

        //
        // GET: /Account/ExternalLoginFailure

        //[AllowAnonymous]
        //public ActionResult ExternalLoginFailure() {
        //    return View();
        //}

        //[AllowAnonymous]
        //[ChildActionOnly]
        //public ActionResult ExternalLoginsList(string returnUrl) {
        //    ViewBag.ReturnUrl = returnUrl;
        //    return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        //}

        //[ChildActionOnly]
        //public ActionResult RemoveExternalLogins() {
        //    ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
        //    List<ExternalLogin> externalLogins = new List<ExternalLogin>();
        //    foreach (OAuthAccount account in accounts) {
        //        AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

        //        externalLogins.Add(new ExternalLogin {
        //            Provider = account.Provider,
        //            ProviderDisplayName = clientData.DisplayName,
        //            ProviderUserId = account.ProviderUserId,
        //        });
        //    }

        //    ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
        //    return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        //}

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl) {
            if (Url.IsLocalUrl(returnUrl)) {
                return Redirect(returnUrl);
            }
            else {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        public static string ErrorCodeToString(MembershipCreateStatus createStatus) {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus) {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
