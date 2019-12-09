using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AccentureAcademyProyecto.Models
{
    public partial class Editorial
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]

        public Editorial()
        {
            this.LibrosDelEditorial = new HashSet<Libro>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "No se ha ingresado un Nombre")]
        [StringLength(30)]
        public string Nombre { get; set; }
        public string PaginaWeb { get; set; }
        public string Contacto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Libro> LibrosDelEditorial { get; set; }
    }
}
