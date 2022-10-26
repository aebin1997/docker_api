using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Domain.Entities;

namespace Infrastructure.Context
{
    public class SystemDBContext : DbContext // : DbContext 뭐가 다른지
    {
        /// <summary>
        /// 유저 테이블
        /// </summary>
        public DbSet<UserModel> User { get; set; } // Users 라고 해도되는지

        public SystemDBContext(DbContextOptions<SystemDBContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // base.OnModelCreating(modelBuilder);

            // #region DateTime Converter
            //
            // #endregion

            modelBuilder.Entity<UserModel>(builder =>
            {
                // primary key
                builder.HasKey(p => p.Idx);
                
                // let DB create values
                builder.Property(p => p.Idx).ValueGeneratedOnAdd();
            });
        }
    }
}