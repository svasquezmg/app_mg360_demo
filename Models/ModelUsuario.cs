using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.EnterpriseServices.Internal;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Configuration;
using WebAppMontGroup.Entity;
using static System.Net.Mime.MediaTypeNames;
namespace WebAppMontGroup.Models
{
    public class ModelUsuario
    {
        Util util = new Util();

        public Usuario loginRV(string opcion, string user, string contrasenia)
        {
            Usuario m_usuario = new Usuario();
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                con.conectar();
                cmd = new MySqlCommand("PROC_LOGIN_RV", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("@x_opcion", opcion));
                cmd.Parameters.Add(new MySqlParameter("@x_usuario", user));
                cmd.Parameters.Add(new MySqlParameter("@x_contrasenia", contrasenia));
                MySqlDataReader reader = cmd.ExecuteReader();

                m_usuario.codigovendedor = "";
                while (reader.Read())
                {
                    m_usuario.idusuario = (int)reader["idusuario"];
                    m_usuario.usuario = reader["usuario"].ToString();
                    m_usuario.contrasenia = reader["contrasenia"].ToString();
                    m_usuario.nombres = reader["nombres"].ToString();
                    m_usuario.correo = reader["correo"].ToString();
                    m_usuario.codigovendedor = reader["codigovendedor"].ToString();
                    m_usuario.codigoalmacen = reader["codigoalmacen"].ToString();
                    m_usuario.codigovendedorasociado = reader["codigovendedorasociado"].ToString();
                    m_usuario.usuarioactualizacion = reader["usuarioactualizacion"].ToString();
                    m_usuario.estado = reader["estado"].ToString();
                    m_usuario.cargo = reader["cargo"].ToString();
                    m_usuario.telefono = reader["telefono"].ToString();
                    m_usuario.estiloWeb = reader["opcion1"].ToString();
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

            return m_usuario;
        }

        public string validarUsuarioAD(string user, string contrasenia)
        {
            string texto_respuesta = "FALSE";
            Stream respuestaDeEnvio;
            string urlEndPoint = WebConfigurationManager.AppSettings["EndPointMG"];
            urlEndPoint = urlEndPoint + "validaUsuarioAD?strUsuario=" + user + "&strContrasena=" + contrasenia;
        
            try
            {        
                using (WebClient webClient = new WebClient())
                {
                    webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json; charset=utf-8");
                    respuestaDeEnvio = webClient.OpenRead(urlEndPoint);
                    StreamReader string_reader = new StreamReader(respuestaDeEnvio);
                    texto_respuesta = string_reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {

            }
            return texto_respuesta.ToUpper();
        }


        public Usuario loginAdministrativo(string user, string contrasenia)
        {
          
            Stream respuestaDeEnvio;
            string texto_respuesta = "";
            Usuario m_usuario = new Usuario();
            m_usuario.idusuario = 0;

            try
            {
              
                string urlEndPoint = WebConfigurationManager.AppSettings["EndPointMG"];
                urlEndPoint = urlEndPoint + "validaUsuarioAD?strUsuario=" + user + "&strContrasena=" + contrasenia;

                using (WebClient webClient = new WebClient())
                {
                    webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json; charset=utf-8");
                    respuestaDeEnvio = webClient.OpenRead(urlEndPoint);
                    StreamReader string_reader = new StreamReader(respuestaDeEnvio);
                    texto_respuesta = string_reader.ReadToEnd();

                    if (texto_respuesta.ToUpper() == "TRUE")
                    {
                        ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Accesos");
                        MySqlCommand cmd = new MySqlCommand();
                        try
                        {
                            con.conectar();
                            cmd = new MySqlCommand("PROC_USUARIO_OBTENER", con.retConeccion());
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new MySqlParameter("@x_opcion", "LOGIN"));
                            cmd.Parameters.Add(new MySqlParameter("@x_usuario", user));
                            cmd.Parameters.Add(new MySqlParameter("@x_contrasenia", ""));
                            cmd.Parameters.Add(new MySqlParameter("@x_estado", ""));

                            MySqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                m_usuario.idusuario = int.Parse(reader["IdUsuario"].ToString());
                                m_usuario.usuario = reader["usuario"].ToString();
                                m_usuario.contrasenia = "";
                                m_usuario.nombres = reader["nombres"].ToString() + " " + reader["apellidos"].ToString();
                                m_usuario.correo = reader["correo"].ToString();
                                m_usuario.codigovendedor = "";
                                m_usuario.codigoalmacen = "";
                                m_usuario.codigovendedorasociado = "";
                                m_usuario.usuarioactualizacion = "";
                                m_usuario.estado = reader["estado"].ToString();
                                m_usuario.estiloWeb = reader["opcion1"].ToString();
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
                    }
                }
            }
            catch (WebException ex)
            {
                util.Escribir_Log(ex.ToString());
                return null;
            }

            return m_usuario;
        }

        public List<Usuario> listarVendedorAsociado(string codVendedorAsociado)
        {

            List<Usuario> lst_usuario = new List<Usuario>();
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {

                con.conectar();
                cmd = new MySqlCommand("PROC_USUARIO_VENDEDOR_ASOCIADO", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("@x_vendedor_asociado", codVendedorAsociado));
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Usuario m_usuario = new Usuario();
                    m_usuario.idusuario = (int)reader["idusuario"];
                    m_usuario.usuario = reader["usuario"].ToString();
                    m_usuario.contrasenia = reader["contrasenia"].ToString();
                    m_usuario.nombres = reader["nombres"].ToString();
                    m_usuario.correo = reader["correo"].ToString();
                    m_usuario.codigovendedor = reader["codigovendedor"].ToString();
                    m_usuario.codigoalmacen = reader["codigoalmacen"].ToString();
                    m_usuario.codigovendedorasociado = reader["codigovendedorasociado"].ToString();
                    m_usuario.usuarioactualizacion = reader["usuarioactualizacion"].ToString();
                    m_usuario.estado = reader["estado"].ToString();
                    m_usuario.estiloWeb = reader["opcion1"].ToString();
                    lst_usuario.Add(m_usuario);
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

            return lst_usuario;
        }

        public DataTable permisos(string usuario, string codRol, string app)
        {
            DataTable dt = new DataTable();
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Accesos");
            con.conectar();
            string storedProcedure = "PROC_PERMISOS_OBTENER";
            MySqlCommand comand = new MySqlCommand(storedProcedure, con.retConeccion());
            comand.CommandType = CommandType.StoredProcedure;
            comand.Parameters.AddWithValue("@x_opcion", "ACCESO");
            comand.Parameters.AddWithValue("@x_usuario", usuario);
            comand.Parameters.AddWithValue("@x_codRol", codRol);
            comand.Parameters.AddWithValue("@x_app", app);
            comand.Parameters.AddWithValue("@x_estado", "1");
            comand.Parameters.AddWithValue("@x_value", "");

            MySqlDataAdapter adapter = new MySqlDataAdapter(comand);
            try
            {
                // comand.ExecuteNonQuery()
                adapter.Fill(dt);
                con.desconectar();
            }
            catch (Exception ex)
            {
                dt = null/* TODO Change to default(_) if this is not a reference type */;
            }
            finally
            {
                con.desconectar();
            }

            return dt;
        }

        public DataTable permisosPagina(string usuario, string codRol, string app, string pagina)
        {
            DataTable dt = new DataTable();
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Accesos");
            con.conectar();
            string storedProcedure = "PROC_PERMISOS_OBTENER";
            MySqlCommand comand = new MySqlCommand(storedProcedure, con.retConeccion());
            comand.CommandType = CommandType.StoredProcedure;
            comand.Parameters.AddWithValue("@x_opcion", "ACCESO_FORMULARIO");
            comand.Parameters.AddWithValue("@x_usuario", usuario);
            comand.Parameters.AddWithValue("@x_codRol", codRol);
            comand.Parameters.AddWithValue("@x_app", app);
            comand.Parameters.AddWithValue("@x_estado", "1");
            comand.Parameters.AddWithValue("@x_value", pagina);

            MySqlDataAdapter adapter = new MySqlDataAdapter(comand);
            try
            {
                // comand.ExecuteNonQuery()
                adapter.Fill(dt);
                con.desconectar();
            }
            catch (Exception ex)
            {
                dt = null/* TODO Change to default(_) if this is not a reference type */;
            }
            finally
            {
                con.desconectar();
            }

            return dt;
        }



        //public List<Usuario> listarUSuarioRV(int id_usuario)
        //{

        //    List<Usuario> lst_usuario = new List<Usuario>();
        //    ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
        //    MySqlCommand cmd = new MySqlCommand();
        //    try
        //    {
        //        con.conectar();
        //        cmd = new MySqlCommand("PROC_USUARIO_OBTENER_RV", con.retConeccion());
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        cmd.Parameters.Add(new MySqlParameter("@x_usuario", id_usuario));
        //        MySqlDataReader reader = cmd.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            Usuario m_usuario = new Usuario();
        //            m_usuario.idusuario = (int)reader["idusuario"];
        //            m_usuario.usuario = reader["usuario"].ToString();
        //            m_usuario.contrasenia = reader["contrasenia"].ToString();
        //            m_usuario.nombres = reader["nombres"].ToString();
        //            m_usuario.correo = reader["correo"].ToString();
        //            m_usuario.codigovendedor = reader["codigovendedor"].ToString();
        //            m_usuario.codigoalmacen = reader["codigoalmacen"].ToString();
        //            m_usuario.codigovendedorasociado = reader["codigovendedorasociado"].ToString();
        //            m_usuario.usuarioactualizacion = reader["usuarioactualizacion"].ToString();
        //            m_usuario.estado = reader["estado"].ToString();
        //            lst_usuario.Add(m_usuario);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        //util_log.Escribir_Log("listarUSuario," + ex.ToString());
        //        return null;
        //    }
        //    finally
        //    {
        //        cmd.Dispose();
        //        con.desconectar();
        //    }

        //    return lst_usuario;
        //}

    }
}