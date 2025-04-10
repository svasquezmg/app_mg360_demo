using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Mysqlx.Crud.Order.Types;

namespace WebAppMontGroup.Entity
{
    public class ClienteDireccion
    {
        public int idDireccion { get; set; }
        public string CoaCliente { get; set; }
        public string tipo { get; set; }
        public string direccion { get; set; }
        public string departamento { get; set; }
        public string provincia { get; set; }
        public string distrito { get; set; }
        public string ubigeo { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string UsuarioActualizacion { get; set; }
        public string estado { get; set; }


    }
}