using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("tb_user")]
    public class UserModel
    {
        [Column("idx")]
        public int Idx { get; set; }

        [Required]
        [Column("user_id")] 
        public string UserId { get; set; } = "";

        [Required]
        [Column("user_pw")] 
        public string UserPw { get; set; } = "";
        
        [Column("life_best_score")]
        public int? LifeBestScore { get; set; }
        
        [Required]
        [Column("created")]
        public DateTime Created { get; set; }
        
        [Required]
        [Column("updated")]
        public DateTime Updated { get; set; }

        [Required] 
        [Column("deleted")] 
        public bool Deleted { get; set; } = false;
    }
}