
/*using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security;
using System.Web;
using System.Web.Configuration;
using System.Web.Management;
using System.Web.Mvc;
using System.Xml.Linq;
using WebAppMontGroup.Entity;
using WebAppMontGroup.Models;
using static Mysqlx.Expect.Open.Types.Condition.Types;
using SecurityManager = WebAppMontGroup.Models.SecurityManager;
using System.Security.Cryptography;
using System.Threading.Tasks;*/



using Newtonsoft.Json;
using System;
using System.Web;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Mvc;
using WebAppMontGroup.Entity;
using WebAppMontGroup.Models;
using SecurityManager = WebAppMontGroup.Models.SecurityManager;
using MySqlX.XDevAPI;
using iText.Commons.Bouncycastle.Asn1.X509;
using System.Xml;
using Microsoft.SharePoint.Client;
using iText.Layout.Splitting;


namespace WebAppMontGroup.Controllers
{
    public class PedidoController : Controller
    {
        SecurityManager security_manager = new SecurityManager();
        ModelPedido model = new ModelPedido();
        ModelProducto model_producto = new ModelProducto();
        ModelPagoTipo m_tipoPago = new ModelPagoTipo();
        ModelGeneral m_gelera = new ModelGeneral();
        // GET: Pedido
        //private List<Producto> lst_produtoAlmacenVent = new List<Producto>();

        Util util = new Util();

        public ActionResult PedidoList()
        {
            if (security_manager.validaSesion() == false)
            {
                return RedirectToAction("Index", "Login");
            }


            if (!security_manager.validaAccesoPagina("Pedido/PedidoList", "R"))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["Usuario"] = Session["SessionUsuario"] as Usuario;
            ViewData["VendedorAsociado"] = Session["VendedorAsociado"] as List<Usuario>;

            return View();
        }

