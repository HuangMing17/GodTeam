using System.ComponentModel.DataAnnotations;

namespace GodTeam.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        // Tự liên kết để tạo cấu trúc cây cho danh mục
        public int? ParentCategoryId { get; set; }
        public Category ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
