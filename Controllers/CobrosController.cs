using DataModels;
using SistemaDeCondominios.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms.VisualStyles;
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


            // Verificar si la sesión no está activa o si no tiene los valores requeridos
            if (Session["idPersona"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (cobro.estado == "Eliminado" || (cobro.fechapagada.HasValue && cobro.fechapagada.Value < DateTime.Now))
            {
                TempData["ErrorMessage"] = "No se puede consultar este cobro debido a su estado o fecha de pago.";
                return RedirectToAction("Index", "Cobros");  // Redirigir si el estado es "Eliminado" o la fecha de pago es menor a la actual
            }

            // Crear el modelo de vista para la acción
            CobroDetalleViewModel viewModel = new CobroDetalleViewModel();

            try
            {
                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {
                    // Obtener el ID de la persona autenticada desde la sesión
                    int idPersonaAutenticada = (int)Session["idPersona"];
                    string tipoPersona = (string)Session["EsEmpleado"];

                    // Verificar si el id es nulo o no válido
                    if (!id.HasValue)
                    {
                        TempData["ErrorMessage"] = "El ID de cobro no es válido.";
                        return RedirectToAction("Index", "Cobros");  // Redirigir a la página principal si el id no es válido
                    }

                    // Obtener los datos del cobro por ID
                    var cobro = db.SpObtenerCobroPorId(id.Value)
                                  .Select(_ => new CobroModel
                                  {
                                      idCobro = _.Id_cobro,
                                      nombreCasa = _.Nombre_casa,
                                      Propietario = _.Propietario,
                                      idPersona = _.Id_persona,
                                      Monto = _.Monto ?? 0m,
                                      estado = _.Estado,
                                      Periodo = _.Periodo,
                                      fechapagada = _.Fecha_pagada,

                                      // Convertir los valores de int a bool
                                      Seguridad = _.Seguridad == 1,
                                      Agua = _.Agua == 1,
                                      Luz = _.Luz == 1,
                                      Internet = _.Internet == 1
                                  })
                                  .FirstOrDefault();

                    // Verificar si el cobro existe
                    if (cobro == null)
                    {
                        TempData["ErrorMessage"] = "No se encontró el cobro solicitado.";
                        return RedirectToAction("Index", "Cobros");  // Redirigir a la página principal si no se encuentra el cobro
                    }

                    // Verificación de acceso para clientes
                    if (tipoPersona != "Empleado")
                    {
                        // Si no es empleado, verificar que el cobro le pertenece al usuario autenticado
                        if (cobro.idPersona != idPersonaAutenticada)
                        {
                            TempData["ErrorMessage"] = "No tienes permiso para ver este cobro.";
                            return RedirectToAction("Index", "Cobros");  // Redirigir a la página principal si no es su cobro
                        }
                    }

                    // Asignar los datos del cobro al modelo de vista
                    viewModel.Cobro = cobro;

                    // Obtener bitácoras asociadas al cobro
                    viewModel.Bitacoras = db.SpObtenerBitacora()
                        .Where(b => b.Id_cobro == id.Value)
                        .OrderByDescending(b => b.Id_bitacora)
                        .Select(b => new BitacoraModel
                        {
                            idCobro = b.Id_cobro,
                            IdBitacora = b.Id_bitacora,
                            Detalle = b.Detalle,
                            IdUser = b.Id_user,
                            Fecha = b.Fecha,
                            NombrePersona = b.NombrePersona + " " + b.ApellidoPersona,
                            Accion = b.Accion
                        })
                        .ToList();
                }
            }
            catch
            {
                // Manejo de errores: Log y mensaje amigable
                TempData["ErrorMessage"] = "Hubo un problema al cargar los datos";
                return RedirectToAction("Index", "Cobros");  // Redirigir con mensaje de error
            }

            // Retornar la vista con el modelo de datos
            return View(viewModel);
        }
        [HttpGet]
        public ActionResult Crear(int? id)
        {
            if (Session["idPersona"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            try
            {
                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {
                    if (id.HasValue) // Modo Editar
                    {
                        var cobroExistente = db.SpObtenerCobroPorId(id.Value).FirstOrDefault();

                        if (cobroExistente == null)
                        {
                            TempData["ErrorMessage"] = "El cobro no existe.";
                            return RedirectToAction("Index", "Cobros");
                        }

                        // Mapear al modelo CobroModel
                        var cobroModel = new CobroModel
                        {
                            idCobro = cobroExistente.Id_cobro,
                            idPersona = cobroExistente.Id_persona,
                            idCasa = cobroExistente.Id_casa,
                            anno = cobroExistente.Anno,
                            mes = cobroExistente.Mes,
                            Seguridad = cobroExistente.Seguridad == 1,
                            Agua = cobroExistente.Agua == 1,
                            Luz = cobroExistente.Luz == 1,
                            Internet = cobroExistente.Internet == 1
                        };

                        return View(cobroModel);
                    }

                    // Modo Crear: Devolver un modelo vacío
                    return View(new CobroModel());
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Hubo un problema al cargar los datos: {ex.Message}";
                return RedirectToAction("Index", "Cobros");
            }
        }
        [HttpPost]
        public ActionResult Crear(CobroModel cobro)
        {
            string resultado;
            try
            {
                var idUsuario = Session["idPersona"] as int?;
                if (!idUsuario.HasValue)
                {
                    return RedirectToAction("Index", "Login");
                }

                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {
                    if (cobro.idCobro == 0) // Modo Crear
                    {
                        var serviciosSeleccionados = new List<int>();
                        if (cobro.Seguridad) serviciosSeleccionados.Add(3);  // id para Seguridad
                        if (cobro.Agua) serviciosSeleccionados.Add(4);       // id para Agua
                        if (cobro.Luz) serviciosSeleccionados.Add(5);        // id para Luz
                        if (cobro.Internet) serviciosSeleccionados.Add(6);   // id para Internet

                        if (serviciosSeleccionados.Count == 0)
                        {
                            resultado = "Debe seleccionar al menos un servicio.";
                        }
                        else
                        {
                            foreach (var servicio in serviciosSeleccionados)
                            {
                                // Llamada al procedimiento para crear cobro
                                int id_Admin = (int)Session["idPersona"];
                                db.SpCrearCobro(cobro.idPersona, cobro.idCasa, cobro.anno, cobro.mes, servicio.ToString(), id_Admin);
                            }

                            resultado = "El cobro ha sido creado exitosamente.";
                        }
                    }
                    else // Modo Editar
                    {
                        var serviciosSeleccionados = new List<string>();
                        if (cobro.Seguridad) serviciosSeleccionados.Add("3");  // id para Seguridad
                        if (cobro.Agua) serviciosSeleccionados.Add("4");       // id para Agua
                        if (cobro.Luz) serviciosSeleccionados.Add("5");        // id para Luz
                        if (cobro.Internet) serviciosSeleccionados.Add("6");   // id para Internet

                        if (serviciosSeleccionados.Count == 0)
                        {
                            resultado = "Debe seleccionar al menos un servicio.";
                        }
                        else
                        {
                            string idServicios = string.Join(",", serviciosSeleccionados);

                            // Llamada al procedimiento para actualizar cobro
                            db.SpActualizarCobro(
                                cobro.idCobro,
                                cobro.idPersona,
                                cobro.idCasa,
                                cobro.anno,
                                cobro.mes,
                                idServicios, // Lista concatenada
                                idUsuario
                            );

                            resultado = "El cobro ha sido actualizado exitosamente.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = "Ocurrió un error: " + ex.Message;
            }

            ViewBag.Resultado = resultado;
            return View(cobro);
        }

        [HttpGet]
        public ActionResult Editar(int? id)
        {
            if (Session["EsEmpleado"] == null || Session["EsEmpleado"].ToString() != "Empleado")
            {
                TempData["ErrorMessage"] = "No tienes permiso para editar cobros.";
                return RedirectToAction("Index", "Cobros");
            }
            try
            {
                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {
                    // Asegurarse de que el idCobro no sea 0
                    if (id == null || id == 0)
                    {
                        TempData["ErrorMessage"] = "Cobro no válido.";
                        return RedirectToAction("Index", "Cobros");
                    }

                    // Obtener el cobro existente desde la base de datos
                    var cobroExistente = db.SpObtenerCobroPorId(id.Value).FirstOrDefault();

                    if (cobroExistente != null)
                    {
                        // Mapear el resultado al modelo CobroModel
                        var cobroModel = new CobroModel
                        {
                            idCobro = cobroExistente.Id_cobro,
                            idPersona = cobroExistente.Id_persona,
                            idCasa = cobroExistente.Id_casa,
                            anno = cobroExistente.Anno,
                            mes = cobroExistente.Mes,
                            // Asignar los servicios seleccionados según corresponda
                            Seguridad = cobroExistente.Seguridad == 1,
                            Agua = cobroExistente.Agua == 1,
                            Luz = cobroExistente.Luz == 1,
                            Internet = cobroExistente.Internet == 1
                        };

                        // Prellenar el formulario con los detalles del cobro
                        return View(cobroModel);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Cobro no encontrado.";
                        return RedirectToAction("Index", "Cobros");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Hubo un problema al obtener los detalles del cobro: {ex.Message}";
                return RedirectToAction("Index", "Cobros");
            }

        }
        public ActionResult Eliminar(int? id)
        {
            // Verificar si el usuario está autenticado
            if (Session["idPersona"] == null || Session["EsEmpleado"] == null)
            {
                TempData["ErrorMessage"] = "No has iniciado sesión.";
                return RedirectToAction("Index", "Login");
            }

            // Verificar si el usuario tiene el rol de empleado
            string esEmpleado = Session["EsEmpleado"].ToString();
            if (esEmpleado != "Empleado")
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index", "Cobros");
            }

            // Validar que el ID del cobro sea válido
            if (id == null)
            {
                TempData["ErrorMessage"] = "El ID del cobro no es válido.";
                return RedirectToAction("Index", "Cobros"); // Redirige si el ID es inválido
            }

            try
            {
                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {
                    // Obtener el cobro desde la base de datos
                    var cobro = db.SpObtenerCobroPorId(id.Value)
                                  .FirstOrDefault();

                    // Verificar si el cobro existe
                    if (cobro == null)
                    {
                        TempData["ErrorMessage"] = "El cobro no se encontró.";
                        return RedirectToAction("Index", "Cobros");
                    }

                    // Verificar si el cobro pertenece al usuario autenticado
                    int idPersonaAutenticada = (int)Session["idPersona"];
                    if (cobro.Id_persona == idPersonaAutenticada)
                    {
                        TempData["ErrorMessage"] = "No puedes eliminar tu propio cobro.";
                        return RedirectToAction("Index", "Cobros"); // Redirige si el cobro le pertenece al usuario
                    }

                    // Ejecutar el procedimiento almacenado para eliminar el cobro
                    db.EliminacionDeCobro(id);

                    TempData["SuccessMessage"] = "El cobro se eliminó con éxito.";
                    return RedirectToAction("Index", "Cobros"); // Redirige con éxito al índice
                }
            }
            catch
            {
                // Manejo de errores
                TempData["ErrorMessage"] = "Hubo un problema al eliminar el cobro ";
                return RedirectToAction("Index", "Cobros"); // Redirige con el mensaje de error
            }
        }
        [HttpGet]
        public ActionResult Pagar(int? idCobro)
        {
            // Verificar si el usuario ha iniciado sesión
            if (Session["idPersona"] == null || Session["EsEmpleado"] == null)
            {
                TempData["ErrorMessage"] = "No has iniciado sesión.";
                return RedirectToAction("Index", "Login");
            }

            // Validar que el usuario tenga permisos para pagar
            string esEmpleado = Session["EsEmpleado"].ToString();
            if (esEmpleado != "Empleado" && esEmpleado != "Cliente")
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index", "Cobros");
            }

            // Validar que el ID del cobro sea válido
            if (idCobro == null || idCobro == 0)
            {
                TempData["ErrorMessage"] = "El ID del cobro no es válido.";
                return RedirectToAction("Index", "Cobros");
            }

            try
            {
                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {
                    // Obtener el cobro desde la base de datos
                    var cobro = db.SpObtenerCobroPorId(idCobro.Value).FirstOrDefault();

                    // Verificar si el cobro existe
                    if (cobro == null)
                    {
                        TempData["ErrorMessage"] = "El cobro no se encontró.";
                        return RedirectToAction("Index", "Cobros");
                    }

                    // Validar que el estado del cobro sea "Pendiente"
                    if (cobro.Estado != "Pendiente")
                    {
                        TempData["ErrorMessage"] = "El cobro no está en estado 'Pendiente' y no se puede pagar.";
                        return RedirectToAction("Index", "Cobros");
                    }
                    int idPersonaAutenticada = (int)Session["idPersona"];
                    // Cambiar el estado del cobro a "Pagado" y registrar la acción en la bitácora
                    db.SpPagarCobro(idCobro, idPersonaAutenticada);

                    // Mensaje de éxito
                    TempData["SuccessMessage"] = "El cobro se ha marcado como pagado correctamente.";
                    return RedirectToAction("Index", "Cobros");
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                TempData["ErrorMessage"] = $"Hubo un problema al pagar el cobro: {ex.Message}";
                return RedirectToAction("Index", "Cobros");
            }
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