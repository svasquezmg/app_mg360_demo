using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using WebAppMontGroup.Entity;

namespace WebAppMontGroup.Models
{
    public class ModelProducto
    {
        /*public List<Producto> listaProductoBusqueda(string dato)
        {
            List<Producto> lst_producto = new List<Producto>();
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {

                con.conectar();
                cmd = new MySqlCommand("PROC_PRODUCTO_OBTENER_LIKE", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("@x_dato", dato));
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Producto m_producto = new Producto();
                    m_producto.idProducto = (int)reader["idProducto"];
                    m_producto.codigo = reader["codigo"].ToString();
                    m_producto.dsc = reader["dsc"].ToString();
                    m_producto.dscCorto = reader["dscCorto"].ToString();
                    m_producto.responsable = reader["responsable"].ToString();
                    m_producto.estado = reader["estado"].ToString();
                    m_producto.lote = "";
                    m_producto.fechaVencimiento = "";
                    m_producto.cantidad = 0;
                    lst_producto.Add(m_producto);
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

            return lst_producto;
        }
        */
        /*public List<Producto> listaProductoBusquedaAlamcen(string almacen, string aniomes)
        {
            List<Producto> lst_producto = new List<Producto>();

            if (almacen != "03")
            {
                DataTable dt = new DataTable();
                string x_usuario = WebConfigurationManager.AppSettings["uWebServ"];
                string x_password = WebConfigurationManager.AppSettings["pWebServ"];

                try
                {
                    WebServiceEasy.Service_EasySoapClient Service_Easy = new WebServiceEasy.Service_EasySoapClient();
                    dt = Service_Easy.sotckAlmacen(x_usuario, x_password, aniomes, almacen, "");

                }
                catch (Exception ex)
                {

                }
                var distinctArticulos = dt.AsEnumerable()
                            .Select(row => new
                            {
                                articulo = row.Field<string>("articulo"),
                            })
                            .Distinct();

                int fila = 1;
                int cantidad = 0;
                string producto = "";
                foreach (var art in distinctArticulos)
                {

                    List<Stock> stock_items = new List<Stock>();

                    var items = from myRow in dt.AsEnumerable()
                                where myRow.Field<string>("articulo") == art.articulo
                                select myRow;

                    foreach (var item in items)
                    {
                        Stock fields = new Stock();
                        fields.Articulo = item["articulo"].ToString();
                        fields.Producto = item["artic_dsc"].ToString().Trim();
                        fields.Lote = item["lote"].ToString();
                        fields.FechaVecimiento = Convert.ToDateTime(item["fch_exp"]).ToString("yyyy-MM-dd");//.Substring(0, 10);
                        fields.Cantidad = Convert.ToInt32(item["stock"]).ToString();
                        cantidad = cantidad + Convert.ToInt32(item["stock"]);
                        producto = item["artic_dsc"].ToString();
                        stock_items.Add(fields);
                    }

                    lst_producto.Add(new Producto()
                    {
                        idProducto = 0,
                        codigo = art.articulo,
                        dsc = producto,
                        dscCorto = "",
                        lote = "",
                        fechaVencimiento = "",
                        cantidad = cantidad,
                        responsable = "",
                        listaStocks = stock_items.ToList()
                    });

                    cantidad = 0;
                    fila++;
                }
            }

            return lst_producto;
        }
        */


        public List<Producto> listaProductoBusqueda(string tipo,string dato)
        {
            List<Producto> lst_producto = new List<Producto>();
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                con.conectar();
                cmd = new MySqlCommand("PROC_PRODUCTO_OBTENER", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("@x_tipo", tipo));
                cmd.Parameters.Add(new MySqlParameter("@x_dato", dato)); // Nuevo parámetro
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Producto m_producto = new Producto();
                    m_producto.idProducto = (int)reader["idProducto"];
                    m_producto.codigo = reader["codigo"].ToString();
                    m_producto.dsc = reader["dsc"].ToString();
                    m_producto.dscCorto = reader["dscCorto"].ToString();
                    m_producto.responsable = reader["responsable"].ToString();
                    m_producto.estado = reader["estado"].ToString();
                    m_producto.responsable = reader["responsable"].ToString();
                    m_producto.lote = "";
                    m_producto.fechaVencimiento = "";
                    m_producto.cantidad = 0;
                    lst_producto.Add(m_producto);
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

            return lst_producto;
        }

