using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Configuration;
using WebAppMontGroup.Entity;

namespace WebAppMontGroup.Models
{
    public class ModelCliente
    {

        string x_usuario = WebConfigurationManager.AppSettings["uWebServ"];
        string x_password = WebConfigurationManager.AppSettings["pWebServ"];

        public int insert_Cliente(Cliente cliente)
        {
            int result = 0;
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {

                con.conectar();
                cmd = new MySqlCommand("CRUD_CLIENTE", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("@x_opcion", "CREATE"));
                cmd.Parameters.Add(new MySqlParameter("@x_CoaCliente", cliente.CoaCliente));
                cmd.Parameters.Add(new MySqlParameter("@x_rucdni", cliente.rucdni));
                cmd.Parameters.Add(new MySqlParameter("@x_razonSocial", cliente.razonSocial));

                cmd.Parameters.Add(new MySqlParameter("@x_codigoVendedor", cliente.codigoVendedor));
                cmd.Parameters.Add(new MySqlParameter("@x_categoria", cliente.categoria));
                cmd.Parameters.Add(new MySqlParameter("@x_codPagoTipo", cliente.codPagoTipo));
                cmd.Parameters.Add(new MySqlParameter("@x_CoaRelacionado", cliente.CoaRelacionado));
                cmd.Parameters.Add(new MySqlParameter("@x_UsuarioActualizacion", cliente.UsuarioActualizacion));

                cmd.Parameters.Add(new MySqlParameter("@x_id", MySqlDbType.Int32)).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                var outParamValue = cmd.Parameters["@x_id"].Value;
                result = Convert.ToInt32(outParamValue);
                //result = Convert.ToInt32(cmd.ExecuteNonQuery());
                return result;
            }
            catch (Exception ex)
            {
                if (ex.ToString().ToUpper().Contains("PRIMARY"))
                {
                    return -2;
                }
                else
                {
                    //new Escribir_Log("update_insert_Producto_95," + ex.ToString(), false);
                    return -1;
                }
            }
            finally
            {
                cmd.Dispose();
                con.desconectar();
            }
        }

        public int insertUpdate_ClienteDireccion(ClienteDireccion clienteDireccion, string opcion)
        {
            int result = 0;
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {

                con.conectar();
                cmd = new MySqlCommand("CRUD_CLIENTE_DIRECCION", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("@x_opcion", opcion));
                cmd.Parameters.Add(new MySqlParameter("@x_idDireccion", 0));
                cmd.Parameters.Add(new MySqlParameter("@x_CoaCliente", clienteDireccion.CoaCliente));
                cmd.Parameters.Add(new MySqlParameter("@x_tipo", clienteDireccion.tipo));
                cmd.Parameters.Add(new MySqlParameter("@x_direccion", clienteDireccion.direccion));
                cmd.Parameters.Add(new MySqlParameter("@x_departamento", clienteDireccion.departamento));
                cmd.Parameters.Add(new MySqlParameter("@x_provincia", clienteDireccion.provincia));
                cmd.Parameters.Add(new MySqlParameter("@x_distrito", clienteDireccion.distrito));
                cmd.Parameters.Add(new MySqlParameter("@x_ubigeo", clienteDireccion.ubigeo));
                cmd.Parameters.Add(new MySqlParameter("@x_UsuarioActualizacion", clienteDireccion.UsuarioActualizacion));

                cmd.Parameters.Add(new MySqlParameter("@x_id", MySqlDbType.Int32)).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                var outParamValue = cmd.Parameters["@x_id"].Value;
                result = Convert.ToInt32(outParamValue);
                //result = Convert.ToInt32(cmd.ExecuteNonQuery());
                return result;
            }
            catch (Exception ex)
            {
                if (ex.ToString().ToUpper().Contains("PRIMARY"))
                {
                    return -2;
                }
                else
                {
                    //new Escribir_Log("update_insert_Producto_95," + ex.ToString(), false);
                    return -1;
                }
            }
            finally
            {
                cmd.Dispose();
                con.desconectar();
            }
        }

