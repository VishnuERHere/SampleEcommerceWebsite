using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SampleEcommerceWebsite.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [DisplayName("Product Name")]
        public string Name { get; set; }
        [Required]
        [MaxLength(250)]
        [DisplayName("Product Description")]
        public string Description { get; set; }
        [Required]
        [DisplayName("Brand")]
        public string Brand { get; set; }
        [Required]
        [DisplayName("List Price")]
        [Range(0, 10000)]
        public double ListPrice { get; set; }
        [Required]
        [DisplayName("Price for 1-50")]
        [Range(0, 10000)]
        public double Price { get; set; }
        [Required]
        [DisplayName("Price for 50+")]
        [Range(0, 10000)]
        public double Price50 { get; set; }

        [Required]
        [DisplayName("Price for 100+")]
        [Range(0, 10000)]
        public double Price100 { get; set; }

        public int CategoryId { get; set; } //Foreign Key
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; } // Navigation property
        [ValidateNever]
        public string ImageUrl { get; set; }


    }
}
