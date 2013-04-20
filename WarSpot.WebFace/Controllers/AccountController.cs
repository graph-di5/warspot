using System.Web.Mvc;
using System.Web.Security;
using WarSpot.Cloud.Storage;
using WarSpot.Common.Utils;
using WarSpot.Contracts.Service;
using WarSpot.WebFace.Models;
using WarSpot.WebFace.Security;

namespace WarSpot.WebFace.Controllers
{
	public class AccountController : Controller
	{

		//
		// GET: /Account/LogOn
		[AllowAnonymous]
		public ActionResult LogOn()
		{
			return View();
		}

		//
		// POST: /Account/LogOn

		[AllowAnonymous]
		[HttpPost]
		public ActionResult LogOn(LogOnModel model, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				if (CustomPrincipal.Login(model.UserName, model.Password, model.RememberMe))
				{
					if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
						 && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
					{
						return Redirect(returnUrl);
					}
					else
					{
						return RedirectToAction("Index", "Home");
					}
				}
				else
				{
					ModelState.AddModelError("", "Неправильный логин или пароль.");
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		//
		// GET: /Account/LogOff

		[AllowAnonymous]
		public ActionResult LogOff()
		{
			CustomPrincipal.Logout();

			return RedirectToAction("Index", "Home");
		}

		//
		// GET: /Account/Register

		[AllowAnonymous]
		public ActionResult Register()
		{
			return View();
		}

		//
		// POST: /Account/Register

		[AllowAnonymous]
		[HttpPost]
		public ActionResult Register(RegisterModel model)
		{
			if (ModelState.IsValid)
			{
				// Attempt to register the user
				var b = Warehouse.Register(model.AccountName, HashHelper.GetMd5Hash(model.Password), model.Username, model.Usersurname, model.Institution, int.Parse(model.Course), model.Email);
				if (b.Type == ErrorType.Ok)
				{
					if (CustomPrincipal.Login(model.AccountName, model.Password, false))
					{
						return RedirectToAction("Index", "Home");
					}
					else
					{
						ModelState.AddModelError("", "Неправильный логин или пароль.");
					}
				}
				else
				{
					// todo make error message
					ModelState.AddModelError("", b.Message);//"Some error while registration"/*ErrorCodeToString(createStatus)/**/);
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}


		//public ActionResult ChangePassword()
		//{
		//  return View();
		//}
		//
		// GET: /Account/ChangePassword

		public ActionResult ChangePassword(ChangePasswordModel model)
		{
			if (ModelState.IsValid)
			{
				bool changePasswordSecceeded = (model.NewPassword == model.ConfirmPassword);

				if (changePasswordSecceeded)
				{
					try
					{
						changePasswordSecceeded = Warehouse.ChangePassword(User.Identity.Name,
							HashHelper.GetMd5Hash(model.OldPassword),
							HashHelper.GetMd5Hash(model.NewPassword));
						if(!changePasswordSecceeded)
						{
							ModelState.AddModelError("", "Вы ввели неверный текущий пароль.");
						}
					}
					catch (System.Exception)
					{
						changePasswordSecceeded = false;
					}
				}
				else
				{
					if (model.ConfirmPassword != null || model.NewPassword != null || model.OldPassword != null)
					{
						ModelState.AddModelError("", "Введенные пароли не совпадают.");
					}
				}


				if (changePasswordSecceeded)
				{
					return RedirectToAction("ChangePasswordSuccess");
				}
				else
				{
					ModelState.AddModelError("", "Неправильный текущий пароль или некорректный новый.");
					if(model.ConfirmPassword != null || model.NewPassword != null || model.OldPassword != null)
					{
					}
				}
			}

			return View(model);
		}

		//
		// POST: /Account/ChangePassword

		// todo rewrite this
#if false
		[HttpPost]
		public ActionResult ChangePassword(ChangePasswordModel model)
		{
			if (ModelState.IsValid)
			{

				// ChangePassword will throw an exception rather
				// than return false in certain failure scenarios.
				bool changePasswordSucceeded;
				try
				{
					MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
					changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
				}
				catch (Exception)
				{
					changePasswordSucceeded = false;
				}

				if (changePasswordSucceeded)
				{
					return RedirectToAction("ChangePasswordSuccess");
				}
				else
				{
					ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}
#endif

		//
		// GET: /Account/ChangePasswordSuccess

		public ActionResult ChangePasswordSuccess()
		{
			return View();
		}

		#region Status Codes
		private static string ErrorCodeToString(MembershipCreateStatus createStatus)
		{
			// See http://go.microsoft.com/fwlink/?LinkID=177550 for
			// a full list of status codes.
			switch (createStatus)
			{
			case MembershipCreateStatus.DuplicateUserName:
				return "Логин уже используется. Введите другой.";

			case MembershipCreateStatus.DuplicateEmail:
				return "Логин для этого e-mail уже используется. Пожалуйста введите другой e-mail.";

			case MembershipCreateStatus.InvalidPassword:
				return "Некорректный пароль. Введите другой.";

			case MembershipCreateStatus.InvalidEmail:
				return "Введенный e-mail некорректен. Пожалуйста проверьте правильность и попробуйте еще раз.";

			case MembershipCreateStatus.InvalidAnswer:
				return "Неправильный ответ на восстановление пароля. Пожалуйста проверьте правильность и попробуйте еще раз.";

			case MembershipCreateStatus.InvalidQuestion:
                return "Неправильный вопрос восстановления пароля. Пожалуйста проверьте правильность и попробуйте еще раз.";

			case MembershipCreateStatus.InvalidUserName:
                return "Некорректный логин. Пожалуйста проверьте правильность и попробуйте еще раз.";

			case MembershipCreateStatus.ProviderError:
				return "При входе в систему возникла ошибка. Проверьте правильность вводимых данных и попробуйте еще раз. Если ошибка повторится, обратитесь к системному администратору.";

			case MembershipCreateStatus.UserRejected:
                return "При регистрации возникла ошибка has been canceled. Проверьте правильность вводимых данных и попробуйте еще раз. Если ошибка повторится, обратитесь к системному администратору.";

			default:
                return "Возникла ошибка. Проверьте правильность вводимых данных и попробуйте еще раз. Если ошибка повторится, обратитесь к системному администратору.";
			}
		}
		#endregion
	}
}
