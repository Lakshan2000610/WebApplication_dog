using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication_dog.Models;

namespace WebApplication_dog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public AdminController()
        {
            _dbContext = new ApplicationDbContext();
        }

        

        
        public ActionResult Index()
        {
            var model = new AdminDashboardViewModel
            {
                TotalPets = _dbContext.PetTypes.Count(),
                AdoptedPets = _dbContext.AdoptionApplications.Count(a => a.Status),
                PetTypes = _dbContext.Breeds.Count(),
                PendingAdoptions = _dbContext.AdoptionApplications.Count(a => !a.Status),
                RecentAdoptions = _dbContext.AdoptionApplications
                    .Where(a => a.Status)
                    .OrderByDescending(a => a.ApplicationDate)
                    .Take(6)
                    .Select(a => new AdoptionRecord
                    {
                        PetName = a.PetName,
                        AdopterName = a.FirstName + " " + a.LastName,
                        AdoptionDate = a.ApplicationDate
                    })
                    .ToArray()
            };
            ViewBag.AdminName = User.Identity.Name;
            return View(model);
        }

        public ActionResult PetListing()
        {
            
           
              
                var petAdds = _dbContext.PetAdds.ToList();

                return View(petAdds );
           
           
        }

        // GET: /Admin/AddPetType
        public ActionResult AddPetType()
        {
            var petTypes = _dbContext.PetTypes
                .Include(pt => pt.Breeds) // Include related breeds to count them
                .ToList();
            return View(petTypes);
        }
        // POST: AddPetType - Handles the form submission to add a new PetType
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPetType(PetType petType)
        {
            if (ModelState.IsValid)
            {
                _dbContext.PetTypes.Add(petType);
                _dbContext.SaveChanges();
                TempData["Message"] = "Success: Pet type added successfully!";
                return RedirectToAction("AddPetType");
            }

            return RedirectToAction("AddPetType");
        }


        // GET: EditPetType - Displays the form to edit a PetType
        public ActionResult EditPetType(int id)
        {
            var petType = _dbContext.PetTypes.Find(id);
            if (petType == null)
            {
                return HttpNotFound();
            }
            return View(petType);
        }

        // POST: EditPetType - Handles updating a PetType
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPetType(PetType petType)
        {
            if (ModelState.IsValid)
            {
                var existingPetType = _dbContext.PetTypes.Find(petType.Id);
                if (existingPetType == null)
                {
                    return HttpNotFound();
                }

                existingPetType.Name = petType.Name;
                _dbContext.SaveChanges();
                TempData["Message"] = "Success: Pet type updated successfully!";
                return RedirectToAction("AddPetType");
            }

            return View(petType);
        }

        public ActionResult AddPetBreed()
        {
            var petTypes = _dbContext.PetTypes.ToList();
            if (!petTypes.Any())
            {
                TempData["Message"] = "Error: No pet types available. Please add a pet type first.";
                return RedirectToAction("AddPetType");
            }
            ViewBag.PetTypes = new SelectList(petTypes, "Id", "Name");
            ViewBag.Breeds = _dbContext.Breeds
                .Include(b => b.PetType) // Include related PetType for display
                .ToList();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPetBreed(Breed model, HttpPostedFileBase BreedImage)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PetTypes = new SelectList(_dbContext.PetTypes.ToList(), "Id", "Name");
                return View(model);
            }

            try
            {
                string imagePath = null;
                if (BreedImage != null && BreedImage.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(BreedImage.FileName) + "_" + Guid.NewGuid() + Path.GetExtension(BreedImage.FileName);
                    imagePath = Path.Combine(Server.MapPath("~/Content/Uploads"), fileName);
                    Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Content/Uploads")));
                    BreedImage.SaveAs(imagePath);
                    imagePath = "/Content/Uploads/" + fileName;
                }

                var breed = new Breed
                {
                    Name = model.Name,
                    ImagePath = imagePath,
                    PetTypeId = model.PetTypeId
                };

                _dbContext.Breeds.Add(breed);
                _dbContext.SaveChanges();

                TempData["Message"] = "Success: Breed added successfully!";
                return RedirectToAction("PetListing");
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error: Failed to add breed - " + ex.Message;
                ViewBag.PetTypes = new SelectList(_dbContext.PetTypes.ToList(), "Id", "Name");
                return View(model);
            }
        }

        public ActionResult EditPetBreed(int id)
        {
            var breed = _dbContext.Breeds.Find(id);
            if (breed == null)
            {
                TempData["Message"] = "Error: Breed not found.";
                return RedirectToAction("AddPetBreed");
            }
            ViewBag.PetTypes = new SelectList(_dbContext.PetTypes.ToList(), "Id", "Name", breed.PetTypeId);
            return View(breed);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPetBreed(Breed model, HttpPostedFileBase BreedImage)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PetTypes = new SelectList(_dbContext.PetTypes.ToList(), "Id", "Name", model.PetTypeId);
                return View(model);
            }

            try
            {
                var breed = _dbContext.Breeds.Find(model.Id);
                if (breed == null)
                {
                    TempData["Message"] = "Error: Breed not found.";
                    return RedirectToAction("AddPetBreed");
                }

                string imagePath = breed.ImagePath;
                if (BreedImage != null && BreedImage.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(BreedImage.FileName) + "_" + Guid.NewGuid() + Path.GetExtension(BreedImage.FileName);
                    imagePath = Path.Combine(Server.MapPath("~/Content/Uploads"), fileName);
                    Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Content/Uploads")));
                    BreedImage.SaveAs(imagePath);
                    imagePath = "/Content/Uploads/" + fileName;

                    // Delete old image if it exists
                    if (!string.IsNullOrEmpty(breed.ImagePath))
                    {
                        var oldImagePath = Server.MapPath(breed.ImagePath);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                }

                breed.Name = model.Name;
                breed.PetTypeId = model.PetTypeId;
                breed.ImagePath = imagePath;

                _dbContext.SaveChanges();

                TempData["Message"] = "Success: Breed updated successfully!";
                return RedirectToAction("AddPetBreed");
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error: Failed to update breed - " + ex.Message;
                ViewBag.PetTypes = new SelectList(_dbContext.PetTypes.ToList(), "Id", "Name", model.PetTypeId);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePetBreed(int id)
        {
            try
            {
                var breed = _dbContext.Breeds.Find(id);
                if (breed == null)
                {
                    TempData["Message"] = "Error: Breed not found.";
                    return RedirectToAction("AddPetBreed");
                }

                // Delete the image file if it exists
                if (!string.IsNullOrEmpty(breed.ImagePath))
                {
                    var imagePath = Server.MapPath(breed.ImagePath);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _dbContext.Breeds.Remove(breed);
                _dbContext.SaveChanges();

                TempData["Message"] = "Success: Breed deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error: Failed to delete breed - " + ex.Message;
            }
            return RedirectToAction("AddPetBreed");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Applications()
        {
            var model = _dbContext.AdoptionApplications
               
                .ToList();
            return View(model);
        }
       

        


        // GET: Admin/AddPet
        public ActionResult AddPet()
        {
            // Populate ViewBag.PetTypes with a SelectList
            ViewBag.PetTypes = new SelectList(_dbContext.PetTypes.ToList(), "Id", "Name");
            ViewBag.Breeds = new SelectList(_dbContext.Breeds.ToList(), "Id", "Name");
            return View();
        }

        // POST: Admin/AddPet
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPet(PetAdd model, HttpPostedFileBase PetImage)
        {
            if (!ModelState.IsValid)
            {
                // Repopulate ViewBag.PetTypes if the model state is invalid
                ViewBag.PetTypes = new SelectList(_dbContext.PetTypes.ToList(), "Id", "Name", model.PetTypeId);
                ViewBag.Breeds = new SelectList(_dbContext.Breeds.ToList(), "Id", "Name", model.BreedId);
                return View(model);
            }

            try
            {
                if (PetImage != null && PetImage.ContentLength > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(PetImage.FileName).ToLower();
                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("PetImage", "Only image files (.jpg, .jpeg, .png, .gif) are allowed.");
                        ViewBag.PetTypes = new SelectList(_dbContext.PetTypes.ToList(), "Id", "Name", model.PetTypeId);
                        ViewBag.Breeds = new SelectList(_dbContext.Breeds.ToList(), "Id", "Name", model.BreedId);
                        return View(model);
                    }

                    string fileName = Path.GetFileNameWithoutExtension(PetImage.FileName) + "_" + Guid.NewGuid() + extension;
                    string imagePath = Path.Combine(Server.MapPath("~/Content/Uploads"), fileName);
                    Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                    PetImage.SaveAs(imagePath);
                    model.PetImagePath = "/Content/Uploads/" + fileName;
                }

                _dbContext.PetAdds.Add(model);
                _dbContext.SaveChanges();

                ViewBag.Message = "Success: Pet added successfully!";
                ModelState.Clear();
                ViewBag.PetTypes = new SelectList(_dbContext.PetTypes.ToList(), "Id", "Name"); // Repopulate after success
                ViewBag.Breeds = new SelectList(_dbContext.Breeds.ToList(), "Id", "Name");
                return View(new PetAdd());
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error: Failed to add pet - {ex.Message}";
                ViewBag.PetTypes = new SelectList(_dbContext.PetTypes.ToList(), "Id", "Name", model.PetTypeId);
                ViewBag.Breeds = new SelectList(_dbContext.Breeds.ToList(), "Id", "Name", model.BreedId);
                return View(model);
            }
        }

        public ActionResult EditApplication(int id)
        {
            var pet = _dbContext.PetAdds.Find(id);

            if (pet == null)
            {
                TempData["Message"] = "Error: Breed not found.";
                return RedirectToAction("AddPet");
            }
            ViewBag.PetTypes = new SelectList(_dbContext.PetTypes.ToList(), "Id", "Name", pet.PetTypeId);
            ViewBag.Breeds = new SelectList(_dbContext.Breeds.ToList(), "Id", "Name", pet.BreedId);
            return View(pet);

            
        }

        // POST: Admin/EditApplication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditApplication(PetAdd model, HttpPostedFileBase PetImage)
        {
            if (_dbContext == null)
            {
                TempData["ApplicationMessage"] = "Error: Unable to connect to the database.";
                return RedirectToAction("PetListing");
            }

            if (!ModelState.IsValid)
            {
                // Repopulate dropdowns if validation fails
                ViewBag.PetTypes = new SelectList(_dbContext.PetTypes.ToList(), "Id", "Name", model.PetTypeId);
                ViewBag.Breeds = new SelectList(_dbContext.Breeds.ToList(), "Id", "Name", model.BreedId);
                return View(model);
            }

            try
            {
                // Find the existing pet record
                var pet = _dbContext.PetAdds.Find(model.PetId);
                if (pet == null)
                {
                    TempData["ApplicationMessage"] = "Error: Pet not found.";
                    return RedirectToAction("PetListing");
                }

                // Update fields
                pet.PetName = model.PetName;
                pet.PetTypeId = model.PetTypeId;
                pet.BreedId = model.BreedId;
                pet.Age = model.Age;
                pet.StreetAddress = model.StreetAddress;
                pet.Sex = model.Sex;
                pet.Vaccinations = model.Vaccinations;

                // Handle image update
                if (PetImage != null && PetImage.ContentLength > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(PetImage.FileName).ToLower();
                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("PetImage", "Only image files (.jpg, .jpeg, .png, .gif) are allowed.");
                        ViewBag.PetTypes = new SelectList(_dbContext.PetTypes.ToList(), "Id", "Name", model.PetTypeId);
                        ViewBag.Breeds = new SelectList(_dbContext.Breeds.ToList(), "Id", "Name", model.BreedId);
                        return View(model);
                    }

                    // Delete old image if it exists
                    if (!string.IsNullOrEmpty(pet.PetImagePath))
                    {
                        var oldImagePath = Server.MapPath(pet.PetImagePath);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Save new image
                    string fileName = Path.GetFileNameWithoutExtension(PetImage.FileName) + "_" + Guid.NewGuid() + extension;
                    string imagePath = Path.Combine(Server.MapPath("~/Content/Uploads"), fileName);
                    Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                    PetImage.SaveAs(imagePath);
                    pet.PetImagePath = "/Content/Uploads/" + fileName;
                }

                _dbContext.SaveChanges();
                TempData["ApplicationMessage"] = "Success: Pet updated successfully!";
                return RedirectToAction("PetListing");
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error: Failed to update pet - {ex.Message}";
                ViewBag.PetTypes = new SelectList(_dbContext.PetTypes.ToList(), "Id", "Name", model.PetTypeId);
                ViewBag.Breeds = new SelectList(_dbContext.Breeds.ToList(), "Id", "Name", model.BreedId);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePet(int id)
        {
            var pet = _dbContext.PetAdds.Find(id);
            if (pet == null) return HttpNotFound();
            _dbContext.PetAdds.Remove(pet);
            _dbContext.SaveChanges();
            TempData["ApplicationMessage"] = "Success: Pet deleted.";
            return RedirectToAction("PetListing");
        }


        public ActionResult ViewApplication(int id)
        {
            var application = _dbContext.AdoptionApplications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }

        // POST: Admin/UpdateApplicationStatus
        [HttpPost]
        public ActionResult UpdateApplicationStatus(int id)
        {
            var application = _dbContext.AdoptionApplications.FirstOrDefault(a => a.ApplicationId == id);
            if (application == null)
            {
                return HttpNotFound("Application not found.");
            }

            // Toggle the Status (true to false, false to true)
            application.Status = !application.Status;
            _dbContext.SaveChanges();

            // Redirect back to the application list or a confirmation page
            return RedirectToAction("Applications");
        }

        public ActionResult ViewMessage()
        {
            var model = _dbContext.Contacts.OrderByDescending(c => c.SubmissionDate).ToList();
            return View(model);
        }
    }


    public class AdminDashboardViewModel
    {
        public int TotalPets { get; set; }
        public int AdoptedPets { get; set; }
        public int PetTypes { get; set; }
        public int PendingAdoptions { get; set; }
        public AdoptionRecord[] RecentAdoptions { get; set; }
    }

    public class AdoptionRecord
    {
        public string PetName { get; set; }
        public string AdopterName { get; set; }
        public DateTime AdoptionDate { get; set; }
    }
}