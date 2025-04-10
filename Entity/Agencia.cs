using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppMontGroup.Entity
{
    public class Agencia
    {
        public int idAgencia { get; set; }
        public string ruc { get; set; }
        public string razon_social { get; set; }
        public string nombre_corto { get; set; }
        public string estado { get; set; }

    }
}