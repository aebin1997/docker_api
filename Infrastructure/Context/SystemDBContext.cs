using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Context
{
    public class SystemDBContext : DbContext 
    {
        public DbSet<UserModel> Users { get; set; } 
        
        public DbSet<CourseModel> Courses { get; set; }
        
        public DbSet<UserByCourseModel> UsersByCourse { get; set; }
        
        public DbSet<UserByClubModel> UsersByClub { get; set; }
        
        public DbSet<UserBestRecordModel> UsersBestRecord { get; set; }
        
        public SystemDBContext(DbContextOptions<SystemDBContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 기본적으로 제공해주는 OnModelCreating 메서드는 아무런 동작이 없는 메서드이기 때문에 호출해도되고 안해도된다.
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserModel>(builder =>
            {
                // primary key
                builder
                    .HasKey(p => p.UserId);

                // let DB create values
                builder
                    .Property(p => p.UserId)
                    .ValueGeneratedOnAdd();
            });
            
            modelBuilder.Entity<CourseModel>(builder =>
            {
                // primary key
                builder
                    .HasKey(p => p.CourseId);

                // let DB create values
                builder
                    .Property(p => p.CourseId)
                    .ValueGeneratedOnAdd();
            });
            
            modelBuilder.Entity<UserByCourseModel>(builder =>
            {
                // primary key
                builder
                    .HasKey(p => p.UserByCourseId);
                
                // let DB create values
                builder
                    .Property(p => p.UserByCourseId)
                    .ValueGeneratedOnAdd();
            });
            
            modelBuilder.Entity<UserByClubModel>(builder =>
            {
                // primary key
                builder
                    .HasKey(p => new { p.UserId, p.Club });
            });
            
            modelBuilder.Entity<UserBestRecordModel>(builder =>
            {
                // primary key
                builder
                    // .HasNoKey();
                .HasKey(p => p.UserId);
            });
        }
    }
}