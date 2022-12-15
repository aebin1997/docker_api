using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("tb_user")]
    public class UserModel
    {
        [Column("id")]
        public int UserId { get; set; }

        [Required]
        [Column("username")] 
        public string Username { get; set; } 

        [Required]
        [Column("password")] 
        public string Password { get; set; } 

        [Required] 
        [Column("name")] 
        public string Name { get; set; } 

        [Required]
        [Column("created")]
        public ulong Created { get; set; }
        
        [Required]
        [Column("updated")]
        public ulong Updated { get; set; }

        [Required] 
        [Column("deleted")] 
        public bool Deleted { get; set; }
    }
    
    [Table("tb_course_info")]
    public class CourseModel
    {
        [Column("id")]
        public int CourseId { get; set; }

        [Required]
        [Column("course_name")] 
        public string CourseName { get; set; }
    }
    
    [Table("tb_user_info_by_course")]
    public class UserByCourseModel
    {
        [Column("id")]
        public int UserByCourseId { get; set; }
        
        [Required]
        [Column("user_id")] 
        public int UserId { get; set; }
        
        [Required]
        [Column("course_id")] 
        public int CourseId { get; set; }
        
        [Required]
        [Column("score")] 
        public int Score { get; set; }
        
        [Required]
        [Column("longest")] 
        public decimal Longest { get; set; }
        
        [Required]
        [Column("updated")] 
        public ulong Updated { get; set; }
    }
    
    [Table("tb_user_info_by_club")]
    public class UserByClubModel
    {
        [Required]
        [Column("user_id")] 
        public int UserId { get; set; }

        [Required]
        [Column("club")] 
        public string Club { get; set; }
        
        [Required]
        [Column("distance")] 
        public decimal Distance { get; set; }
        
        [Required]
        [Column("updated")] 
        public ulong Updated { get; set; }
    }
    
    [Table("tb_user_best_record")]
    public class UserBestRecordModel
    {
        [Required]
        [Column("user_id")] 
        public int UserId { get; set; }
        
        [Required]
        [Column("score")] 
        public int Score { get; set; }
        
        [Required]
        [Column("score_updated")] 
        public ulong ScoreUpdated { get; set; }
        
        [Required]
        [Column("longest")] 
        public decimal Longest { get; set; }
        
        [Required]
        [Column("longest_updated")] 
        public ulong LongestUpdated { get; set; }
    }
}