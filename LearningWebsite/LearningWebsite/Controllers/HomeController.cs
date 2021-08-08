using LearningWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LearningWebsite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            ViewBag.Message = "Create a new account.";

            return View();
        }

        [HttpPost]
        public ActionResult Register(Models.Register register)
        {
            if (ModelState.IsValid)
            {
                using (var databaseContext = new Models.Entities())
                {
                   User reglog = new User();


                    reglog.UserEmail = register.Email;
                    reglog.UserPassword = register.Password;


                    databaseContext.Users.Add(reglog);
                    databaseContext.SaveChanges();
                }

                ViewBag.Message = "User Details Saved";
                return View("Register");
            }
            else
            {

                return View("Register", register);
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.Message = "Login to account";

            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.Login login)
        {
            if (ModelState.IsValid)
            {
                 var isValidUser = IsValidUser(login);

                 if (isValidUser != null)
                {
                    using (var databaseContext = new Models.Entities())
                    {
                        User user = databaseContext.Users.Where(query => query.UserEmail.Equals(login.Email)).SingleOrDefault();
                        Session["User"] = new LearningWebsite.Models.Login
                        {
                            Email = login.Email,
                            UserId = user.UserId
                        };
                    }
                    FormsAuthentication.SetAuthCookie(login.Email, false);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Failure", "Wrong Username and password combination!");
                    return View();
                }
            }
            else
            {
                return View(login);
            }
        }

        public Login IsValidUser(Login login)
        {
            using (var databaseContext = new Models.Entities())
            {
                User user = databaseContext.Users.Where(query => query.UserEmail.Equals(login.Email)).SingleOrDefault();
                if (user == null)
                    return null;
                else
                    return login;
            }
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index");
        }

        public ActionResult ClassList()
        {
            ViewBag.Message = "Classes";

            using (var databaseContext = new Models.Entities())
            {
                var courses = databaseContext.Classes;
                return View(courses.ToList());
            }
        }

        [HttpGet]
        public ActionResult EnrollInClass()
        {
            ViewBag.Message = "Enroll in Class";

            using (var databaseContext = new Models.Entities())
            {
                var courses = databaseContext.Classes;
                return View(courses.ToList());
            }
        }

        [HttpPost]
        public ActionResult EnrollInClass(Models.EnrollInClass enrollInClass)
        {
            if (ModelState.IsValid)
            {
                var user = (LearningWebsite.Models.Login)Session["User"];
                string optionsValue = Request.Form["SelectedClass"];
                using (var databaseContext = new Models.Entities())

                {
                    var stud = new EnrollInClass()
                    {
                        UserId = user.UserId,
                        ClassId = Int32.Parse(optionsValue)
                };

                    databaseContext.UpdateUserClass(stud.UserId, stud.ClassId);
                 

                    databaseContext.SaveChanges();
                }

                ViewBag.Message = "User Details Saved";
                return View("EnrollInClass");
            }
            else
            {

                return View("EnrollInClass", enrollInClass);
            }
        }

        public ActionResult StudentClasses()
        {
            ViewBag.Message = "Student Classes";

            return View();
        }
    }
}