using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaDeCondominios.Models;
using static DataModels.PviProyectoFinalDBStoredProcedures;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;

namespace SistemaDeCondominios.Controllers
{
    public class LoginController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Autenticacion(PersonaModel model)
        {
            try
            {
                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {
                    var persona = db.SpAutenticarUsuario(model.Email, model.Clave).FirstOrDefault();
                    if (persona != null)
                    {
                        Session["idPersona"] = persona.Id_persona;
                        Session["NombreCompleto"] = $"{persona.Nombre}{persona.Apellido}";
                        Session["Email"] = persona.Email;
                        Session["EsEmpleado"] = persona.Tipo_persona;
                        return RedirectToAction("Index","Cobros");
                    }
                    else
                    {
                        ViewBag.Mensaje = "Usuario o Contraseña incorrectos.";
                    }

                }
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = "Algo ha salido mal:" + ex;
            }
            return View(model);
        }
        public ActionResult LogOut()
        {
            Session["idPersona"] = null;
            Session["NombreCompleto"] = null;
            Session["Email"] = null;
            Session["EsEmpleado"] = null;


            return RedirectToAction("Index", "Login");
        }

    }
}