        public int insertUpdate_ClienteContacto(ClienteContacto clienteContacto, string opcion)
        {
            int result = 0;
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {

                con.conectar();
                cmd = new MySqlCommand("CRUD_CLIENTE_CONTACTO", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("@x_opcion", opcion));
                cmd.Parameters.Add(new MySqlParameter("@x_CoaCliente", clienteContacto.CoaCliente));
                cmd.Parameters.Add(new MySqlParameter("@x_tipo", clienteContacto.tipo));
                cmd.Parameters.Add(new MySqlParameter("@x_persona", clienteContacto.persona));
                cmd.Parameters.Add(new MySqlParameter("@x_correo", clienteContacto.correo));
                cmd.Parameters.Add(new MySqlParameter("@x_telefono", clienteContacto.telefono));
                cmd.Parameters.Add(new MySqlParameter("@x_detalle", clienteContacto.detalle));
                cmd.Parameters.Add(new MySqlParameter("@x_UsuarioActualizacion", clienteContacto.UsuarioActualizacion));

                cmd.Parameters.Add(new MySqlParameter("@x_id", MySqlDbType.Int32)).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                var outParamValue = cmd.Parameters["@x_id"].Value;
                result = Convert.ToInt32(outParamValue);
                //result = Convert.ToInt32(cmd.ExecuteNonQuery());
                return result;
            }
            catch (Exception ex)
            {
                if (ex.ToString().ToUpper().Contains("PRIMARY"))
                {
                    return -2;
                }
                else
                {
                    //new Escribir_Log("update_insert_Producto_95," + ex.ToString(), false);
                    return -1;
                }
            }
            finally
            {
                cmd.Dispose();
                con.desconectar();
            }
        }

        public List<ClienteDireccion> listaClienteDireccion(string coa_cliente, string tipo)
        {
            List<ClienteDireccion> lst_clienteDireccion = new List<ClienteDireccion>();
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                con.conectar();
                cmd = new MySqlCommand("PROC_CLIENTE_DIRECCION_OBTENER", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("@x_coa_cliente", coa_cliente));
                cmd.Parameters.Add(new MySqlParameter("@x_tipo", tipo));
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ClienteDireccion m_clienteDireccion = new ClienteDireccion();
                    m_clienteDireccion.idDireccion = (int)reader["idDireccion"];
                    m_clienteDireccion.CoaCliente = reader["CoaCliente"].ToString();
                    m_clienteDireccion.direccion = reader["direccion"].ToString();
                    m_clienteDireccion.departamento = reader["departamento"].ToString();
                    m_clienteDireccion.provincia = reader["provincia"].ToString();
                    m_clienteDireccion.distrito = reader["distrito"].ToString();
                    m_clienteDireccion.tipo = reader["tipo"].ToString();
                    m_clienteDireccion.ubigeo = reader["ubigeo"].ToString();
                    m_clienteDireccion.estado = reader["estado"].ToString();
                    lst_clienteDireccion.Add(m_clienteDireccion);
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

            return lst_clienteDireccion;
        }

