using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using WebAppMontGroup.Entity;

namespace WebAppMontGroup.Controllers
{
    public class UtilController : Controller
    {
        // GET: Util
        //public ActionResult Index()
        //{
        //    return View();
        //}


        [HttpGet]
        public JsonResult ConsultarReniec(string documento)
        {
            if (string.IsNullOrWhiteSpace(documento))
            {
                return Json(new { mensaje = "El documento no puede estar vacío" }, JsonRequestBehavior.AllowGet);
            }

            string urlReniec = WebConfigurationManager.AppSettings["URL_RENIEC"];
            string tokenReniec = WebConfigurationManager.AppSettings["TOKEN_RENIEC"];


            string tipo = documento.Length == 8 ? "dni" : documento.Length == 11 ? "ruc" : null;

            if (tipo == null)
            {
                return Json(new { mensaje = "El documento ingresado no es válido" }, JsonRequestBehavior.AllowGet);
            }

            // Construir la URL final
            string apiUrl = $"{urlReniec}{tipo}/{documento}?token={tokenReniec}";

            try
            {
                using (var client = new HttpClient(new HttpClientHandler { ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true }))
                {
                    var response = client.GetAsync(apiUrl).Result;
                    return response.IsSuccessStatusCode
                        ? Json(new { mensaje = "OK", data = response.Content.ReadAsStringAsync().Result }, JsonRequestBehavior.AllowGet)
                        : Json(new { mensaje = "Error al consultar la API", estado = (int)response.StatusCode }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { mensaje = "Error interno al realizar la consulta", detalle = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }



        [HttpPost]
        public JsonResult EnvioDeCorreoCpanel(string usuario, string password, [FromBody] string DataCorreo) //,string correoDestino,string asunto,string EncodeBase64correo)
        {
            string user = System.Configuration.ConfigurationManager.AppSettings["userUtil"];
            string pass = System.Configuration.ConfigurationManager.AppSettings["passUtil"];

            Util util = new Util();
            string res = "-1";

            if (usuario == user && password == pass)
            {

                JObject jObject = JObject.Parse(DataCorreo);
                //JObject version = (JObject)jObject["version"];
                string correoDestino = "";//DataCorreo.correoDestino;
                string asunto = "";//DataCorreo.asunto;
                string EncodeBase64correo = "";//DataCorreo.EncodeBase64correo;
                res = util.EnvioDeCorreoCpanel(correoDestino, asunto, EncodeBase64correo);
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        private class FromBodyAttribute : Attribute
        {
        }




        //public class ESendEmail
        //{
        //    public string correoDestino { get; set; }
        //    public string asunto { get; set; }
        //    public string EncodeBase64correo { get; set; }
        //}
    }
}