using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication_dog.Models
{
    public class PetType
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public virtual ICollection<Breed> Breeds { get; set; } // Added
    }
}