using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppMontGroup.Entity
{
    public class Promocion
    {
        public int idPromocion { get; set; }
        public string tipoPromocion { get; set; }
        public string categoriaCliente { get; set; }
        public string codigoProducto { get; set; }
        public string cantidad_desde { get; set; }
        public string cantidad_hasta { get; set; }
        public string bonificacion { get; set; }

        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public string descripcionProducto { get; set; }

        public string accion { get; set; }
        public string estado { get; set; }
        public string created_at { get; set; }
    }
}