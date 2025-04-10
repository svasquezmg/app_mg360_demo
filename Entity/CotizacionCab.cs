using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppMontGroup.Entity
{
    public class CotizacionCab
    {
        public int Id { get; set; }
        public string CodCotizacion { get; set; }
        public string Coa { get; set; }
        public string Cliente { get; set; }
        public string CodVendedor { get; set; }
        public string Contacto { get; set; }
        public decimal Total { get; set; }
        public int Item { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string UsuarioCreador { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}