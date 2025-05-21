using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication_dog.Models
{
    public class Breed
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public int PetTypeId { get; set; }
        [ForeignKey("PetTypeId")]
        public virtual PetType PetType { get; set; }
    }
}