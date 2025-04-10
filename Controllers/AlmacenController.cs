using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using WebAppMontGroup.Models;
using WebAppMontGroup.Entity;
using SecurityManager = WebAppMontGroup.Models.SecurityManager;
using WebAppMontGroup.WebServiceEasy;
using System.Data;
using System.Web.Configuration;
using System.Web.DynamicData;
using Microsoft.Ajax.Utilities;
using static System.Net.Mime.MediaTypeNames;
using System.Collections;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace WebAppMontGroup.Controllers
{
    [Route("api/[controller]")]
    public class AlmacenController : Controller
    {

        SecurityManager security_manager = new SecurityManager();
        Usuario user = new Usuario();


        public ActionResult Stock()
        {

            if (security_manager.validaSesion() == false)
            {
                return RedirectToAction("Index", "Login");
            }

            //StockAlmacenPrincipal("2408", "03", "");

            return View();
        }

        public ActionResult Kardex()
        {
            if (security_manager.validaSesion() == false)
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        [HttpGet]
        public JsonResult StockAlmacen(string aniomes, string almacen, string articulo)
        {
            IList<Stock> stockList = new List<Stock>();

            if (security_manager.validaSesion() == true)
            {
                user = (Usuario)Session["SessionUsuario"];
                almacen = user.codigoalmacen;
                if (almacen != "03")
                {
                    DataTable dt = new DataTable();
                    string x_usuario = WebConfigurationManager.AppSettings["uWebServ"];
                    string x_password = WebConfigurationManager.AppSettings["pWebServ"];

                    try
                    {
                        WebServiceEasy.Service_EasySoapClient Service_Easy = new WebServiceEasy.Service_EasySoapClient();
                        dt = Service_Easy.sotckAlmacen(x_usuario, x_password, aniomes, almacen, articulo);

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
                            fields.Producto = item["artic_dsc"].ToString();
                            fields.Lote = item["lote"].ToString();
                            fields.FechaVecimiento = Convert.ToDateTime(item["fch_exp"]).ToString("yyyy-MM-dd");//.ToString("dd/MM/yyyy");//.Substring(0, 10);
                            fields.Cantidad = Convert.ToInt32(item["stock"]).ToString();
                            fields.Fila = fila.ToString();
                            cantidad = cantidad + Convert.ToInt32(item["stock"]);
                            producto = item["artic_dsc"].ToString();
                            stock_items.Add(fields);
                        }

                        stockList.Add(new Stock()
                        {
                            Fila = fila.ToString(),
                            Articulo = art.articulo,
                            Producto = producto,
                            Lote = "",
                            FechaVecimiento = "",
                            Cantidad = cantidad.ToString(),
                            Detalle = "Total",
                            listaStocks = stock_items.ToList()
                        });

                        cantidad = 0;
                        fila++;
                    }
                }
            }
            else
            {
                return Json("-1", JsonRequestBehavior.AllowGet);
            }
            return Json(stockList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult sotckArticulosAlmacen(string aniomes, string almacen)
        {
            DataTable dt = new DataTable();
            if (security_manager.validaSesion() == true)
            {
                user = (Usuario)Session["SessionUsuario"];
                almacen = user.codigoalmacen;
                if (almacen != "03")
                {
                    string x_usuario = WebConfigurationManager.AppSettings["uWebServ"];
                    string x_password = WebConfigurationManager.AppSettings["pWebServ"];

                    try
                    {
                        WebServiceEasy.Service_EasySoapClient Service_Easy = new WebServiceEasy.Service_EasySoapClient();
                        dt = Service_Easy.sotckArticulosAlmacen(x_usuario, x_password, aniomes, almacen);

                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            else
            {
                return Json("-1", JsonRequestBehavior.AllowGet);
            }
            var distinctArticulos = from data in dt.AsEnumerable()
                                    group data by data["articulo"]
                                    into groupDt
                                    select new
                                    {
                                        articulo = groupDt.Key,
                                        artic_dsc = groupDt.Select(r => r.Field<string>("artic_dsc").Trim()).FirstOrDefault()
                                    };

            return Json(distinctArticulos, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult KardexAlmacen(string aniomes, string almacen, string articulo)
        {



            DataTable dt = new DataTable();
            if (security_manager.validaSesion() == true)
            {
                user = (Usuario)Session["SessionUsuario"];
                almacen = user.codigoalmacen;
                if (almacen != "03")
                {
                    string x_usuario = WebConfigurationManager.AppSettings["uWebServ"];
                    string x_password = WebConfigurationManager.AppSettings["pWebServ"];

                    try
                    {
                        WebServiceEasy.Service_EasySoapClient Service_Easy = new WebServiceEasy.Service_EasySoapClient();
                        dt = Service_Easy.kardexArticulosAlmacen(x_usuario, x_password, aniomes, almacen, articulo);

                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            else
            {
                return Json("-1", JsonRequestBehavior.AllowGet);
            }

            var data = dt.Rows.OfType<DataRow>()
                .Select(row => dt.Columns.OfType<DataColumn>()
                    .ToDictionary(col => col.ColumnName, c => row[c]));

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StockPrincipal()
        {
            if (security_manager.validaSesion() == false)
            {
                return RedirectToAction("Index", "Login");
            }
            if (!security_manager.validaAccesoPagina("Almacen/StockPrincipal", "R"))
            {
                return RedirectToAction("Index", "Home");
            }

            ModelProducto modelProducto = new ModelProducto();
            ViewData["Proveedor"] = modelProducto.listaProveedor();

            return View();
        }

        [HttpGet]
        public JsonResult StockAlmacenPrincipal(string aniomes, string idProveedor)
        {
            string res = "";
            if (security_manager.validaSesion() == true)
            {
                if (security_manager.validaAccesoPagina("Almacen/StockPrincipal", "R"))
                {

                    DataTable dtAlmacen0 = new DataTable();
                    DataTable dtProductos = new DataTable();
                    DataTable dtSeparacion = new DataTable();

                    string x_usuario = WebConfigurationManager.AppSettings["uWebServ"];
                    string x_password = WebConfigurationManager.AppSettings["pWebServ"];

                    try
                    {
                        ModelProducto model_producto = new ModelProducto();
                        dtProductos = model_producto.listaProductoDetalle(idProveedor);

                        var distinctProveedor = dtProductos.AsEnumerable()
                             .Select(row => new
                             {
                                 proveedor = row.Field<string>("proveedor"),
                             })
                             .Distinct();


                        WebServiceEasy.Service_EasySoapClient Service_Easy = new WebServiceEasy.Service_EasySoapClient();

                        string lst_productos = "";
                        foreach (var prov in distinctProveedor)
                        {
                            lst_productos = "";
                            DataRow[] result = dtProductos.Select("proveedor = '" + prov.proveedor + "'");
                            foreach (DataRow row in result)
                            {
                                lst_productos += row["codigo"].ToString() + ",";
                            }

                            lst_productos = lst_productos.Remove(lst_productos.Length - 1);
                            DataTable dtAlmacen1 = new DataTable();
                            dtAlmacen1 = Service_Easy.sotckAlmacenGeneral(x_usuario, x_password, aniomes, lst_productos);
                            if (dtAlmacen1.Rows.Count > 0)
                            {
                                dtAlmacen0.Merge(dtAlmacen1);
                            }
                        }



                        dtProductos.Columns.Add("separacion", typeof(decimal));
                        int _fila = 0;


                        ModelProducto mProducto = new ModelProducto();
                        dtSeparacion = mProducto.Producto_separacion_lista(0, "LST_STOCK", "");

                        foreach (DataRow row in dtProductos.Rows)
                        {
                            DataRow[] result = dtSeparacion.Select("codigo = '" + row["codigo"].ToString() + "'");
                            if (result.Length > 0)
                            {
                                dtProductos.Rows[_fila]["separacion"] = result[0]["cantidad"];
                            }
                            else
                            {
                                dtProductos.Rows[_fila]["separacion"] = 0;
                            }
                            _fila++;
                        }



                    }
                    catch (Exception ex)
                    {

                    }

                    var results = from table2 in dtProductos.AsEnumerable()
                                  join table1 in dtAlmacen0.AsEnumerable() on table2["codigo"].ToString().Trim() equals table1["articulo"].ToString().Trim()
                                  //join table3 in dtSeparacion.AsEnumerable() on table2["codigo"].ToString().Trim() equals table3["codigo"].ToString().Trim()
                                  select new
                                  {
                                      articulo = table1["articulo"].ToString().Trim(),
                                      artic_dsc = table1["artic_dsc"].ToString().Trim(),
                                      fch_exp = Convert.ToDateTime(table1["fch_exp"]),
                                      stock = Convert.ToDecimal(table1["stock"]),
                                      artic_dsc_corto = table2["dscCorto"].ToString().Trim(),
                                      presentacion = table2["presentacion"].ToString().Trim(),
                                      cajon = table2["cajon"].ToString(),
                                      responsable = table2["responsable"].ToString().Trim(),
                                      proveedor = table2["proveedor"].ToString().Trim(),
                                      categoria = table2["categoria"].ToString().Trim(),
                                      subcategoria = table2["subcategoria"].ToString().Trim(),
                                      separacion = Convert.ToDecimal(table2["separacion"])
                                  };

                    var results_2 = from tab in results.AsEnumerable()
                                    group tab by tab.articulo
                         into groupDt
                                    select new
                                    {
                                        articulo = groupDt.Key,
                                        stock = groupDt.Sum((r) => Convert.ToInt32(r.stock)),
                                        separacion = groupDt.Select(r => r.separacion).FirstOrDefault(),
                                        artic_dsc = groupDt.Select(r => r.artic_dsc).FirstOrDefault() ?? "",
                                        FechaVencimiento = groupDt.Where(r => r.fch_exp > DateTime.Now.AddMonths(6)).Select(x => x.fch_exp.ToString("yyyy-MM-dd")).FirstOrDefault() is null ? groupDt.Where(r => r.fch_exp < DateTime.Now.AddMonths(6)).Select(x => x.fch_exp.ToString("yyyy-MM-dd")).FirstOrDefault() : groupDt.Where(r => r.fch_exp > DateTime.Now.AddMonths(6)).Select(x => x.fch_exp.ToString("yyyy-MM-dd")).FirstOrDefault(),
                                        artic_dsc_corto = groupDt.Select(r => r.artic_dsc_corto).FirstOrDefault(),
                                        presentacion = groupDt.Select(r => r.presentacion).FirstOrDefault() ?? "",
                                        cajon = groupDt.Select(r => r.cajon).FirstOrDefault() ?? "",
                                        responsable = groupDt.Select(r => r.responsable).FirstOrDefault(),
                                        proveedor = groupDt.Select(r => r.proveedor).FirstOrDefault(),
                                        categoria = groupDt.Select(r => r.categoria).FirstOrDefault(),
                                        subcategoria = groupDt.Select(r => r.subcategoria).FirstOrDefault()
                                    };


                    return Json(results_2, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                res = "-1";
            }
            return Json(res, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Separacion()
        {
            if (security_manager.validaSesion() == false)
            {
                return RedirectToAction("Index", "Login");
            }

            if (!security_manager.validaAccesoPagina("Almacen/Separacion", "R"))
            {
                return RedirectToAction("Index", "Home");
            }

            ModelProducto model_producto = new ModelProducto();
            ViewData["dtProducctos"] = model_producto.listaProductoDetalle("0");
            ViewData["Editar"] = security_manager.validaAccesoPagina("Almacen/Separacion", "E");
            ViewData["Eliminar"] = security_manager.validaAccesoPagina("Almacen/Separacion", "D");
            return View();
        }

        [HttpGet]
        public JsonResult Producto_separacion(int idSeparacion, string codigo, double cantidad, string fechaLimite, string solicitante, string detalle, string opcion)
        {
            string res = "";
            bool acceso = false;
            if (security_manager.validaSesion() == true)
            {
                if (opcion == "DELETE")
                {
                    if (security_manager.validaAccesoPagina("Almacen/Separacion", "D"))
                    {
                        acceso = true;
                    }
                }
                else
                {
                    if (security_manager.validaAccesoPagina("Almacen/Separacion", "E"))
                    {
                        acceso = true;
                    }
                }

                if (acceso == true)
                {
                    if (opcion == "DELETE" || (cantidad > 0 && Convert.ToDateTime(fechaLimite) >= DateTime.Now))
                    {
                        user = (Usuario)Session["SessionUsuario"];
                        ModelProducto modelProducto = new ModelProducto();
                        res = modelProducto.Producto_separacion(idSeparacion, codigo, cantidad, fechaLimite, solicitante, detalle, user.usuario, opcion).ToString();
                    }
                    else
                    {
                        res = "0";
                    }
                }
                else
                {
                    res = "0";
                }


            }
            else
            {
                res = "-1";
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Producto_separacion_lista(string estado)
        {
            if (security_manager.validaSesion() == true)
            {
                if (security_manager.validaAccesoPagina("Almacen/Separacion", "R"))
                {
                    ModelProducto modelProducto = new ModelProducto();
                    Respuesta_Json respuesta_Json = new Respuesta_Json();
                    DataTable dt = new DataTable();
                    dt = modelProducto.Producto_separacion_lista(0, "LST_REGISTRO", estado);
                    return Json(respuesta_Json.DatTableToDictionary(dt), JsonRequestBehavior.AllowGet);

                }
                return Json("0", JsonRequestBehavior.AllowGet);
            }
            return Json("-1", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuiaTransferencia( string idGuia )
        {
            if (security_manager.validaSesion() == false)
            {
                return RedirectToAction("Index", "Login");
            }

            if (!security_manager.validaAccesoPagina("Almacen/GuiaTransferencia", "R"))
            {
                return RedirectToAction("Index", "Home");
            }

            //Session["SessionRegGuia"] = "SI";
            ViewData["Usuario"] = Session["SessionUsuario"] as Usuario;
            ModelGeneral m_gelera = new ModelGeneral();
            ViewData["Ubigeo"] = m_gelera.listaUbigeo("");
            ViewData["Agencia"] = m_gelera.listaAgencia("");


            if (!string.IsNullOrEmpty(idGuia))
            {
                ModelGuia modelGuia = new ModelGuia();
                Usuario usuario = new Usuario();
                usuario = Session["SessionUsuario"] as Usuario;
                ViewData["mGuia"] = modelGuia.listarGuiaTransferencia("CAB-2", Convert.ToInt32(idGuia), 0, usuario.usuario, DateTime.MinValue.ToString("yyyy-MM-dd"), DateTime.MinValue.ToString("yyyy-MM-dd"), "").FirstOrDefault();
            }
            else
            {
                ViewData["mGuia"] = null;
            }

            return View();
        }

        public ActionResult GuiaTransferenciaLst()
        {
            if (security_manager.validaSesion() == false)
            {
                return RedirectToAction("Index", "Login");
            }

            if (!security_manager.validaAccesoPagina("Almacen/GuiaTransferenciaLst", "R"))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public ActionResult GuiaTransferenciaView(int idGuia)
        {

            if (security_manager.validaSesion() == false)
            {
                return RedirectToAction("Index", "Login");
            }

            if (!security_manager.validaAccesoPagina("Almacen/GuiaTransferenciaView", "R"))
            {
                return RedirectToAction("Index", "Home");
            }

            ModelGuia modelGuia = new ModelGuia();
            Usuario usuario = new Usuario();
            usuario = Session["SessionUsuario"] as Usuario;
            GuiaTransferencia guiaTransferencia = new GuiaTransferencia();
            guiaTransferencia = modelGuia.listarGuiaTransferencia("CAB-2", idGuia, 0, usuario.usuario, DateTime.MinValue.ToString("yyyy-MM-dd"), DateTime.MinValue.ToString("yyyy-MM-dd"), "").FirstOrDefault();
            ViewData["mGuia"] = guiaTransferencia;
            List<Ubigeo> lst_ubigeos = new List<Ubigeo>();
            ModelGeneral m_gelera = new ModelGeneral();
            lst_ubigeos = m_gelera.listaUbigeo(guiaTransferencia.punto_de_partida_ubigeo);
            lst_ubigeos.AddRange(m_gelera.listaUbigeo(guiaTransferencia.punto_de_llegada_ubigeo));
            ViewData["ubigeos"] = lst_ubigeos;
            return View();
        }

        [HttpGet]
        public string consultarFacturaElectronica(string doc, string ser, string num)
        {
            //FORMATO DE ENVÍO EJEMPLO: FV - 0001 - 0000001
            string urlDoc = "";
            if (security_manager.validaSesion() == true)
            {
                ModelDocFiscal modelDocFiscal = new ModelDocFiscal();
                urlDoc = modelDocFiscal.consultarFacturaElectronica(doc, ser, num);
            }
            else
            {
                return "-1";
            }
            return urlDoc;
        }

        [HttpGet]
        public string consultarGuiaElectronica(string serie, string num)
        {
            //FORMATO DE ENVÍO EJEMPLO: T001 - 0000001
            string urlDoc = "";
            if (security_manager.validaSesion() == true)
            {
                ModelDocFiscal modelDocFiscal = new ModelDocFiscal();
                urlDoc = modelDocFiscal.consultarGuiaElectronica(serie, num);
            }
            else
            {
                return "-1";
            }
            return urlDoc;
        }

        [HttpPost]
        public JsonResult inserta_guia_transferencia(string access_pass, GuiaTransferencia m_guia)
        {
            ObjRespuesta obj_respuesta = new ObjRespuesta();

            bool acceso = false;
            try
            {
                if (security_manager.validaAcceso(access_pass))
                {
                    acceso = true;
                }

                if (acceso == false)
                {
                    if (security_manager.validaAccesoPagina("Almacen/GuiaTransferencia", "E"))
                    {
                        acceso = true;
                        user = (Usuario)Session["SessionUsuario"];
                        m_guia.UsuarioActualizacion = user.usuario;
                    }
                }
            }
            catch
            {

            }

            if (acceso == true)
            {

                ModelGuia modelGuia = new ModelGuia();
                ModelDocFiscal modelDocFiscal = new ModelDocFiscal();
                ModelGeneral modelGeneral = new ModelGeneral();

                int res_cabecera = 0;
                int res_detalle = 0;
                string res_string_detalle = "";

                m_guia.transportista_placa_numero = m_guia.transportista_placa_numero.Replace("-", "");
                res_cabecera = modelGuia.crud_Guia_Transferencia_Cabecera(m_guia, "CREATE");

                if (res_cabecera > 0)
                {
                    obj_respuesta.respuesta_1 = res_cabecera.ToString();
                    obj_respuesta.observacion_1 = "Cabecera de guía registrada";

                    foreach (GuiaTransferenciaDetalle detalle in m_guia.lst_guiaDetalle)
                    {
                        detalle.IdGuiaTransferencia = res_cabecera;
                        res_detalle = modelGuia.crud_Guia_Transferencia_Detalle(detalle, "CREATE");
                        if (res_detalle < 1)
                        {
                            res_string_detalle += "x";

                            //M_log log = new M_log();
                            //log.tabla = "guia_transferencia_detalle";
                            //log.data1 = m_guia.serie.ToString();
                            //log.data2 = "Articulo:" + detalle.codigo.ToString() + "; Cantidad:" + detalle.cantidad.ToString() + "; Lote:" + detalle.lote.ToString() + "; FechaExp:" + detalle.fecha_vencimiento.ToString("yyyy-MM-dd");
                            //log.data3 = "";
                            //log.usuario = m_guia.UsuarioActualizacion;
                            //b_log.insert_Log(log);
                        }
                    }

                    if (res_string_detalle == "")
                    {
                        Util util = new Util();
                        obj_respuesta.respuesta_2 = "1";
                        obj_respuesta.observacion_2 = "Detalle de guía registrada correctamente";
                        m_guia.IdGuiaTransferencia = res_cabecera;
                        m_guia.numero = modelGeneral.obtenerCorrelativo("GR", m_guia.serie).ToString();

                        if (Convert.ToInt32(m_guia.numero) > 0)
                        {
                            obj_respuesta.observacion_3 = modelDocFiscal.envioGuiaTrasnferenciaSunat(m_guia);
                            if (obj_respuesta.observacion_3.ToLower().Contains("https://www.nubefact.com/guia/") || obj_respuesta.observacion_3.ToLower() == "error_consulta_documento")
                            {
                                modelGuia.crud_Guia_Transferencia_Cabecera(m_guia, "ENV");
                                obj_respuesta.respuesta_3 = "1";
                                obj_respuesta.respuesta_4 = m_guia.numero;
                                obj_respuesta.observacion_4 = "Número Guía";
                            }
                            else
                            {
                                obj_respuesta.respuesta_3 = "0";
                            }
                        }
                        else
                        {
                            obj_respuesta.respuesta_3 = "0";
                            obj_respuesta.observacion_3 = "Error al obtener el correlativo para la guía";
                        }
                    }
                    else
                    {
                        obj_respuesta.respuesta_2 = "0";
                        obj_respuesta.observacion_2 = "Error al registrar un detalle de guía";
                    }
                }
                else
                {
                    obj_respuesta.respuesta_1 = res_cabecera.ToString();
                    obj_respuesta.observacion_1 = "Error al registrar la cabecera de guía";
                }
                // -1: Error al registrar
                // -2: 

            }
            //return obj_respuesta;
            return Json(obj_respuesta, JsonRequestBehavior.AllowGet);
        }

        //[HttpPut]
        [HttpPost]
        public JsonResult actualiza_guia_transferencia(string access_pass, GuiaTransferencia m_guia)
        {

            ModelGuia modelGuia = new ModelGuia();
            ModelDocFiscal modelDocFiscal = new ModelDocFiscal();
            ModelGeneral modelGeneral = new ModelGeneral();
            ObjRespuesta obj_respuesta = new ObjRespuesta();

            bool acceso = false;
            try
            {
                //string val_sesionRegGuia = Session["SessionRegGuia"].ToString();

                if (security_manager.validaAcceso(access_pass))
                {
                    acceso = true;
                }
                if (acceso == false)
                {
                    if (security_manager.validaAccesoPagina("Almacen/GuiaTransferencia", "E"))
                    {
                        acceso = true;
                        user = (Usuario)Session["SessionUsuario"];
                        m_guia.UsuarioActualizacion = user.usuario;
                    }
                }
            }
            catch
            {

            }


            if (acceso == true)
            {
                int res_cabecera = 0;
                int res_detalle = 0;
                string res_string_detalle = "";

                res_cabecera = modelGuia.crud_Guia_Transferencia_Cabecera(m_guia, "UPDATE");

                if (res_cabecera > 0)
                {
                    obj_respuesta.respuesta_1 = res_cabecera.ToString();
                    obj_respuesta.observacion_1 = "Cabecera de guía actualizada";

                    List<GuiaTransferenciaDetalle> lst_detalle_guia = new List<GuiaTransferenciaDetalle>();
                    lst_detalle_guia = modelGuia.listarGuiaTransferenciaDetalle("DET-1", m_guia.IdGuiaTransferencia, 0, m_guia.UsuarioActualizacion, DateTime.MinValue.ToString("yyyy-MM-dd"), DateTime.MinValue.ToString("yyyy-MM-dd"), "");

                    foreach (GuiaTransferenciaDetalle detalle in m_guia.lst_guiaDetalle)
                    {
                        detalle.IdGuiaTransferencia = m_guia.IdGuiaTransferencia;
                        if (detalle.IdGuiaTransferenciaDetalle != 0)
                        {
                            res_detalle = modelGuia.crud_Guia_Transferencia_Detalle(detalle, "UPDATE");
                            if (res_detalle < 1)
                            {
                                res_string_detalle += "x";

                                /* M_log log = new M_log();
                                 log.tabla = "guia_transferencia_detalle";
                                 log.data1 = m_guia.serie.ToString();
                                 log.data2 = "Articulo:" + detalle.codigo.ToString() + "; Cantidad:" + detalle.cantidad.ToString() + "; Lote:" + detalle.lote.ToString() + "; FechaExp:" + detalle.fecha_vencimiento.ToString("yyyy-MM-dd");
                                 log.data3 = "";
                                 log.usuario = m_guia.UsuarioActualizacion;
                                 b_log.insert_Log(log);*/
                            }
                        }
                        else
                        {
                            res_detalle = modelGuia.crud_Guia_Transferencia_Detalle(detalle, "CREATE");
                            if (res_detalle < 1)
                            {
                                res_string_detalle += "x";

                                /* M_log log = new M_log();
                                 log.tabla = "guia_transferencia_detalle";
                                 log.data1 = m_guia.serie.ToString();
                                 log.data2 = "Articulo:" + detalle.codigo.ToString() + "; Cantidad:" + detalle.cantidad.ToString() + "; Lote:" + detalle.lote.ToString() + "; FechaExp:" + detalle.fecha_vencimiento.ToString("yyyy-MM-dd");
                                 log.data3 = "";
                                 log.usuario = m_guia.UsuarioActualizacion;
                                 b_log.insert_Log(log);*/
                            }
                        }
                    }

                    string res_string_detalle_elimina = "";

                    if (res_string_detalle == "")
                    {
                        int result_elimina = 0;
                        foreach (GuiaTransferenciaDetalle detalle_pre_actualizacion in lst_detalle_guia)
                        {
                            GuiaTransferenciaDetalle m_GuiaDetalleElimina = m_guia.lst_guiaDetalle.Where(n => n.IdGuiaTransferenciaDetalle == detalle_pre_actualizacion.IdGuiaTransferenciaDetalle).FirstOrDefault();
                            if (m_GuiaDetalleElimina == null)
                            {
                                result_elimina = modelGuia.crud_Guia_Transferencia_Detalle(detalle_pre_actualizacion, "DELETE");
                                if (result_elimina < 1)
                                {
                                    res_string_detalle_elimina += "x";
                                }
                            }
                        }
                    }

                    if (res_string_detalle == "" && res_string_detalle_elimina == "")
                    {
                        Util util = new Util();
                        obj_respuesta.respuesta_2 = "1";
                        obj_respuesta.observacion_2 = "Detalle de guía editada correctamente";
                        //m_guia.IdGuiaTransferencia = res_cabecera;
                        m_guia.numero = modelGeneral.obtenerCorrelativo("GR", m_guia.serie).ToString();

                        if (Convert.ToInt32(m_guia.numero) > 0)
                        {
                            obj_respuesta.observacion_3 = modelDocFiscal.envioGuiaTrasnferenciaSunat(m_guia);
                            if (obj_respuesta.observacion_3.ToLower().Contains("https://www.nubefact.com/guia/") || obj_respuesta.observacion_3.ToLower() == "error_consulta_documento")
                            {
                                modelGuia.crud_Guia_Transferencia_Cabecera(m_guia, "ENV");
                                obj_respuesta.respuesta_3 = "1";
                                obj_respuesta.respuesta_4 = m_guia.numero;
                                obj_respuesta.observacion_4 = "Número Guía";
                            }
                            else
                            {
                                obj_respuesta.respuesta_3 = "0";
                            }
                        }
                        else
                        {
                            obj_respuesta.respuesta_3 = "0";
                            obj_respuesta.observacion_3 = "Error al obtener el correlativo para la guía";
                        }
                    }
                    else
                    {
                        obj_respuesta.respuesta_2 = "0";
                        obj_respuesta.observacion_2 = "Error al editar un detalle de guía";
                    }

                }
                else
                {
                    obj_respuesta.respuesta_1 = res_cabecera.ToString();
                    obj_respuesta.observacion_1 = "Error al editar la cabecera de guía o el documento ya fue enviado a la sunat";
                }

                // -1: Error al registrar
                // -2: 
            }

            //return obj_respuesta;
            return Json(obj_respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult listarGuiaTransferencia(string access_pass, string usuario, string fecha_inicio, string fecha_fin, string estado)
        {

            bool acceso = false;
            List<GuiaTransferencia> lst_guia = new List<GuiaTransferencia>();

            if (security_manager.validaAcceso(access_pass))
            {
                acceso = true;
            }

            if (acceso == false)
            {
                if (security_manager.validaSesion() == true)
                {
                    if (security_manager.validaAccesoPagina("Almacen/GuiaTransferencia", "E"))
                    {
                        acceso = true;
                        user = (Usuario)Session["SessionUsuario"];
                        usuario = user.usuario;
                    }
                }
                else
                {
                    return Json("-1", JsonRequestBehavior.AllowGet);
                }
            }

            if (acceso == true)
            {
                ModelGuia modelGuia = new ModelGuia();

                if (usuario == null)
                    usuario = "";

                if (estado == null)
                    estado = "";

                lst_guia = modelGuia.listarGuiaTransferencia("CAB-1", 0, 0, usuario, fecha_inicio, fecha_fin, estado);

            }

            return Json(lst_guia, JsonRequestBehavior.AllowGet);
            //return lst_guia;
        }

        [HttpGet]
        public JsonResult listarGuiaTransferenciaID(string access_pass, int id)
        {
            bool acceso = false;
            GuiaTransferencia m_Guia = new GuiaTransferencia();

            if (security_manager.validaAcceso(access_pass))
            {
                acceso = true;
            }

            if (acceso == false)
            {
                if (security_manager.validaSesion() == true)
                {
                    if (security_manager.validaAccesoPagina("Almacen/GuiaTransferencia", "R"))
                    {
                        acceso = true;

                    }
                }
                else
                {
                    return Json("-1", JsonRequestBehavior.AllowGet);
                }
            }
            if (acceso == true)
            {
                ModelGuia modelGuia = new ModelGuia();
                if (security_manager.validaAcceso(access_pass))
                {
                    m_Guia = modelGuia.listarGuiaTransferencia("CAB-2", id, 0, "", DateTime.MinValue.ToString("yyyy-MM-dd"), DateTime.MinValue.ToString("yyyy-MM-dd"), "").FirstOrDefault();
                }
            }

            return Json(m_Guia, JsonRequestBehavior.AllowGet);
            //return m_Guia;
        }

        public ActionResult Agencia()
        {
            if (security_manager.validaSesion() == false)
            {
                return RedirectToAction("Index", "Login");
            }

            //if (!security_manager.validaAccesoPagina("Almacen/Separacion", "R"))
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            //ModelGeneral m_gelera = new ModelGeneral();
            //ViewData["Agencia"] = m_gelera.listaAgencia("");
            ViewData["Editar"] = security_manager.validaAccesoPagina("Almacen/Agencia", "E");
            ViewData["Eliminar"] = security_manager.validaAccesoPagina("Almacen/Agencia", "D");
            return View();
        }

        [HttpGet]
        public JsonResult agencias_gestion(int idAgencia, string ruc, string razon_social, string nombre_corto, string opcion)
        {
            string res = "";
            bool acceso = false;
            if (security_manager.validaSesion() == true)
            {
                if (opcion == "DELETE")
                {
                    if (security_manager.validaAccesoPagina("Almacen/Agencia", "D"))
                    {
                        acceso = true;
                    }
                }
                else
                {
                    if (security_manager.validaAccesoPagina("Almacen/Agencia", "E"))
                    {
                        acceso = true;
                    }
                }

                if (acceso == true)
                {
                    if (opcion != "DELETE" && ruc.Trim() != "" && razon_social.Trim() != "")
                    {
                        //user = (Usuario)Session["SessionUsuario"];
                        ModelProducto modelProducto = new ModelProducto();
                        res = modelProducto.agencia_gestion(idAgencia, ruc, razon_social, nombre_corto, opcion).ToString();
                    }
                    else if (opcion == "DELETE" && idAgencia != 0)
                    {
                        ModelProducto modelProducto = new ModelProducto();
                        res = modelProducto.agencia_gestion(idAgencia, "", "", "", opcion).ToString();
                    }
                    else
                    {
                        res = "0";
                    }
                }
                else
                {
                    res = "0";
                }


            }
            else
            {
                res = "-1";
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult listarAgencias()
        {
            List<Agencia> agenciaList = new List<Agencia>();
            if (security_manager.validaSesion() == true)
            {
                ModelGeneral m_gelera = new ModelGeneral();
                agenciaList = m_gelera.listaAgencia("");
            }
            else
            {
                return Json("-1", JsonRequestBehavior.AllowGet);
            }

            return Json(agenciaList, JsonRequestBehavior.AllowGet);
            //return lst_guia;
        }




        // GET: Almacen
        public ActionResult Index()
        {
            return View();
        }

        // GET: Almacen/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Almacen/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Almacen/Create
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

        // GET: Almacen/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Almacen/Edit/5
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

        // GET: Almacen/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Almacen/Delete/5
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
