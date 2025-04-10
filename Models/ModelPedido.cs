using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using WebAppMontGroup.Entity;

namespace WebAppMontGroup.Models
{
    public class ModelPedido
    {



        public int insert_Pedido_Cabecera(Pedido pedido, string accion)
        {
            int result = 0;
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {

                con.conectar();
                cmd = new MySqlCommand("CRUD_PEDIDO", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("@x_opcion", accion)); // Parámetro recibido
                cmd.Parameters.Add(new MySqlParameter("@x_idPedido", pedido.IdPedido));
                cmd.Parameters.Add(new MySqlParameter("@x_Documento", pedido.Documento));
                cmd.Parameters.Add(new MySqlParameter("@x_Serie", pedido.Serie));
                cmd.Parameters.Add(new MySqlParameter("@x_Coa", pedido.Coa));
                cmd.Parameters.Add(new MySqlParameter("@x_TipoDocumentoFiscal", pedido.TipoDocumentoFiscal));
                cmd.Parameters.Add(new MySqlParameter("@x_CodigoTipoPago", pedido.CodigoTipoPago));
                cmd.Parameters.Add(new MySqlParameter("@x_Moneda", pedido.Moneda));
                cmd.Parameters.Add(new MySqlParameter("@x_TipoCambio", pedido.TipoCambio));
                cmd.Parameters.Add(new MySqlParameter("@x_FechaEntrega", pedido.FechaEntrega));
                cmd.Parameters.Add(new MySqlParameter("@x_RucDni", pedido.RucDni));
                cmd.Parameters.Add(new MySqlParameter("@x_Cliente", pedido.Cliente));
                cmd.Parameters.Add(new MySqlParameter("@x_DireccionCliente", pedido.DireccionCliente));
                cmd.Parameters.Add(new MySqlParameter("@x_Ubigeo", pedido.Ubigeo));
                cmd.Parameters.Add(new MySqlParameter("@x_OrdenCompra", pedido.OrdenCompra));
                cmd.Parameters.Add(new MySqlParameter("@x_DocOrdenCompra", pedido.DocOrdenCompra));
                cmd.Parameters.Add(new MySqlParameter("@x_CodCategoriaCliente", pedido.CodCategoriaCliente));
                cmd.Parameters.Add(new MySqlParameter("@x_CodVendedor", pedido.CodVendedor));
                cmd.Parameters.Add(new MySqlParameter("@x_Vendedor", pedido.Vendedor));
                cmd.Parameters.Add(new MySqlParameter("@x_ObservacionCredito", pedido.ObservacionCredito));
                cmd.Parameters.Add(new MySqlParameter("@x_ObservacionPrecio", pedido.ObservacionPrecio));
                cmd.Parameters.Add(new MySqlParameter("@x_ObservacionAlmacen", pedido.ObservacionAlmacen));
                cmd.Parameters.Add(new MySqlParameter("@x_ZonaVenta", pedido.ZonaVenta));
                cmd.Parameters.Add(new MySqlParameter("@x_SubTotal", pedido.SubTotal));
                cmd.Parameters.Add(new MySqlParameter("@x_TotalDescuento", pedido.TotalDescuento));
                cmd.Parameters.Add(new MySqlParameter("@x_igvPorcentaje", pedido.IgvPorcentaje));
                cmd.Parameters.Add(new MySqlParameter("@x_Igv", pedido.Igv));
                cmd.Parameters.Add(new MySqlParameter("@x_Total", pedido.Total));
                cmd.Parameters.Add(new MySqlParameter("@x_UsuarioActualizacion", pedido.UsuarioActualizacion));

                cmd.Parameters.Add(new MySqlParameter("@x_idPedidoNuevo", MySqlDbType.Int32)).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                var outParamValue = cmd.Parameters["@x_idPedidoNuevo"].Value; // Aquí cambias el nombre
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

        public int insert_Pedido_Detalle(PedidoDetalle pedido_detalle, string accion)
        {
            int result = 0;
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {

                con.conectar();
                cmd = new MySqlCommand("CRUD_PEDIDO_DETALLE", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("@x_Opcion", accion));
                cmd.Parameters.Add(new MySqlParameter("@x_idPedidoDet", pedido_detalle.IdPedidoDetalle));
                cmd.Parameters.Add(new MySqlParameter("@x_idPedido", pedido_detalle.IdPedido));
                cmd.Parameters.Add(new MySqlParameter("@x_Almacen", pedido_detalle.Almacen));
                cmd.Parameters.Add(new MySqlParameter("@x_Lote", pedido_detalle.Lote));
                cmd.Parameters.Add(new MySqlParameter("@x_Articulo", pedido_detalle.Articulo));
                cmd.Parameters.Add(new MySqlParameter("@x_ArticuloDesc", pedido_detalle.ArticuloDesc));
                cmd.Parameters.Add(new MySqlParameter("@x_Cantidad", pedido_detalle.Cantidad));
                cmd.Parameters.Add(new MySqlParameter("@x_Precio", pedido_detalle.Precio));
                cmd.Parameters.Add(new MySqlParameter("@x_SubTotal", pedido_detalle.SubTotal));
                cmd.Parameters.Add(new MySqlParameter("@x_Descuento", pedido_detalle.Descuento));
                cmd.Parameters.Add(new MySqlParameter("@x_TipoPromocion", pedido_detalle.TipoPromocion));
                cmd.Parameters.Add(new MySqlParameter("@x_AplicaPromocion", pedido_detalle.AplicaPromocion));
                cmd.Parameters.Add(new MySqlParameter("@x_ResponsableP", pedido_detalle.UsuarioResponsable));
                cmd.Parameters.Add(new MySqlParameter("@x_UsuarioActualiza", pedido_detalle.UsuarioActualiza));
                cmd.Parameters.Add(new MySqlParameter("@x_idP", MySqlDbType.Int32)).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                var outParamValue = cmd.Parameters["@x_idP"].Value;
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

        public int insert_Pedido_Guia(PedidoGuia pedido_guia, string accion)
        {
            int result = 0;
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {

                con.conectar();
                cmd = new MySqlCommand("PROC_PEDIDO_GUIA", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("@x_opcion", accion));
                cmd.Parameters.Add(new MySqlParameter("@x_idPedidoGuia", pedido_guia.idPedidoGuia));
                cmd.Parameters.Add(new MySqlParameter("@x_IdPedido", pedido_guia.IdPedido));
                cmd.Parameters.Add(new MySqlParameter("@x_GuiaRegion", pedido_guia.GuiaRegion));
                cmd.Parameters.Add(new MySqlParameter("@x_GuiaDireccionPartida", pedido_guia.GuiaDireccionPartida));
                cmd.Parameters.Add(new MySqlParameter("@x_GuiaUbigeoPartida", pedido_guia.GuiaUbigeoPartida));
                cmd.Parameters.Add(new MySqlParameter("@x_GuiaPuntoEntrega", pedido_guia.GuiaPuntoEntrega));
                cmd.Parameters.Add(new MySqlParameter("@x_GuiaDireccionEntrega", pedido_guia.GuiaDireccionEntrega));
                cmd.Parameters.Add(new MySqlParameter("@x_GuiaUbigeoEntrega", pedido_guia.GuiaUbigeoEntrega));
                cmd.Parameters.Add(new MySqlParameter("@x_GuiaTransporte", pedido_guia.GuiaTransporte));
                cmd.Parameters.Add(new MySqlParameter("@x_GuiaTransporteEmpRuc", pedido_guia.GuiaTransporteEmpRuc));
                cmd.Parameters.Add(new MySqlParameter("@x_GuiaTransporteEmpresa", pedido_guia.GuiaTransporteEmpresa));
                cmd.Parameters.Add(new MySqlParameter("@x_RepcionTipo1", pedido_guia.RepcionTipo1));
                cmd.Parameters.Add(new MySqlParameter("@x_Dni1", pedido_guia.Dni1));
                cmd.Parameters.Add(new MySqlParameter("@x_Nombre1", pedido_guia.Nombre1));
                cmd.Parameters.Add(new MySqlParameter("@x_Telefono1", pedido_guia.Telefono1));
                cmd.Parameters.Add(new MySqlParameter("@x_RepcionTipo2", pedido_guia.RepcionTipo2));
                cmd.Parameters.Add(new MySqlParameter("@x_Dni2", pedido_guia.Dni2));
                cmd.Parameters.Add(new MySqlParameter("@x_Nombre2", pedido_guia.Nombre2));
                cmd.Parameters.Add(new MySqlParameter("@x_Telefono2", pedido_guia.Telefono2));
                cmd.Parameters.Add(new MySqlParameter("@x_UsuarioActualizacion", pedido_guia.UsuarioActualizacion));
                cmd.Parameters.Add(new MySqlParameter("@x_EstadoGuia", "ACT"));

                cmd.Parameters.Add(new MySqlParameter("@x_id", MySqlDbType.Int32)).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                var outParamValue = cmd.Parameters["@x_id"].Value;
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

        public Pedido PedidoPorId(int idPedido)
        {
            Pedido m_pedido = new Pedido();
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {

                con.conectar();
                cmd = new MySqlCommand("PROC_PEDIDO_OBTENER", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("@x_IdPedido", idPedido));
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    m_pedido.IdPedido = (int)reader["IdPedido"];
                    m_pedido.Documento = reader["Documento"].ToString();
                    m_pedido.Serie = reader["Serie"].ToString();
                    m_pedido.Numero = reader["Numero"].ToString();
                    m_pedido.CodigoPedido = reader["CodigoPedido"].ToString();
                    m_pedido.Coa = reader["Coa"].ToString();
                    m_pedido.TipoDocumentoFiscal = reader["TipoDocumentoFiscal"].ToString();
                    m_pedido.DocumentoFiscal = reader["DocumentoFiscal"].ToString(); 
                    m_pedido.CodigoTipoPago = reader["CodigoTipoPago"].ToString();
                    m_pedido.NombreTipoPago = reader["NombreTipoPago"].ToString();
                    m_pedido.Moneda = reader["Moneda"].ToString();
                    m_pedido.TipoCambio = reader["TipoCambio"].ToString();
                    m_pedido.FechaEntrega = Convert.ToDateTime(reader["FechaEntrega"]);
                    m_pedido.RucDni = reader["RucDni"].ToString();
                    m_pedido.Cliente = reader["Cliente"].ToString();
                    m_pedido.DireccionCliente = reader["DireccionCliente"].ToString();
                    m_pedido.Ubigeo = reader["Ubigeo"].ToString();
                    m_pedido.OrdenCompra = reader["OrdenCompra"].ToString();
                    m_pedido.DocOrdenCompra = reader["DocOrdenCompra"].ToString();
                    m_pedido.CodCategoriaCliente = reader["CodCategoriaCliente"].ToString();
                    m_pedido.CodVendedor = reader["CodVendedor"].ToString();
                    m_pedido.Vendedor = reader["Vendedor"].ToString();
                    m_pedido.FechaRegistro = Convert.ToDateTime(reader["FechaRegistro"]);
                    m_pedido.ObservacionPrecio = reader["ObservacionPrecio"].ToString();
                    m_pedido.ObservacionCredito = reader["ObservacionCredito"].ToString();
                    m_pedido.ObservacionAlmacen = reader["ObservacionAlmacen"].ToString();
                    m_pedido.ZonaVenta = reader["ZonaVenta"].ToString();
                    m_pedido.SubTotal = (double)reader["SubTotal"];
                    m_pedido.TotalDescuento = (double)reader["TotalDescuento"];
                    m_pedido.IgvPorcentaje = (double)reader["IgvPorcentaje"];
                    m_pedido.Igv = (double)reader["Igv"];
                    m_pedido.Total = (double)reader["Total"];
                    m_pedido.Estado = reader["Estado"].ToString();
                    m_pedido.EstadoProducto = reader["EstadoProducto"].ToString();
                    m_pedido.EstadoCredito = reader["EstadoCredito"].ToString();
                    m_pedido.EstadoAlmacen = reader["EstadoAlmacen"].ToString();
                    m_pedido.SeguimientoCredito = reader["SeguimientoCredito"].ToString();
                    m_pedido.FechaActualizacion = Convert.ToDateTime(reader["FechaActualizacion"]);
                    m_pedido.UsuarioActualizacion = reader["UsuarioActualizacion"].ToString();

                    m_pedido.pedido_detalle = listaPedidoDetalle(m_pedido.IdPedido, "RE");
                    m_pedido.pedido_guia = PedidoGuia(m_pedido.IdPedido);
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

            return m_pedido;
        }

        public List<PedidoDetalle> listaPedidoDetalle(int idPedido, string estado)
        {
            List<PedidoDetalle> lst_pedidoDetalle = new List<PedidoDetalle>();
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {

                con.conectar();
                cmd = new MySqlCommand("PROC_PEDIDO_DETALLE_OBTENER", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("@x_IdPedido", idPedido));
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

        public PedidoGuia PedidoGuia(int idPedido)
        {
            PedidoGuia m_pedidoGuia = new PedidoGuia();
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {

                con.conectar();
                cmd = new MySqlCommand("PROC_PEDIDO_GUIA_OBTENER", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("@x_IdPedido", idPedido));
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    m_pedidoGuia.idPedidoGuia = (int)reader["idPedidoGuia"];
                    m_pedidoGuia.IdPedido = (int)reader["IdPedido"];
                    m_pedidoGuia.Guia = reader["Guia"].ToString();
                    m_pedidoGuia.GuiaRegion = reader["GuiaRegion"].ToString();
                    m_pedidoGuia.GuiaDireccionPartida = reader["GuiaDireccionPartida"].ToString();
                    m_pedidoGuia.GuiaUbigeoPartida = reader["GuiaUbigeoPartida"].ToString();
                    m_pedidoGuia.GuiaPuntoEntrega = reader["GuiaPuntoEntrega"].ToString();
                    m_pedidoGuia.GuiaDireccionEntrega = reader["GuiaDireccionEntrega"].ToString();
                    m_pedidoGuia.GuiaUbigeoEntrega = reader["GuiaUbigeoEntrega"].ToString();
                    m_pedidoGuia.GuiaTransporte = reader["GuiaTransporte"].ToString();
                    m_pedidoGuia.GuiaTransporteEmpRuc = reader["GuiaTransporteEmpRuc"].ToString();
                    m_pedidoGuia.GuiaTransporteEmpresa = reader["GuiaTransporteEmpresa"].ToString();
                    m_pedidoGuia.RepcionTipo1 = reader["RepcionTipo1"].ToString();
                    m_pedidoGuia.Dni1 = reader["Dni1"].ToString();
                    m_pedidoGuia.Nombre1 = reader["Nombre1"].ToString();
                    m_pedidoGuia.Telefono1 = reader["Telefono1"].ToString();
                    m_pedidoGuia.RepcionTipo2 = reader["RepcionTipo2"].ToString();
                    m_pedidoGuia.Dni2 = reader["Dni2"].ToString();
                    m_pedidoGuia.Nombre2 = reader["Nombre2"].ToString();
                    m_pedidoGuia.Telefono2 = reader["Telefono2"].ToString();
                    m_pedidoGuia.EstadoGuia = reader["EstadoGuia"].ToString();
                    m_pedidoGuia.FechaActualizacion = Convert.ToDateTime(reader["FechaActualizacion"]);
                    m_pedidoGuia.UsuarioActualizacion = reader["UsuarioActualizacion"].ToString();
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

            return m_pedidoGuia;
        }

        private string EstadoPedido(string est_g_p_c_a)
        {

            /*
             RE => Registrado
             PE => Pendiente
             APR = Aprobado
             DES = Desaprobado
             */
            string estado_rex = "";
            switch (est_g_p_c_a)
            {
                case "RE-PE-PE-PE":
                    estado_rex = "Pendiente";
                    break;
                case "RE-APR-PE-PE":
                    estado_rex = "Pendiente credito y Almacén";
                    break;
                case "RE-PE-APR-PE":
                    estado_rex = "Pendiente Precio y Almacén";
                    break;
                case "RE-APR-APR-PE":
                    estado_rex = "Pendiente Almacén";
                    break;
                case "RE-APR-APR-APR":
                    estado_rex = "Por facturar";
                    break;
                default:
                    // code block
                    break;
            }

            return estado_rex;
        }

       /* public List<Pedido> listaPedido(int anio, int mes, string estado)
        {
            List<Pedido> lst_pedidos = new List<Pedido>();
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                con.conectar();
                cmd = new MySqlCommand("PROC_PEDIDOLISTADO_OBTENER", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("p_Anio", anio));
                cmd.Parameters.Add(new MySqlParameter("p_Mes", mes));
                cmd.Parameters.Add(new MySqlParameter("p_Estado", estado));
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Pedido pedido = new Pedido();
                    pedido.IdPedido = (int)reader["IdPedido"];
                    pedido.FechaRegistro = Convert.ToDateTime(reader["FechaRegistro"]);
                    pedido.FechaEntrega = Convert.ToDateTime(reader["FechaEntrega"]);
                    pedido.Estado = reader["Estado"].ToString();
                    pedido.Coa = reader["Coa"].ToString();
                    pedido.Cliente = reader["Cliente"].ToString();
                    pedido.Total = (double)reader["Total"];
                    lst_pedidos.Add(pedido);
                }
            }
            catch (Exception ex)
            {
                // util_log.Escribir_Log("listaPedido, " + ex.ToString());
                return null;
            }
            finally
            {
                cmd.Dispose();
                con.desconectar();
            }

            return lst_pedidos;
        }
       */
        public List<Pedido> listaPedido(int anio, int mes, string CodVendedor, string estado)
        {
            List<Pedido> lst_pedidos = new List<Pedido>();
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                con.conectar();
                cmd = new MySqlCommand("PROC_PEDIDO_LISTADO_OBTENER", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("p_Anio", anio));
                cmd.Parameters.Add(new MySqlParameter("p_Mes", mes));
                cmd.Parameters.Add(new MySqlParameter("p_CodVendedor", CodVendedor));
                cmd.Parameters.Add(new MySqlParameter("p_Estado", estado));
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Pedido pedido = new Pedido();
                    pedido.IdPedido = (int)reader["IdPedido"];
                    pedido.CodigoPedido = reader["CodigoPedido"].ToString();
                    pedido.Estado = reader["Estado"].ToString();
                    pedido.Coa = reader["Coa"].ToString();
                    pedido.Cliente = reader["Cliente"].ToString();
                    pedido.FechaRegistro = Convert.ToDateTime(reader["FechaRegistro"]);
                    pedido.FechaEntrega = Convert.ToDateTime(reader["FechaEntrega"]);
                    pedido.CodigoTipoPago = reader["CodigoTipoPago"].ToString();
                    // Asignar el nombre del tipo de pago
                    pedido.NombreTipoPago = reader["NombreTipoPago"].ToString();
                    pedido.TipoDocumentoFiscal = reader["TipoDocumentoFiscal"].ToString();
                    pedido.Total = (double)reader["Total"];
                    pedido.Vendedor = reader["Vendedor"].ToString();

                    lst_pedidos.Add(pedido);
                }
            }
            catch (Exception ex)
            {
                // util_log.Escribir_Log("listaPedido, " + ex.ToString());
                return null;
            }
            finally
            {
                cmd.Dispose();
                con.desconectar();
            }

            return lst_pedidos;
        }


        public RespuestaValidacion ValidarCredito(string coa, double montoventa, double montoVentaPendiente)
        {
            RespuestaValidacion respuesta = new RespuestaValidacion();

            string x_usuario = WebConfigurationManager.AppSettings["uWebServ"];
            string x_password = WebConfigurationManager.AppSettings["pWebServ"];

            try
            {
                List<string> lstCoas = new List<string>();
                ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
                MySqlCommand cmd = new MySqlCommand();

                try
                {
                    con.conectar();
                    cmd = new MySqlCommand("PROC_LISTADOCOAS_RELACIONADOS_OBTENER", con.retConeccion());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CoaCliente", coa);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lstCoas.Add(reader["ListadoCoas"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error al obtener COAs relacionados: {ex.Message}");
                }

                // Convertir la lista en un array antes de enviarla al servicio
                string[] arrayCoas = lstCoas.ToArray();

                WebServiceEasy.Service_EasySoapClient Service_Easy = new WebServiceEasy.Service_EasySoapClient();
                DataTable dt = Service_Easy.cliente_ValidaVenta(x_usuario, x_password, arrayCoas, montoventa, montoVentaPendiente);

                if (dt.Rows.Count > 0)
                {
                    respuesta.Aprobado = dt.Rows[0]["Aprobado"].ToString();
                    respuesta.Mensaje = dt.Rows[0]["Mensaje"].ToString();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al validar venta: {ex.Message}");
                respuesta.Aprobado = "NO";
                respuesta.Mensaje = "Error al conectar con el servicio.";
            }

            return respuesta;
        }

        public PedidoTotales ObtenerTotalesPedido(string coa, int idpedido)
        {
            PedidoTotales totales = null;
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");

            try
            {
                con.conectar();
                using (MySqlCommand cmd = new MySqlCommand("PROC_PEDIDO_OBTENERTOTALES", con.retConeccion()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("coa_param", coa));
                    cmd.Parameters.Add(new MySqlParameter("x_idPedido", idpedido));

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            totales = new PedidoTotales
                            {
                                SumaTotal = reader.IsDBNull(reader.GetOrdinal("suma_total")) ? 0 : reader.GetDecimal(reader.GetOrdinal("suma_total")),
                                UltimoTotal = reader.IsDBNull(reader.GetOrdinal("ultimo_total")) ? 0 : reader.GetDecimal(reader.GetOrdinal("ultimo_total")),
                                Coa = coa
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener totales: " + ex.Message);
            }
            finally
            {
                con.desconectar();
            }

            return totales;
        }

        public void ActualizarEstadoPedidoCredito(int idpedido, string coa, string aprobado, string observacion)
        {
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            {
                con.conectar();
                using (MySqlCommand cmd = new MySqlCommand("PROC_PEDIDO_ACTUALIZAR_ESTADOCREDITO", con.retConeccion()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_IdPedido", idpedido);
                    cmd.Parameters.AddWithValue("p_Coa", coa);
                    cmd.Parameters.AddWithValue("p_Aprobado", aprobado);
                    cmd.Parameters.AddWithValue("p_ObservacionCredito", observacion);

                    cmd.ExecuteNonQuery();
                }
            }
        }



        public int actualizar_Pedido_datos(int idPedido)
        {
            int result = 0;
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {

                con.conectar();
                cmd = new MySqlCommand("PROC_ACTUALIZAR_DATOS_PEDIDO", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("@x_IdPedido", idPedido));
                cmd.ExecuteNonQuery();
                result = cmd.ExecuteNonQuery();
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
        

        public List<PedidoLog> lista_Pedido_log(int idPedido, string accion)
        {
            List<PedidoLog> lst_pedidoLog = new List<PedidoLog>();
            ConeccionMysql con = new ConeccionMysql("MYSQL_Conexion_Pedido");
            MySqlCommand cmd = new MySqlCommand();
            try
            {

                con.conectar();
                cmd = new MySqlCommand("PROC_PEDIDO_LOG_OBTENER", con.retConeccion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("@x_IdPedido", idPedido));
                cmd.Parameters.Add(new MySqlParameter("@x_accion", accion));
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    PedidoLog m_pedidoLog = new PedidoLog();

                    m_pedidoLog.IdPedido = (int)reader["IdPedido"];
                    m_pedidoLog.Fecha = Convert.ToDateTime(reader["Fecha"]);
                    m_pedidoLog.Usuario = reader["Usuario"].ToString();
                    m_pedidoLog.Accion = reader["Accion"].ToString();
                    m_pedidoLog.Obs = reader["Obs"].ToString();
                    m_pedidoLog.Detalle1 = reader["Detalle1"].ToString();
                    m_pedidoLog.Detalle2 = reader["Detalle2"].ToString();
                    lst_pedidoLog.Add(m_pedidoLog);
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

            return lst_pedidoLog;
        }
    }
}