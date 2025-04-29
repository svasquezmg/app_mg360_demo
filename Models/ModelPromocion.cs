using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebAppMontGroup.Entity;

namespace WebAppMontGroup.Models
{
    public class ModelPromocion
    {

        public List<Promocion> listaPromocion(string codigoProducto, string categoriaCliente)
        {
            List<Promocion> lst_promociones = new List<Promocion>();
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                con.conectar();
                cmd = new MySqlCommand("PROC_PROMOCION_OBTENER", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("@x_categoriaCliente", categoriaCliente));
                cmd.Parameters.Add(new MySqlParameter("@x_codigoProducto", codigoProducto));
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Promocion promocion = new Promocion();
                    promocion.idPromocion = (int)reader["idPromocion"];
                    promocion.tipoPromocion = reader["tipoPromocion"].ToString();
                    promocion.categoriaCliente = reader["categoriaCliente"].ToString();
                    promocion.codigoProducto = reader["codigoProducto"].ToString();
                    promocion.bonificacion = reader["bonificacion"].ToString();
                    promocion.cantidad_desde = reader["cantidad_desde"].ToString();
                    promocion.cantidad_hasta = reader["cantidad_hasta"].ToString();
                    lst_promociones.Add(promocion);
                }
            }
            catch (Exception ex)
            {
                //util_log.Escribir_Log("listaPromocion, " + ex.ToString());
                return null;
            }
            finally
            {
                cmd.Dispose();
                con.desconectar();
            }

            return lst_promociones;
        }


        public List<Promocion> listaPreciosPromocionesGeneral()
        {
            List<Promocion> lst_promociones = new List<Promocion>();
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                con.conectar();
                cmd = new MySqlCommand("PROC_PROMOCION_OBTENER_TODOS", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Promocion promocion = new Promocion();
                    promocion.idPromocion = (int)reader["idPromocion"];
                    promocion.tipoPromocion = reader["tipoPromocion"].ToString();
                    promocion.descripcionProducto = reader["descripcionProducto"].ToString();
                    promocion.categoriaCliente = reader["categoriaCliente"].ToString();
                    promocion.codigoProducto = reader["codigoProducto"].ToString();
                    promocion.bonificacion = reader["bonificacion"].ToString();
                    promocion.cantidad_desde = reader["cantidad_desde"].ToString();
                    promocion.cantidad_hasta = reader["cantidad_hasta"].ToString();
                    promocion.fechaInicio = Convert.ToDateTime(reader["fecha_inicio"]);
                    promocion.fechaFin = Convert.ToDateTime(reader["fecha_termino"]);
                    lst_promociones.Add(promocion);
                }
            }
            catch (Exception ex)
            {
                //util_log.Escribir_Log("listaPromocion, " + ex.ToString());
                return null;
            }
            finally
            {
                cmd.Dispose();
                con.desconectar();
            }

            return lst_promociones;
        }

        public int crud_promocion(Promocion promocion)
        {
            int result = 0;
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                con.conectar();
                cmd = new MySqlCommand("PROC_PROMOCION_CRUD", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("@x_opcion", promocion.accion));
                cmd.Parameters.Add(new MySqlParameter("@x_id", promocion.idPromocion));
                cmd.Parameters.Add(new MySqlParameter("@x_tipoPromocion", promocion.tipoPromocion));
                cmd.Parameters.Add(new MySqlParameter("@x_categoriaCliente", promocion.categoriaCliente));
                cmd.Parameters.Add(new MySqlParameter("@x_codigoProducto", promocion.codigoProducto));
                cmd.Parameters.Add(new MySqlParameter("@x_fecha_inicio", promocion.fechaInicio));
                cmd.Parameters.Add(new MySqlParameter("@x_fecha_termino", promocion.fechaFin));
                cmd.Parameters.Add(new MySqlParameter("@x_bonificacion", promocion.bonificacion));
                cmd.Parameters.Add(new MySqlParameter("@x_cantidad_desde", promocion.cantidad_desde));
                cmd.Parameters.Add(new MySqlParameter("@x_cantidad_hasta", promocion.cantidad_hasta));
                cmd.Parameters.Add(new MySqlParameter("@x_estado", '1'));
                
                cmd.ExecuteNonQuery();
                result = 1; // Asumes éxito
            }
            catch (Exception ex)
            {
                // Loguear el error si es necesario
                result = -1;
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
