using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RetriveCoordinates.Models
{
    public class Cities
    {
        [Required()]
        [Key]
        public int CityId { get; set; }

        [Required()]
        [StringLength(50)]
        public string Name { get; set; }

        public int CountryId { get; set; }

        [Column(TypeName = "decimal(11, 8)")]
        public Nullable<decimal> Latitude { get; set; }

        [Column(TypeName = "decimal(11, 8)")]
        public Nullable<decimal> Longitude { get; set; }

        public virtual Countries Country { get; set; }
    }
}
