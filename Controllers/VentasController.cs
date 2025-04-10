using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppMontGroup.Entity;
using SecurityManager = WebAppMontGroup.Models.SecurityManager;
using WebAppMontGroup.WebServiceEasy;
using System.Data;
using System.Web.Configuration;
using System.Web.DynamicData;
using Microsoft.Ajax.Utilities;
using WebAppMontGroup.Models;
using System.Configuration;

namespace WebAppMontGroup.Controllers
{
    [Route("api/[controller]")]
    public class VentasController : Controller
    {

        SecurityManager security_manager = new SecurityManager();
        Usuario user = new Usuario();

        public ActionResult Historial()
        {

            if (security_manager.validaSesion() == false)
            {
                return RedirectToAction("Index", "Login");
            }

            if (!security_manager.validaAccesoPagina("Ventas/Historial", "R"))
            {
                return RedirectToAction("Index", "Home");
            }

            //StockAlmacenPrincipal("2408", "03", "");

            return View();
        }

        public ActionResult Documento()
        {

            if (security_manager.validaSesion() == false)
            {
                return RedirectToAction("Index", "Login");
            }

            string documento = Session["getDocumento"].ToString();
            ModelDocumento model_documento = new ModelDocumento();
            DataTable dtDocumento = new DataTable();

            string usr = security_manager.codUserConsulta();

            dtDocumento = model_documento.getDocumento(documento, usr);
            ViewData["ViewDocumento"] = dtDocumento;
            //StockAlmacenPrincipal("2408", "03", "");

            string docFch = string.Empty;
            if (dtDocumento.Rows.Count > 0 && dtDocumento.Columns.Count > 1)
            {
                DateTime fechaDocumento = Convert.ToDateTime(dtDocumento.Rows[0]["doc_fch"]);
                docFch = fechaDocumento.ToString("yyMM");

                // Obtener los detalles del documento con el campo `doc_fch`
                DataTable dtDetalleDocumento = model_documento.getDetalleDocumento(documento, docFch, usr);

                // Guardar el DataTable de los detalles en un ViewBag
                ViewBag.DetalleDocumento = dtDetalleDocumento;

                DataTable dtdocumentoplanilla = model_documento.getDocumentoPlanilla(documento);
                ViewBag.documentoxplanilla = dtdocumentoplanilla;
            }

            return View();

        }

        [HttpGet]
        public string getDocumento(string documento)
        {
            string respuesta = "1";
            if (security_manager.validaSesion() == true)
            {
                //dt = model_documento.getDocumento(documento);
                Session["getDocumento"] = documento;
            }
            else
            {
                respuesta = "-1";
            }
            return respuesta;//Json(respuesta_Json.DatTableToDictionary(dt), JsonRequestBehavior.AllowGet);
        }


        public ActionResult DocumentList()
        {
            ModelDocumento model_documento = new ModelDocumento();
            if (security_manager.validaSesion() == false)
            {
                return RedirectToAction("Index", "Login");
            }

            if (!security_manager.validaAccesoPagina("Ventas/DocumentList", "R"))
            {
                return RedirectToAction("Index", "Home");
            }

            string tipo_usuario = Session["TipoUsuario"].ToString();
            if (tipo_usuario == "1")
            {
                ViewData["TipoUsuario"] = "1";
                ViewData["Usuario"] = Session["SessionUsuario"] as Usuario;
                ViewData["VendedorAsociado"] = Session["VendedorAsociado"] as List<Usuario>;
                string codVendedor = security_manager.codUserConsulta();
                DataTable dt = new DataTable();
                ModelCliente modelCliente = new ModelCliente();
                ViewData["ClientesEasy"] = modelCliente.getClienteEasy(codVendedor);
            }
            else
            {
                ViewData["TipoUsuario"] = "2";
            }

            ViewBag.MesesMaximosSinFiltro = ConfigurationManager.AppSettings["MesesMaximosSinFiltro"];
            ViewBag.MesesMaximosConFiltro = ConfigurationManager.AppSettings["MesesMaximosConFiltro"];
            DataTable vendedores = model_documento.Vendedores();
            ViewBag.Vendedores = vendedores;
            return View();
        }

        [HttpGet]
        public JsonResult getDocumentosfiltrados(DateTime fechaInicio, DateTime fechaFin, string tipoDocumento,
                          string serie, string vendedor, string cliente,
                          string cancelado, string anulado, string articulo, string lote)
        {
            if (security_manager.validaAccesoPagina("Ventas/DocumentList", "R"))
            {
                try
                {
                    string usr = security_manager.codUserConsulta();
                    ModelDocumento model_documento = new ModelDocumento();
                    // Obtener el DataTable desde el modelo
                    DataTable dt = model_documento.GetDocumentosFiltrados(fechaInicio.ToString("yyyy-MM-dd"), fechaFin.ToString("yyyy-MM-dd"), tipoDocumento, serie, vendedor, cliente, cancelado, anulado, articulo, lote, usr);
                    // Obtener el número total de documentos sin paginación
                    int totalDocuments = dt.Rows.Count;
                    Respuesta_Json respuestaJson = new Respuesta_Json();
                    var documentos = respuestaJson.DatTableToDictionary(dt);
                    var jsonResult = Json(documentos, JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;
                    //return Json(new { success = true, data = documentos, totalDocuments = totalDocuments }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    // Manejar cualquier error y devolver un mensaje apropiado
                    //Console.WriteLine("error en getDocumentosfiltrados - controller" + ex.Message);
                    return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json("-1", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getDocumentosDetallefiltrados(DateTime fechaInicio, DateTime fechaFin, string tipoDocumento,
                                 string serie, string vendedor, string cliente,
                                 string cancelado, string anulado, string articulo, string lote)
        {
            if (security_manager.validaAccesoPagina("Ventas/DocumentList", "R"))
            {
                try
                {

                    string usr = security_manager.codUserConsulta();
                    ModelDocumento model_documento = new ModelDocumento();
                    // Obtener el DataTable desde el modelo
                    DataTable dt = model_documento.GetDocumentosDetalleFiltrados(fechaInicio.ToString("yyyy-MM-dd"), fechaFin.ToString("yyyy-MM-dd"), tipoDocumento, serie, vendedor, cliente, cancelado, anulado, articulo, lote, usr);
                    // Obtener el número total de documentos sin paginación
                    int totalDocuments = dt.Rows.Count;
                    Respuesta_Json respuestaJson = new Respuesta_Json();
                    var documentos = respuestaJson.DatTableToDictionary(dt);
                    var jsonResult = Json(documentos, JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;
                }
                catch (Exception ex)
                {
                    // Manejar cualquier error y devolver un mensaje apropiado
                    //Console.WriteLine("error en getDocumentosfiltrados - controller" + ex.Message);
                    return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json("-1", JsonRequestBehavior.AllowGet);
        }



   


        

        // GET: Ventas
        public ActionResult Index()
        {
            return View();
        }

        // GET: Ventas/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Ventas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ventas/Create
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

        // GET: Ventas/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Ventas/Edit/5
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

        // GET: Ventas/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Ventas/Delete/5
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
