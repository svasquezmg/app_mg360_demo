using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppMontGroup.Models
{
    public class GuiaElectronica
    {
        public string operacion = "";
        public int tipo_de_comprobante = 0;
        public string serie = "";
        public int numero = 0;
        public int cliente_tipo_de_documento = 0;
        public string cliente_numero_de_documento = "";
        public string cliente_denominacion = "";
        public string cliente_direccion = "";
        public string cliente_email = "";
        public string cliente_email_1 = "";
        public string cliente_email_2 = "";
        public string fecha_de_emision = "";
        public string observaciones = "";
        public string motivo_de_traslado = "";
        public string motivo_de_traslado_otros_descripcion = "";
        public string peso_bruto_total = "";
        public string peso_bruto_unidad_de_medida = "";
        public string numero_de_bultos = "";
        public string tipo_de_transporte = "";
        public string fecha_de_inicio_de_traslado = "";
        public string transportista_documento_tipo = "";
        public string transportista_documento_numero = "";
        public string transportista_denominacion = "";
        public string transportista_placa_numero = "";
        public string conductor_documento_tipo = "";
        public string conductor_documento_numero = "";
        public string conductor_nombre = "";
        public string conductor_apellidos = "";
        public string conductor_numero_licencia = "";
        public string punto_de_partida_ubigeo = "";
        public string punto_de_partida_direccion = "";
        public string punto_de_partida_codigo_establecimiento_sunat = "";
        public string punto_de_llegada_ubigeo = "";
        public string punto_de_llegada_direccion = "";
        public string punto_de_llegada_codigo_establecimiento_sunat = "";
        public string enviar_automaticamente_al_cliente = "";
        public string formato_de_pdf = "";
        public string sunat_envio_indicador = "";
    }

    public class GuiaElectronicaDetalle
    {
        public string unidad_de_medida = "";
        public string codigo = "";
        public string descripcion = "";
        public double cantidad = 0;
    }
}