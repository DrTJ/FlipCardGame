using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using RoyaMVC_EN.AccountManagement;
using System.Web.Mvc;

namespace FlipCardGame.Models.Repositories
{
    public class UsersRepository : RepositoryBase, IRepositoryBase<DAL.FlipCardGameEntities>
    {
        public UsersRepository() : base() { }


        public void SetCurrentUser(string userName) {
            var user = this.Context.Users.Include("webpages_Roles").FirstOrDefault(w => w.UserName == userName);
            if (user != null) {
                CurrentUser.DisplayName = user.UserFullName;
                CurrentUser.LastUpdate = DateTime.Now;
                CurrentUser.UserID = user.IDUser;
                CurrentUser.UserName = user.UserName;
                CurrentUser.UserTypeName = string.Join(", ", user.webpages_Roles.Select(w => w.RoleName).ToList());
            }
        }


        public DAL.Users GetUserSettings(long userID) {
            var res = this.Context.Users.FirstOrDefault(w => w.IDUser == userID);
            return res;
        }


        public bool UpdateUserSettings(DAL.Users newUserData) {
            var res = this.Context.Users.FirstOrDefault(w => w.IDUser == newUserData.IDUser);

            if (res == null)
                return false;

            res.EMail = newUserData.EMail;
            // res.IsActive = newUserData.IsActive;             // Will be updated using ActivateUser()
            // res.IsApproved = newUserData.IsApproved;         // Will be updated using ApproveUser
            // res.IsEmployee = newUserData.IsEmployee;         // Will be updated using SetAsEmployee
            res.Mobile = newUserData.Mobile;
            res.Password = newUserData.Password;
            // res.RegisterationDateTime = newUserData.RegisterationDateTime;   // Is NOT neccessary to update this
            res.UserFullName = newUserData.UserFullName;
            res.UserName = newUserData.UserName;
           
            this.Context.SaveChanges();
            return true;
        }


        public List<DAL.Users> GetUsersList() {
            var res = this.Context.Users.Include("webpages_Roles").Include("webpages_Membership").ToList();
            return res;
        }

        #region Activation


        public bool ActivateUser(int userID, bool active) {
            var user = this.Context.Users.FirstOrDefault(w => (w.IDUser == userID));
            if (user == null)
                return false;

            user.IsActive = active;

            this.Context.SaveChanges();
            return true;
        }


        public bool IsActive(string userName) {
            var user = this.Context.Users.FirstOrDefault(w => w.UserName == userName);

            return (user == null) ? false : user.IsActive;
        }


        public bool IsActive(int userID) {
            var user = this.Context.Users.FirstOrDefault(w => w.IDUser == userID);

            return (user == null) ? false : user.IsActive;
        }

        #endregion
        
        #region Approving


        public bool ApproveUser(int userID, bool approve) {
            var user = this.Context.Users.FirstOrDefault(w => w.IDUser == userID);
            if (user == null)
                return false;

            user.IsApproved = approve;

            this.Context.SaveChanges();
            return true;
        }


        public bool IsApproved(string userName) {
            var user = this.Context.Users.FirstOrDefault(w => w.UserName == userName);

            return (user == null) ? false : user.IsApproved;
        }


        public bool IsApproved(int userID) {
            var user = this.Context.Users.FirstOrDefault(w => w.IDUser == userID);

            return (user == null) ? false : user.IsApproved;
        }

        #endregion

        #region Employment


        public bool SetAsEmployee(int userID, bool isEmployee) {
            var user = this.Context.Users.FirstOrDefault(w => w.IDUser == userID);

            if (user == null)
                return false;

            user.IsEmployee = isEmployee;

            this.Context.SaveChanges();
            return true;
        }


        public bool IsEmployee(string userName) {
            var user = this.Context.Users.FirstOrDefault(w => w.UserName == userName);

            return (user == null) ? false : user.IsEmployee;
        }


        public bool IsEmployee(int userID) {
            var user = this.Context.Users.FirstOrDefault(w => w.IDUser == userID);

            return (user == null) ? false : user.IsEmployee;
        }

        #endregion


        public long GetNewUserID() {
            var res = this.Context.Users.Select(w => w.IDUser);

            return (res.Count() == 0) ? 1 : res.Max() + 1;
        }


        public RoyaMVC_EN.AccountManagement.RegisterModel GetUserByUserName(string userName) {
            var res = this.Context.Users.Include("webpages_Roles")
                                        .Where(w => w.UserName == userName);

            if (res == null)
                return new RoyaMVC_EN.AccountManagement.RegisterModel();
            else if (res.Count() == 0)
                return new RoyaMVC_EN.AccountManagement.RegisterModel();
            else
                return res.AsEnumerable()
                          .Select(w => new RoyaMVC_EN.AccountManagement.RegisterModel() {
                              IDUser = w.IDUser,
                              UserName = w.UserName,
                              UserFullName = w.UserFullName,
                              Password = w.Password,
                              EMail = w.EMail,
                              FirstName = "",   // Depricated
                              GenderID = -1,    // Depricated
                              LastName = "",    // Depricated
                              Mobile = w.Mobile,
                              RegisterationDateTimeGregorian = w.RegisterationDateTime,
                          })
                          .First();
        }


        public void ResetPassword(string username, string password) {
            var user = this.Context.Users.FirstOrDefault(w => w.UserName == username);
            if (user != null) {
                user.Password = password;
                this.Context.SaveChanges();
            }
        }


        public void Delete(int id) {
            var user = this.Context.Users.FirstOrDefault(w => w.IDUser == id);
            if (user != null) {
                this.Context.Users.Remove(user);
                this.Context.SaveChanges();
            }
        }


        public DAL.Users GetUser(int id) {
            var user = this.Context.Users.FirstOrDefault(w => w.IDUser == id);
            return (user == null) ? new DAL.Users() { IDUser = -1 } : user;
        }


        public List<DAL.webpages_Roles> GetRolesList() {
            var res = this.Context.webpages_Roles.ToList();
            return res;
        }


        public void AddRemoveRole(int id, int roleID) {
            var user = this.Context.Users.Include("webpages_Roles").FirstOrDefault(w => w.IDUser == id);
            if (user != null) {
                var userRole = user.webpages_Roles.FirstOrDefault(w => w.RoleID == roleID);
                var tmpRole = this.Context.webpages_Roles.FirstOrDefault(w => w.RoleID == roleID);
                
                if (tmpRole != null) {
                    if (userRole == null)
                        user.webpages_Roles.Add(tmpRole);
                    else
                        user.webpages_Roles.Remove(tmpRole);
                }

                this.Context.SaveChanges();
            }
        }
    }
}
