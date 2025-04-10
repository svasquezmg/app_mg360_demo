using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppMontGroup.Entity
{
    public class CotizacionDetalle
    {
        public int Id { get; set; }
        public int idCotizacion { get; set; }
        public string CodigoProducto { get; set; }
        public string NombreProducto { get; set; }
        public string Det_Producto { get; set; }
        public string Proveedor { get; set; }
        public string Presentacion { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string Promocion { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string UsuarioCreador { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificador { get; set; }
    }
}