using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppMontGroup.Entity
{
    public class ProductoPrecio
    {
    }

    public class PrecioPorCategoria
    {
        public int IdProductoPrecio { get; set; }
        public string Categoria { get; set; }
        public decimal Codigo { get; set; }
        public decimal CantidadDe { get; set; }
        public decimal CantidadHasta { get; set; }
        public decimal Precio { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public decimal UsuarioActualizacion { get; set; }
        public decimal Estado { get; set; }
    }

    public class PrecioPorCliente
    {
        /* FALTA */
    }
}