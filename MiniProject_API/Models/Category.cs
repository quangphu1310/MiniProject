using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MiniProject_API.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Category Name")]
        [MaxLength(30)]
        public string Name { get; set; }
    }
}
