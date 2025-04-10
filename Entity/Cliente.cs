using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppMontGroup.Entity
{
    public class Cliente
    {
        public int id_cliente { get; set; }
        public string CoaCliente { get; set; }
        public string rucdni { get; set; }
        public string razonSocial { get; set; }
        public string codigoVendedor { get; set; }
        public string codPagoTipo { get; set; }
        public string CoaRelacionado { get; set; }
        public string categoria { get; set; }
        public string UsuarioActualizacion { get; set; }
        public string Estado { get; set; }
        public string EstadoEasy { get; set; }
    }
}