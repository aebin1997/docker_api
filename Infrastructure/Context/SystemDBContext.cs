using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Context
{
    public class SystemDBContext : DbContext // : DbContext 뭐가 다른지
    {
        /// <summary>
        /// 유저 테이블
        /// </summary>
        public DbSet<UserModel> Users { get; set; } 

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
                    .HasKey(p => p.Idx);
                
                // let DB create values
                builder
                    .Property(p => p.Idx)
                    .ValueGeneratedOnAdd();
            });
        }
    }
}