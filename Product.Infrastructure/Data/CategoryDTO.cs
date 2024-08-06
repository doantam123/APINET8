using System.ComponentModel.DataAnnotations;

namespace Product.Infrastructure.Data
{
    public class CategoryDTO
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ListCategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
