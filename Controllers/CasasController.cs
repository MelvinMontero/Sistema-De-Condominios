using DataModels;
using LinqToDB.Tools;
using SistemaDeCondominios.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Services.Description;
using static DataModels.PviProyectoFinalDBStoredProcedures;

namespace SistemaDeCondominios.Controllers
{
    public class CasasController : Controller
    {
        public ActionResult Index()
        {
            if (Session["idPersona"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                if(Session["EsEmpleado"]?.ToString() != "Empleado")
                {
                    return RedirectToAction("Index", "Cobros");
                }
            }
            using (var db = new PviProyectoFinalDB("MyDatabase"))
            {
                List<SpObtenerCasasResult> spResult = new List<SpObtenerCasasResult>();
                spResult = db.SpObtenerCasas(null).ToList();
                var list = spResult.Select(_ => new CasasModel
                {
                    IdCasa = _.Id_casa,
                    NombreCasa = _.Nombre_casa,
                    mtsCuadrados = _.Metros_cuadrados,
                    nHabitaciones = _.Numero_habitaciones,
                    nBanos = _.Numero_banos,
                    NombreCliente = _.Nombre,
                    ApellidoCliente = _.Apellido,
                    FechaConstruccion = _.Fecha_construccion,
                    Estado = _.Estado
                }).ToList();
                return View(list);
            }              
        }
        public ActionResult ModificarCasa(int? Id)
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
            var casas = new CasasModel();
            using (var db = new PviProyectoFinalDB("MyDatabase"))
            {
                casas = db.SpObtenerCasas(Id).Select(_ => new CasasModel
                {
                    IdCasa = _.Id_casa,
                    NombreCasa = _.Nombre_casa,
                    mtsCuadrados = _.Metros_cuadrados,
                    nHabitaciones = _.Numero_habitaciones,
                    nBanos=_.Numero_banos,
                    NombreCliente=_.Nombre,
                    ApellidoCliente=_.Apellido,
                    FechaConstruccion=_.Fecha_construccion,
                    EstadoCobro = _.EstadoPago,
                    Estado=_.Estado
                }).FirstOrDefault();
                if (casas == null)
                {
                    return RedirectToAction("Index", "Casas");
                }
            }
            return View(casas);
        }
        [HttpPost]
        public ActionResult ModificarCasa(CasasModel casa)
        {
            try
            {
                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {
                    if (casa.IdCasa == 0)
                    {
                        db.SpCrearCasa(casa.NombreCasa, casa.mtsCuadrados, casa.nHabitaciones, casa.nBanos, casa.IdCliente, casa.FechaConstruccion);
                        return Json(new { success = true, message = "La casa ha sido insertada exitosamente." });
                    }
                    else
                    {
                        db.SpModificarCasas(casa.IdCasa, casa.nHabitaciones, casa.nBanos);
                        return RedirectToAction("Exito", "Casas", new { mensaje = "La casa ha sido insertada exitosamente." });

                    }
                }
            }
            catch
            {
                ViewBag.Resultado = "Ha ocurrido un error";
            }
            return View();
        }
        public ActionResult InactivarCasa(int? id)
        {
            using (var db = new PviProyectoFinalDB("MyDatabase"))
            {
                db.SpInactivarCasa(id);
            }
            return RedirectToAction("Exito", "Casas", new { mensaje = "La casa ha sido inactivada exitosamente." });
        }

        public JsonResult RetornaClientes()
        {
            var list = new List<Dropdown>();
            using (var db = new PviProyectoFinalDB("MyDatabase"))
            {
                list = db.RetornarPersonas().Select(_ => new Dropdown 
                {
                    Id = _.IdPersona, 
                    Nombre = $"{_.Nombre} {_.Apellido}"
                }).ToList();
            }
            return Json(list);
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

    }
}