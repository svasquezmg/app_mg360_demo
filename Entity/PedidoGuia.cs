using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;

namespace WebAppMontGroup.Entity
{
    public class PedidoGuia
    {
        public int idPedidoGuia { get; set; }
        public int IdPedido { get; set; }
        public string Guia { get; set; }
        public string GuiaRegion { get; set; }
        public string GuiaDireccionPartida { get; set; }
        public string GuiaUbigeoPartida { get; set; }
        public string GuiaPuntoEntrega { get; set; }
        public string GuiaDireccionEntrega { get; set; }
        public string GuiaUbigeoEntrega { get; set; }
        public string GuiaTransporte { get; set; }
        public string GuiaTransporteEmpRuc { get; set; }
        public string GuiaTransporteEmpresa { get; set; }
        public string RepcionTipo1 { get; set; }
        public string Dni1 { get; set; }
        public string Nombre1 { get; set; }
        public string Telefono1 { get; set; }
        public string RepcionTipo2 { get; set; }
        public string Dni2 { get; set; }
        public string Nombre2 { get; set; }
        public string Telefono2 { get; set; }
        public string EstadoGuia { get; set; }
        public string UsuarioActualizacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}