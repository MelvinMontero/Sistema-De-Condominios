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
        public ActionResult CrearCobro(int? idCobro)
        {
            var cobros = new List<CobroModel>();
            using (var db = new PviProyectoFinalDB("MyDatabase"))
            {
                
            }
            return View(cobros);
        }
    }
}