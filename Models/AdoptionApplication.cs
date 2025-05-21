using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace WebApplication_dog.Models
{
    public class AdoptionApplication
    {
        [Key]
        public int ApplicationId { get; set; }

        public int PetId { get; set; }

        [Required(ErrorMessage = "Pet name is required.")]
        public string PetName { get; set; }

        [Required(ErrorMessage = "Pet type is required.")]
        public string PetType { get; set; }

        public bool Status { get; set; } = false;

        [Required(ErrorMessage = "Breed is required.")]
        public string Breed { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Street address is required.")]
        public string StreetAddress { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Province is required.")]
        public string Province { get; set; }

        [Required(ErrorMessage = "Postal/Zip code is required.")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        public DateTime ApplicationDate { get; set; }
        public bool HasOtherPet { get; set; }

        public string DisciplineMethod { get; set; }

        public bool HasVeterinarian { get; set; }

        public string ClinicName { get; set; }

        public string ClinicPhoneNumber { get; set; }

        public bool IsConfirmed { get; set; }

        public string Signature { get; set; }
    }
}