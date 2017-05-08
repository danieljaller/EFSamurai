using System;
using EFSamurai.Data.Migrations;
using EFSamurai.Domain;
using Microsoft.EntityFrameworkCore;
using Battle = EFSamurai.Domain.Battle;

namespace EFSamurai.Data
{
    public class SamuraiContext : DbContext
    {
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<Domain.SamuraiBattle> SamuraiBattles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server = (localdb)\\mssqllocaldb; Database = EfSamurai; Trusted_Connection = True; ");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.SamuraiBattle>().HasKey(t => new {t.SamuraiId, t.BattleId});
        }
    }
}
