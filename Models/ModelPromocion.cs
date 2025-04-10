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


    }
}
