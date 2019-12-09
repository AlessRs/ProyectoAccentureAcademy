using AccentureAcademyProyecto.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace AccentureAcademyProyecto.Controllers
{
    public class HomeController : Controller
    {
        Libreria libreria = new Libreria();

        public ActionResult Index()
        {
            ViewBag.ListaGeneros = libreria.Generos.ToList();
            var collection = libreria.Libros.ToList();
            return View(collection);
        }
        public ActionResult Buscar(string PalabraClave, string Titulo, string Autor, string Editorial, string ISBN, string Genero, string Edicion)
        {

            IEnumerable<Libro> libros = new List<Libro>();

            if (!String.IsNullOrEmpty(PalabraClave))
            {
                libros = libros.Where(lib =>
                        lib.Titulo.Contains(PalabraClave) &&
                        lib.Autores.FirstOrDefault(au => au.Nombre.Contains(PalabraClave)) != null &&
                        lib.Editoriales.FirstOrDefault(ed => ed.Nombre.Contains(PalabraClave)) != null &&
                        lib.ISBN.Contains(PalabraClave) &&
                        lib.Genero.Nombre.Contains(PalabraClave) || lib.Genero.GeneroPadre.Nombre.Contains(PalabraClave)
                        ).ToList();
            }
            if (!String.IsNullOrEmpty(Titulo))
            {
                libros = libros.Where(lib => lib.Titulo.Contains(Titulo)).ToList();
            }
            if (!String.IsNullOrEmpty(Autor))
            {
                libros = libros.Where(lib => lib.Autores.FirstOrDefault(au => au.Nombre.Contains(Autor)) != null).ToList();
            }
            if (!String.IsNullOrEmpty(Editorial))
            {
                libros = libros.Where(lib => lib.Editoriales.FirstOrDefault(ed => ed.Nombre.Contains(Editorial)) != null).ToList();
            }
            if (!String.IsNullOrEmpty(ISBN))
            {
                libros = libros.Where(lib => lib.ISBN.Contains(ISBN)).ToList();
            }
            if (!String.IsNullOrEmpty(Genero))
            {
                libros = libros.Where(lib => lib.Genero.Nombre.Contains(Genero) || lib.Genero.GeneroPadre.Nombre.Contains(Genero)).ToList();
            }

            ViewBag.ListaGeneros = libreria.Generos.ToList();
            return View("Index", libros);
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
            Libro libro = libreria.Libros.Find(formId);
            libreria.Libros.Remove(libro);
            libreria.SaveChanges();
            return Content("Done");
        }
        [HttpPost]
        public ActionResult Editar(FormCollection form)
        {
            int formId = Convert.ToInt32(form["Id"]);
            string formGenero = form["Genero"];
            string[] formAutor = form["Autor"].Split(',');
            for (var i = 0; i < formAutor.Length; i++)
            {
                formAutor[i] = formAutor[i].Trim();
            }
            string[] formEditorial = form["Editorial"].Split(',');
            for (var i = 0; i < formEditorial.Length; i++)
            {
                formEditorial[i] = formEditorial[i].Trim();
            }
            Libro libro = libreria.Libros.First(lib => lib.Id == formId);
            libro.Autores = new HashSet<Autor>();
            foreach (var nombreAutor in formAutor)
            {
                Autor autor = libreria.Autores.FirstOrDefault(au => au.Nombre == nombreAutor);
                if (autor == null)
                {
                    autor = new Autor() { Nombre = nombreAutor };
                    libreria.Autores.Add(autor);
                }
                libro.Autores.Add(autor);
            }
            libro.Editoriales = new HashSet<Editorial>();
            foreach (var nombreEditorial in formEditorial)
            {
                Editorial editorial = libreria.Editoriales.FirstOrDefault(ed => ed.Nombre == nombreEditorial);
                if (editorial == null)
                {
                    editorial = new Editorial() { Nombre = nombreEditorial };
                    libreria.Editoriales.Add(editorial);
                }
                libro.Editoriales.Add(editorial);
            }

            libro.Titulo = form["Titulo"];
            libro.ISBN = form["ISBN"];
            libro.Genero = libreria.Generos.First((g) => g.Nombre == formGenero);
            libro.Edicion = Convert.ToInt32(form["Edicion"]);

            try
            {
                libreria.Libros.Attach(libro);
                libreria.Entry(libro).State = EntityState.Modified;
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
            string formGenero = form["Genero"];
            string[] formAutor = form["Autor"].Split(',');
            for (var i = 0; i < formAutor.Length; i++)
            {
                formAutor[i] = formAutor[i].Trim();
            }
            string[] formEditorial = form["Editorial"].Split(',');
            for (var i = 0; i < formEditorial.Length; i++)
            {
                formEditorial[i] = formEditorial[i].Trim();
            }

            Libro nuevoLibro = new Libro()
            {
                Titulo = form["Titulo"],
                ISBN = form["ISBN"],
                Edicion = Convert.ToInt32(form["Edicion"]),
                Genero = libreria.Generos.First(gen => gen.Nombre == formGenero),
            };
            foreach (var nombreAutor in formAutor)
            {
                Autor autor = libreria.Autores.FirstOrDefault(au => au.Nombre == nombreAutor);
                if (autor == null || autor.Nombre.Equals(" "))
                {
                    autor = new Autor() { Nombre = nombreAutor };
                    libreria.Autores.Add(autor);
                }
                nuevoLibro.Autores.Add(autor);
            }
            foreach (var nombreEditorial in formEditorial)
            {
                Editorial editorial = libreria.Editoriales.FirstOrDefault(ed => ed.Nombre == nombreEditorial);
                if (editorial == null || editorial.Nombre.Equals(" "))
                {
                    editorial = new Editorial() { Nombre = nombreEditorial };
                    libreria.Editoriales.Add(editorial);
                }
                nuevoLibro.Editoriales.Add(editorial);
            }
            try
            {
                libreria.Libros.Add(nuevoLibro);
                libreria.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                return Content("Error se han ingresado valores invalidos en los campos");
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