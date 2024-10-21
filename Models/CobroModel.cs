using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeCondominios.Models
{
    public class CobroModel
    {
        public int idCobro { get; set; }
        public string nombreCasa { get; set; }
        public int idPersona { get; set; }
        public string nombrePersona { get; set; }
        public string apellidoPersona { get; set; }
        public int mes {  get; set; }
        public int anno {  get; set; }
        public string estado { get; set; }

    }
}