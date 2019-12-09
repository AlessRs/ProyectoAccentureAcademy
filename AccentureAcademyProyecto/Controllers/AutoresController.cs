using AccentureAcademyProyecto.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace AccentureAcademyProyecto.Controllers
{
    public class AutoresController : Controller
    {
        Libreria libreria = new Libreria();

        public ActionResult Index()
        {
            ViewBag.ListaGeneros = libreria.Generos.ToList();
            var collection = libreria.Autores.ToList();
            return View(collection);
        }
        public ActionResult Buscar(string PalabraClave, string Nombre)
        {
            if (String.IsNullOrEmpty(PalabraClave)) PalabraClave = "";
            if (String.IsNullOrEmpty(Nombre)) Nombre = PalabraClave;

            var autores = libreria.Editoriales.Where(ed =>
                    Nombre.Length == 0 ? false : ed.Nombre.Contains(Nombre)
                    ).ToList();

            return View("Index", autores);
        }

        public ActionResult Borrar(FormCollection form)
        {
            int formId = Convert.ToInt32(form["Id"]);
            Autor autor = libreria.Autores.Find(formId);
            try
            {
                libreria.Autores.Remove(autor);
                libreria.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                return Content("Nombre Equivocado");
            }

            return Content("ConExito");
        }
        [HttpPost]
        public ActionResult Editar(FormCollection form)
        {
            int formId = Convert.ToInt32(form["Id"]);

            Autor autor = libreria.Autores.First(lib => lib.Id == formId);

            autor.Nombre = form["Nombre"];
            try
            {
                libreria.Autores.Attach(autor);
                libreria.Entry(autor).State = EntityState.Modified;
                libreria.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                return Content("Error interno de validación");
            }
            return Content("ConExito");
        }
        [HttpPost]
        public ActionResult Agregar(FormCollection form)
        {
            int formId = Convert.ToInt32(form["Id"]);

            Autor nuevoAutor = new Autor()
            {
                Nombre = form["Nombre"]
            };

            try
            {
                libreria.Autores.Add(nuevoAutor);
                libreria.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                return Content("Error interno de validación");
            }
            return Content("Con Exito");
        }

        public ActionResult AgregarElementosEjemplo()
        {
            Data.GeneradorDeEntradas.Predeterminadas();
            var collection = libreria.Libros.ToList();
            return RedirectToAction("Index", collection);
        }
    }
}