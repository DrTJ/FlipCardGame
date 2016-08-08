using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;
using FlipCardGame.Models;
using RoyaMVC_EN.AccountManagement;
using FlipCardGame.Models.Repositories;

namespace FlipCardGame.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public static ContactUsMessagesRepository repoMessages = new ContactUsMessagesRepository();


        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            //// TODO: Remove
            ////var repoProducts = new FreeMarket.Models.Repositories.ProductsRepository();
            ////filterContext.Controller.ViewBag.MenuItems = repoProducts.GetProductsMenu();

            //var repoOrders = new FreeMarket.Models.Repositories.OrdersRepository();
            //filterContext.Controller.ViewBag.ShoppingCartItems = repoOrders.GetShoppingCart(CurrentUser.UserID);

            // InitializeProductsCategories(filterContext);

            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);

            if (WebMatrix.WebData.WebSecurity.IsAuthenticated) {
                if (string.IsNullOrWhiteSpace(CurrentUser.UserName)) {
                    var repoUsers = new FlipCardGame.Models.Repositories.UsersRepository();
                    repoUsers.SetCurrentUser(WebMatrix.WebData.WebSecurity.CurrentUserName);
                }

                filterContext.Controller.ViewBag.UserDisplayName = CurrentUser.DisplayName;
                filterContext.Controller.ViewBag.UserImageURL = "/Content/themes/Panel/images/img.jpg";    //   CurrentUser.ProfileImageURL; 
                filterContext.Controller.ViewBag.NewMessagesList = repoMessages.GetUnreadMessagesList();
            }
            else {
                filterContext.Controller.ViewBag.UserImageURL = "/Content/themes/Panel/images/img.jpg";    // TODO: Remove it!     TEMPORARY
                filterContext.Controller.ViewBag.UserDisplayName = "John Doe";    // TODO: Remove it!     TEMPORARY
                filterContext.Controller.ViewBag.NewMessagesList = repoMessages.GetUnreadMessagesList();

                if (!string.IsNullOrWhiteSpace(CurrentUser.UserName)) {
                    CurrentUser.ClearData();
                }
            }
        }

        //private void InitializeProductsCategories(ActionExecutingContext filterContext) {
        //    var repoProducts = new Models.Repositories.ProductsRepository();
        //    var resCategoriesList = repoProducts.GetCategoriesListWithProducts();

        //    filterContext.Controller.ViewBag.ProductCategoriesList = resCategoriesList;
        //}

        private class SimpleMembershipInitializer {
            public SimpleMembershipInitializer() {
                System.Data.Entity.Database.SetInitializer<UsersContext<RoyaMVC_EN.Models.Users>>(null);

                try {
                    using (var context = new UsersContext<RoyaMVC_EN.Models.Users>("DefaultConnection")) {
                        if (!context.Database.Exists()) {
                            // Create the SimpleMembership database without Entity Framework migration schema
                            //((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                            throw new InvalidOperationException("Database does not exists!");
                        }
                    }

                    WebSecurity.InitializeDatabaseConnection("DefaultConnection", "Users", "IDUser", "UserName", autoCreateTables: false);
                }
                catch (Exception ex) {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }
    }
}