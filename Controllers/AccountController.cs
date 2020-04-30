using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SunStack.Models;
using SunStack.ViewModel;
using System.Web.Security;

namespace SunStack.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        //The form's data in Register view is posted to this method. 
        //We have binded the Register View with Register ViewModel, so we can accept object of Register class as parameter.
        //This object contains all the values entered in the form by the user.
        [HttpPost]
        public ActionResult SaveRegisterDetails(Register registerDetails)
        {
            //We check if the model state is valid or not. We have used DataAnnotation attributes.
            //If any form value fails the DataAnnotation validation the model state becomes invalid.
            if (ModelState.IsValid)
            {
                //create database context using Entity framework 
                using (var databaseContext = new SunStackEntities())
                {
                    //If the model state is valid i.e. the form values passed the validation then we are storing the User's details in DB.
                    RegisterUser reglog = new RegisterUser();

                    //Save all details in RegitserUser object

                    reglog.FirstName = registerDetails.FirstName;
                    reglog.LastName = registerDetails.LastName;
                    reglog.Email = registerDetails.Email;
                    reglog.Password = registerDetails.Password;


                    //Calling the SaveDetails method which saves the details.
                    databaseContext.RegisterUsers.Add(reglog);
                    databaseContext.SaveChanges();
                }

                @ViewBag.Message = "User Details Saved";
                return View("Login");
            }
            else
            {

                //If the validation fails, we are returning the model object with errors to the view, which will display the error messages.
                return View("Register", registerDetails);
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        //The login form is posted to this method.
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            //Checking the state of model passed as parameter.
            if (ModelState.IsValid)
            {

                //Validating the user, whether the user is valid or not.
                var isValidUser = IsValidUser(model);

                //If user is valid & present in database, we are redirecting it to Welcome page.
                if (isValidUser != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    return RedirectToAction("Index");
                }
                else
                {
                    //If the username and password combination is not present in DB then error message is shown.
                    ModelState.AddModelError("Failure", "Wrong Username and password combination !");
                    return View();
                }
            }
            else
            {
                //If model state is not valid, the model with error message is returned to the View.
                return View(model);
            }
        }

        //function to check if User is valid or not
        public RegisterUser IsValidUser(LoginViewModel model)
        {
            using (var dataContext = new SunStackEntities())
            {
                //Retireving the user details from DB based on username and password enetered by user.
                RegisterUser user = dataContext.RegisterUsers.Where(query => query.Email.Equals(model.Email) && query.Password.Equals(model.Password)).FirstOrDefault();
                //If user is present, then true is returned.
                if (user == null)
                    return null;
                //If user is not present false is returned.
                else
                    return user;
            }
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Index");
        }

        public ActionResult Question()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveQuestion(Question question)
        {
            using (var databaseContext = new SunStackEntities())
            {
                Question ques = new Question();

                ques.Question1 = question.Question1;
                ques.CreatedDate = DateTime.Now;
                ques.Postedby = User.Identity.Name;

                databaseContext.Questions.Add(ques);
                try
                {
                    databaseContext.SaveChanges();
                }
                catch
                {
                    return View("Question");
                }
            }
            @ViewBag.Message = "Question posted successfully";
            return View("QuestionList");

        }
        public ActionResult QuestionList()
        {
           

            using (var databaseContext = new SunStackEntities())
                return View(databaseContext.Questions.ToList());

        }

        public ActionResult Answer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveAnswer(Answer answer)
        {
            using (var databaseContext = new SunStackEntities())
            {
                Answer ans = new Answer();

                ans.Answer1 = answer.Answer1;
                ans.UserId = User.Identity.Name ;



                databaseContext.Answers.Add(ans);
                try
                {
                    databaseContext.SaveChanges();
                }
                catch
                {
                    return View("Question");
                }






            }
            return View("Answer");
        }
    }
}



