using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace AccentureAcademyProyecto.Models
{
    public class Libreria : DbContext
    {
        //Source Home
        //Data Source=(localdb)\MSSQLLocalDB
        //Source EducacionIT
        //Data Source = PC-10-03-15\SQLEXPRESS
        public Libreria()
            : base(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AccentureAcademyProyecto;Integrated Security=True")
        {

        }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Editorial> Editoriales { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Genero> Generos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}