using DataModels;
using SistemaDeCondominios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeCondominios.Controllers
{
    public class CobrosController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var cobros = new List<CobroModel>();
            using (var db = new PviProyectoFinalDB("MyDatabase"))
            {
                var spResult = db.SpObtenerCobros().ToList();
                cobros = spResult.Select(c  => new CobroModel 
                { 
                    idCobro = c.Id_cobro,
                    nombreCasa = c.Nombre_casa,
                    idPersona = c.Id_persona,
                    nombrePersona = c.Nombre,
                    apellidoPersona = c.Apellido,
                    mes = c.Mes,
                    anno = c.Anno,
                    estado = c.Estado
                }).ToList();
            }
            return View(cobros);
        }
        public ActionResult ConsultarCobro(int? idCobro)
        {
            return View();
        }

        [HttpPost]
        public ActionResult CrearCobro(CobroModel cobro)
        {
            string resultado = string.Empty;
            try
            {
                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {
                    cobro = db.SpCrearCobro(cobro.idPersona, cobro.idCasa, cobro.anno, cobro.mes, cobro.idServicio);
                }
            }
            catch (Exception ex) 
            { 
            
            }
            return View();
        }
        public JsonResult Clientes()
        {
            var list = new List<Dropdown>();
            using (var db = new PviProyectoFinalDB("MyDatabase"))
            {
                list = db.RetornarPersonas().Select(_ => new Dropdown { Id=_.IdPersona, Nombre=_.Nombre}).ToList();
            }
            return Json(list);
        }
        public JsonResult Casas()
        {
            var list = new List<Dropdown>();
            using (var db = new PviProyectoFinalDB("MyDatabase"))
            {
                list = db.RetornarCasas().Select(_ => new Dropdown { Id = _.IdCasa, Nombre = _.NombreCasa }).ToList();
            }
            return Json(list);
        }

    }
}