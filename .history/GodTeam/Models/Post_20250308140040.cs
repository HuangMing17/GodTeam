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

        // Many-to-many relationship with Category through PostCategory
        public ICollection<PostCategory> PostCategories { get; set; }

        // Khóa ngoại đến bảng User
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User Author { get; set; }
    }
}
