using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppMontGroup.Entity;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Data;
using System.IO;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Diagnostics;


namespace WebAppMontGroup.Models
{
    public class ModelPedidoApr
    {
        
        /* LISTAR EL PEDIDO GENERAL */
        public List<Pedido> listaPedido(int anio, int mes, string estado, string responsable, string tipo)
        {
            List<Pedido> lst_pedidos = new List<Pedido>();
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                con.conectar();
                cmd = new MySqlCommand("PROC_PEDIDO_APROBACIONES_OBTENER", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("p_anio", anio));
                cmd.Parameters.Add(new MySqlParameter("p_mes", mes));
                cmd.Parameters.Add(new MySqlParameter("p_estado", estado));
                cmd.Parameters.Add(new MySqlParameter("p_responsable", responsable));
                cmd.Parameters.Add(new MySqlParameter("p_tipo", tipo));
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Pedido pedido = new Pedido();
                    pedido.IdPedido = (int)reader["IdPedido"];
                    pedido.CodigoPedido = reader["CodigoPedido"].ToString();
                    pedido.FechaRegistro = Convert.ToDateTime(reader["FechaRegistro"]);
                    pedido.Coa = reader["Coa"].ToString();
                    pedido.Cliente = reader["Cliente"].ToString();
                    pedido.CodigoTipoPago = reader["CodigoTipoPago"].ToString();
                    pedido.Vendedor = reader["Vendedor"].ToString();
                    pedido.FechaEntrega = Convert.ToDateTime(reader["FechaEntrega"]);
                    pedido.Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0.0 : Convert.ToDouble(reader["Total"]);
                    pedido.CodCategoriaCliente = reader["CodCategoriaCliente"].ToString();
                    lst_pedidos.Add(pedido);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmd.Dispose();
                con.desconectar();
            }

            return lst_pedidos;
        }


        /* public Pedido listaPedidoApr(int idpedido, string proviene)
        {
            Pedido m_pedidoapr = null;
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                con.conectar();
                cmd = new MySqlCommand("PROC_APROBACIONES_OBTENER", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("p_idpedido", idpedido));
                cmd.Parameters.Add(new MySqlParameter("p_proviene", proviene));
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read()) // Si hay al menos un registro
                {
                    m_pedidoapr = new Pedido
                    {
                        IdPedidoAprobacion = Convert.ToInt32(reader["IdPedidoAprobacion"]),
                        UsuarioAprobador = reader["UsuarioAprobador"].ToString(),
                        Observacion = reader["Observacion"].ToString(),
                        Decision = reader["Decision"].ToString(),
                        Proviene = reader["Proviene"].ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message); // Loguea el error
                return null;
            }
            finally
            {
                cmd.Dispose();
                con.desconectar();
            }

            return m_pedidoapr;
        } */


