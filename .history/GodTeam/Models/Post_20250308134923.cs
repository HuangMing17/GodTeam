using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GodTeam.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime PublishedDate { get; set; }

        // Khóa ngoại đến bảng Category
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        // Khóa ngoại đến bảng User
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User Author { get; set; }
    }
}