        public JsonResult lstPedidos95(string anio, string mes, string user, string estado)
        {

            if (security_manager.validaAccesoPagina("Pedido/PedidoList", "R"))
            {
                try
                {
                    string tipo_usuario = Session["TipoUsuario"].ToString();
                    if (tipo_usuario == "1")
                    {
                        Usuario usuario = new Usuario();
                        usuario = Session["SessionUsuario"] as Usuario;
                        List<Usuario> lst_usuario = new List<Usuario>();
                        lst_usuario = Session["VendedorAsociado"] as List<Usuario>;

                        user = "'" + usuario.codigovendedor + "'";
                        foreach (var usr in lst_usuario)
                        {
                            user += ",'" + usr.codigovendedor + "'";
                        }
                    }
                    else if (tipo_usuario == "2")
                    {
                        user = "ALL";
                    }
                    else
                    {
                        return Json("0", JsonRequestBehavior.AllowGet);
                    }

                    Stream respuestaDeEnvio;
                    string texto_respuesta = "";

                    string urlEndPoint = WebConfigurationManager.AppSettings["EndPointMG"];
                    string pasEndPoint = WebConfigurationManager.AppSettings["PassMG"];
                    urlEndPoint = urlEndPoint + "lstPedido?access_pass=" + pasEndPoint + "&anio=" + anio + "&mes=" + mes + "&usuario=" + user + "&estado=" + estado;

                    using (WebClient webClient = new WebClient())
                    {
                        webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json; charset=utf-8");
                        respuestaDeEnvio = webClient.OpenRead(urlEndPoint);
                        StreamReader string_reader = new StreamReader(respuestaDeEnvio);
                        texto_respuesta = string_reader.ReadToEnd();
                        var myUserList = JsonConvert.DeserializeObject<List<Pedido95>>(texto_respuesta);
                        return Json(myUserList, JsonRequestBehavior.AllowGet);

                    }
                }
                catch (WebException ex)
                {
                    util.Escribir_Log(ex.ToString());
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("-1", JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult listaPedidos(int anio, int mes, string CodVendedor, string estado)
        {
            ModelPedido model_pedido = new ModelPedido();

            // Verificar si la sesión es válida
            if (!security_manager.validaSesion())
            {
                return Json(new { error = "Sesión no válida" }, JsonRequestBehavior.AllowGet);
            }

            bool accesoConsulta = security_manager.validaAccesoUserData(CodVendedor);

            if (accesoConsulta == true)
            {
                var pedidos = model_pedido.listaPedido(anio, mes, CodVendedor, estado);

                if (pedidos != null && pedidos.Any())
                {
                    return Json(new
                    {
                        idPedido = pedidos.Select(p => p.IdPedido),
                        CodigoPedido = pedidos.Select(p => p.CodigoPedido),
                        estado = pedidos.Select(p => p.Estado),
                        fechaRegistro = pedidos.Select(p => p.FechaRegistro.ToString("yyyy-MM-dd HH:mm:ss")),
                        fechaEntrega = pedidos.Select(p => p.FechaEntrega.ToString("yyyy-MM-dd HH:mm:ss")),
                        coa = pedidos.Select(p => p.Coa),
                        cliente = pedidos.Select(p => p.Cliente),
                        total = pedidos.Select(p => p.Total),
                        TipoDocumentoFiscal = pedidos.Select(p => p.TipoDocumentoFiscal), // Se agregó
                        CodigoTipoPago = pedidos.Select(p => p.CodigoTipoPago), // Se agregó
                        NombreTipoPago = pedidos.Select(p => p.NombreTipoPago), // Se agregó
                        vendedor = pedidos.Select(p => p.Vendedor) // Se agregó
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { error = "No se encontraron pedidos" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { error = "No se encontraron pedidos" }, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult PedidoCrear(string id)
        {
            if (security_manager.validaSesion() == false)
            {
                return RedirectToAction("Index", "Login");
            }

            if (!security_manager.validaAccesoPagina("Pedido/PedidoCrear", "E"))
            {
                return RedirectToAction("Index", "Home");
            }

            var usuario = Session["SessionUsuario"] as Usuario;
            var lst_usuario = Session["VendedorAsociado"] as List<Usuario>;
            ViewData["Usuario"] = usuario;
            ViewData["VendedorAsociado"] = lst_usuario;   
            ViewData["PagoTipo"] = m_tipoPago.listaTipoPago("");            
            ViewData["Ubigeo"] = m_gelera.listaUbigeo("");
            ViewData["Agencia"] = m_gelera.listaAgencia("");

            
            Session["lst_produtoAlmacenVent"] = model_producto.listaProductoBusquedaAlamcen(usuario.codigoalmacen, DateTime.Now.ToString("yyMM"));

            if (!String.IsNullOrEmpty(id) && id != "0")
            {

                ViewBag.id = id;       
                Pedido ePedido = new Pedido();
                ePedido = model.PedidoPorId(Convert.ToInt32(id));
                bool accesoConsulta = security_manager.validaAccesoUserData(ePedido.CodVendedor);
                
                if (accesoConsulta == true)
                {
                    ViewData["PedidoCab"] = ePedido;
                }
                else
                {
                    return RedirectToAction("PedidoCrear", "Pedido");
                }           
            }
            else
            {
                ViewBag.id = "0";
            }

            return View();
        }

        public ActionResult PedidoVer(int? id)
        {
            if (security_manager.validaSesion() == false)
            {
                return RedirectToAction("Index", "Login");
            }

            if (!security_manager.validaAccesoPagina("Pedido/PedidoVer", "R"))
            {
                return RedirectToAction("Index", "Home");
            }

            ModelPedido m_pedido = new ModelPedido();
            Pedido e_pedido = new Pedido();
            e_pedido = m_pedido.PedidoPorId(id.Value);

            bool accesoConsulta = security_manager.validaAccesoUserData(e_pedido.CodVendedor);

            if (accesoConsulta == false)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["PedidoPorId"] = e_pedido;
            ViewData["PedidoSeguimiento"] = model.lista_Pedido_log(id.Value, "");
            return View();
        }

        [HttpPost]
        public JsonResult insert_Pedido(Pedido pedido)
        {
            ModelPedido m_pedido = new ModelPedido();
            General general = new General();

            if (!security_manager.validaSesion())
                return Json(new { error = "Sesión no válida" });

            int res_cabecera = 0;
            int res_guia = 0;
            var usuario = Session["SessionUsuario"] as Usuario;
            if (usuario == null)
                return Json(new { error = "Usuario no autenticado" });

            pedido.UsuarioActualizacion = usuario.usuario;
            string mensaje_detalle = "";

            try
            {
                if (pedido.IdPedido == 0)
                {
                    // Insertar cabecera del pedido
                    res_cabecera = m_pedido.insert_Pedido_Cabecera(pedido, "CREATE");
                    if (res_cabecera == 0)
                        return Json(new { error = "Error al registrar la cabecera del pedido" });

                    // Insertar guía del pedido
                    pedido.pedido_guia.IdPedido = res_cabecera;
                    pedido.pedido_guia.UsuarioActualizacion = usuario.usuario;
                    res_guia = m_pedido.insert_Pedido_Guia(pedido.pedido_guia, "CREATE");

                    // Insertar detalles del pedido
                    foreach (var item in pedido.pedido_detalle)
                    {
                        item.IdPedido = res_cabecera;
                        item.UsuarioActualiza = usuario.usuario;
                        int resultado = m_pedido.insert_Pedido_Detalle(item, "CREATE");
                        if (resultado < 1)
                            mensaje_detalle += $"\nError al registrar el producto: {item.ArticuloDesc} (Resultado: {resultado})";
                    }
                    m_pedido.actualizar_Pedido_datos(res_cabecera);
                    // Validación de crédito
                    validacionCredito(pedido.Coa, res_cabecera);
                }
                else
                {
                    // Actualizar cabecera del pedido
                    res_cabecera = m_pedido.insert_Pedido_Cabecera(pedido, "UPDATE");
                    if (res_cabecera == 0)
                        return Json(new { error = "Error al actualizar la cabecera del pedido" });

                    // Actualizar guía del pedido
                    pedido.pedido_guia.IdPedido = pedido.IdPedido;
                    pedido.pedido_guia.UsuarioActualizacion = usuario.usuario;
                    res_guia = m_pedido.insert_Pedido_Guia(pedido.pedido_guia, "UPDATE");

                    // Obtener detalles actuales y gestionar eliminaciones
                    var detallesActuales = m_pedido.listaPedidoDetalle(pedido.IdPedido, "RE");
                    var itemsEliminar = detallesActuales.Where(det => !pedido.pedido_detalle.Any(nuevo => nuevo.IdPedidoDetalle == det.IdPedidoDetalle)).ToList();
                    foreach (var item in itemsEliminar)
                    {
                        m_pedido.insert_Pedido_Detalle(item, "DELETE");
                    }

                    // Insertar o actualizar los detalles
                    foreach (var item in pedido.pedido_detalle)
                    {
                        item.IdPedido = pedido.IdPedido;
                        item.UsuarioActualiza = usuario.usuario;
                        string accion = item.IdPedidoDetalle == 0 ? "CREATE" : "UPDATE";
                        int resultado = m_pedido.insert_Pedido_Detalle(item, accion);
                        if (resultado < 1)
                            mensaje_detalle += $"\nError al registrar el producto: {item.ArticuloDesc} (Resultado: {resultado})";
                    }

                    // Validación de crédito
                    m_pedido.actualizar_Pedido_datos(pedido.IdPedido);
                    validacionCredito(pedido.Coa, pedido.IdPedido);
                }
            }
            catch (Exception ex)
            {
                return Json(new { error = $"Error en la transacción: {ex.Message}" });
            }

            general.valor_1 = res_cabecera.ToString();
            general.valor_2 = res_guia.ToString();
            general.valor_3 = mensaje_detalle;
            general.valor_4 = pedido.IdPedido == 0 ? "insertado" : "actualizado";
            return Json(general);
        }







        /* Hecho por sebastian */
        [HttpGet]
        public JsonResult listaProductoBusqueda(string tipo)
        {
            ModelProducto model_producto = new ModelProducto();
            List<Producto> lst_produto = new List<Producto>();
            List<Producto> lst_produtoAlmacenVent = new List<Producto>();
            string aniomes = DateTime.Now.ToString("yyMM");//"2406"; // Fecha fija que se pasará al modelo

            if (security_manager.validaSesion() == true)
            {
                // Diferenciar si el tipo es 03
                if (tipo == "03")
                {
                    lst_produto = model_producto.listaProductoBusqueda("TODOS", ""); // Método específico para tipo 03
                }
                else
                {
                    lst_produtoAlmacenVent = model_producto.listaProductoBusquedaAlamcen(tipo, aniomes);

                    if (lst_produtoAlmacenVent != null)
                    {
                        lst_produto = lst_produtoAlmacenVent;
                    }
                }
            }

            var result = lst_produto.Select(p => new
            {
                codigo = p.codigo,
                dsc = p.dsc,
                responsable = tipo == "03" ? p.responsable : null, // Solo incluir si el tipo es 03
                dscCorto = tipo == "03" ? p.dscCorto : null, // Solo incluir si el tipo es 03
                lote = tipo != "03" ? p.listaStocks.FirstOrDefault()?.Lote : null, // No incluir si es tipo 03
                stock = tipo != "03" ? p.listaStocks.Sum(s => Convert.ToDecimal(s.Cantidad)) : (decimal?)null // No incluir si es tipo 03
            }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /* Hecho por Sebastian */
        [HttpGet]
        public JsonResult listaPreciosFijo(string codigo, string coa)
        {
            ModelProducto model_producto = new ModelProducto();
            Producto producto = null;

            if (security_manager.validaSesion() == true)
            {
                // Llama al modelo para obtener el producto con su precio y coa
                producto = model_producto.obtenerPrecioPorCodigoYCoa(codigo, coa);
            }

            // Retorna el precio o un mensaje si el producto no se encuentra
            if (producto != null)
            {
                return Json(new { precio = producto.precio }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { mensaje = "Producto no encontrado" }, JsonRequestBehavior.AllowGet);
            }
        }

        //HECHO POR SEBASTIAN
        [HttpGet]
        public JsonResult listaPrecios(string codigo, int cant, string tipo)
        {
            // Verificar que los parámetros sean válidos
            if (string.IsNullOrEmpty(codigo) || cant <= 0)
            {
                return Json(new { error = "Código o cantidad no válidos" }, JsonRequestBehavior.AllowGet);
            }

            ModelProducto model_producto = new ModelProducto();

            // Verificar si la sesión es válida
            if (!security_manager.validaSesion())
            {
                return Json(new { error = "Sesión no válida" }, JsonRequestBehavior.AllowGet);
            }

            // Obtener el precio del producto por código y cantidad
            var precioProducto = model_producto.obtenerPrecioPorCodigoYTipoCliente(codigo, cant, tipo);

            // Verificar si el precio fue encontrado
            if (precioProducto != null)
            {
                return Json(new { precio = precioProducto.precio }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { error = "No se encontró un precio válido" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult direccionUltimoPedido(string coa)
        {
            ModelGeneral model_general = new ModelGeneral();

            // Verificar si la sesión es válida
            if (!security_manager.validaSesion())
            {
                return Json(new { error = "Sesión no válida" }, JsonRequestBehavior.AllowGet);
            }

            var direcciones = model_general.direccionUltimoPedido(coa);

            if (direcciones != null)
            {
                return Json(new
                {
                    direccion = direcciones.valor_1,
                    ubigeo = direcciones.valor_2,
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { error = "No se encontró una direccion válida" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult obtenerLoteStockAlmacen(string aniomes, string almacen, string articulo)
        {
            string user = System.Configuration.ConfigurationManager.AppSettings["uWebServ"];
            string pass = System.Configuration.ConfigurationManager.AppSettings["pWebServ"];

            ProductoLote productoLote = new ProductoLote();
            /*if (security_manager.validaSesion() == true)
            {
                WebServiceEasy.Service_EasySoapClient Service_Easy = new WebServiceEasy.Service_EasySoapClient();
                DataTable dt = new DataTable();
                dt = Service_Easy.sotckAlmacen(user, pass, aniomes, almacen,"");

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr[""].ToString().Trim() == articulo)
                    {

                    }
                }

            }*/
            return Json(productoLote, JsonRequestBehavior.AllowGet);
        }

        /* Hecho por Sebastián */
        [HttpGet]
        public JsonResult validacionCredito(string coa, int idpedido)
        {
            ModelPedido m_pedido = new ModelPedido();

            // Valor por defecto en caso de error
            var respuesta = new { Aprobado = "NO", Mensaje = "No se obtuvo respuesta del servicio." };

            if (security_manager.validaSesion())
            {
                PedidoTotales totales = m_pedido.ObtenerTotalesPedido(coa, idpedido); // Obtiene los totales desde la BD

                if (totales != null)
                {
                    var cliente = m_pedido.ValidarCredito(coa, (double)totales.SumaTotal, (double)totales.UltimoTotal); // Conversión de decimal a double

                    if (cliente != null)
                    {
                        respuesta = new
                        {
                            Aprobado = cliente.Aprobado ?? "NO",
                            Mensaje = !string.IsNullOrEmpty(cliente.Mensaje) ? cliente.Mensaje : "Sin mensaje"
                        };
                    }
                    // Llamar a la función del modelo en lugar de escribir el código SQL aquí
                    m_pedido.ActualizarEstadoPedidoCredito(idpedido, coa, respuesta.Aprobado, respuesta.Mensaje);
                }
            }

            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult guardarArchivo(HttpPostedFileBase archivoOC)
        {
            if (archivoOC != null && archivoOC.ContentLength > 0)
            {
                // Llamar al método para guardar el archivo
                string rutaArchivo = GuardarArchivo(archivoOC);

                return Json(new { success = true, rutaArchivo = rutaArchivo });
            }

            return Json(new { success = false });
        }

        private string GuardarArchivo(HttpPostedFileBase archivoOC)
        {
            // Obtener la carpeta desde Web.config
            string rutaCarpetaRelativa = System.Configuration.ConfigurationManager.AppSettings["CarpetaOC"];

            // Convertir la ruta relativa en absoluta dentro de la aplicación
            string rutaCarpetaAbsoluta = System.Web.HttpContext.Current.Server.MapPath("~/" + rutaCarpetaRelativa);

            // Si la carpeta no existe, crearla
            if (!Directory.Exists(rutaCarpetaAbsoluta))
            {
                Directory.CreateDirectory(rutaCarpetaAbsoluta);
            }

            // Generar nombre aleatorio para el archivo
            string nombreArchivoAleatorio = $"{Guid.NewGuid()}_{Path.GetFileName(archivoOC.FileName)}";

            // Construir la ruta completa
            string rutaCompleta = Path.Combine(rutaCarpetaAbsoluta, nombreArchivoAleatorio);

            // Guardar el archivo
            archivoOC.SaveAs(rutaCompleta);

            // Devolver la ruta relativa (útil para acceder desde la web)
            return $"{rutaCarpetaRelativa}{nombreArchivoAleatorio}";
        }

        [HttpGet]
        public JsonResult listaPromociones(string codigo, string coa)
        {
            ModelPromocion model_promocion = new ModelPromocion();

            // Verificar si la sesión es válida
            if (!security_manager.validaSesion())
            {
                return Json(new { error = "Sesión no válida" }, JsonRequestBehavior.AllowGet);
            }

            var promociones = model_promocion.listaPromocion(codigo, coa);

            if (promociones != null && promociones.Any())
            {
                return Json(new
                {
                    idpromo = promociones.Select(p => p.idPromocion),
                    tipoPromocion = promociones.Select(p => p.tipoPromocion), // Solo mandamos los tipos de promoción
                    codigoProducto = promociones.Select(p => p.codigoProducto),
                    bonificacion = promociones.Select(p => p.bonificacion),
                    cantidad_desde = promociones.Select(p => p.cantidad_desde),
                    cantidad_hasta = promociones.Select(p => p.cantidad_hasta)
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { error = "No se encontró una promoción válida" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        /*public JsonResult listaPedidos(int anio, int mes, string estado)
        {
            ModelPedido model_pedido = new ModelPedido();

            // Verificar si la sesión es válida
            if (!security_manager.validaSesion())
            {
                return Json(new { error = "Sesión no válida" }, JsonRequestBehavior.AllowGet);
            }

            var pedidos = model_pedido.listaPedido(anio, mes, estado);

            if (pedidos != null && pedidos.Any())
            {
                return Json(new
                {
                    idPedido = pedidos.Select(p => p.IdPedido),
                    fechaRegistro = pedidos.Select(p => p.FechaRegistro.ToString("yyyy-MM-dd HH:mm:ss")),
                    fechaEntrega = pedidos.Select(p => p.FechaEntrega.ToString("yyyy-MM-dd HH:mm:ss")),
                    estado = pedidos.Select(p => p.Estado),
                    coa = pedidos.Select(p => p.Coa),
                    cliente = pedidos.Select(p => p.Cliente),
                    total = pedidos.Select(p => p.Total)
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { error = "No se encontraron pedidos" }, JsonRequestBehavior.AllowGet);
            }
        }
        */









        public ActionResult CotizacionCrear(string id)
        {
            if (security_manager.validaSesion() == false)
            {
                return RedirectToAction("Index", "Login");
            }

            if (!security_manager.validaAccesoPagina("Pedido/CotizacionCrear", "E"))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["Usuario"] = Session["SessionUsuario"] as Usuario;
            ViewData["VendedorAsociado"] = Session["VendedorAsociado"] as List<Usuario>;
            ViewData["dtProducctos"] = model_producto.listaProductoDetalle("0");

            if (!String.IsNullOrEmpty(id) && id != "0")
            {
                ViewBag.id = id;
                ModelCotizacion model = new ModelCotizacion();
                List<CotizacionCab> lstCotizacion = new List<CotizacionCab>();
                lstCotizacion = model.GetCotizacionCabList("SELECT", Convert.ToInt32(id), "", 0);
                bool accesoConsulta = security_manager.validaAccesoUserData(lstCotizacion[0].CodVendedor);
                if (lstCotizacion.Count > 0 && accesoConsulta == true)
                {
                    ViewData["CotizacionCab"] = lstCotizacion;
                    ViewData["CotizacionDet"] = model.SelectCotizacionDetCotizacion(Convert.ToInt32(id));
                }
                else
                {
                    return RedirectToAction("CotizacionCrear", "Pedido");
                }
            }
            else
            {
                ViewBag.id = "0";
            }
            //DescargarCotizacion();
            return View();
        }

        public ActionResult CotizacionList()
        {

            if (security_manager.validaSesion() == false)
            {
                return RedirectToAction("Index", "Login");
            }

            if (!security_manager.validaAccesoPagina("Pedido/CotizacionCrear", "R"))
            {
                return RedirectToAction("Index", "Home");
            }

            Usuario usuario = new Usuario();
            ViewData["Usuario"] = Session["SessionUsuario"] as Usuario;
            //System.Web.HttpContext.Current.Session["VendedorAsociado"] = lista_vendedor_asociado;
            ViewData["VendedorAsociado"] = Session["VendedorAsociado"] as List<Usuario>;
            //ModelCotizacion model = new ModelCotizacion();
            //CotizacionCab cotizacionCab = model.GetCotizacionCabList(1, usuario.usuario);
            //DescargarCotizacion();
            return View();
        }

        public JsonResult lstCotizacion(int anio, int mes, string codVendedor)
        {

            try
            {

                bool accesoConsulta = security_manager.validaAccesoUserData(codVendedor);

                if(accesoConsulta == false)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }

                /*string codVendedor = "";
                Usuario usuario = new Usuario();
                string tipo_usuario = Session["TipoUsuario"].ToString();
                if (tipo_usuario == "1")
                {
                    usuario = Session["SessionUsuario"] as Usuario;
                    codVendedor = usuario.codigovendedor;
                }
                else if (tipo_usuario == "2")
                {
                    codVendedor = "";
                }
                else
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }*/

                ModelCotizacion model = new ModelCotizacion();
                List<CotizacionCab> cotizacionCabLst = model.GetCotizacionCabList("SELECT_AAMM", anio, codVendedor, mes);
                return Json(cotizacionCabLst, JsonRequestBehavior.AllowGet);

            }
            catch (WebException ex)
            {
                util.Escribir_Log(ex.ToString());
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult InsertCotizacion(CotizacionCab cotizacionCab, List<CotizacionDetalle> listaProductos)
        {
            int result = 0;
            int resultDet = 0;
            // Logging inicial para saber si llega correctamente la data
            //Console.WriteLine("InsertCotizacion: Data received from the view");
            //Console.WriteLine("Cabecera - CodCotizacion: " + cotizacionCab.CodCotizacion);
            //Console.WriteLine("Cabecera - Cliente: " + cotizacionCab.Cliente);
            //Console.WriteLine("Number of Details: " + listaProductos.Count);

            ModelCotizacion model = new ModelCotizacion();

            try
            {
                // Primero, insertamos la cabecera
                Usuario usuario = new Usuario();
                usuario = Session["SessionUsuario"] as Usuario;
                cotizacionCab.UsuarioCreador = usuario.usuario;
                if (cotizacionCab.Id == 0)
                {
                    result = model.InsertCotizacionCab(cotizacionCab, "INSERT");
                    if (result < 0)
                    {
                        //Console.WriteLine("Failed to insert Cotizacion Cabecera.");
                        return Json(new { success = false, message = "Failed to insert Cotizacion Cabecera." }, JsonRequestBehavior.AllowGet);
                    }

                    // Insertamos los detalles
                    foreach (var cotizacionDetalle in listaProductos)
                    {
                        cotizacionDetalle.idCotizacion = result; // Asegurar que el código es el mismo
                        cotizacionDetalle.UsuarioCreador = cotizacionCab.UsuarioCreador;
                        resultDet = model.InsertCotizacionDetalle(cotizacionDetalle, "INSERT");
                        if (resultDet < 0)
                        {
                            //Console.WriteLine("Failed to insert Cotizacion Detalle for product: " + cotizacionDetalle.CodigoProducto);
                            return Json(new { success = false, message = "Failed to insert Cotizacion Detalle." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    result = model.InsertCotizacionCab(cotizacionCab, "UPDATE");

                    if (result < 0)
                    {
                        //Console.WriteLine("Failed to insert Cotizacion Cabecera.");
                        return Json(new { success = false, message = "Failed to update Cotizacion Cabecera." }, JsonRequestBehavior.AllowGet);
                    }

                    List<CotizacionDetalle> lstCotDetalle = new List<CotizacionDetalle>();
                    lstCotDetalle = model.SelectCotizacionDetCotizacion(cotizacionCab.Id);

                    var lista2Filtrada = listaProductos.Where(p => p.Id != 0).ToList();
                    var itemsSinCoincidencia = lstCotDetalle
                                                .Where(p1 => !lista2Filtrada.Any(p2 => p2.Id == p1.Id))
                                                .ToList();

                    foreach (var item1 in itemsSinCoincidencia)
                    {
                        CotizacionDetalle dtProductos = new CotizacionDetalle();
                        dtProductos.Id = item1.Id;
                        dtProductos.idCotizacion = item1.idCotizacion;
                        dtProductos.CodigoProducto = item1.CodigoProducto;
                        dtProductos.NombreProducto = item1.NombreProducto;
                        dtProductos.Det_Producto = item1.Det_Producto;
                        dtProductos.Proveedor = item1.Proveedor;
                        dtProductos.Presentacion = item1.Presentacion;
                        dtProductos.FechaVencimiento = item1.FechaVencimiento;
                        dtProductos.Promocion = item1.Promocion;
                        dtProductos.Cantidad = item1.Cantidad;
                        dtProductos.Precio = item1.Precio;
                        dtProductos.Total = item1.Total;
                        dtProductos.UsuarioCreador = item1.UsuarioCreador;
                        resultDet = model.InsertCotizacionDetalle(dtProductos, "DELETE");
                    }


                    foreach (var cotizacionDetalle in listaProductos)
                    {
                        cotizacionDetalle.idCotizacion = result; // Asegurar que el código es el mismo
                        cotizacionDetalle.UsuarioCreador = cotizacionCab.UsuarioCreador;

                        if (cotizacionDetalle.Id == 0)
                        {
                            resultDet = model.InsertCotizacionDetalle(cotizacionDetalle, "INSERT");
                        }
                        else
                        {
                            resultDet = model.InsertCotizacionDetalle(cotizacionDetalle, "UPDATE");
                        }

                        if (resultDet < 0)
                        {
                            //Console.WriteLine("Failed to insert Cotizacion Detalle for product: " + cotizacionDetalle.CodigoProducto);
                            return Json(new { success = false, message = "Failed to insert Cotizacion Detalle." }, JsonRequestBehavior.AllowGet);
                        }
                    }

                }
                //Console.WriteLine("InsertCotizacion: Successfully inserted Cotizacion.");
                return Json(new { success = true, message = "Cotizacion inserted successfully.", id = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error in InsertCotizacion: " + ex.Message);
                return Json(new { success = false, message = "Exception occurred: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CotizacionDescargar(string id)
        {

            ModelCotizacion model = new ModelCotizacion();
            ModelUsuario musuario = new ModelUsuario();

            string tipo_usuario = Session["TipoUsuario"].ToString();
            var VarUsuario = Session["SessionUsuario"] as Usuario;
            string codVendedor = VarUsuario.codigovendedor;
            if (tipo_usuario != "1")
            {
                codVendedor = "";
            }

            List<CotizacionCab> lstCotizacion_cab = model.GetCotizacionCabList("SELECT", Convert.ToInt32(id), codVendedor, 0);// usuario.usuario);

            List<CotizacionDetalle> Cotizacion_det = model.SelectCotizacionDetCotizacion(Convert.ToInt32(id));
            Usuario usuarioCotizacion = musuario.loginRV("XCODVEN", lstCotizacion_cab[0].CodVendedor, "");

            int cantidadProducto = Cotizacion_det.Count;

            string readText_1 = System.Web.Hosting.HostingEnvironment.MapPath("~/assets/PlatillasHtml/pdf_cotizacion.html");
            string readText_2 = System.Web.Hosting.HostingEnvironment.MapPath("~/assets/PlatillasHtml/cotizaCabecera.html");
            string readText_3 = System.Web.Hosting.HostingEnvironment.MapPath("~/assets/PlatillasHtml/cotizaProductos.html");
            string readText_4 = System.Web.Hosting.HostingEnvironment.MapPath("~/assets/PlatillasHtml/cotizaTotalProductos.html");
            string readText_5 = System.Web.Hosting.HostingEnvironment.MapPath("~/assets/PlatillasHtml/cotizaDatosVendedor.html");
            string readText_6 = System.Web.Hosting.HostingEnvironment.MapPath("~/assets/PlatillasHtml/cotizaCodiciones.html");
            string readText_7 = System.Web.Hosting.HostingEnvironment.MapPath("~/assets/PlatillasHtml/cotizaFormaPago.html");
            string readText_8 = System.Web.Hosting.HostingEnvironment.MapPath("~/assets/PlatillasHtml/cotizafooter.html");
            string readText_9 = System.Web.Hosting.HostingEnvironment.MapPath("~/assets/PlatillasHtml/pdf_imagenes.html");
            string readText_10 = System.Web.Hosting.HostingEnvironment.MapPath("~/assets/PlatillasHtml/cssjs.html");
            string readText_11 = System.Web.Hosting.HostingEnvironment.MapPath("~/assets/PlatillasHtml/imgNoDisponible.html");

            string pdf_plantilla = System.IO.File.ReadAllText(readText_1);
            string pdf_cabecera = System.IO.File.ReadAllText(readText_2);
            string pdf_TabProductos = System.IO.File.ReadAllText(readText_3);
            string pdf_TotProductos = System.IO.File.ReadAllText(readText_4);
            string pdf_dataVendedor = System.IO.File.ReadAllText(readText_5);
            string pdf_condiciones = System.IO.File.ReadAllText(readText_6);
            string pdf_formaPago = System.IO.File.ReadAllText(readText_7);
            string pdf_footer = System.IO.File.ReadAllText(readText_8);
            string pdf_imgProductos = System.IO.File.ReadAllText(readText_9);
            string pdf_cssjs = System.IO.File.ReadAllText(readText_10);
            string imgNoDisponible = System.IO.File.ReadAllText(readText_11);

            int itemsCantidad = 1;
            string lineasProducto1 = "";
            string lineasProducto2 = "";
            bool validaFecha = false;

            List<General> lisCodProducto = new List<General>();
            foreach (var det in Cotizacion_det)
            {
                if (det.FechaVencimiento.ToString("dd/MM/yyyy").Replace("01/01/0001", "") != "")
                {
                    validaFecha = true;
                    break;
                }
            }

            int cantidadFilas = 13;
            if (validaFecha == false)
            {
                pdf_TabProductos = pdf_TabProductos.Replace("<th class='titulos_tabla'>Fecha de Vencimiento</th>", "");
                cantidadFilas = 19;
            }

            string fechaVencimiento = "";

            foreach (var det in Cotizacion_det)
            {
                fechaVencimiento = "";
                if (validaFecha == true)
                {
                    fechaVencimiento = "<td>" + det.FechaVencimiento.ToString("dd/MM/yyyy").Replace("01/01/0001", "") + "</td>";
                }

                if (itemsCantidad < cantidadFilas)
                {
                    lineasProducto1 += "<tr><td style='text-align:center'>" + itemsCantidad.ToString() + "</td>" +
                                          "<td>" + det.Proveedor + "</td>" +
                                          "<td>" + det.NombreProducto + "</td>" +
                                          "<td style='text-align:center'>" + det.Det_Producto + "</td>" +
                                          fechaVencimiento +
                                          "<td style='text-align:right'>" + det.Cantidad.ToString() + "</td>" +
                                          "<td style='text-align:right'>S/" + det.Precio.ToString() + "</td>" +
                                          "<td style='text-align:right'>S/ " + det.Total.ToString() + "</td></tr>";
                }
                else
                {
                    lineasProducto2 += "<tr><td style='text-align:center'>" + itemsCantidad.ToString() + "</td>" +
                                            "<td>" + det.Proveedor + "</td>" +
                                            "<td>" + det.NombreProducto + "</td>" +
                                            "<td style='text-align:center'>" + det.Det_Producto + "</td>" +
                                             fechaVencimiento +
                                            "<td style='text-align:right'>" + det.Cantidad.ToString() + "</td>" +
                                            "<td style='text-align:right'>S/" + det.Precio.ToString() + "</td>" +
                                            "<td style='text-align:right'>S/ " + det.Total.ToString() + "</td></tr>";
                }

                General itemGeneralCod = new General();
                itemGeneralCod.valor_1 = det.CodigoProducto;
                itemGeneralCod.valor_2 = det.NombreProducto;
                lisCodProducto.Add(itemGeneralCod);
                itemsCantidad++;
            }

            int cantidad = lisCodProducto.Count;
            DataSharepoint sharepoint = new DataSharepoint();
            lisCodProducto = sharepoint.imgBase64_Producto(lisCodProducto);

            int veces = 0;
            string imgProductos = "<div class='flex-container'>";
            string imgProductos2 = "<div class='flex-container'>";
            string imgBase64 = "";
            while (veces < cantidad)
            {
                if (lisCodProducto[veces].valor_3 != null)
                {
                    imgBase64 = lisCodProducto[veces].valor_3.ToString();
                }
                else
                {
                    imgBase64 = imgNoDisponible;
                }

                if (veces < 17)
                {
                    imgProductos += "<div class='flex-item'><img src='data:image/png;base64," + imgBase64 + "' /><div style='font-size:9px'>" + lisCodProducto[veces].valor_2.ToString() + "</div></div>";
                }
                else
                {
                    imgProductos2 += "<div class='flex-item'><img src='data:image/png;base64," + imgBase64 + "' /><div style='font-size:9px'>" + lisCodProducto[veces].valor_2.ToString() + "</div></div>";
                }

                veces++;
            }
            imgProductos += "</div>";
            imgProductos2 += "</div>";

            string hojaPdf = "";

            if (validaFecha == true)
            {
                if (cantidadProducto < 5)
                {
                    hojaPdf = "<div class='a4-section'><div class='content'>";
                    hojaPdf += pdf_cabecera;
                    hojaPdf += pdf_TabProductos.Replace("{lineasProducto}", lineasProducto1);
                    hojaPdf += pdf_TotProductos;
                    hojaPdf += "<div class='footer'>";
                    hojaPdf += pdf_dataVendedor;
                    hojaPdf += pdf_condiciones;
                    hojaPdf += pdf_formaPago;
                    hojaPdf += pdf_footer;
                    hojaPdf += "</div></div></div>";

                    pdf_imgProductos = pdf_imgProductos.Replace("{divImagenes}", imgProductos);
                    hojaPdf += "<div class='a4-section'><div class='content'><br><br>";
                    hojaPdf += pdf_imgProductos;
                    hojaPdf += "<div class='footer'>";
                    hojaPdf += pdf_footer;
                    hojaPdf += "</div></div></div>";
                }
                else if (cantidadProducto < 13)
                {
                    hojaPdf = "<div class='a4-section'><div class='content'>";
                    hojaPdf += pdf_cabecera;
                    hojaPdf += pdf_TabProductos.Replace("{lineasProducto}", lineasProducto1);
                    hojaPdf += pdf_TotProductos;
                    hojaPdf += "<div class='footer'>";
                    hojaPdf += pdf_dataVendedor + "<br/><br/>";
                    hojaPdf += pdf_footer;
                    hojaPdf += "</div></div></div>";

                    pdf_imgProductos = pdf_imgProductos.Replace("{divImagenes}", imgProductos);
                    hojaPdf += "<div class='a4-section'><div class='content'><br><br>";
                    hojaPdf += pdf_imgProductos;
                    hojaPdf += "<div class='footer'>";
                    hojaPdf += pdf_condiciones;
                    hojaPdf += pdf_formaPago;
                    hojaPdf += pdf_footer;
                    hojaPdf += "</div></div></div>";
                }
                else
                {
                    hojaPdf = "<div class='a4-section'><div class='content'>";
                    hojaPdf += pdf_cabecera;
                    hojaPdf += pdf_TabProductos.Replace("{lineasProducto}", lineasProducto1);
                    hojaPdf += "<div class='footer'>";
                    hojaPdf += pdf_dataVendedor + "<br/><br/>";
                    hojaPdf += pdf_footer;
                    hojaPdf += "</div></div></div>";

                    hojaPdf += "<div class='a4-section'><div class='content'>";
                    hojaPdf += pdf_TabProductos.Replace("{lineasProducto}", lineasProducto2);
                    hojaPdf += pdf_TotProductos;
                    hojaPdf += "<div class='footer'>";
                    hojaPdf += pdf_dataVendedor;
                    hojaPdf += pdf_condiciones;
                    hojaPdf += pdf_formaPago;
                    hojaPdf += pdf_footer;
                    hojaPdf += "</div></div></div>";


                    string imgDivProducto = "";

                    hojaPdf += "<div class='a4-section'><div class='content'>";
                    imgDivProducto = pdf_imgProductos.Replace("{divImagenes}", imgProductos);
                    hojaPdf += "<div class='a4-section'><div class='content'><br>";
                    hojaPdf += imgDivProducto;
                    hojaPdf += "<div class='footer'>";
                    hojaPdf += pdf_footer;
                    hojaPdf += "</div></div></div>";

                    if (cantidadProducto > 16)
                    {
                        hojaPdf += "<div class='a4-section'><div class='content'>";
                        imgDivProducto = pdf_imgProductos.Replace("{divImagenes}", imgProductos2);
                        hojaPdf += "<div class='a4-section'><div class='content'><br>";
                        hojaPdf += imgDivProducto;
                        hojaPdf += "<div class='footer'>";
                        hojaPdf += pdf_footer;
                        hojaPdf += "</div></div></div>";
                    }

                }
            }
            else
            {
                if (cantidadProducto < 6)
                {
                    hojaPdf = "<div class='a4-section'><div class='content'>";
                    hojaPdf += pdf_cabecera;
                    hojaPdf += pdf_TabProductos.Replace("{lineasProducto}", lineasProducto1);
                    hojaPdf += pdf_TotProductos;
                    hojaPdf += "<div class='footer'>";
                    hojaPdf += pdf_dataVendedor;
                    hojaPdf += pdf_condiciones;
                    hojaPdf += pdf_formaPago;
                    hojaPdf += pdf_footer;
                    hojaPdf += "</div></div></div>";

                    pdf_imgProductos = pdf_imgProductos.Replace("{divImagenes}", imgProductos);
                    hojaPdf += "<div class='a4-section'><div class='content'><br><br>";
                    hojaPdf += pdf_imgProductos;
                    hojaPdf += "<div class='footer'>";
                    hojaPdf += pdf_footer;
                    hojaPdf += "</div></div></div>";
                }
                else if (cantidadProducto < 19)
                {
                    hojaPdf = "<div class='a4-section'><div class='content'>";
                    hojaPdf += pdf_cabecera;
                    hojaPdf += pdf_TabProductos.Replace("{lineasProducto}", lineasProducto1);
                    hojaPdf += pdf_TotProductos;
                    hojaPdf += "<div class='footer'>";
                    hojaPdf += pdf_footer;
                    hojaPdf += "</div></div></div>";

                    hojaPdf += "<div class='a4-section'><div class='content'>";
                    hojaPdf += pdf_dataVendedor;
                    hojaPdf += pdf_condiciones;
                    hojaPdf += pdf_formaPago;
                    hojaPdf += "<div class='footer'>";
                    hojaPdf += pdf_footer;
                    hojaPdf += "</div></div></div>";

                    string imgDivProducto = "";

                    hojaPdf += "<div class='a4-section'><div class='content'>";
                    imgDivProducto = pdf_imgProductos.Replace("{divImagenes}", imgProductos);
                    hojaPdf += "<div class='a4-section'><div class='content'><br>";
                    hojaPdf += imgDivProducto;
                    hojaPdf += "<div class='footer'>";
                    hojaPdf += pdf_footer;
                    hojaPdf += "</div></div></div>";

                    if (cantidadProducto > 16)
                    {
                        hojaPdf += "<div class='a4-section'><div class='content'>";
                        imgDivProducto = pdf_imgProductos.Replace("{divImagenes}", imgProductos2);
                        hojaPdf += "<div class='a4-section'><div class='content'><br>";
                        hojaPdf += imgDivProducto;
                        hojaPdf += "<div class='footer'>";
                        hojaPdf += pdf_footer;
                        hojaPdf += "</div></div></div>";
                    }
                }
                else
                {
                    hojaPdf = "<div class='a4-section'><div class='content'>";
                    hojaPdf += pdf_cabecera;
                    hojaPdf += pdf_TabProductos.Replace("{lineasProducto}", lineasProducto1);
                    hojaPdf += "<div class='footer'>";
                    hojaPdf += pdf_dataVendedor + "<br/><br/>";
                    hojaPdf += pdf_footer;
                    hojaPdf += "</div></div></div>";

                    hojaPdf += "<div class='a4-section'><div class='content'>";
                    hojaPdf += pdf_TabProductos.Replace("{lineasProducto}", lineasProducto2);
                    hojaPdf += pdf_TotProductos;
                    hojaPdf += "<div class='footer'>";
                    hojaPdf += pdf_dataVendedor;
                    hojaPdf += pdf_condiciones;
                    hojaPdf += pdf_formaPago;
                    hojaPdf += pdf_footer;
                    hojaPdf += "</div></div></div>";


                    string imgDivProducto = "";

                    hojaPdf += "<div class='a4-section'><div class='content'>";
                    imgDivProducto = pdf_imgProductos.Replace("{divImagenes}", imgProductos);
                    hojaPdf += "<div class='a4-section'><div class='content'><br>";
                    hojaPdf += imgDivProducto;
                    hojaPdf += "<div class='footer'>";
                    hojaPdf += pdf_footer;
                    hojaPdf += "</div></div></div>";

                    if (cantidadProducto > 16)
                    {
                        hojaPdf += "<div class='a4-section'><div class='content'>";
                        imgDivProducto = pdf_imgProductos.Replace("{divImagenes}", imgProductos2);
                        hojaPdf += "<div class='a4-section'><div class='content'><br>";
                        hojaPdf += imgDivProducto;
                        hojaPdf += "<div class='footer'>";
                        hojaPdf += pdf_footer;
                        hojaPdf += "</div></div></div>";
                    }

                }
            }


            hojaPdf = hojaPdf.Replace("{CodigoCotizacion}", lstCotizacion_cab[0].CodCotizacion);
            hojaPdf = hojaPdf.Replace("{fechaCotizacion}", lstCotizacion_cab[0].FechaRegistro.ToString("yyyy-MM-dd"));
            hojaPdf = hojaPdf.Replace("{NombreCliente}", lstCotizacion_cab[0].Cliente);
            hojaPdf = hojaPdf.Replace("{NombreContacto}", lstCotizacion_cab[0].Contacto.ToString());
            hojaPdf = hojaPdf.Replace("{ImporteTotal}", lstCotizacion_cab[0].Total.ToString("########.00"));
            hojaPdf = hojaPdf.Replace("{nombreVendedor}", usuarioCotizacion.nombres);
            hojaPdf = hojaPdf.Replace("{cargoVendedor}", usuarioCotizacion.cargo);
            hojaPdf = hojaPdf.Replace("{telefonoVendedor}", usuarioCotizacion.telefono);
            hojaPdf = hojaPdf.Replace("{correoVendedor}", usuarioCotizacion.correo);
            hojaPdf = hojaPdf.Replace("{fechaValido}", lstCotizacion_cab[0].FechaRegistro.AddDays(7).ToString("dd/MM/yyyy"));
            hojaPdf = hojaPdf.Replace("{correoVendedor}", usuarioCotizacion.correo);

            pdf_plantilla = pdf_plantilla.Replace("{titulo}", lstCotizacion_cab[0].CodCotizacion);
            pdf_plantilla = pdf_plantilla.Replace("{contenido}", hojaPdf);
            pdf_plantilla = pdf_plantilla.Replace("{cssjs}", pdf_cssjs);


            PdfService _pdfService = new PdfService();
            byte[] pdfBytes = _pdfService.GeneratePdfHtml(pdf_plantilla);
            MemoryStream ms = new MemoryStream(pdfBytes, 0, 0, true, true);
            Response.AddHeader("content-disposition", "attachment;filename=" + lstCotizacion_cab[0].CodCotizacion + ".pdf");
            Response.Buffer = true;
            Response.Clear();
            Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.End();
            return new FileStreamResult(Response.OutputStream, "application/pdf");

        }





    }
}
