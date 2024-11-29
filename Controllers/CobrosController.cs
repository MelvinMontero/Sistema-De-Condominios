using DataModels;
using SistemaDeCondominios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static DataModels.PviProyectoFinalDBStoredProcedures;

namespace SistemaDeCondominios.Controllers
{
    public class CobrosController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["idPersona"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var cobros = new List<CobroModel>();
            using (var db = new PviProyectoFinalDB("MyDatabase"))
            {
                List<SpObtenerCobrosResult> spResult = new List<SpObtenerCobrosResult>();
                if (Session["EsEmpleado"]?.ToString() == "Empleado")
                {
                     spResult = db.SpObtenerCobros(null).ToList();
                }
                else if (Session["EsEmpleado"]?.ToString() != "Empleado")
                {
                    int idPersona = Convert.ToInt32(Session["idPersona"]);
                    spResult = db.SpObtenerCobros(idPersona).ToList();
                }

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
        [HttpGet]
        public ActionResult ConsultarCobros(int? id)
        {

            if (Session["idPersona"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            CobroDetalleViewModel viewModel = new CobroDetalleViewModel();

            try
            {
                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {
                    // Obtener el usuario autenticado y validar si es empleado
                    string usuarioActual = User.Identity.Name;
                    bool esEmpleado = User.IsInRole("Empleado");

                    // Obtener datos del cobro
                    viewModel.Cobro = db.SpObtenerCobroPorId(id.Value)
                        .Select(_ => new CobroModel
                        {
                            idCobro = _.Id_cobro,
                            nombreCasa = _.Nombre_casa,
                            Propietario = _.Propietario,
                            Periodo = _.Periodo,
                            Monto = _.Monto,
                            estado = _.Estado,
                            //FechaCancelacion = _.Fecha_cancelacion
                        })
                        .FirstOrDefault();

                    if (viewModel.Cobro == null || (!esEmpleado && viewModel.Cobro.Propietario != usuarioActual))
                    {
                        // Redirigir si el cobro no existe o no pertenece al cliente
                        return RedirectToAction("MisCobros", "Cobro");
                    }

                    // Obtener bitácoras asociadas al cobro específico
                    viewModel.Bitacoras = db.SpObtenerBitacora()
                        .Where(b => b.IdCobro == id.Value)
                        .OrderByDescending(b => b.IdBitacora)
                        .Select(b => new BitacoraModel
                        {
                            idCobro = b.IdCobro,
                            IdBitacora = b.IdBitacora,
                            Detalle = b.Detalle,
                            IdUser = b.IdUser,
                            Fecha = b.Fecha
                        })
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return RedirectToAction("Index", "Cobro");
            }

            // Retornar la vista con el modelo de datos
            return View(viewModel);
        }
        [HttpGet]
        public ActionResult CrearCobro()
        {
            if (Session["idPersona"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
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
                    // Validación de campos obligatorios
                    if (cobro.idPersona == 0 || cobro.idCasa == 0 || cobro.anno == 0 || cobro.mes == 0)
                    {
                        resultado = "Por favor complete todos los campos requeridos.";
                        ViewBag.Resultado = resultado;
                        return View(cobro);
                    }

                    // Verificar si se seleccionó al menos un servicio
                    /*if (!cobro.Seguridad && !cobro.Agua && !cobro.Luz && !cobro.Internet)
                    {
                        resultado = "Debe seleccionar al menos un servicio.";
                        ViewBag.Resultado = resultado;
                        return View(cobro);
                    }*/

                    // Pasar los servicios seleccionados al procedimiento de inserción
                    var serviciosSeleccionados = new List<int>();
                    if (cobro.Seguridad) serviciosSeleccionados.Add(5);
                    if (cobro.Agua) serviciosSeleccionados.Add(6);
                    if (cobro.Luz) serviciosSeleccionados.Add(7);
                    if (cobro.Internet) serviciosSeleccionados.Add(8);

                    // Insertar cobros para los servicios seleccionados
                    foreach (var servicio in serviciosSeleccionados)
                    {
                        db.SpCrearCobro(cobro.idPersona, cobro.idCasa, cobro.anno, cobro.mes, servicio);
                    }

                    resultado = "Se ha guardado exitosamente.";
                }
            }
            catch (Exception ex)
            {
                resultado = "No se ha insertado correctamente: " + ex.Message;
            }

            ViewBag.Resultado = resultado;
            return View(cobro);
        }

        [HttpPost]
        public JsonResult FiltrarCobros(string idPersona, string mesCobro, int annoCobro)
        {
            var cobros = new List<CobroModel>();
            using (var db = new PviProyectoFinalDB("MyDatabase"))
            {
                var spResult = db.SpObtenerCobros(null).ToList();

                if (!string.IsNullOrEmpty(idPersona))
                {
                    spResult = spResult.Where(c => c.Id_persona.ToString() == idPersona).ToList();
                }

                if (!string.IsNullOrEmpty(mesCobro) && int.TryParse(mesCobro, out int mes))
                {
                    spResult = spResult.Where(c => c.Mes == mes).ToList();
                }

                if (annoCobro > 0)
                {
                    spResult = spResult.Where(c => c.Anno == annoCobro).ToList();
                }

                cobros = spResult.Select(c => new CobroModel
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

            return Json(cobros);
        }
        [HttpPost]
        public JsonResult Clientes()
        {
            var list = new List<Dropdown>();
            using (var db = new PviProyectoFinalDB("MyDatabase"))
            {
                list = db.RetornarPersonas().Select(_ => new Dropdown { Id=_.IdPersona, Nombre=_.Nombre}).ToList();
            }
            return Json(list);
        } 

        public JsonResult Casas(int idPersona)
        {
            var list = new List<Dropdown>();
            using (var db = new PviProyectoFinalDB("MyDatabase"))
            {
                list = db.ObtenerCasasPorCliente(idPersona).Select(_ => new Dropdown
                {
                    Id = _.Id_casa,
                    Nombre = _.Nombre_casa
                }).ToList();
            }
            return Json(list);
        }

    }
}