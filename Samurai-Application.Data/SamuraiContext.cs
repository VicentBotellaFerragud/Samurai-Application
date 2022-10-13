using Microsoft.EntityFrameworkCore;
using Samurai_Application.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samurai_Application.Data
{
    public class SamuraiContext : DbContext
    {
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = SamuraiApplicationData");  
            base.OnConfiguring(optionsBuilder); 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //First identifies the relationship directly between Samurai and Battle.
            modelBuilder.Entity<Samurai>()
                .HasMany(s => s.Battles)
                .WithMany(b => b.Samurais)

                //Then makes the context use the BattleSamurai class instead of infering the join.
                .UsingEntity<BattleSamurai>
                (bs => bs.HasOne<Battle>().WithMany(), //We express the relationship between the new class and the two that it is joining.
                bs => bs.HasOne<Samurai>().WithMany())

                //The table BattleSamurai (in the database) will have a new column with the BattleSamurai property "DateJoined".
                .Property(bs => bs.DateJoined) 
                .HasDefaultValueSql("getDate()");
        }
    }
}
