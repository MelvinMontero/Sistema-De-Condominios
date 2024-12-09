using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeCondominios.Models
{
    public class CobroDetalleViewModel
    {
        public CobroModel Cobro { get; set; } // Detalles del cobro
        public List<BitacoraModel> Bitacoras { get; set; } // Lista de bitácoras asociadas

    }
}