using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebAppMontGroup.Entity;

namespace WebAppMontGroup.Models
{
    public class ModelGuia
    {
        public int crud_Guia_Transferencia_Cabecera(GuiaTransferencia guia_cabecera, string opcion)
        {
            int result = 0;
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {

                con.conectar();
                cmd = new MySqlCommand("CRUD_GUIA_TRANSFERENCIA", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("@x_opcion", opcion));
                cmd.Parameters.Add(new MySqlParameter("@x_IdGuiaTransferencia", guia_cabecera.IdGuiaTransferencia));
                cmd.Parameters.Add(new MySqlParameter("@x_serie", guia_cabecera.serie));
                cmd.Parameters.Add(new MySqlParameter("@x_numero", guia_cabecera.numero));
                cmd.Parameters.Add(new MySqlParameter("@x_cliente_documento", guia_cabecera.cliente_documento));
                cmd.Parameters.Add(new MySqlParameter("@x_cliente_tipo_documento", guia_cabecera.cliente_tipo_documento));
                cmd.Parameters.Add(new MySqlParameter("@x_cliente_nombre", guia_cabecera.cliente_nombre));
                cmd.Parameters.Add(new MySqlParameter("@x_cliente_direccion", guia_cabecera.cliente_direccion));
                cmd.Parameters.Add(new MySqlParameter("@x_fecha_emision", guia_cabecera.fecha_emision));
                cmd.Parameters.Add(new MySqlParameter("@x_observaciones", guia_cabecera.observaciones));
                cmd.Parameters.Add(new MySqlParameter("@x_motivo_traslado", guia_cabecera.motivo_traslado));
                cmd.Parameters.Add(new MySqlParameter("@x_tipo_transporte", guia_cabecera.tipo_transporte));
                cmd.Parameters.Add(new MySqlParameter("@x_fecha_traslado", guia_cabecera.fecha_traslado));
                cmd.Parameters.Add(new MySqlParameter("@x_transportista_documento_tipo", guia_cabecera.transportista_documento_tipo));
                cmd.Parameters.Add(new MySqlParameter("@x_transportista_documento_numero", guia_cabecera.transportista_documento_numero));
                cmd.Parameters.Add(new MySqlParameter("@x_transportista_nombre", guia_cabecera.transportista_nombre));
                cmd.Parameters.Add(new MySqlParameter("@x_transportista_placa_numero", guia_cabecera.transportista_placa_numero));
                cmd.Parameters.Add(new MySqlParameter("@x_conductor_documento_tipo", guia_cabecera.conductor_documento_tipo));
                cmd.Parameters.Add(new MySqlParameter("@x_conductor_documento_numero", guia_cabecera.conductor_documento_numero));
                cmd.Parameters.Add(new MySqlParameter("@x_conductor_nombre", guia_cabecera.conductor_nombre));
                cmd.Parameters.Add(new MySqlParameter("@x_conductor_apellidos", guia_cabecera.conductor_apellidos));
                cmd.Parameters.Add(new MySqlParameter("@x_conductor_numero_licencia", guia_cabecera.conductor_numero_licencia));
                cmd.Parameters.Add(new MySqlParameter("@x_punto_de_partida_ubigeo", guia_cabecera.punto_de_partida_ubigeo));
                cmd.Parameters.Add(new MySqlParameter("@x_punto_de_partida_direccion", guia_cabecera.punto_de_partida_direccion));
                cmd.Parameters.Add(new MySqlParameter("@x_punto_de_llegada_ubigeo", guia_cabecera.punto_de_llegada_ubigeo));
                cmd.Parameters.Add(new MySqlParameter("@x_punto_de_llegada_direccion", guia_cabecera.punto_de_llegada_direccion));
                cmd.Parameters.Add(new MySqlParameter("@x_atencion_dni_1", guia_cabecera.atencion_dni_1));
                cmd.Parameters.Add(new MySqlParameter("@x_atencion_1", guia_cabecera.atencion_1));
                cmd.Parameters.Add(new MySqlParameter("@x_atencion_dni_2", guia_cabecera.atencion_dni_2));
                cmd.Parameters.Add(new MySqlParameter("@x_atencion_2", guia_cabecera.atencion_2));
                cmd.Parameters.Add(new MySqlParameter("@x_agencia", guia_cabecera.agencia));
                cmd.Parameters.Add(new MySqlParameter("@x_flete_x_pagar", guia_cabecera.flete_x_pagar));
                cmd.Parameters.Add(new MySqlParameter("@x_estado", guia_cabecera.estado));
                cmd.Parameters.Add(new MySqlParameter("@x_UsuarioActualizacion", guia_cabecera.UsuarioActualizacion));
                cmd.Parameters.Add(new MySqlParameter("@x_id", MySqlDbType.Int32)).Direction = ParameterDirection.Output;

                if (opcion == "CREATE")
                {
                    cmd.ExecuteNonQuery();
                    var outParamValue = cmd.Parameters["@x_id"].Value;
                    result = Convert.ToInt32(outParamValue);
                }
                else
                {
                    result = cmd.ExecuteNonQuery();
                }

                return result;
            }
            catch (Exception ex)
            {
                //util_log.Escribir_Log("crud_Guia_Transferencia_Cabecera," + ex.ToString());
                return -1;
            }
            finally
            {
                cmd.Dispose();
                con.desconectar();
            }
        }

