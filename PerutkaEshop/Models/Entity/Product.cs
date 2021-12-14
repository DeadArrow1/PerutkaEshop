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

        [Table(nameof(Product))]

        public class Product
        {
            [Key]
            [Required]
            public int ID { get; set; }

            [StringLength(255)]
            [Required]
            public string Name { get; set; }

            [Required]
            public int Number { get; set; }

            [Required]
            public int Price { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

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

