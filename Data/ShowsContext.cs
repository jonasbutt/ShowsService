using Microsoft.EntityFrameworkCore;
using ShowsService.Data.Model;

namespace ShowsService.Data
{
    public class ShowsContext : DbContext
    {
        public ShowsContext(DbContextOptions<ShowsContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ShowCastMember>()
                        .HasKey(x => new { x.ShowId, x.CastMemberId });
        }

        public DbSet<Show> Shows { get; set; }

        public DbSet<CastMember> CastMembers { get; set; }
    }
}
