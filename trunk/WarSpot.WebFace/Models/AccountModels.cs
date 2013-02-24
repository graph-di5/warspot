using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

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
		[Display(Name = "User name")]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "Email address")]
		public string Email { get; set; }

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

	public class ViewAccountModel
	{
		[Display(Name = "Id пользователя")]
		public string Id { get; set; }

		[Display(Name = "Логин")]
		public string UserName { get; set; }

		// todo add expirayion date for every role

		[Display(Name = "Пользователь")]
		public bool IsUser { get; set; }
		[Display(Name = "Администратор турниров")]
		public bool IsAdminTournaments { get; set; }
	}

	// todo delete
#if false
	public class ViewAccountListModel
	{
		public List<ViewAccountModel> Accounts { get; set; }
	}
#endif
}
