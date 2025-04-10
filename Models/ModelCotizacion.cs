using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using WebAppMontGroup.Entity;

namespace WebAppMontGroup.Models
{
    public class ModelCotizacion
    {
        private readonly ConeccionMysql _con;

        public ModelCotizacion()
        {
            _con = new ConeccionMysql("MYSQL_Conexion_Pedido");
        }
        // Insert Cotizacion Detalle


        public int InsertCotizacionCab(CotizacionCab cotizacion,string accion)
        {
            int result = 0;
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                _con.conectar();
                cmd = new MySqlCommand("crud_cotizacion_cab", _con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("@action", accion));
                cmd.Parameters.Add(new MySqlParameter("@p_id", cotizacion.Id));
                cmd.Parameters.Add(new MySqlParameter("@p_cod_cotizacion", cotizacion.CodCotizacion));
                cmd.Parameters.Add(new MySqlParameter("@p_coa", cotizacion.Coa));
                cmd.Parameters.Add(new MySqlParameter("@p_cliente", cotizacion.Cliente));
                cmd.Parameters.Add(new MySqlParameter("@p_cod_vendedor", cotizacion.CodVendedor));
                cmd.Parameters.Add(new MySqlParameter("@p_contacto", cotizacion.Contacto));
                cmd.Parameters.Add(new MySqlParameter("@p_total", cotizacion.Total));
                cmd.Parameters.Add(new MySqlParameter("@p_item", cotizacion.Item));
                cmd.Parameters.Add(new MySqlParameter("@p_usuario", cotizacion.UsuarioCreador));


                cmd.Parameters.Add(new MySqlParameter("@x_id", MySqlDbType.Int32)).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                var outParamValue = cmd.Parameters["@x_id"].Value;
                result = Convert.ToInt32(outParamValue);


                //cmd.ExecuteNonQuery();
                //result = (int)cmd.LastInsertedId;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in InsertCotizacionCabecera: " + ex.Message);
                result = -1;
            }
            finally
            {
                cmd.Dispose();
                _con.desconectar();
            }
            return result;
        }

        public int InsertCotizacionDetalle(CotizacionDetalle cotizacionDetalle, string accion)
        {
            int result = 0;

            MySqlCommand cmd = new MySqlCommand();
            try
            {
                _con.conectar();
                cmd = new MySqlCommand("crud_cotizacion_det", _con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("@action", accion));
                cmd.Parameters.Add(new MySqlParameter("@p_id", cotizacionDetalle.Id));
                cmd.Parameters.Add(new MySqlParameter("@p_idCotizacion", cotizacionDetalle.idCotizacion));
                cmd.Parameters.Add(new MySqlParameter("@p_codigoproducto", cotizacionDetalle.CodigoProducto));
                cmd.Parameters.Add(new MySqlParameter("@p_nombreproducto", cotizacionDetalle.NombreProducto));
                cmd.Parameters.Add(new MySqlParameter("@p_det_producto", cotizacionDetalle.Det_Producto));
                cmd.Parameters.Add(new MySqlParameter("@p_proveedor", cotizacionDetalle.Proveedor));
                cmd.Parameters.Add(new MySqlParameter("@p_presentacion", cotizacionDetalle.Presentacion));
                cmd.Parameters.Add(new MySqlParameter("@p_fecha_vencimiento", cotizacionDetalle.FechaVencimiento));
                cmd.Parameters.Add(new MySqlParameter("@p_promocion", cotizacionDetalle.Promocion));
                cmd.Parameters.Add(new MySqlParameter("@p_cantidad", cotizacionDetalle.Cantidad));
                cmd.Parameters.Add(new MySqlParameter("@p_precio", cotizacionDetalle.Precio));
                cmd.Parameters.Add(new MySqlParameter("@p_total", cotizacionDetalle.Total));
                cmd.Parameters.Add(new MySqlParameter("@p_usuario", cotizacionDetalle.UsuarioCreador));

                cmd.ExecuteNonQuery();
                result = 1;
            }
            catch (Exception ex)
            {
                // Log the exception
                result = -1;
            }
            finally
            {
                cmd.Dispose();
                _con.desconectar();
            }
            return result;
        }



        // Select Cotizacion Detalle by ID

