using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebAppMontGroup.Entity;
namespace WebAppMontGroup.Models
{
    public class ModelPagoTipo
    {

        public List<PagoTipo> listaTipoPago(string codigo)
        {
            List<PagoTipo> lst_tipoPago = new List<PagoTipo>();
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                con.conectar();
                cmd = new MySqlCommand("PROC_PAGO_TIPO_OBTENER", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("@x_codigo", codigo));
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    PagoTipo m_tipoPago = new PagoTipo();
                    m_tipoPago.idPagoTipo = (int)reader["idPagoTipo"];
                    m_tipoPago.codigo = reader["codigo"].ToString();
                    m_tipoPago.descripcion = reader["descripcion"].ToString();
                    m_tipoPago.estado = reader["estado"].ToString();
                    lst_tipoPago.Add(m_tipoPago);
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

            return lst_tipoPago;
        }
    }
}