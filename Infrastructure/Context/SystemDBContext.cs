using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Context
{
    // TODO: [박예빈] : DbContext 뭐가 다른지 (해결)
    // TODO: [20221106-권용진] :(콜론) 뒤에 있는 DbContext는 EF Core에서 제공해주는 클래스입니다.
    // TODO: [20221106-권용진] DB와 관련된 많은 기능들이 포함되어 있는 클래스입니다. connection 연결부터 해제, 데이터 조회 및 처리를 위한 요청 등 무수히 많은 기능들이 포함되어 있어서 우리가 연결하려는 DBContext를 선언한 후 해당 클래스를 상속받으면 간편하게 DB와 통신할 수 있게됩니다.
    public class SystemDBContext : DbContext 
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

                builder
                    .Property(p => p.Updated)
                    .ValueGeneratedOnUpdate()
                    .ValueGeneratedOnAdd();
                    
                // let DB create values
                builder
                    .Property(p => p.Idx)
                    .ValueGeneratedOnAdd();
            });
        }
    }
}