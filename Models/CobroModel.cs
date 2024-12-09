using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeCondominios.Models
{
    public class CobroModel
    {
        public int idCobro { get; set; }
        public int? idCasa { get; set; }
        public string nombreCasa { get; set; }
        public int? idPersona { get; set; }
        public string nombrePersona { get; set; }
        public string apellidoPersona { get; set; }
        public int mes { get; set; }
        public int anno { get; set; }
        public string estado { get; set; }
        public int idServicio { get; set; }
        public string Periodo { get; set; }
        public string Propietario { get; set; }
        public decimal Monto { get; set; }
        public bool Seguridad { get; set; }
        public bool Agua { get; set; }
        public bool Luz { get; set; }
        public bool Internet { get; set; }
        public DateTime? fechapagada { get; set; }

        public string Detalle { get; set; }


    }
}