        public List<ClienteContacto> listaClienteContacto(string codigo, string tipo)
        {
            List<ClienteContacto> lst_clienteContacto = new List<ClienteContacto>();
            ClienteContacto m_clienteContacto = new ClienteContacto();
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                con.conectar();
                cmd = new MySqlCommand("PROC_CLIENTE_CONTACTO_OBTENER", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("@x_codigo", codigo));
                cmd.Parameters.Add(new MySqlParameter("@x_tipo", tipo));
                MySqlDataReader reader = cmd.ExecuteReader();

                m_clienteContacto.CoaCliente = "";
                while (reader.Read())
                {
                    m_clienteContacto.CoaCliente = reader["CoaCliente"].ToString();
                    m_clienteContacto.tipo = reader["tipo"].ToString();
                    m_clienteContacto.persona = reader["persona"].ToString();
                    m_clienteContacto.correo = reader["correo"].ToString();
                    m_clienteContacto.telefono = reader["telefono"].ToString();
                    m_clienteContacto.detalle = reader["detalle"].ToString();
                    m_clienteContacto.FechaActualizacion = Convert.ToDateTime(reader["FechaActualizacion"]);

                    m_clienteContacto.UsuarioActualizacion = reader["UsuarioActualizacion"].ToString();
                    lst_clienteContacto.Add(m_clienteContacto);



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

            return lst_clienteContacto;
        }

        public List<Cliente> listaClienteBusqueda(string tipo,string data1, string data2)
        {
            List<Cliente> lst_cliente = new List<Cliente>();
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                con.conectar();
                cmd = new MySqlCommand("PROC_CLIENTE_OBTENER", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("@x_tipo", tipo));
                cmd.Parameters.Add(new MySqlParameter("@x_dato", data1));
                cmd.Parameters.Add(new MySqlParameter("@x_dato2", data2));
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Cliente m_cliente = new Cliente();
                    //m_cliente.id_cliente = (int)reader["id_cliente"];
                    m_cliente.CoaCliente = reader["CoaCliente"].ToString();
                    m_cliente.rucdni = reader["rucdni"].ToString();
                    m_cliente.razonSocial = reader["razonSocial"].ToString();
                    m_cliente.codigoVendedor = reader["codigoVendedor"].ToString();
                    m_cliente.categoria = reader["categoria"].ToString();
                    m_cliente.codPagoTipo = reader["codPagoTipo"].ToString();
                    m_cliente.UsuarioActualizacion = reader["UsuarioActualizacion"].ToString();
                    m_cliente.Estado = reader["Estado"].ToString();
                    m_cliente.EstadoEasy = reader["EstadoEasy"].ToString();
                    lst_cliente.Add(m_cliente);
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

            return lst_cliente;
        }



        public DataTable getClienteHistorialVentaAdmin(string tipoBusqueda, string Razon_Social, string Ruc, string usr)
        {

            DataTable dt = new DataTable();

            WebServiceEasy.Service_EasySoapClient Service_Easy = new WebServiceEasy.Service_EasySoapClient();

            WebServiceEasy.dto_Parametros parametros_where_1 = new WebServiceEasy.dto_Parametros();
            WebServiceEasy.dto_Parametros parametros_where_2 = new WebServiceEasy.dto_Parametros();
            List<WebServiceEasy.dto_Parametros> _parametrosWhere = new List<WebServiceEasy.dto_Parametros>();

            if (tipoBusqueda == "exacto")
            {

                if (Razon_Social.Trim() != "")
                {
                    parametros_where_1.parametro = "dsc";
                    parametros_where_1.operador = "=";
                    parametros_where_1.valor = "'" + Razon_Social + "'";
                    _parametrosWhere.Add(parametros_where_1);
                }
                if (Ruc.Trim() != "")
                {
                    parametros_where_2.parametro = "coa";
                    parametros_where_2.operador = "=";
                    parametros_where_2.valor = "'" + Ruc + "'";
                    _parametrosWhere.Add(parametros_where_2);
                }
            }
            else if (tipoBusqueda == "like")
            {
                if (Razon_Social.Trim() != "")
                {
                    parametros_where_1.parametro = "dsc";
                    parametros_where_1.operador = "like";
                    parametros_where_1.valor = "'%" + Razon_Social + "%'";
                    _parametrosWhere.Add(parametros_where_1);
                }
                if (Ruc.Trim() != "")
                {
                    parametros_where_2.parametro = "coa";
                    parametros_where_2.operador = "like";
                    parametros_where_2.valor = "'%" + Ruc + "%'";
                    _parametrosWhere.Add(parametros_where_2);
                }
            }

            if (usr != "ALL")
            {
                // para busqueda que noso
                //if (codVendedor.Trim() != "")
                //{
                WebServiceEasy.dto_Parametros parametros_where_3 = new WebServiceEasy.dto_Parametros();
                parametros_where_3.parametro = "vendedor";
                parametros_where_3.operador = "in";
                parametros_where_3.valor = usr;
                _parametrosWhere.Add(parametros_where_3);
                //}
            }



            //string x_usuario = WebConfigurationManager.AppSettings["uWebServ"];
            //string x_password = WebConfigurationManager.AppSettings["pWebServ"];

            DataTable dtCliente = new DataTable();
            dtCliente.TableName = "dtClientes";
            dt = Service_Easy.cliente_historial_venta(x_usuario, x_password, "ALL", _parametrosWhere.ToArray(), dtCliente);
            //dt = Service_Easy.cliente_historial_venta(x_usuario, x_password,"Todo", _parametrosWhere.ToArray(), dtCliente);

            return dt;
        }

        public DataTable getClienteHistorialDocumentos(string coa, string usr)
        {

            DataTable dt = new DataTable();

            WebServiceEasy.Service_EasySoapClient Service_Easy = new WebServiceEasy.Service_EasySoapClient();



            DataTable dtCliente = new DataTable();
            dtCliente.TableName = "dtClientes";
            dt = Service_Easy.cliente_historial_Documentos(x_usuario, x_password, coa, usr);

            return dt;
        }

        public DataTable getClienteEasy(string codVendedor)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("coa", typeof(string));
            dt.Columns.Add("ruc", typeof(string));
            dt.Columns.Add("dni", typeof(string));
            dt.Columns.Add("nombre", typeof(string));
            dt.Columns.Add("direccion", typeof(string));
            dt.Columns.Add("ubigeo", typeof(string));
            dt.Columns.Add("departamento", typeof(string));
            dt.Columns.Add("provincia", typeof(string));
            dt.Columns.Add("distrito", typeof(string));
            dt.Columns.Add("telefono", typeof(string));
            dt.Columns.Add("correo", typeof(string));
            dt.Columns.Add("credito", typeof(string));
            dt.Columns.Add("saldo", typeof(string));
            dt.Columns.Add("vendedor", typeof(string));

            if (codVendedor != "ALL")
            {
                WebServiceEasy.Service_EasySoapClient Service_Easy = new WebServiceEasy.Service_EasySoapClient();
                List<WebServiceEasy.dto_Cliente> Lst_dto_Cliente = new List<WebServiceEasy.dto_Cliente>();
                List<WebServiceEasy.dto_Parametros> _parametrosWhere = new List<WebServiceEasy.dto_Parametros>();
                WebServiceEasy.dto_Parametros parametros_where_1 = new WebServiceEasy.dto_Parametros();
                parametros_where_1.parametro = "vendedor";
                parametros_where_1.operador = "in";
                parametros_where_1.valor = codVendedor;
                _parametrosWhere.Add(parametros_where_1);

                string x_usuario = WebConfigurationManager.AppSettings["uWebServ"];
                string x_password = WebConfigurationManager.AppSettings["pWebServ"];

                DataTable dtCliente = new DataTable();
                dtCliente.TableName = "dtClientes";
                Lst_dto_Cliente = Service_Easy.cliente_obtener_lista(x_usuario, x_password, _parametrosWhere.ToArray()).ToList();

                foreach (var dataCliente in Lst_dto_Cliente)
                {
                    dt.Rows.Add(dataCliente.coa,
                                dataCliente.ruc,
                                dataCliente.lib_elec,
                                dataCliente.dsc,
                                dataCliente.ubigeo,
                                dataCliente.dpto,
                                dataCliente.provincia,
                                dataCliente.distrito,
                                dataCliente.telef1,
                                dataCliente.referen1,
                                dataCliente.credito,
                                dataCliente.saldo_cred,
                                dataCliente.vendedor);
                }

            }

            return dt;
        }

