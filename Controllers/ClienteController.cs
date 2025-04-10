using iText.Kernel.XMP.Impl;
using iText.StyledXmlParser.Css.Resolve.Shorthand.Impl;
using iText.StyledXmlParser.Css.Selector.Item;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Security;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using WebAppMontGroup.Entity;
using WebAppMontGroup.Models;
using SecurityManager = WebAppMontGroup.Models.SecurityManager;

namespace WebAppMontGroup.Controllers
{
    public class ClienteController : Controller
    {
        SecurityManager security_manager = new SecurityManager();
        // GET: Cliente
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult insertUpdate_Cliente(Cliente cliente, ClienteDireccion clienteDireccion, ClienteContacto clienteContacto)
        {
            ModelCliente m_cliente = new ModelCliente();
            General general = new General();

            bool permiso = false;
            permiso = security_manager.validaSesion();
            if (!security_manager.validaAccesoPagina("Cliente/Clientecrear", "E"))
            {
                permiso = false;
            }

            if (permiso == true)
            {
                int respuesta = 0;
                bool usuariohabilitado = false;
                var usuario = Session["SessionUsuario"] as Usuario;
                cliente.UsuarioActualizacion = usuario.usuario;
                string tipoUsuario = Session["TipoUsuario"].ToString();

                if (tipoUsuario == "1")
                {
                    if (String.IsNullOrEmpty(cliente.CoaCliente)) //if (cliente.CoaCliente == "" || cliente.CoaCliente == null)
                    {
                        if (cliente.rucdni.Length == 11)
                        {
                            cliente.CoaCliente = cliente.rucdni;
                        }
                        else
                        {
                            cliente.CoaCliente = "000000000" + cliente.rucdni;
                            cliente.CoaCliente = cliente.CoaCliente.Substring(cliente.CoaCliente.Length - 11, 11);
                        }

                        respuesta = m_cliente.insert_Cliente(cliente);

                        if (respuesta == 1)
                        {
                            clienteDireccion.CoaCliente = cliente.CoaCliente;
                            clienteDireccion.tipo = "Fiscal";
                            clienteDireccion.UsuarioActualizacion = usuario.usuario;
                            int res = m_cliente.insertUpdate_ClienteDireccion(clienteDireccion, "CREATE");

                            clienteContacto.CoaCliente = cliente.CoaCliente;
                            clienteContacto.tipo = "Principal";
                            clienteContacto.UsuarioActualizacion = usuario.usuario;
                            int res2 = m_cliente.insertUpdate_ClienteContacto(clienteContacto, "CREATE");
                        }
                    }
                    else
                    {
                        List<Cliente> LstCliente = new List<Cliente>(); ;
                        LstCliente = m_cliente.listaClienteBusqueda( "id", cliente.CoaCliente, "");

                        if (usuario.codigovendedor == LstCliente[0].codigoVendedor)
                        {
                            usuariohabilitado = true;
                            cliente.EstadoEasy = LstCliente[0].EstadoEasy;
                        }
                        else
                        {
                            List<Usuario> lstUsuario = Session["VendedorAsociado"] as List<Usuario>;
                            foreach (var item in lstUsuario)
                            {
                                if (usuario.codigovendedor == item.codigovendedor)
                                {
                                    usuariohabilitado = true;
                                    cliente.EstadoEasy = LstCliente[0].EstadoEasy;
                                    break;
                                }
                            }
                        }
                        if (usuariohabilitado == true)
                        {
                            clienteContacto.CoaCliente = cliente.CoaCliente;
                            clienteContacto.tipo = "Principal";
                            clienteContacto.UsuarioActualizacion = usuario.usuario;
                            respuesta = m_cliente.insertUpdate_ClienteContacto(clienteContacto, "UPDATE-RV");

                            if (cliente.EstadoEasy.ToString() == "1" && clienteContacto.tipo == "Principal")
                            {
                                m_cliente.updateEasyClienteCorreoTelefono(cliente.CoaCliente, clienteContacto.telefono, clienteContacto.correo);
                            }
                        }
                        else
                        {
                            general.valor_1 = "0";
                            general.valor_2 = "No tiene permiso para editar cliente " + cliente.CoaCliente;
                        }
                    }
                }

                general.valor_1 = respuesta.ToString();

                if (respuesta == -2)
                {
                    general.valor_2 = "El Ruc o Dni ya se encuentran registrado";
                }
                if (respuesta == 1)
                {
                    general.valor_1 = cliente.CoaCliente;
                    general.valor_2 = "";
                }
            }
            else
            {
                general.valor_1 = "0";
                general.valor_2 = "No tiene permiso para editar cliente";
            }


            return Json(general, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult listaClienteBusqueda(string ruc_razon)
        {
            ModelCliente model_cliente = new ModelCliente();
            List<Cliente> lst_cliente = new List<Cliente>();
            if (security_manager.validaSesion() == true)
            {
                lst_cliente = model_cliente.listaClienteBusqueda("like", ruc_razon,"");
            }
            return Json(lst_cliente, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult listaClienteEasy()
        {
            ModelCliente model_cliente = new ModelCliente();
            Respuesta_Json respuesta_Json = new Respuesta_Json();
            DataTable dt = new DataTable();
            if (security_manager.validaSesion() == true)
            {
                string codVendedor = security_manager.codUserConsulta();
                dt = model_cliente.getClienteEasy(codVendedor);
            }
            return Json(respuesta_Json.DatTableToDictionary(dt), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult listaClienteDireccion(string coa_cliente)
        {
            ModelCliente model_cliente = new ModelCliente();
            List<ClienteDireccion> lst_clienteDireccion = new List<ClienteDireccion>();
            if (security_manager.validaSesion() == true)
            {
                lst_clienteDireccion = model_cliente.listaClienteDireccion(coa_cliente, "");
            }
            return Json(lst_clienteDireccion, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult clienteContactoObtener(string coa_cliente, string tipo)
        {
            ModelCliente model_cliente = new ModelCliente();
            ClienteContacto cliente_contacto = new ClienteContacto();
            if (security_manager.validaSesion() == true)
            {
                cliente_contacto = model_cliente.clienteContactoObtener(coa_cliente, tipo);
            }
            return Json(cliente_contacto, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ClientelstVendedor(string codVendedor)
        {
            //if (security_manager.validaAccesoPagina("Pedido/PedidoList", "R"))
            //{
            try
            {
                /*Usuario usuario = new Usuario();
                string tipo_usuario = Session["TipoUsuario"].ToString();
                if (tipo_usuario == "1")
                {
                    usuario = Session["SessionUsuario"] as Usuario;
                    /*List<Usuario> lst_usuario = new List<Usuario>();
                    lst_usuario = Session["VendedorAsociado"] as List<Usuario>;

                    user = "'" + usuario.codigovendedor + "'";
                    foreach (var usr in lst_usuario)
                    {
                        user += ",'" + usr.codigovendedor + "'";
                    }*/
                /*}
                /*else if (tipo_usuario == "2")
                {
                    user = "ALL";
                }*/
                /* else
                 {
                     return Json("0", JsonRequestBehavior.AllowGet);
                 }*/

                bool accesoConsulta = security_manager.validaAccesoUserData(codVendedor);

                if(accesoConsulta == false)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }

                ModelCliente model_cliente = new ModelCliente();
                List<Cliente> lst_cliente = new List<Cliente>();
                if (security_manager.validaSesion() == true)
                {
                    lst_cliente = model_cliente.listaClienteBusqueda("vendedor", codVendedor, "");
                }
                return Json(lst_cliente, JsonRequestBehavior.AllowGet);

            }
            catch (WebException ex)
            {
                //util.Escribir_Log(ex.ToString());
                return Json("0", JsonRequestBehavior.AllowGet);
            }
            //}
            //return Json("-1", JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult getClienteHistorialVentaAdmin(string tipoBusqueda, string Razon_Social, string Ruc)
        {
            ModelCliente model_cliente = new ModelCliente();
            Respuesta_Json respuesta_Json = new Respuesta_Json();
            DataTable dt = new DataTable();
            if (security_manager.validaSesion() == true)
            {
                string usr = security_manager.codUserConsulta();
                dt = model_cliente.getClienteHistorialVentaAdmin(tipoBusqueda, Razon_Social, Ruc, usr);

            }

            return Json(respuesta_Json.DatTableToDictionary(dt), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getClienteHistorialDocumentos(string coa)
        {
            ModelCliente model_cliente = new ModelCliente();
            Respuesta_Json respuesta_Json = new Respuesta_Json();
            DataTable dt = new DataTable();
            if (security_manager.validaSesion() == true)
            {
                string usr = security_manager.codUserConsulta();
                dt = model_cliente.getClienteHistorialDocumentos(coa, usr);

            }

            return Json(respuesta_Json.DatTableToDictionary(dt), JsonRequestBehavior.AllowGet);
        }


        public ActionResult ClienteLst()
        {

            if (security_manager.validaSesion() == false)
            {
                return RedirectToAction("Index", "Login");
            }

            if (!security_manager.validaAccesoPagina("Cliente/ClienteLst", "R"))
            {
                return RedirectToAction("Index", "Home");
            }

            Usuario usuario = new Usuario();
            ViewData["Usuario"] = Session["SessionUsuario"] as Usuario;
            ViewData["VendedorAsociado"] = Session["VendedorAsociado"] as List<Usuario>;

            return View();
        }

        public ActionResult ClienteCrear(string id)
        {

            if (security_manager.validaSesion() == false)
            {
                return RedirectToAction("Index", "Login");
            }

            if (!security_manager.validaAccesoPagina("Cliente/ClienteCrear", "E"))
            {
                return RedirectToAction("Index", "Home");
            }

            string tipo_usuario = Session["TipoUsuario"].ToString();
            ModelCliente modelCliente = new ModelCliente();
            ModelGeneral m_gelera = new ModelGeneral();
            ModelPagoTipo m_tipoPago = new ModelPagoTipo();
            Usuario eUsusario = new Usuario();
            eUsusario = Session["SessionUsuario"] as Usuario;

            string codVendedor = "";

            if (tipo_usuario == "1")
            {
                codVendedor = eUsusario.codigovendedor;
                ViewData["TipoUsuario"] = "1";
                ViewData["Usuario"] = eUsusario;
                ViewData["VendedorAsociado"] = Session["VendedorAsociado"] as List<Usuario>;
            }
            else
            {
                ViewData["TipoUsuario"] = "2";
            }

            ViewData["PagoTipo"] = m_tipoPago.listaTipoPago("");
            ViewData["Ubigeo"] = m_gelera.listaUbigeo("");
            ViewData["ClienteCategoria"] = modelCliente.lstClienteCategoria();


            if (!String.IsNullOrEmpty(id) && id != "0")
            {
                ViewBag.id = id;
                List<Cliente> lstClient = new List<Cliente>();
                lstClient = modelCliente.listaClienteBusqueda("vendedor", codVendedor, id );
                if (lstClient.Count > 0)
                {
                    ViewData["Cliente"] = lstClient;
                    ViewData["Contactos"] = modelCliente.listaClienteContacto(id, "");
                    ViewData["Direcciones"] = modelCliente.listaClienteDireccion(id, "");
                }
                else
                {
                    return RedirectToAction("ClienteCrear","Cliente");
                }
            }
            else
            {
                ViewBag.id = "0";
            }

            return View();
        }








        // GET: Cliente/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // POST: Cliente/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Cliente/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Cliente/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Cliente/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Cliente/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
