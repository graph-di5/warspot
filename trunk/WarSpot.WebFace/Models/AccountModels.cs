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
		[Display(Name = "Старый пароль")]
		public string OldPassword { get; set; }

		[Required]
		[StringLength(1024, ErrorMessage = "{0} должен содержать не менее {2} символов.", MinimumLength = 8)]
		[DataType(DataType.Password)]
		[Display(Name = "Новый пароль")]
		public string NewPassword { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Подтверждение пароля")]
		[Compare("NewPassword", ErrorMessage = "Введенные пароли не совпадают.")]
		public string ConfirmPassword { get; set; }
	}

	public class LogOnModel
	{
		[Required]
		[Display(Name = "Логин")]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Пароль")]
		public string Password { get; set; }

		[Display(Name = "Запомнить")]
		public bool RememberMe { get; set; }
	}

	public class RegisterModel
	{
		[Required]
		[Display(Name = "Логин")]
		public string AccountName { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required]
		[Display(Name = "Имя")]
		public string Username { get; set; }

		[Required]
		[Display(Name = "Фамилия")]
		public string Usersurname { get; set; }

		[Required]
		[Display(Name = "Учебное заведение")]
		public string Institution { get; set; }

		[Required]
		[Display(Name = "Курс")]
		public string Course { get; set; }

		[Required]
		[StringLength(1024, ErrorMessage = "{0} должен содержать не менее {2} символов.", MinimumLength = 8)]
		[DataType(DataType.Password)]
		[Display(Name = "Пароль")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Подтверждение")]
		[Compare("Password", ErrorMessage = "Введенные пароли не совпадают.")]
		public string ConfirmPassword { get; set; }
	}

	public class AccountRole
	{
		public RoleType RoleType { get; set; }

		public bool Is { get; set; }

		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:u}", ApplyFormatInEditMode = true)]
		//[DisplayFormat(DataFormatString = "{0:yyyy.MM.dd}", ApplyFormatInEditMode = true)]
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
