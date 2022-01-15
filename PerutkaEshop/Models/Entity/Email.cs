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

    [Table(nameof(Email))]

    public class Email
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [StringLength(255)]
        [Required]
        public string Name { get; set; }       

        [StringLength(255)]
        public string Body { get; set; }        


    }
}
