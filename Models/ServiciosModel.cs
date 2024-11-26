using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeCondominios.Models
{
    public class ServiciosModel
    {
        public int IdServicio { get; set; }
        public string Nombre { get; set; }
        public string Descripcion {  get; set; }
        public decimal Precio { get; set; }
        public int IdCategoria { get; set; }
        public bool Estado { get; set; }
        public string Categoria { get; set; }
        public string EstadoCobro { get; set; }
    }
}