        public int crud_Guia_Transferencia_Detalle(GuiaTransferenciaDetalle guia_detalle, string opcion)
        {
            int result = 0;
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {

                con.conectar();
                cmd = new MySqlCommand("CRUD_GUIA_TRANSFERENCIA_DETALLE", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("@x_opcion", opcion));
                cmd.Parameters.Add(new MySqlParameter("@x_IdGuiaTransferenciaDetalle", guia_detalle.IdGuiaTransferenciaDetalle));
                cmd.Parameters.Add(new MySqlParameter("@x_IdGuiaTransferencia", guia_detalle.IdGuiaTransferencia));

                cmd.Parameters.Add(new MySqlParameter("@x_codigo", guia_detalle.codigo));
                cmd.Parameters.Add(new MySqlParameter("@x_producto", guia_detalle.producto));
                cmd.Parameters.Add(new MySqlParameter("@x_lote", guia_detalle.lote));
                cmd.Parameters.Add(new MySqlParameter("@x_fecha_vencimiento", guia_detalle.fecha_vencimiento));
                cmd.Parameters.Add(new MySqlParameter("@x_cantidad", guia_detalle.cantidad));
                cmd.Parameters.Add(new MySqlParameter("@x_Estado", guia_detalle.Estado));
                cmd.Parameters.Add(new MySqlParameter("@x_UsuarioActualizacion", guia_detalle.UsuarioActualizacion));
                cmd.Parameters.Add(new MySqlParameter("@x_id_guiadetalle", MySqlDbType.Int32)).Direction = ParameterDirection.Output;

                if (opcion == "CREATE")
                {
                    cmd.ExecuteNonQuery();

                    var outParamValue = cmd.Parameters["@x_id_guiadetalle"].Value;
                    result = Convert.ToInt32(outParamValue);
                }
                else
                {
                    result = cmd.ExecuteNonQuery();
                }

                return result;
            }
            catch (Exception ex)
            {
                //util_log.Escribir_Log("crud_Guia_Transferencia_Detalle," + ex.ToString());
                return -1;
            }
            finally
            {
                cmd.Dispose();
                con.desconectar();
            }
        }


        public List<GuiaTransferencia> listarGuiaTransferencia(string opcion, int idGuiaTransferencia, int idGuiaTransferenciaDetalle, string usuario, string fecha_inicio, string fecha_fin, string estado)
        {

            List<GuiaTransferencia> lst_guia = new List<GuiaTransferencia>();
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {

                con.conectar();
                cmd = new MySqlCommand("PROC_OBTENER_GUIAS_TRANSFERENCIA", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("@x_opcion", opcion));
                cmd.Parameters.Add(new MySqlParameter("@x_IdGuiaTransferencia", idGuiaTransferencia));
                cmd.Parameters.Add(new MySqlParameter("@x_IdGuiaTransferenciaDetalle", idGuiaTransferenciaDetalle));
                cmd.Parameters.Add(new MySqlParameter("@x_usuario", usuario));
                cmd.Parameters.Add(new MySqlParameter("@x_fecha_inicio", fecha_inicio));
                cmd.Parameters.Add(new MySqlParameter("@x_fecha_fin", fecha_fin));
                cmd.Parameters.Add(new MySqlParameter("@x_estado", estado));

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    GuiaTransferencia m_Guia = new GuiaTransferencia();
                    m_Guia.IdGuiaTransferencia = int.Parse(reader["IdGuiaTransferencia"].ToString());
                    m_Guia.serie = reader["serie"].ToString();
                    m_Guia.numero = reader["numero"].ToString();
                    m_Guia.cliente_documento = reader["cliente_documento"].ToString();
                    m_Guia.cliente_tipo_documento = Convert.ToInt32(reader["cliente_tipo_documento"]);
                    m_Guia.cliente_nombre = reader["cliente_nombre"].ToString();
                    m_Guia.cliente_direccion = reader["cliente_direccion"].ToString();
                    m_Guia.fecha_emision = Convert.ToDateTime(reader["fecha_emision"].ToString());
                    m_Guia.observaciones = reader["observaciones"].ToString();
                    m_Guia.motivo_traslado = reader["motivo_traslado"].ToString();
                    m_Guia.tipo_transporte = reader["tipo_transporte"].ToString();
                    m_Guia.fecha_traslado = Convert.ToDateTime(reader["fecha_traslado"].ToString());
                    m_Guia.transportista_documento_tipo = reader["transportista_documento_tipo"].ToString();
                    m_Guia.transportista_documento_numero = reader["transportista_documento_numero"].ToString();
                    m_Guia.transportista_nombre = reader["transportista_nombre"].ToString();
                    m_Guia.transportista_placa_numero = reader["transportista_placa_numero"].ToString();
                    m_Guia.conductor_documento_tipo = reader["conductor_documento_tipo"].ToString();
                    m_Guia.conductor_documento_numero = reader["conductor_documento_numero"].ToString();
                    m_Guia.conductor_nombre = reader["conductor_nombre"].ToString();
                    m_Guia.conductor_apellidos = reader["conductor_apellidos"].ToString();
                    m_Guia.conductor_numero_licencia = reader["conductor_numero_licencia"].ToString();
                    m_Guia.atencion_dni_1 = reader["atencion_dni_1"].ToString();
                    m_Guia.atencion_1 = reader["atencion_1"].ToString();
                    m_Guia.atencion_dni_2 = reader["atencion_dni_2"].ToString();
                    m_Guia.atencion_2 = reader["atencion_2"].ToString();
                    m_Guia.punto_de_partida_ubigeo = reader["punto_de_partida_ubigeo"].ToString();
                    m_Guia.punto_de_partida_direccion = reader["punto_de_partida_direccion"].ToString();
                    m_Guia.punto_de_llegada_ubigeo = reader["punto_de_llegada_ubigeo"].ToString();
                    m_Guia.punto_de_llegada_direccion = reader["punto_de_llegada_direccion"].ToString();
                    m_Guia.agencia = reader["agencia"].ToString();
                    m_Guia.flete_x_pagar = reader["flete_x_pagar"].ToString();
                    m_Guia.estado = reader["estado"].ToString();
                    m_Guia.UsuarioActualizacion = reader["UsuarioActualizacion"].ToString();
                    m_Guia.FechaActualizacion = Convert.ToDateTime(reader["FechaActualizacion"].ToString());
                    m_Guia.lst_guiaDetalle = listarGuiaTransferenciaDetalle("DET-1", m_Guia.IdGuiaTransferencia, 0, "", DateTime.MinValue.ToString("yyyy-MM-dd"), DateTime.MinValue.ToString("yyyy-MM-dd"), "");
                    lst_guia.Add(m_Guia);
                }
            }
            catch (Exception ex)
            {
                //util_log.Escribir_Log("listarGuiaTransferencia," + ex.ToString());
                return null;
            }
            finally
            {
                cmd.Dispose();
                con.desconectar();
            }
            return lst_guia;
        }


