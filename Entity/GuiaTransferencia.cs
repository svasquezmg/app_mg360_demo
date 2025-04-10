using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppMontGroup.Entity
{
    public class GuiaTransferencia
    {
        public int IdGuiaTransferencia { get; set; }
        public string serie { get; set; }
        public string numero { get; set; }
        public string cliente_documento { get; set; }
        public int cliente_tipo_documento { get; set; }
        public string cliente_nombre { get; set; }
        public string cliente_direccion { get; set; }
        public DateTime fecha_emision { get; set; }
        public string observaciones { get; set; }
        public string motivo_traslado { get; set; }
        public string tipo_transporte { get; set; }
        public DateTime fecha_traslado { get; set; }
        public string transportista_documento_tipo { get; set; }
        public string transportista_documento_numero { get; set; }
        public string transportista_nombre { get; set; }
        public string transportista_placa_numero { get; set; }
        public string conductor_documento_tipo { get; set; }
        public string conductor_documento_numero { get; set; }
        public string conductor_nombre { get; set; }
        public string conductor_apellidos { get; set; }
        public string conductor_numero_licencia { get; set; }
        public string punto_de_partida_ubigeo { get; set; }
        public string punto_de_partida_direccion { get; set; }
        public string punto_de_llegada_ubigeo { get; set; }
        public string punto_de_llegada_direccion { get; set; }
        public string atencion_dni_1 { get; set; }
        public string atencion_1 { get; set; }
        public string atencion_dni_2 { get; set; }
        public string atencion_2 { get; set; }
        public string agencia { get; set; }
        public string flete_x_pagar { get; set; }
        public string estado { get; set; }
        public string UsuarioActualizacion { get; set; }
        public DateTime FechaActualizacion { get; set; }

        public List<GuiaTransferenciaDetalle> lst_guiaDetalle { get; set; } = new List<GuiaTransferenciaDetalle>(); 
        
    }
}