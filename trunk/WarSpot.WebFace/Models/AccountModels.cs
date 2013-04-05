using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using WarSpot.Cloud.Storage;

namespace WarSpot.WebFace.Models
{
	public class ChangePasswordModel
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

	public class LogOnModel
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
		[Display(Name = "Account name")]
		public string AccountName { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "Email address")]
		public string Email { get; set; }

        [Required]
        [Display(Name = "User firstname")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "User surname")]
        public string Usersurname { get; set; }

        [Required]
        [Display(Name = "Institution")]
        public string Institution { get; set; }

        [Required]
        [Display(Name = "Course")]
        public string Course { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}

	public class AccountRole
	{
		public RoleType RoleType { get; set; }

		public bool Is { get; set; }

		//[DisplayFormat(DataFormatString = "{0:dd.mm.yyyy}", ApplyFormatInEditMode = true)]
		public DateTime Until { get; set; }
	}

	public class ViewAccountModel
	{
		[Display(Name = "Id пользователя")]
		public string Id { get; set; }

		[Display(Name = "Логин")]
		public string UserName { get; set; }

		[Display(Name = "Роли")]
		public List<AccountRole> Roles { get; set; }
	}

}
