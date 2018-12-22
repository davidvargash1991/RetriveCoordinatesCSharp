using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RetriveCoordinates.Models
{
    public class Countries
    {
        [Required()]
        [Key]
        public int CountryId { get; set; }

        [Required()]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Cities> Cities { get; set; }
    }
}
