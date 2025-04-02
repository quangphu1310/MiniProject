using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MiniProject_API.Models.DTO
{
    public class ProductUpdateDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ISBN { get; set; }
        public string Author { get; set; }
        [Range(1, 1000)]
        public double Price { get; set; }
        public int CategoryId { get; set; }
    }
}
