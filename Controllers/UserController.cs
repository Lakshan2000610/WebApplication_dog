using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication_dog.Models;

namespace WebApplication_dog.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public UserController()
        {
            _dbContext = new ApplicationDbContext();
        }

        public ActionResult PetAddApplication()
        {
            ViewBag.Message = TempData["ApplicationMessage"] as string;
            ViewBag.PetTypes = new SelectList(_dbContext.PetTypes.ToList(), "Id", "Name"); // Pass PetTypes as SelectList
            ViewBag.Breeds = new SelectList(_dbContext.Breeds.ToList(), "Id", "Name");
            return View();
        }

        
        public ActionResult Adopt(int id)
        {
            var pet = _dbContext.PetAdds.FirstOrDefault(p => p.PetId == id);
            if (pet == null)
            {
                return HttpNotFound("Pet not found.");
            }

            var petTypeName = pet.PetTypeId.HasValue ? _dbContext.PetTypes.Find(pet.PetTypeId.Value)?.Name ?? "N/A" : "N/A";
            var breedName = pet.BreedId.HasValue ? _dbContext.Breeds.Find(pet.BreedId.Value)?.Name ?? "N/A" : "N/A";

            var model = new AdoptionApplication
            {
                PetId = pet.PetId,
                PetName = pet.PetName,
                PetType = petTypeName,
                Breed = breedName
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Adopt(AdoptionApplication model)
        {
            if (ModelState.IsValid)
            {
                model.Status = false;
                model.ApplicationDate = DateTime.Now;
                _dbContext.AdoptionApplications.Add(model);
                _dbContext.SaveChanges();
                return RedirectToAction("AdoptSuccess");
            }
            return View(model);
        }

        public ActionResult AdoptSuccess()
        {
            return View();
        }
        public ActionResult Resources()
        {
            return View();
        }
        public ActionResult CatCare()
        {
            return View();
        }

        public ActionResult DogCare()
        {
            return View();
        }

        public ActionResult PetListing(string petType = null, int? age = null, string breed = null, string sex = null, string location = null)
        {
            // Fetch all pets
            var query = _dbContext.PetAdds.AsQueryable();

            // Pre-fetch PetType and Breed names into dictionaries for efficient lookup
            var petTypes = _dbContext.PetTypes.ToDictionary(pt => pt.Id, pt => pt.Name);
            var breeds = _dbContext.Breeds.ToDictionary(b => b.Id, b => b.Name);

            // Apply search filters
            if (!string.IsNullOrEmpty(petType) && petType != "All Pet Types")
            {
                var petTypeId = petTypes.FirstOrDefault(pt => pt.Value == petType).Key;
                query = query.Where(a => a.PetTypeId == petTypeId);
            }
            if (age.HasValue)
            {
                query = query.Where(a => a.Age == age.Value);
            }
            if (!string.IsNullOrEmpty(breed) && breed != "All Breeds")
            {
                var breedId = breeds.FirstOrDefault(b => b.Value == breed).Key;
                query = query.Where(a => a.BreedId == breedId);
            }
            if (!string.IsNullOrEmpty(sex) && sex != "All Sexes")
            {
                query = query.Where(a => a.Sex == sex);
            }
            if (!string.IsNullOrEmpty(location) && location != "All Locations")
            {
                query = query.Where(a => a.StreetAddress == location);
            }

            var model = query.ToList();

            // Populate ViewBag for dropdowns with proper Value and Text properties
            ViewBag.PetTypes = new SelectList(new[] { new { Value = "", Text = "All Pet Types" } }
                .Concat(petTypes.Select(pt => new { Value = pt.Value, Text = pt.Value })), "Value", "Text");
            ViewBag.Ages = new SelectList(new[] { new { Value = "", Text = "All Ages" } }
                .Concat(Enumerable.Range(0, 100).Select(x => new { Value = x.ToString(), Text = x.ToString() })), "Value", "Text");
            ViewBag.Breeds = new SelectList(new[] { new { Value = "", Text = "All Breeds" } }
                .Concat(breeds.Select(b => new { Value = b.Value, Text = b.Value })), "Value", "Text");
            ViewBag.Locations = new SelectList(new[] { new { Value = "", Text = "All Locations" } }
                .Concat(_dbContext.PetAdds.Select(a => a.StreetAddress).Distinct().Where(l => l != null)
                    .Select(l => new { Value = l, Text = l })), "Value", "Text");
            ViewBag.Sexes = new SelectList(new[] { new { Value = "", Text = "All Sexes" } }
                .Concat(new[] { "Male", "Female" }.Select(s => new { Value = s, Text = s })), "Value", "Text");

            // Pass dictionaries for PetType and Breed name lookups
            ViewBag.PetTypeNames = petTypes;
            ViewBag.BreedNames = breeds;

            return View(model);
        }

        public ActionResult PetDetails(int id)
        {
            var pet = _dbContext.PetAdds.FirstOrDefault(a => a.PetId == id);
            if (pet == null)
            {
                return HttpNotFound("Pet not found.");
            }

            var petTypes = _dbContext.PetTypes.ToDictionary(pt => pt.Id, pt => pt.Name);
            var breeds = _dbContext.Breeds.ToDictionary(b => b.Id, b => b.Name);
            ViewBag.PetTypeNames = petTypes;
            ViewBag.BreedNames = breeds;

            return View(pet);
        }


        public ActionResult Contact()
        {
            ViewBag.Message = TempData["ContactMessage"] as string; // Use a specific key
            return View();
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

                TempData["ContactMessage"] = "Success: Message sent successfully!";
                return RedirectToAction("Contact");
            }
            catch (Exception ex)
            {
                var errorMessage = "Error: Failed to send message - " + ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " Inner Exception: " + ex.InnerException.Message;
                    if (ex.InnerException.InnerException != null)
                    {
                        errorMessage += " Inner Inner Exception: " + ex.InnerException.InnerException.Message;
                    }
                }
                TempData["ContactMessage"] = errorMessage;
                return RedirectToAction("Contact");
            }
        }
    }
}