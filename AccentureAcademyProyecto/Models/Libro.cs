using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccentureAcademyProyecto.Models
{
    public partial class Libro
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Libro()
        {
            this.Autores = new HashSet<Autor>();
            this.Editoriales = new HashSet<Editorial>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(13, MinimumLength = 10, ErrorMessage = "El ISBN debe ser de 10 o 13 digitos")]
        [Required(ErrorMessage = "Falta el ISBN")]
        public string ISBN { get; set; }
        [Required(ErrorMessage = "No se ha ingresado un Titulo")]
        public string Titulo { get; set; }
        public int Edicion { get; set; }
        [Required(ErrorMessage = "No se ha ingresado un Genero")]
        public Genero Genero { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Required(ErrorMessage = "No se ha ingresado un Autor")]
        public virtual ICollection<Autor> Autores { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Required(ErrorMessage = "No se ha ingresado una Editorial")]
        public virtual ICollection<Editorial> Editoriales { get; set; }
    }
}