        /* VALIDACIONES PARA APROBACION */
        public int insert_Aprobacion(Pedido pedido)
        {
            int result = 0;
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                con.conectar();

                foreach (int idDetalle in pedido.IdPedidoDetalle) // Iterar sobre la lista de enteros
                {
                    cmd = new MySqlCommand("CRUD_APROBACIONES", con.retConeccion());
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Clear(); // Limpiar parámetros en cada iteración
                    cmd.Parameters.Add(new MySqlParameter("@x_IdPedido", pedido.IdPedido));
                    cmd.Parameters.Add(new MySqlParameter("@x_IdPedidoAprobacion", pedido.IdPedidoAprobacion));
                    cmd.Parameters.Add(new MySqlParameter("@x_UsuarioAprobador", pedido.UsuarioAprobador));
                    cmd.Parameters.Add(new MySqlParameter("@x_Decision", pedido.Decision));
                    cmd.Parameters.Add(new MySqlParameter("@x_IdPedidoDetalle", idDetalle)); // Pasar ID individual
                    cmd.Parameters.Add(new MySqlParameter("@x_Tipo", pedido.Proviene));

                    cmd.ExecuteNonQuery();
                }

                result = 1; // Indicar éxito
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en insert_Aprobacion: " + ex.Message);
                return -1; // Indicar error
            }
            finally
            {
                cmd.Dispose();
                con.desconectar();
            }
        }


        /* PEDIDO DETALLE LISTAR */
        public List<PedidoDetalle> listaPedidoDetalleApr(int idPedido, string responsable, string estado)
        {
            List<PedidoDetalle> lst_pedidoDetalle = new List<PedidoDetalle>();
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {

                con.conectar();
                cmd = new MySqlCommand("PROC_PEDIDO_DETALLE_APROBACIONES_OBTENER", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("@x_IdPedido", idPedido));
                cmd.Parameters.Add(new MySqlParameter("@x_Responsable", responsable));
                cmd.Parameters.Add(new MySqlParameter("@x_Estado", estado));
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    PedidoDetalle m_pedidoDetalle = new PedidoDetalle();
                    m_pedidoDetalle.IdPedidoDetalle = (int)reader["IdPedidoDetalle"];
                    m_pedidoDetalle.IdPedido = (int)reader["IdPedido"];
                    m_pedidoDetalle.Almacen = reader["Almacen"].ToString();
                    m_pedidoDetalle.Lote = reader["Lote"].ToString();
                    m_pedidoDetalle.Articulo = reader["Articulo"].ToString();
                    m_pedidoDetalle.ArticuloDesc = reader["ArticuloDesc"].ToString();
                    m_pedidoDetalle.Cantidad = (double)reader["Cantidad"];
                    m_pedidoDetalle.Precio = (double)reader["Precio"];
                    m_pedidoDetalle.SubTotal = (double)reader["SubTotal"];
                    m_pedidoDetalle.Descuento = (double)reader["Descuento"];
                    m_pedidoDetalle.TipoPromocion = reader["TipoPromocion"].ToString();
                    m_pedidoDetalle.AplicaPromocion = reader["AplicaPromocion"].ToString();
                    m_pedidoDetalle.UsuarioResponsable = reader["UsuarioResponsable"].ToString();
                    m_pedidoDetalle.UsuarioAprobador = reader["UsuarioAprobador"].ToString();
                    m_pedidoDetalle.Decision = reader["Decision"].ToString();
                    m_pedidoDetalle.Observacion = reader["Observacion"].ToString();
                    m_pedidoDetalle.FechaActualizacion = Convert.ToDateTime(reader["FechaActualizacion"]);
                    m_pedidoDetalle.UsuarioActualiza = reader["UsuarioActualiza"].ToString();
                    m_pedidoDetalle.Estado = reader["Estado"].ToString();
                    m_pedidoDetalle.Obs = reader["Obs"].ToString();
                    m_pedidoDetalle.Log = reader["Log"].ToString();
                    lst_pedidoDetalle.Add(m_pedidoDetalle);
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

            return lst_pedidoDetalle;
        }


        //Para que inserte 3 veces en pedido_aprobaciones pero por ahora no es necesario 
        /* public int insert_Pedido_Aprobaciones(int idPedido)
        {
            int result = 0;
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                con.conectar();

                // Lista de aprobaciones predefinidas
                string[,] aprobaciones = new string[,]
                {
            { "MTUPIA", "SYSTEM", "PE", "CRE" },
            { "VTERNORIO", "SYSTEM", "PE", "INV" },
            { "AP-PRECIO", "jcasanova", "PE", "PRO" }
                };

                for (int i = 0; i < aprobaciones.GetLength(0); i++)
                {
                    cmd = new MySqlCommand("PROC_PEDIDO_APROBACIONES", con.retConeccion());
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@x_opcion", "CREATE");
                    cmd.Parameters.AddWithValue("@x_idAprobacion", DBNull.Value);
                    cmd.Parameters.AddWithValue("@x_idPedido", idPedido);
                    cmd.Parameters.AddWithValue("@x_UsuarioResponsable", aprobaciones[i, 0]);
                    cmd.Parameters.AddWithValue("@x_UsuarioAprobador", aprobaciones[i, 1]);
                    cmd.Parameters.AddWithValue("@x_Observacion", DBNull.Value);
                    cmd.Parameters.AddWithValue("@x_Decision", aprobaciones[i, 2]);
                    cmd.Parameters.AddWithValue("@x_UsuarioActualiza", "SYSTEM");
                    cmd.Parameters.AddWithValue("@x_Proviene", aprobaciones[i, 3]);

                    cmd.ExecuteNonQuery();
                }

                result = 1; // Indicar que la operación fue exitosa
            }
            catch (Exception ex)
            {
                return -1; // Retornar error en caso de excepción
            }
            finally
            {
                cmd.Dispose();
                con.desconectar();
            }

            return result;
        } */


        /* TRAER DATOS DE LOG PARA EL SEGUIMIENTO DEL PEDIDO */
        public List<Pedido> lista_Pedido_log(int idPedido, string accion, string proviene)
        {
            List<Pedido> lst_pedido = new List<Pedido>();
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {

                con.conectar();
                cmd = new MySqlCommand("PROC_PEDIDO_LOG_OBTENER", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("@x_IdPedido", idPedido));
                cmd.Parameters.Add(new MySqlParameter("@x_accion", accion));
                cmd.Parameters.Add(new MySqlParameter("@x_proviene", proviene));
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Pedido m_pedido = new Pedido();

                    m_pedido.IdPedido = (int)reader["IdPedido"];
                    m_pedido.Fecha = Convert.ToDateTime(reader["Fecha"]);
                    m_pedido.Usuario = reader["Usuario"].ToString();
                    m_pedido.Accion = reader["Accion"].ToString();
                    m_pedido.Obs = reader["Obs"].ToString();
                    m_pedido.Detalle1 = reader["Detalle1"].ToString();
                    m_pedido.Detalle2 = reader["Detalle2"].ToString();

                    lst_pedido.Add(m_pedido);
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

            return lst_pedido;
        }


        /* LOG con observaciones en caso de APROBACIONES */
        public int insert_Pedido_Log(Pedido pedido)
        {
            int result = 0;
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                con.conectar();

                cmd = new MySqlCommand("PROC_PEDIDO_LOG_CREA", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Clear(); // Limpiar parámetros antes de usarlos
                cmd.Parameters.Add(new MySqlParameter("@x_IdPedido", pedido.IdPedido));
                cmd.Parameters.Add(new MySqlParameter("@x_Usuario", pedido.UsuarioAprobador));
                cmd.Parameters.Add(new MySqlParameter("@x_Accion", pedido.Decision));
                cmd.Parameters.Add(new MySqlParameter("@x_Obs", pedido.Obs));
                cmd.Parameters.Add(new MySqlParameter("@x_Detalle1", pedido.Detalle1));
                cmd.Parameters.Add(new MySqlParameter("@x_Detalle2", pedido.Detalle2));
                cmd.Parameters.Add(new MySqlParameter("@x_Proviene", pedido.Proviene));

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


        /* LOG con observaciones en caso de UPDATE o CREAR*/
        public int insert_Pedido_Log2(Pedido pedido, string accion, string obs, string proviene)
        {
            int result = 0;
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                con.conectar();

                cmd = new MySqlCommand("PROC_PEDIDO_LOG_CREA", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Clear(); // Limpiar parámetros antes de usarlos
                cmd.Parameters.Add(new MySqlParameter("@x_IdPedido", pedido.IdPedido));
                cmd.Parameters.Add(new MySqlParameter("@x_Usuario", pedido.UsuarioActualizacion));
                cmd.Parameters.Add(new MySqlParameter("@x_Accion", accion));
                cmd.Parameters.Add(new MySqlParameter("@x_Obs", obs));
                cmd.Parameters.Add(new MySqlParameter("@x_Detalle1", pedido.Detalle1));
                cmd.Parameters.Add(new MySqlParameter("@x_Detalle2", pedido.Detalle2));
                cmd.Parameters.Add(new MySqlParameter("@x_Proviene", proviene));

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