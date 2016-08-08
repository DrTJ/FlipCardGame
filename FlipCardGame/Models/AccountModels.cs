﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace FlipCardGame.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection") {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName
        {
            get;
            set;
        }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password
        {
            get;
            set;
        }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword
        {
            get;
            set;
        }


        [Required]
        [Display(Name = "Full name")]
        public string UserFullName
        {
            get;
            set;
        }

        [Display(Name = "EMail")]
        public string EMail
        {
            get;
            set;
        }

        public DateTime RegisterationDateTime
        {
            get;
            set;
        }

        [Display(Name = "Mobile")]
        public string Mobile
        {
            get;
            set;
        }

        [Display(Name = "Birth date")]
        public DateTime BirthDate
        {
            get;
            set;
        }

        [Display(Name = "Description")]
        public string Description
        {
            get;
            set;
        }

        [Display(Name = "Country name")]
        public string AddressCountryName
        {
            get;
            set;
        }

        [Display(Name = "Province name")]
        public string AddressProvinceName
        {
            get;
            set;
        }

        [Display(Name = "City name")]
        public string AddressCityName
        {
            get;
            set;
        }

        [Display(Name = "Address")]
        public string Address
        {
            get;
            set;
        }

        [Display(Name = "Profile picture")]
        public string ProfilePicturePath
        {
            get;
            set;
        }

    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}