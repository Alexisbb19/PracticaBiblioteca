namespace PracticaBiblioteca.Models
{
    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int AñoPublicacion { get; set; }
        public int AutorId { get; set; }
        public int CategoriaId { get; set; }
        public string Resumen { get; set; } = string.Empty;

        public Autor? Autor { get; set; }
    }
}
