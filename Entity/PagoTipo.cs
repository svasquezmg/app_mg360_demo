using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppMontGroup.Entity
{
    public class PagoTipo
    {
        public int idPagoTipo { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public string estado { get; set; }
    }
}