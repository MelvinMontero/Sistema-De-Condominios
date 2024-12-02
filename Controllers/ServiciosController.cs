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
                    Categoria = _.NombreCategoria
                }).ToList();
                return View(list);
            }    
        }
        public ActionResult InsertarServicio(int? Id)
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
            var servicios = new ServiciosModel();        
            using (var db = new PviProyectoFinalDB("MyDatabase"))
            {
                if (Id != null)
                {
                    servicios = db.SpRetornaServicios(Id).Select(_ => new ServiciosModel
                    {
                        IdServicio = _.Id_servicio,
                        Nombre = _.Nombre,
                        Descripcion = _.Descripcion,
                        Precio = _.Precio,
                        IdCategoria = _.Id_categoria,
                        Estado = _.Estado,
                        Categoria = _.NombreCategoria,
                        EstadoCobro = _.EstadoCobro
                    }).FirstOrDefault();
                }
                else
                {       
                    servicios = null;
                }                
            }
            return View(servicios);
        }
        [HttpPost]
        public ActionResult InsertarServicio(ServiciosModel servicio)
        {
            string mensaje;
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
                        db.SpModificarServicio(servicio.IdServicio, servicio.Descripcion, servicio.Precio);
                        return RedirectToAction("Exito", "Servicios", new { mensaje = "El servicio ha sido modificado exitosamente." });
                    }
                }
            }
            catch (Exception) {
                mensaje = "El servicio no puede tener un nombre igual al de un servicio existente.";
            }
            ViewBag.Mensaje = mensaje;
            return View();
        }
        public ActionResult Inactivar(int? Id)
        {
            using (var db = new PviProyectoFinalDB("MyDatabase"))
            {
                db.SpInactivarServicio(Id);
            }
            return RedirectToAction("Exito", "Servicios", new { mensaje = "El servicio ha sido inactivado exitosamente." });
        }
        
        public ActionResult Exito(string mensaje)
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
            if (!string.IsNullOrEmpty(mensaje))
            {
                ViewBag.Mensaje = mensaje;
            }
            return View();
        }
        public JsonResult Categorias()
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