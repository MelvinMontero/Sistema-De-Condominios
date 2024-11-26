using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static DataModels.PviProyectoFinalDBStoredProcedures;
using SistemaDeCondominios.Models;

namespace SistemaDeCondominios.Controllers
{
    public class ServiciosController : Controller
    {
        
        public ActionResult Index()
        {
            if (Session["idPersona"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                if (Session["EsEmpleado"]?.ToString() != "Empleado")
                {
                    return RedirectToAction("Index", "Cobros");
                }
            }
            using (var db = new PviProyectoFinalDB("MyDatabase"))
            {
                List<SpRetornaServiciosResult> spResult = new List<SpRetornaServiciosResult>();
                spResult = db.SpRetornaServicios(null).ToList();
                var list = spResult.Select(_ => new ServiciosModel
                {
                    IdServicio = _.Id_servicio,
                    Nombre = _.Nombre,
                    Descripcion = _.Descripcion,
                    Precio = _.Precio,
                    IdCategoria = _.Id_categoria,
                    Estado = _.Estado,
                    Categoria = _.NombreCategoria,
                    CategoriaDescripcion = _.CategoriaDescripcion,
                    EstadoCategoria = _.EstadoCategoria
                }).ToList();
                return View(list);
            }    
        }

        [HttpPost]
        public ActionResult InsertarServicio(ServiciosModel servicio)
        {    
            try
            {
                using(var db = new PviProyectoFinalDB("MyDatabase"))
                {
                    if (servicio.IdServicio == 0)
                    {
                        db.SpCrearServicio(servicio.Nombre, servicio.Descripcion, servicio.Precio, servicio.IdCategoria);
                        return RedirectToAction("Exito", "Servicios", new { mensaje = "El servicio ha sido creado exitosamente." });
                    }
                    else
                    {
                        
                        return RedirectToAction("Exito", "Servicios", new { mensaje = "El servicio ha sido insertado exitosamente." });

                    }
                }
            }
            catch (Exception) {
                ViewBag.Resultado = "Ha ocurrido un error";
            }
            return View();
        }
        public JsonResult RetornaCategorias()
        {
            var list = new List<Dropdown>();
            using (var db = new PviProyectoFinalDB("MyDatabase"))
            {
                list = db.RetornarCategorias().Select(_ => new Dropdown
                {
                    Id = _.Id_categoria,
                    Nombre = _.Nombre
                }).ToList();
            }
            return Json(list);
        }
    }
}