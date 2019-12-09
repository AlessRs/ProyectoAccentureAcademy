using AccentureAcademyProyecto.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccentureAcademyProyecto.Controllers
{
    public class EditorialesController : Controller
    {
        Libreria libreria = new Libreria();

        public ActionResult Index()
        {
            ViewBag.ListaGeneros = libreria.Generos.ToList();
            var collection = libreria.Editoriales.ToList();
            return View(collection);
        }
        public ActionResult Buscar(string PalabraClave, string Nombre, string PaginaWeb, string Contacto)
        {
            if (String.IsNullOrEmpty(PalabraClave)) PalabraClave = "";
            if (String.IsNullOrEmpty(Nombre)) Nombre = PalabraClave;
            if (String.IsNullOrEmpty(PaginaWeb)) PaginaWeb = PalabraClave;
            if (String.IsNullOrEmpty(Contacto)) Contacto = PalabraClave;

            var editoriales = libreria.Editoriales.Where(ed =>
                    PaginaWeb.Length == 0 ? false : ed.Nombre.Contains(PaginaWeb) &&
                    Contacto.Length == 0 ? false : ed.Contacto.Contains(Contacto)
                    ).ToList();

            return View("Index", editoriales);
        }
        public ActionResult ValAutor(string values)
        {
            if (values.Length == 0) Content("true");
            foreach (var value in values.Split(','))
            {
                if (libreria.Autores.FirstOrDefault(au => au.Nombre == value) == null) return Content("false");
            }
            return Content("true");
        }
        public ActionResult ValEditorial(string values)
        {
            if (values.Length == 0) Content("true");
            foreach (var value in values.Split(','))
            {
                if (libreria.Editoriales.FirstOrDefault(au => au.Nombre == value) == null) return Content("false");
            }
            return Content("true");
        }

        public ActionResult Borrar(FormCollection form)
        {
            int formId = Convert.ToInt32(form["Id"]);
            Editorial editorial = libreria.Editoriales.Find(formId);
            try
            {
                libreria.Editoriales.Remove(editorial);
                libreria.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                return Content("Parametros no válidos");
            }

            return Content("ConExito");
        }
        [HttpPost]
        public ActionResult Editar(FormCollection form)
        {
            int formId = Convert.ToInt32(form["Id"]);

            Editorial editorial = libreria.Editoriales.First(lib => lib.Id == formId);

            editorial.Nombre = form["Nombre"];
            editorial.PaginaWeb = form["PaginaWeb"];
            editorial.Contacto = form["Contacto"];
            try
            {
                libreria.Editoriales.Attach(editorial);
                libreria.Entry(editorial).State = EntityState.Modified;
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

            Editorial nuevaEditorial = new Editorial()
            {
                Nombre = form["Nombre"],
                PaginaWeb = form["PaginaWeb"],
                Contacto = form["Contacto"],
            };

            try
            {
                libreria.Editoriales.Add(nuevaEditorial);
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