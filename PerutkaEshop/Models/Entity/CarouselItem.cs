using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Perutka.Eshop.Web.Models.Validation;

namespace Perutka.Eshop.Web.Models.Entity
{
    [Table(nameof(CarouselItem))]
    
    public class CarouselItem
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [ContentType("image")]
        [NotMapped]
        public IFormFile Image { get; set; }


        [StringLength(255)]
        [Required]
        public string ImageSource { get; set; }

        [StringLength(50)]
        public string ImageAlt { get; set; }
    }

   
}
