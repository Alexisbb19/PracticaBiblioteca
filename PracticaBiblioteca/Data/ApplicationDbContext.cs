namespace PracticaBiblioteca.Data
{
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using Microsoft.EntityFrameworkCore;
    using PracticaBiblioteca.Models;
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Libro> Libros { get; set; }
        public DbSet<Autor> Autores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar relación entre Libro y Autor
            modelBuilder.Entity<Libro>()
                .HasOne(l => l.Autor)
                .WithMany(a => a.Libros)
                .HasForeignKey(l => l.AutorId);
        }
    }
}
