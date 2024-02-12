using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder){}


        // Переопределел метод конфигурации
        // Указывает где у нас будут прописываться миграции
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite(b => b.MigrationsAssembly("ScientificExperiment.WebAPI"));

        public DbSet<Entities.File> Files { get; set; } = null!;
        public DbSet<Value> Values { get; set; } = null!;
        public DbSet<Result> Results { get; set; } = null!;
    }
}
