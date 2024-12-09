using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeCondominios.Models
{
    public class BitacoraModel
    {
        public int? idCobro { get; set; }
        public int IdBitacora { get; set; }
        public string Detalle { get; set; }
        public int? IdUser { get; set; }
        public DateTime? Fecha { get; set; }
        public string Accion { get; set; }
        public string NombrePersona { get; set; }
    }
}
