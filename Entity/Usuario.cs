using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppMontGroup.Entity
{
    public class Usuario
    {
        public int idusuario { get; set; }
        public string usuario { get; set; }
        public string contrasenia { get; set; }
        public string nombres { get; set; }
        public string correo { get; set; }
        public string codigovendedor { get; set; }
        public string codigoalmacen { get; set; }
        public string codigovendedorasociado { get; set; }
        public string usuarioactualizacion { get; set; }
        public string estado { get; set; }
        public string estiloWeb { get; set; }
        public string cargo { get; set; }
        public string telefono { get; set; }
        public List<Usuario> lstUaurio { get; set; } = new List<Usuario>();
    }
}