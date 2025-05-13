using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Foy5Wpf.Models;
using System.Reflection.Emit;
using Pomelo.EntityFrameworkCore.MySql;
        
namespace Foy5Wpf
{   
    public class OkulContext : DbContext
    {
        public DbSet<tFakulte> tFakulteler { get; set; }
        public DbSet<tBolum> tBolumler { get; set; }        
        public DbSet<tOgrenci> tOgrenciler { get; set; }
        public DbSet<tDers> tDersler { get; set; }
        public DbSet<tOgrenciDers> tOgrenciDersler { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));

            optionsBuilder.UseMySql(
                "Server=localhost;Database=OkulDb;User=root;Password=password;", // Password gelecek
                serverVersion
            );
        } 
        //mysql -u root -p

        //Microsoft SQL Server Konfigürasyonu
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(
        //        "Server=.;Database=OkulDb;Integrated Security=True;TrustServerCertificate=True;"); // Windows Auth
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tFakulte>().ToTable("tFakulte");
            modelBuilder.Entity<tBolum>().ToTable("tBolum");
            modelBuilder.Entity<tOgrenci>().ToTable("tOgrenci");
            modelBuilder.Entity<tDers>().ToTable("tDers");
            modelBuilder.Entity<tOgrenciDers>().ToTable("tOgrenciDers");
        }
    }
}
