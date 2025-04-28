using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebAppMontGroup.Entity;

namespace WebAppMontGroup.Models
{
    public class ModelGeneral
    {



        public int obtenerCorrelativo(string tipoDocumento, string serie)
        {
            int result = 0;
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {

                con.conectar();
                cmd = new MySqlCommand("PROC_GENERA_CORRELATIVO", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("@x_opcion", "OBTENER"));
                cmd.Parameters.Add(new MySqlParameter("@x_tipoDocumento", tipoDocumento));
                cmd.Parameters.Add(new MySqlParameter("@x_serie", serie));

                cmd.Parameters.Add(new MySqlParameter("@x_correlativo", MySqlDbType.Int32)).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                var outParamValue = cmd.Parameters["@x_correlativo"].Value;
                result = Convert.ToInt32(outParamValue);

                return result;
            }
            catch (Exception ex)
            {
                //new Escribir_Log("update_insert_Producto_95," + ex.ToString(), false);
                return -1;
            }
            finally
            {
                cmd.Dispose();
                con.desconectar();
            }
        }

        public List<Ubigeo> listaUbigeo(string dato)
        {
            List<Ubigeo> lst_ubigeo = new List<Ubigeo>();
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {

                con.conectar();
                cmd = new MySqlCommand("PROC_UBIGEO_OBTENER", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("@x_codUbigeo", dato));
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Ubigeo m_ubigeo = new Ubigeo();
                    m_ubigeo.codigo = reader["codigo"].ToString();
                    m_ubigeo.distrito = reader["distrito"].ToString();
                    m_ubigeo.provincia = reader["provincia"].ToString();
                    m_ubigeo.departamento = reader["departamento"].ToString();
                    lst_ubigeo.Add(m_ubigeo);
                }
            }
            catch (Exception ex)
            {
                //util_log.Escribir_Log("listarUSuario," + ex.ToString());
                return null;
            }
            finally
            {
                cmd.Dispose();
                con.desconectar();
            }

            return lst_ubigeo;
        }

        public List<Agencia> listaAgencia(string ruc)
        {
            List<Agencia> lst_agencia = new List<Agencia>();
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {

                con.conectar();
                cmd = new MySqlCommand("PROC_AGENCIA_OBTENER", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("@x_ruc", ruc));
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Agencia m_agencia = new Agencia();
                    m_agencia.idAgencia = (int)reader["IdAgencia"];
                    m_agencia.ruc = reader["ruc"].ToString();
                    m_agencia.razon_social = reader["razon_social"].ToString();
                    m_agencia.nombre_corto = reader["nombre_corto"].ToString();
                    m_agencia.estado = reader["estado"].ToString();
                    lst_agencia.Add(m_agencia);
                }

                //var itemToRemove = lst_agencia.Single(r => r.ruc == ruc_omitir);
                //lst_agencia.Remove(itemToRemove);

            }
            catch (Exception ex)
            {
                //util_log.Escribir_Log("listarUSuario," + ex.ToString());
                return null;
            }
            finally
            {
                cmd.Dispose();
                con.desconectar();
            }

            return lst_agencia;
        }

        public General direccionUltimoPedido(string coa)
        {
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            General m_general = new General();
            try
            {

                con.conectar();
                cmd = new MySqlCommand("PROC_PEDIDO_ULTIMA_ENTREGA", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("@x_coa", coa));
                MySqlDataReader reader = cmd.ExecuteReader();

           
                m_general.valor_1 = "-1";
                while (reader.Read())
                {
                    m_general.valor_1 = reader["ruc"].ToString();
                    m_general.valor_2 = reader["razon_social"].ToString();
                }
            }
            catch (Exception ex)
            {
                //util_log.Escribir_Log("listarUSuario," + ex.ToString());
                return null;
            }
            finally
            {
                cmd.Dispose();
                con.desconectar();
            }

            return m_general;
        }

        /*  public General direccionUltimoPedido(string coa)
         {
             ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
             MySqlCommand cmd = new MySqlCommand();
             General m_general = new General();
             try
             {

                 con.conectar();
                 cmd = new MySqlCommand("PROC_PEDIDO_ULTIMA_ENTREGA", con.retConeccion());
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Parameters.Add(new MySqlParameter("@x_coa", coa));
                 MySqlDataReader reader = cmd.ExecuteReader();


                 m_general.valor_1 = "-1";
                 while (reader.Read())
             {
                 m_general.valor_1 = reader["GuiaUbigeoLlegada"].ToString();  // Cambiado de "ruc"
                 m_general.valor_2 = reader["GuiaDireccionLlegada"].ToString();  // Cambiado de "razon_social"
             }

             }
             catch (Exception ex)
             {
                 //util_log.Escribir_Log("listarUSuario," + ex.ToString());
                 return null;
             }
             finally
             {
                 cmd.Dispose();
                 con.desconectar();
             }

             return m_general;
         } */

         public int LOG_AUDITORIA( string proviene, string accion,int usuarioId,string descripcion)
        {
            int result = 0;
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                con.conectar();

                cmd = new MySqlCommand("PROC_REGISTRAR_AUDITORIA", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Clear(); // Limpiar parámetros antes de usarlos
                cmd.Parameters.AddWithValue("@x_usuarioId", usuarioId);
                cmd.Parameters.Add(new MySqlParameter("@x_modulo", proviene));
                cmd.Parameters.Add(new MySqlParameter("@x_accion", accion));
                cmd.Parameters.Add(new MySqlParameter("@x_descripcion", descripcion));

                cmd.ExecuteNonQuery();
                result = 1; // Indicar éxito
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en insert_Pedido_Log: " + ex.Message);
                result = -1; // Indicar error
            }
            finally
            {
                cmd.Dispose();
                con.desconectar();
            }

            return result;
        }

    }


}