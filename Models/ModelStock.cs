using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppMontGroup.Models
{
    public class ModelStock
    {
        public string Fila { get; set; }
        public string Articulo { get; set; }
        public string Producto { get; set; }
        public string Lote { get; set; }
        public string FechaVecimiento { get; set; }
        public string Cantidad { get; set; }
        public string Detalle { get; set; }
        
        public List<ModelStock> listaStocks { get; set; } = new List<ModelStock>();
    }
}