        public DataTable lstClienteCategoria()
        {
            DataTable dt = new DataTable();
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {

                con.conectar();
                cmd = new MySqlCommand("PROC_CLIENTE_CATEGORIA_OBTENER", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;

                using (var da = new MySqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(dt);
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

            return dt;
        }


        public int updateEasyClienteCorreoTelefono(string coa, string telefono, string correo)
        {

            int respuesta = 0;

            string x_usuario = WebConfigurationManager.AppSettings["uWebServ"];
            string x_password = WebConfigurationManager.AppSettings["pWebServ"];
            try
            {
                WebServiceEasy.Service_EasySoapClient Service_Easy = new WebServiceEasy.Service_EasySoapClient();
                respuesta = Service_Easy.cliente_actualiza_TelefonoCorreo(x_usuario, x_password, coa, telefono, correo);
            }
            catch (Exception ex)
            {

            }

            return respuesta;
        }



        //Evaluar quitar esta función para usar la lista
        public ClienteContacto clienteContactoObtener(string codigo, string tipo)
        {
            ClienteContacto m_clienteContacto = new ClienteContacto();
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                con.conectar();
                cmd = new MySqlCommand("PROC_CLIENTE_CONTACTO_OBTENER", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("@x_codigo", codigo));
                cmd.Parameters.Add(new MySqlParameter("@x_tipo", tipo));
                MySqlDataReader reader = cmd.ExecuteReader();

                m_clienteContacto.CoaCliente = "";
                while (reader.Read())
                {
                    m_clienteContacto.CoaCliente = reader["CoaCliente"].ToString();
                    m_clienteContacto.tipo = reader["tipo"].ToString();
                    m_clienteContacto.persona = reader["persona"].ToString();
                    m_clienteContacto.correo = reader["correo"].ToString();
                    m_clienteContacto.telefono = reader["telefono"].ToString();
                    m_clienteContacto.detalle = reader["detalle"].ToString();
                    m_clienteContacto.FechaActualizacion = Convert.ToDateTime(reader["FechaActualizacion"]);
                    m_clienteContacto.UsuarioActualizacion = reader["UsuarioActualizacion"].ToString();
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

            return m_clienteContacto;
        }



    }
}