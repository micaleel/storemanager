using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using StoreManager.Infrastructure;
using StoreManager.Models;
using StoreManager.ViewModels;

namespace StoreManager.Controllers {

    public class AccountController : BaseController {

        private const int MinRequiredPasswordLength = 6;
        public static string GlobalFullUploadPath = "";

        [AuthorizeAndRedirect(Roles = "Admin")]
        public ActionResult Index() {
            return View(Db.Users.ToList());
        }

        public ActionResult Details(int id) {
            var user = Db.Users.SingleOrDefault(u => u.UserId == id);
            if (user == null) return HttpNotFound();
            return View(user);
        }

        public ActionResult DetailsByName(string userName) {
            var user = Db.Users.SingleOrDefault(u => u.UserName == userName);
            if (user == null) return HttpNotFound();
            return View("Details", user);
        }

        [AuthorizeAndRedirect(Roles = "Admin")]
        public ActionResult Create()
        {
            return View(new RegisterModel());
        }

        [HttpPost]
        public ActionResult Create(RegisterModel input) {
            if (!ModelState.IsValid) 
                return View(input);
            

            if (Db.Users.Any(x => x.UserName == input.UserName)) {
                ModelState.AddModelError("UserName", "UserName already taken");
                return View(input);
            }

            var user = new User();
            user = Mapper.Map(input, user);

            user.LastLogin = DateTime.UtcNow;
            user.Locked = false;
            user.Approved = true;
            user.Role = Role.User.ToString();
            user.DisplayName = input.DisplayName;

            Db.Users.Add(user);
            Db.SaveChanges();

            FlashSuccess(string.Format("User account '{0}' has been created",
                input.DisplayName));

            return RedirectToAction("Edit",
                new { id = Db.Users.Single(u => u.UserName == input.UserName).UserId });
        }

        [AuthorizeAndRedirect(Roles = "Admin")]
        public ActionResult Edit(int id) {
            var entity = Db.Users.SingleOrDefault(u => u.UserId == id);
            if (entity == null) return HttpNotFound();
            return View(entity);
        }

        [HttpPost, AuthorizeAndRedirect(Roles = "Admin")]
        public ActionResult Edit(User input) {
            var entity = Db.Users.SingleOrDefault(u => u.UserId == input.UserId);
            if (entity == null) return HttpNotFound();

            if (entity.Role == input.Role) return RedirectToAction("Index");

            entity.Role = input.Role;
            Db.SaveChanges();

            FlashSuccess(string.Format(
                "The user '{0}' has been assigned a role of '{1}'",
                entity.DisplayName,
                entity.Role));

            return RedirectToAction("Details", new { id = entity.UserId });
        }