        //HECHO POR SEBASTIAN PARA JALAR PRECIO POR CODIGO DE PRODUCTO Y COA DE CLIENTE
        public Producto obtenerPrecioPorCodigoYCoa(string codigo, string coa)
        {
            Producto producto = null;
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                con.conectar();
                cmd = new MySqlCommand("PROC_PRODUCTO_PRECIO_POR_CODIGO_COA", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("@x_codigo", codigo));
                cmd.Parameters.Add(new MySqlParameter("@x_coa", coa)); // Nuevo parámetro
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    producto = new Producto
                    {
                        IdProductoPrecioFijo = reader["IdProductoPrecioFijo"].ToString(),
                        codigo = reader["Codigo"].ToString(),
                        precio = Convert.ToDecimal(reader["Precio"]) // Asegúrate de que el tipo coincida
                    };
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores (log, etc.)
                return null;
            }
            finally
            {
                cmd.Dispose();
                con.desconectar();
            }

            return producto;
        }

        //hecho por sebastian
        public Producto obtenerPrecioPorCodigoYTipoCliente(string codigo, int cantidad, string tipo)
        {
            Producto producto = null;
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                con.conectar();
                cmd = new MySqlCommand("PROC_OBTENER_PRECIO_POR_CODIGO_Y_TIPO_CLIENTE", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("@x_codigo", codigo));
                cmd.Parameters.Add(new MySqlParameter("@x_cantidad", cantidad));
                cmd.Parameters.Add(new MySqlParameter("@x_tipoCat", tipo));
                /* cmd.Parameters.Add(new MySqlParameter("@x_tipo", tipoCliente)); */

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string codigoProducto = reader["Codigo"].ToString();
                    string precioProducto = reader["Precio"].ToString();

                    producto = new Producto
                    {
                        codigo = codigoProducto,
                        precio = Convert.ToDecimal(precioProducto)
                    };
                }
                else
                {
                    Console.WriteLine("El procedimiento almacenado no devolvió resultados.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en obtenerPrecioPorCodigoYTipoCliente: {ex.Message}");
                return null;
            }
            finally
            {
                cmd.Dispose();
                con.desconectar();
            }

