using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;

namespace WebAppMontGroup.Entity
{
    public class ProductoLote
    {
        public string articulo { get; set; }
        public string producto { get; set; }
        public string lote { get; set; }
        public string fechaExpira { get; set; }
        public string cantidadEntrada { get; set; }
        public string cantidadSalida { get; set; }
        public string total { get; set; }

        public string documento { get; set; }
        public string tipoDocumento { get; set; }
        public string fechaDocumento { get; set; }
    }
}