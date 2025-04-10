using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppMontGroup.Entity
{
    public class GuiaTransferenciaDetalle
    {
        public int IdGuiaTransferenciaDetalle { get; set; }
        public int IdGuiaTransferencia { get; set; }
        public string codigo { get; set; }
        public string producto { get; set; }
        public string lote { get; set; }
        public DateTime fecha_vencimiento { get; set; }
        public double cantidad { get; set; }
        public string Estado { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string UsuarioActualizacion { get; set; }
    }
}