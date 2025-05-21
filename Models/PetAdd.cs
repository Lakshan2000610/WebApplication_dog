using System.ComponentModel.DataAnnotations;
using System.Web;

namespace WebApplication_dog.Models
{
    public class PetAdd
    {
        [Key]
        public int PetId { get; set; }

        [Required(ErrorMessage = "Pet name is required.")]
        [Display(Name = "Pet Name")]
        public string PetName { get; set; }

        [Required(ErrorMessage = "Breed is required.")]
        [Display(Name = "Breed")]
        public int? BreedId { get; set; }

        [Range(0, 100, ErrorMessage = "Age must be between 0 and 100.")]
        [Required(ErrorMessage = "Age is required.")]
        [Display(Name = "Age")]
        public int? Age { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [Display(Name = "Location")]
        public string StreetAddress { get; set; }

        [Required(ErrorMessage = "Sex is required.")]
        [Display(Name = "Sex")]
        public string Sex { get; set; }

        [Display(Name = "Vaccinations")]
        public string Vaccinations { get; set; }

        [Required(ErrorMessage = "Pet type is required.")]
        [Display(Name = "Pet Type")]
        public int? PetTypeId { get; set; }

        [Display(Name = "Pet Image Path")]
        public string PetImagePath { get; set; }
    }
}