using Microsoft.EntityFrameworkCore;
using PM.Infrastructure.EF.Entities;

namespace PM.Infrastructure.EF.Context
{
    public class PMContext : DbContext, IPMContext
    {
        public PMContext(DbContextOptions<PMContext> options) : base(options)
        {

        }
        public DbSet<PersonEntity> People { get; set; }
        public DbSet<PeopleRelationEntity> PeopleRelations { get; set; }
        public DbSet<CityEntity> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PeopleRelationEntity>()
                .HasOne(e => e.Person)
                .WithMany(e => e.Relations);

            modelBuilder.Entity<PeopleRelationEntity>()
                .HasOne(e => e.RelatedPerson)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
