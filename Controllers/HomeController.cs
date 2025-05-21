using System;
using System.Web.Mvc;
using WebApplication_dog.Models;

namespace WebApplication_dog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public HomeController()
        {
            _dbContext = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Adopt()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("PetAddApplication", "User") });
            }

            return RedirectToAction("PetAddApplication", "User");
        }

        public ActionResult About()
        {
            return View();
        }

      
        public ActionResult Contact()
        {
            ViewBag.Message = TempData["Message"] as string;
            return View(new ContactModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Error: Please correct the errors in the form.";
                return View(model);
            }

            try
            {
                var contact = new Contact
                {
                    Name = model.Name,
                    Email = model.Email,
                    Message = model.Message,
                    SubmissionDate = DateTime.Now
                };

                _dbContext.Contacts.Add(contact);
                _dbContext.SaveChanges();

                TempData["Message"] = "Success: Message sent successfully!";
                return RedirectToAction("Contact");
            }
            catch (Exception ex)
            {
                // Capture full exception details, including inner exceptions
                var errorMessage = "Error: Failed to send message - " + ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " Inner Exception: " + ex.InnerException.Message;
                    if (ex.InnerException.InnerException != null)
                    {
                        errorMessage += " Inner Inner Exception: " + ex.InnerException.InnerException.Message;
                    }
                }
                TempData["Message"] = errorMessage;
                return RedirectToAction("Contact");
            }
        }
    }
}