        public List<CotizacionDetalle> SelectCotizacionDetCotizacion(int idcotizacion)
        {
            List<CotizacionDetalle> detalles = new List<CotizacionDetalle>();
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                _con.conectar();
                cmd = new MySqlCommand("crud_cotizacion_det", _con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("@action", "SELECT"));
                cmd.Parameters.Add(new MySqlParameter("@p_id", DBNull.Value));
                cmd.Parameters.Add(new MySqlParameter("@p_idCotizacion", idcotizacion));
                cmd.Parameters.Add(new MySqlParameter("@p_codigoproducto", DBNull.Value));
                cmd.Parameters.Add(new MySqlParameter("@p_nombreproducto", DBNull.Value));
                cmd.Parameters.Add(new MySqlParameter("@p_det_producto", DBNull.Value));
                cmd.Parameters.Add(new MySqlParameter("@p_proveedor", DBNull.Value));
                cmd.Parameters.Add(new MySqlParameter("@p_presentacion", DBNull.Value));
                cmd.Parameters.Add(new MySqlParameter("@p_fecha_vencimiento", DBNull.Value));
                cmd.Parameters.Add(new MySqlParameter("@p_promocion", DBNull.Value));
                cmd.Parameters.Add(new MySqlParameter("@p_cantidad", DBNull.Value));
                cmd.Parameters.Add(new MySqlParameter("@p_precio", DBNull.Value));
                cmd.Parameters.Add(new MySqlParameter("@p_total", DBNull.Value));
                cmd.Parameters.Add(new MySqlParameter("@p_usuario", DBNull.Value));

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CotizacionDetalle detalle = new CotizacionDetalle
                        {
                            Id = reader["id"] != DBNull.Value ? reader.GetInt32("id") : 0,
                            idCotizacion = reader["idCotizacion"] != DBNull.Value ? reader.GetInt32("idCotizacion") : 0,
                            CodigoProducto = reader["codigoproducto"] != DBNull.Value ? reader.GetString("codigoproducto") : string.Empty,
                            NombreProducto = reader["nombreproducto"] != DBNull.Value ? reader.GetString("nombreproducto") : string.Empty,
                            Det_Producto = reader["det_producto"] != DBNull.Value ? reader.GetString("det_producto") : string.Empty,
                            Proveedor = reader["proveedor"] != DBNull.Value ? reader.GetString("proveedor") : string.Empty,
                            Presentacion = reader["presentacion"] != DBNull.Value ? reader.GetString("presentacion") : string.Empty,
                            FechaVencimiento = (DateTime)(reader["fecha_vencimiento"] != DBNull.Value ? reader.GetDateTime("fecha_vencimiento") : (DateTime?)null),
                            Promocion = reader["promocion"] != DBNull.Value ? reader.GetString("promocion") : string.Empty,
                            Cantidad = reader["cantidad"] != DBNull.Value ? reader.GetDecimal("cantidad") : 0,
                            Precio = reader["precio"] != DBNull.Value ? reader.GetDecimal("precio") : 0,
                            Total = reader["total"] != DBNull.Value ? reader.GetDecimal("total") : 0,
                            UsuarioModificador = reader["usuario_modificador"] != DBNull.Value ? reader.GetString("usuario_modificador") : string.Empty,
                            FechaModificacion = reader["fecha_modificacion"] != DBNull.Value ? reader.GetDateTime("fecha_modificacion") : (DateTime?)null
                        };

                        detalles.Add(detalle);
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                cmd.Dispose();
                _con.desconectar();
            }

            return detalles;
        }

        public List<CotizacionCab> GetCotizacionCabList(string tipo,int idcotizacion, string codVendedor, int value1)
        {
            CotizacionCab cotizacion = null;
            List<CotizacionCab> lst = new List<CotizacionCab>();
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                _con.conectar();
                cmd = new MySqlCommand("crud_cotizacion_cab", _con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("@action", tipo));
                cmd.Parameters.Add(new MySqlParameter("@p_id", idcotizacion));
                cmd.Parameters.Add(new MySqlParameter("@p_cod_cotizacion", DBNull.Value));
                cmd.Parameters.Add(new MySqlParameter("@p_coa", DBNull.Value));
                cmd.Parameters.Add(new MySqlParameter("@p_cliente", DBNull.Value));
                cmd.Parameters.Add(new MySqlParameter("@p_cod_vendedor", codVendedor));
                cmd.Parameters.Add(new MySqlParameter("@p_contacto", DBNull.Value));
                cmd.Parameters.Add(new MySqlParameter("@p_total", DBNull.Value));
                cmd.Parameters.Add(new MySqlParameter("@p_item", value1));
                cmd.Parameters.Add(new MySqlParameter("@p_usuario", DBNull.Value));

                cmd.Parameters.Add(new MySqlParameter("@x_id", MySqlDbType.Int32)).Direction = ParameterDirection.Output;

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cotizacion = new CotizacionCab
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        CodCotizacion = reader["cod_cotizacion"].ToString(),
                        Coa = reader["coa"].ToString(),
                        Cliente = reader["cliente"].ToString(),
                        CodVendedor = reader["cod_vendedor"].ToString(),
                        Contacto = reader["contacto"].ToString(),
                        Total = Convert.ToDecimal(reader["total"]),
                        Item = Convert.ToInt32(reader["item"]),
                        FechaRegistro = Convert.ToDateTime(reader["fecha_registro"]),
                        UsuarioCreador = reader["usuario_creador"].ToString(),
                        FechaModificacion = reader.IsDBNull(reader.GetOrdinal("fecha_modificacion")) ? (DateTime?)null : Convert.ToDateTime(reader["fecha_modificacion"]),
                        UsuarioModificacion = reader["usuario_modificacion"].ToString()
                    };
                    lst.Add(cotizacion);
                }
            }
            catch (Exception ex)
            {
                // Log exception here
                return null;
            }
            finally
            {
                cmd.Dispose();
                _con.desconectar();
            }
            return lst;
        }


    }
}