        public List<GuiaTransferenciaDetalle> listarGuiaTransferenciaDetalle(string opcion, int idGuiaTransferencia, int idGuiaTransferenciaDetalle, string usuario, string fecha_inicio, string fecha_fin, string estado)
        {

            List<GuiaTransferenciaDetalle> lst_guiaDetalle = new List<GuiaTransferenciaDetalle>();
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {

                con.conectar();
                cmd = new MySqlCommand("PROC_OBTENER_GUIAS_TRANSFERENCIA", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("@x_opcion", opcion));
                cmd.Parameters.Add(new MySqlParameter("@x_IdGuiaTransferencia", idGuiaTransferencia));
                cmd.Parameters.Add(new MySqlParameter("@x_IdGuiaTransferenciaDetalle", idGuiaTransferenciaDetalle));
                cmd.Parameters.Add(new MySqlParameter("@x_usuario", usuario));
                cmd.Parameters.Add(new MySqlParameter("@x_fecha_inicio", fecha_inicio));
                cmd.Parameters.Add(new MySqlParameter("@x_fecha_fin", fecha_fin));
                cmd.Parameters.Add(new MySqlParameter("@x_estado", estado));

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    GuiaTransferenciaDetalle m_GuiaDetalle = new GuiaTransferenciaDetalle();
                    m_GuiaDetalle.IdGuiaTransferenciaDetalle = int.Parse(reader["IdGuiaTransferenciaDetalle"].ToString());
                    m_GuiaDetalle.IdGuiaTransferencia = int.Parse(reader["IdGuiaTransferencia"].ToString());
                    m_GuiaDetalle.codigo = reader["codigo"].ToString();
                    m_GuiaDetalle.producto = reader["producto"].ToString();
                    m_GuiaDetalle.lote = reader["lote"].ToString();
                    m_GuiaDetalle.fecha_vencimiento = Convert.ToDateTime(reader["fecha_vencimiento"]);
                    m_GuiaDetalle.cantidad = double.Parse(reader["cantidad"].ToString());
                    m_GuiaDetalle.Estado = reader["Estado"].ToString();
                    m_GuiaDetalle.FechaActualizacion = Convert.ToDateTime(reader["FechaActualizacion"].ToString());
                    m_GuiaDetalle.UsuarioActualizacion = reader["UsuarioActualizacion"].ToString();
                    lst_guiaDetalle.Add(m_GuiaDetalle);
                }

            }
            catch (Exception ex)
            {
                //util_log.Escribir_Log("listarGuiaTransferenciaDetalle," + ex.ToString());
                return null;
            }
            finally
            {
                cmd.Dispose();
                con.desconectar();
            }

            return lst_guiaDetalle;
        }
    }
}