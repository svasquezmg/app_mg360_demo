using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppMontGroup.Entity
{
    public class ClienteContacto
    {
        public string CoaCliente { get; set; }
        public string tipo { get; set; }
        public string persona { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public string detalle { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string UsuarioActualizacion { get; set; }
    }
}