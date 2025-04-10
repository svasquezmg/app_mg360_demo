using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppMontGroup.Entity
{
    public class Kardex
    {
        public string Empresa { get; set; }
        public string Lote { get; set; }
        public string Vecimiento { get; set; }
        public string Tipo { get; set; }
        public string Serie { get; set; }
        public string Numero { get; set; }
        public string Ingreso { get; set; }
        public string Salida { get; set; }
        public string Saldo { get; set; }
    }
}