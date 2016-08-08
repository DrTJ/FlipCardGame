using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlipCardGame
{
    public static class UserRoles {
        public static string Customer = "Customer";
        public static string Admin = "Admin";
        public static string Operator = "Operator";
        public static string DemoUser = "DemoUser";

        public static bool IsAdminGroup(string userRoles) {
            if (userRoles == null)
                return false;

            var adminList = new List<string>() { Admin, Operator, DemoUser };
            var userRoleNames = userRoles.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in adminList) {
                if (userRoleNames.Contains(item))
                    return true;
            }

            return false;
        }
    }
}