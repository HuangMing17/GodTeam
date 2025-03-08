using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GodTeam.Models
{
    public class PostCategory
    {
        [Key]
        public int PostId { get; set; }
        public Post Post { get; set; }

        [Key]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}