        [AuthorizeAndRedirect(Roles = "Admin")]
        public ActionResult Approve(int id) {
            var user = Db.Users.Find(id);
            user.Approved = true;
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AuthorizeAndRedirect(Roles = "Admin")]
        public ActionResult Deny(int id) {
            var user = Db.Users.Find(id);
            user.Approved = false;
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AuthorizeAndRedirect(Roles = "Admin")]
        public ActionResult Lock(int id) {
            var user = Db.Users.Find(id);
            user.Locked = true;
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AuthorizeAndRedirect(Roles = "Admin")]
        public ActionResult Unlock(int id) {
            var user = Db.Users.Find(id);
            user.Locked = false;
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Login() {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        private void SetAuthCookie(User user, bool rememberMe = false) {
            var userData = user.Role + "|" + user.DisplayName;
            var ticket = new FormsAuthenticationTicket(
                1,
                user.UserName,
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(1),
                rememberMe, userData);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            Response.Cookies.Add(cookie);
            user.LastLogin = DateTime.UtcNow;
            Db.Entry(user).State = EntityState.Modified;
            Db.SaveChanges();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl) {
            if (ModelState.IsValid) {
                var authenticated = Db.Users.Any(x => x.UserName == model.UserName && x.Password == model.Password);

                if (authenticated) {
                    var user = Db.Users.SingleOrDefault(x => x.UserName == model.UserName);

                    if (user != null && !user.Approved) {
                        ModelState.AddModelError("", "Your account has not been approved to access this system.");
                        return View(model);
                    }

                    if (user != null && user.Locked) {
                        ModelState.AddModelError("", "Your account has been locked by an administrator.");
                        return View(model);
                    }

                    SetAuthCookie(user, model.RememberMe);

                    return SafeRedirectFromLogin(returnUrl);
                }
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }

            return View(model);
        }

        [NonAction]
        public ActionResult SafeRedirectFromLogin(string returnUrl) {
            if (Url.IsLocalUrl(returnUrl)
                && returnUrl.Length > 1
                && returnUrl.StartsWith("/")
                && !returnUrl.StartsWith("//")
                && !returnUrl.StartsWith("/\\"))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register() {
            if (!User.Identity.IsAuthenticated) {
                var vm = new RegisterModel();
                return View(vm);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model) {
            if (ModelState.IsValid)
            {
                const bool lockNewlyCreatedAccounts = false;
                const bool approveNewlyCreatedAccounts = false;

                if (Db.Users.Any(x => x.UserName == model.UserName)) {
                    ModelState.AddModelError("UserName", "UserName already taken");
                    return View(model);
                }

                var user = new User();
                user = Mapper.Map(model, user);

                user.LastLogin = DateTime.UtcNow;
                user.Locked = lockNewlyCreatedAccounts;
                user.Approved = approveNewlyCreatedAccounts;
                user.Role = Role.User.ToString();
                user.DisplayName = model.DisplayName;

                if (!Db.Users.Any())
                {
                    user.Role = Role.Admin.ToString();
                    user.Approved = true;
                    user.Locked = false;
                }

                Db.Users.Add(user);
                Db.SaveChanges();

                if (User.Identity.IsAuthenticated)
                    return RedirectToAction("Index");

                if (user.Approved && !user.Locked) {
                    SetAuthCookie(user);
                    return RedirectToAction("Index", "Home");
                }

                return RedirectToAction("RegisterSuccess");
            }

            return View(model);
        }

        public ActionResult RegisterSuccess() {
            return View();
        }

        [AuthorizeAndRedirect]
        public ActionResult ChangePassword() {
            ViewBag.PasswordLength = MinRequiredPasswordLength;
            return View();
        }

        [HttpPost, AuthorizeAndRedirect]
        public ActionResult ChangePassword(ChangePasswordModel model) {
            if (ModelState.IsValid) {
                var changePasswordSucceeded = false;
                try {
                    var currentUser = Db.Users.SingleOrDefault(x => x.UserName == User.Identity.Name);

                    if (currentUser == null) return HttpNotFound();
                    if (currentUser.Password == model.OldPassword) {
                        currentUser.Password = model.NewPassword;
                        Db.SaveChanges();
                        changePasswordSucceeded = true;
                    }
                    else {
                        ModelState.AddModelError("Password", "The current password is incorrect");
                    }
                }
                catch (Exception) {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                    return RedirectToAction("ChangePasswordSuccess");

                ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
            }

            ViewBag.PasswordLength = MinRequiredPasswordLength;

            return View(model);
        }

        public ActionResult ChangePasswordSuccess() {
            return View();
        }

        [AuthorizeAndRedirect]
        public ActionResult Logout() {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ForgotPassword() {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string userName) {
            var user = Db.Users.SingleOrDefault(x => x.UserName == userName);
            var question = user != null ? user.SecretQuestion : String.Empty;

            return RedirectToAction("ResetPassword", new { userName, secretQuestion = question });
        }

        public ActionResult ResetPassword(string userName, string question) {
            return View(new ResetPasswordModel { UserName = userName, Question = question });
        }

        [HttpPost]
        public ActionResult ResetPassword(string userName, string question, string answer, string password) {
            var user = Db.Users.SingleOrDefault(x => x.UserName == userName);
            if (user == null) {
                ModelState.AddModelError("UserName", "Invalid UserName");
                return RedirectToAction("ForgotPassword", new { userName });
            }

            if (user.SecretAnswer == answer) {
                user.Password = password;
                Db.SaveChanges();
                return RedirectToAction("Login", new { userName, password });
            }
            ModelState.AddModelError("Answer", "Invalid Answer");

            return RedirectToAction("ForgotPassword", new { userName });
        }

    }
}