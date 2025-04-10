using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Web;

namespace WebAppMontGroup.Entity
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public string Documento { get; set; }
        public string Serie { get; set; }
        public string Numero { get; set; }
        public string CodigoPedido { get; set; }
        public string NombreTipoPago { get; set; }
        public string Coa { get; set; }
        public string Cliente { get; set; }
        public string RucDni { get; set; }
        public string DireccionCliente { get; set; }
        public string Ubigeo { get; set; }
        public string CodCategoriaCliente { get; set; }
        public string TipoDocumentoFiscal { get; set; }
        public string DocumentoFiscal { get; set; }   
        public string CodigoTipoPago { get; set; }
        public string Moneda { get; set; }
        public string TipoCambio { get; set; }
        public DateTime FechaEntrega { get; set; }
        public string OrdenCompra { get; set; }
        public string DocOrdenCompra { get; set; }
        public string CodVendedor { get; set; }
        public string Vendedor { get; set; }
        public DateTime FechaRegistro { get; set; } 
        public string ZonaVenta { get; set; }
        public double SubTotal { get; set; }
        public double TotalDescuento { get; set; }
        public double IgvPorcentaje { get; set; }
        public double Igv { get; set; }
        public double Total { get; set; }

        public string ObservacionCredito { get; set; }
        public string ObservacionPrecio { get; set; }
        public string ObservacionAlmacen { get; set; }

        public string EstadoCredito { get; set; }
        public string EstadoProducto { get; set; }
        public string EstadoAlmacen { get; set; }
        public string SeguimientoCredito { get; set; }
        public string Estado { get; set; }

        public int VecesEditado { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string UsuarioActualizacion { get; set; }

        public PedidoGuia pedido_guia { get; set; } = new PedidoGuia();
        public List<PedidoDetalle> pedido_detalle { get; set; } = new List<PedidoDetalle>();
    }

    public class PedidoTotales
    {
        public decimal SumaTotal { get; set; }
        public decimal UltimoTotal { get; set; }
        public string Coa { get; set; }
    }

    public class RespuestaValidacion
    {
        public string Aprobado { get; set; }
        public string Mensaje { get; set; }
    }

    public class PedidoAprobaciones
    {
        public int IdPedidoAprobacion { get; set; }
        public string UsuarioAprobador { get; set; }
        public string Observacion { get; set; }
        public string Decision { get; set; }
        public string Proviene { get; set; }
        public List<int> IdPedidoDetalle { get; set; }
    }


    public class PedidoLog { 
    
        public int IdPedido { get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }
        public string Accion { get; set; }
        public string Obs { get; set; }
        public string Detalle1 { get; set; }
        public string Detalle2 { get; set; }
    }



    public class Pedido95
    {
        public string idPedido { get; set; }
        public string codpedido { get; set; }
        public string fecha_pedido { get; set; }
        public string fecha_entrega { get; set; }
        public string coa { get; set; }
        public string coa_desc { get; set; }
        public string t_pago { get; set; }
        public string CodVendedor { get; set; }
        public string vendedor { get; set; }
        public string t_documento { get; set; }
        public string est_pedido { get; set; }
        public string total { get; set; }
        public string apCredito { get; set; }
        public string apProducto { get; set; }
        public string apAlmacen { get; set; }
        public string acc { get; set; }
        public string doc_guia { get; set; }
        public string doc_FvBv { get; set; }
        public string fAprobCredito { get; set; }
        public string fecha_aproAlmacen { get; set; }
        public string usuario { get; set; }
        public string aprob1 { get; set; }
        public string aprob2 { get; set; }
    }

}