using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeCondominios.Models
{
    public class PersonaModel
    {
        public int IdPersona { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Clave { get; set; }
        public string EsEmpleado { get; set; }
        public bool Estado { get; set; }
    }
}