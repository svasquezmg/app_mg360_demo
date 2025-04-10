using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;

namespace WebAppMontGroup.Entity
{
    public class Producto
    {
       public int idProducto { get; set; }
        public string codigo { get; set; }
        public string dsc { get; set; }
        public string dscCorto { get; set; }
        public string responsable { get; set; }
        public string estado { get; set; }
        public string lote { get; set; }
        public string fechaVencimiento { get; set; }
        public double cantidad { get; set; }

        // Agrega esta propiedad para manejar el precio
        public decimal precio { get; set; }
        public string IdProductoPrecioFijo { get; set; }

        public List<Stock> listaStocks { get; set; } = new List<Stock>();
    }
}