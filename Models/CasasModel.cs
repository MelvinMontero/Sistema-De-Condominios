using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeCondominios.Models
{
    public class CasasModel
    {
        public int IdCasa  { get; set; }
        public string NombreCasa { get; set; }
        public int mtsCuadrados { get; set; }
        public int nHabitaciones { get; set; }
        public int nBanos { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
        public int IdCliente { get; set; }
        public DateTime FechaConstruccion { get; set; }
        public bool Estado { get; set; }
        public int IdCobro { get; set; }
        public int IdBitacora {  get; set; }
        public string EstadoCobro {  get; set; }
    }
}