            return producto;
        }

        //PARA LO DE STOCK CON SERVICIO, HECHO POR SEBASTIAN
        public List<Producto> listaProductoBusquedaAlamcen(string almacen, string aniomes)
        {
            List<Producto> lst_producto = new List<Producto>();

            // Usamos el valor de "almacen" recibido como parámetro
            if (!string.IsNullOrEmpty(almacen) && almacen != "03")
            {
                DataTable dt = new DataTable();
                string x_usuario = WebConfigurationManager.AppSettings["uWebServ"];
                string x_password = WebConfigurationManager.AppSettings["pWebServ"];

                try
                {
                    // Llamada al servicio web usando los parámetros dinámicos
                    WebServiceEasy.Service_EasySoapClient Service_Easy = new WebServiceEasy.Service_EasySoapClient();
                    dt = Service_Easy.sotckAlmacen(x_usuario, x_password, aniomes, almacen, "");

                    // Verificamos si hay productos
                    if (dt.Rows.Count == 0)
                    {
                        Debug.WriteLine("No se encontraron productos.");
                        return lst_producto;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error al obtener productos: {ex.Message}");
                    return lst_producto;
                }

                // Procesamos los productos
                var distinctArticulos = dt.AsEnumerable()
                    .Select(row => new
                    {
                        articulo = row.Field<string>("articulo"),
                    })
                    .Distinct();




                foreach (var art in distinctArticulos)
                {

                    List<Stock> stock_items = new List<Stock>();
                    int cantidad = 0;
                    string producto = "";

                    var items = from myRow in dt.AsEnumerable()
                                where myRow.Field<string>("articulo") == art.articulo
                                select myRow;

                    foreach (var item in items)
                    {
                        Stock fields = new Stock
                        {
                            Articulo = item["articulo"].ToString(),
                            Producto = item["artic_dsc"].ToString().Trim(),
                            Lote = item["lote"].ToString(),
                            FechaVecimiento = Convert.ToDateTime(item["fch_exp"]).ToString("yyyy-MM-dd"),
                            Cantidad = Convert.ToInt32(item["stock"]).ToString()
                        };

                        cantidad += Convert.ToInt32(item["stock"]);
                        producto = item["artic_dsc"].ToString();
                        stock_items.Add(fields);
                    }

                    lst_producto.Add(new Producto()
                    {
                        idProducto = 0,
                        codigo = art.articulo,
                        dsc = producto,
                        dscCorto = "",
                        lote = "",
                        fechaVencimiento = "",
                        cantidad = cantidad,
                        responsable = "",
                        listaStocks = stock_items
                    });



                }
            }

            return lst_producto;
        }

        public DataTable listaProductoDetalle(string idProveedor)
        {
            DataTable dt = new DataTable();
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            try
            {
                con.conectar();
                cmd = new MySqlCommand("PROC_PRODUCTOS_OBTENER_DETALLES", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("@x_opcion", "REPORTE-AL-VENTA"));
                cmd.Parameters.Add(new MySqlParameter("@x_value", idProveedor));
                //MySqlDataReader reader = cmd.ExecuteReader();
                adapter.SelectCommand = cmd;
                adapter.Fill(dt);
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

            return dt;
        }

        public DataTable listaProveedor()
        {
            DataTable dt = new DataTable();
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            try
            {
                con.conectar();
                cmd = new MySqlCommand("PROC_PROVEEDOR_OBTENER", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("@x_opcion", "TODOS_ACTIVOS"));
                cmd.Parameters.Add(new MySqlParameter("@x_value", ""));
                //MySqlDataReader reader = cmd.ExecuteReader();
                adapter.SelectCommand = cmd;
                adapter.Fill(dt);
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

            return dt;
        }

        public int Producto_separacion(int idSeparacion, string codigo, double cantidad, string fechaLimite,string solicitante, string detalle, string usuario, string opcion)
        {
            int result = 0;
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {

                con.conectar();
                cmd = new MySqlCommand("CRUD_PRODUCTO_SEPARACION", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("@x_opcion", opcion));
                cmd.Parameters.Add(new MySqlParameter("@x_idSeparacion", idSeparacion));
                cmd.Parameters.Add(new MySqlParameter("@x_codigo", codigo));
                cmd.Parameters.Add(new MySqlParameter("@x_cantidad", cantidad));
                cmd.Parameters.Add(new MySqlParameter("@x_fechaLimite", fechaLimite));
                cmd.Parameters.Add(new MySqlParameter("@x_UsuarioSolicitante", solicitante));
                cmd.Parameters.Add(new MySqlParameter("@x_Detalle", detalle));
                cmd.Parameters.Add(new MySqlParameter("@x_UsuarioActualizacion", usuario));
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
                return 0;
            }
            finally
            {
                cmd.Dispose();
                con.desconectar();
            }
        }

        public DataTable Producto_separacion_lista(int idSeparacion, string opcion, string estado)
        {
            DataTable dt = new DataTable();
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            try
            {
                con.conectar();
                cmd = new MySqlCommand("PROC_PRODUCTO_SEPARACION_OBTENER", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("@x_opcion", opcion));
                cmd.Parameters.Add(new MySqlParameter("@x_idSeparacion", idSeparacion));
                cmd.Parameters.Add(new MySqlParameter("@x_value", estado));
                //MySqlDataReader reader = cmd.ExecuteReader();
                adapter.SelectCommand = cmd;
                adapter.Fill(dt);
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

            return dt;
        }

        public int agencia_gestion(int idAgencia, string ruc, string razon_social, string nombre_corto, string opcion)
        {
            int result = 0;
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {

                con.conectar();
                cmd = new MySqlCommand("CRUD_AGENCIAS", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("@x_opcion", opcion));
                cmd.Parameters.Add(new MySqlParameter("@x_idAgencia", idAgencia));
                cmd.Parameters.Add(new MySqlParameter("@x_Ruc", ruc));
                cmd.Parameters.Add(new MySqlParameter("@x_RazonSocial", razon_social));
                cmd.Parameters.Add(new MySqlParameter("@x_NombreCorto", nombre_corto));
                //cmd.Parameters.Add(new MySqlParameter("@x_Estado", estado));
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
                return 0;
            }
            finally
            {
                cmd.Dispose();
                con.desconectar();
            }
        }

    }
}