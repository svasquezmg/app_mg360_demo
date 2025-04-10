using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppMontGroup.Entity
{
    public class PedidoDetalle
    {
        public int IdPedidoDetalle { get; set; }
        public int IdPedido { get; set; }
        public string Almacen { get; set; }
        public string Lote { get; set; }
        public string Articulo { get; set; }
        public string ArticuloDesc { get; set; }
        public double Cantidad { get; set; }
        public double Precio { get; set; }
        public double SubTotal { get; set; }
        public double Descuento { get; set; }
        public string TipoPromocion { get; set; }
        public string AplicaPromocion { get; set; }
        public string UsuarioResponsable { get; set; }
        public string UsuarioAprobador { get; set; }
        public string Decision { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string UsuarioActualiza { get; set; }
        public string Estado { get; set; }
        public string Obs { get; set; }
        public string Log { get; set; }
    }
}