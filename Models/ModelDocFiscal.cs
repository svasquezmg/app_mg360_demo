using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebAppMontGroup.Entity;

namespace WebAppMontGroup.Models
{
    public class ModelDocFiscal
    {

        Util util = new Util();
        string rutaArcvhivos = ConfigurationManager.AppSettings["RutaArcvhivos"];
        string userFactur = WebConfigurationManager.AppSettings["usrFactur"];
        string passFactur = WebConfigurationManager.AppSettings["passFactur"];
        string rutaGuia = WebConfigurationManager.AppSettings["usrGuia"];
        string tokenGuia = WebConfigurationManager.AppSettings["passGuia"];




        public string consultarFacturaElectronica(string doc, string ser, string num)
        {
            //FORMATO DE ENVÍO EJEMPLO: FV - 0001 - 0000001
            string codCPE = "";
            string nunSerieCPE = "";

            string nser = ser.TrimStart('0').PadLeft(3, '0');
            if (doc == "BV")
            {
                codCPE = "03";
                nunSerieCPE = "B" + nser;
            }
            if (doc == "FV")
            {
                codCPE = "01";
                nunSerieCPE = "F" + nser;
            }

            string respuesta = "-1";

            JObject rss_datos = new JObject(new JProperty("user", new JObject(new JProperty("username", userFactur), new JProperty("password", passFactur))), new JProperty("codCPE", codCPE), new JProperty("numSerieCPE", nunSerieCPE), new JProperty("numCPE", num));

            try
            {
                string url = "";

                //Asigando el TLS v. 1.2 
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                url = "https://www.escondatagate.net/wsBackend/clients/getPdfURL";

                Uri uri = new Uri(url);
                string data = rss_datos.ToString();
                if ((uri.Scheme == Uri.UriSchemeHttps))
                {
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                    request.Method = WebRequestMethods.Http.Post;
                    request.ContentLength = data.Length;
                    request.ContentType = "application/json";
                    StreamWriter writer = new StreamWriter(request.GetRequestStream());
                    writer.Write(data);
                    writer.Close();
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string tmp = reader.ReadToEnd();
                    respuesta = JObject.Parse(tmp)["pdfURL"].ToString();
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                util.Escribir_Log(ex.ToString());
                respuesta = "0";
            }
            return respuesta;
        }







        /// <summary>
        /// GUIAS ELECTRONICAS
        /// </summary>
        /// <param name="serie"></param>
        /// <param name="num"></param>
        /// <returns></returns>

        public string consultarGuiaElectronica(string serie, string num)
        {


            // ENVIAR DOCUMENTO PORQUE CUMPLE CON LOS REQUICITOS

            string enlace = "-1";
            var consulta = new Consulta();
            consulta.operacion = "consultar_guia";
            consulta.tipo_de_comprobante = 7;
            consulta.serie = serie;
            consulta.numero = Convert.ToInt32(num);

            var json_envio = JsonConvert.SerializeObject(consulta, Formatting.Indented);
            var json_sincodificar = Encoding.UTF8.GetBytes(json_envio);
            var json_utf8 = Encoding.UTF8.GetString(json_sincodificar);
            var json_de_respuesta = sendJson(json_envio.ToString());

            var r = JsonConvert.DeserializeObject<Respuesta>(json_de_respuesta);
            var json_r_in = JsonConvert.SerializeObject(r, Formatting.Indented);

            JObject jsonObject = JObject.Parse(json_r_in);

            // Acceder a los valores utilizando la clase JToken
            int tipoDeComprobanten = Convert.ToInt32(jsonObject.GetValue("tipo_de_comprobante")); //jsonObject("tipo_de_comprobante").ToObject<int>();
            string serien = jsonObject.GetValue("serie").ToString(); //jsonObject("serie").ToObject<string>();
            int numeron = Convert.ToInt32(jsonObject.GetValue("numero")); //jsonObject("numero").ToObject<int>();
            string enlaceDelPdfn = jsonObject.GetValue("enlace_del_pdf").ToString(); //jsonObject("enlace_del_pdf").ToObject<string>();

            if (tipoDeComprobanten > 0)
            {
                enlace = enlaceDelPdfn;
                //Process.Start(enlace);
            }

            return enlace;
        }

        private string sendJson(string json)
        {
            using (WebClient wc = new WebClient())
            {
                try
                {
                    wc.Headers.Add(HttpRequestHeader.ContentType, "application/json; charset=utf-8");
                    wc.Headers.Add(HttpRequestHeader.Authorization, "Token token=" + tokenGuia);
                    var respuesta = wc.UploadString(rutaGuia, "POST", json);
                    return respuesta;
                }
                catch (WebException ex)
                {
                    util.Escribir_Log(ex.ToString());
                    return new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
            }
        }

        public string envioGuiaTrasnferenciaSunat(GuiaTransferencia m_guia)
        {

            string respuesta = "";
            try
            {
                GuiaElectronica obj_GuiaCabecera = new GuiaElectronica();
                List<GuiaElectronicaDetalle> list_GuiDetalle = null;
                list_GuiDetalle = new List<GuiaElectronicaDetalle>();

                DateTime fechaActual = DateTime.Now;
                string fechaFormateada = fechaActual.ToString("dd-MM-yyyy");
                string lote_fecha = "";

                obj_GuiaCabecera.operacion = "generar_guia";
                obj_GuiaCabecera.tipo_de_comprobante = 7;
                obj_GuiaCabecera.serie = m_guia.serie;
                obj_GuiaCabecera.numero = Convert.ToInt32(m_guia.numero);
                obj_GuiaCabecera.cliente_numero_de_documento = m_guia.cliente_documento;
                obj_GuiaCabecera.cliente_tipo_de_documento = m_guia.cliente_tipo_documento;
                obj_GuiaCabecera.fecha_de_emision = m_guia.fecha_emision.ToString("dd/MM/yyyy");
                obj_GuiaCabecera.fecha_de_inicio_de_traslado = m_guia.fecha_traslado.ToString("dd/MM/yyyy");
                obj_GuiaCabecera.cliente_denominacion = m_guia.cliente_nombre;
                obj_GuiaCabecera.cliente_direccion = "";
                obj_GuiaCabecera.cliente_email = "";
                obj_GuiaCabecera.cliente_email_1 = "";
                obj_GuiaCabecera.cliente_email_2 = "";
                obj_GuiaCabecera.motivo_de_traslado = m_guia.motivo_traslado.ToString();// 01 venta, 04 tranlado entre almacenes, 13 OTROS
                obj_GuiaCabecera.motivo_de_traslado_otros_descripcion = "";
                obj_GuiaCabecera.peso_bruto_total = "1.00";
                obj_GuiaCabecera.peso_bruto_unidad_de_medida = "KGM";
                obj_GuiaCabecera.numero_de_bultos = "1";
                obj_GuiaCabecera.tipo_de_transporte = m_guia.tipo_transporte.ToString();
                obj_GuiaCabecera.transportista_documento_tipo = "6";
                obj_GuiaCabecera.transportista_documento_numero = m_guia.transportista_documento_numero == null ? "" : m_guia.transportista_documento_numero; //; m_guia.transportista_documento_numero.ToString();
                obj_GuiaCabecera.transportista_denominacion = m_guia.transportista_nombre == null ? "" : m_guia.transportista_nombre;

                if (obj_GuiaCabecera.tipo_de_transporte == "02") // 02 privado, 01 publico
                {
                    obj_GuiaCabecera.conductor_numero_licencia = m_guia.conductor_numero_licencia == null ? "" : m_guia.conductor_numero_licencia;
                    obj_GuiaCabecera.transportista_placa_numero = m_guia.transportista_placa_numero == null ? "" : m_guia.transportista_placa_numero; 

                    if (obj_GuiaCabecera.conductor_documento_tipo.ToString() != "")
                    {
                        obj_GuiaCabecera.sunat_envio_indicador = "";
                        obj_GuiaCabecera.conductor_nombre = m_guia.conductor_nombre == null ? "" : m_guia.conductor_nombre; 
                        obj_GuiaCabecera.conductor_apellidos = m_guia.conductor_apellidos == null ? "" : m_guia.conductor_apellidos;  
                        obj_GuiaCabecera.conductor_documento_tipo = m_guia.conductor_documento_tipo == null ? "" : m_guia.conductor_documento_tipo; 
                        obj_GuiaCabecera.conductor_documento_numero = m_guia.conductor_documento_numero == null ? "" : m_guia.conductor_documento_numero;
                    }
                    else
                    {
                        obj_GuiaCabecera.sunat_envio_indicador = "06";
                        obj_GuiaCabecera.transportista_documento_tipo = "";
                        obj_GuiaCabecera.transportista_documento_numero = "";
                        obj_GuiaCabecera.transportista_denominacion = "";
                    }

                }

                obj_GuiaCabecera.punto_de_partida_ubigeo = m_guia.punto_de_partida_ubigeo;
                obj_GuiaCabecera.punto_de_partida_direccion = m_guia.punto_de_partida_direccion;
                obj_GuiaCabecera.punto_de_partida_codigo_establecimiento_sunat = "0000";
                obj_GuiaCabecera.punto_de_llegada_ubigeo = m_guia.punto_de_llegada_ubigeo;
                obj_GuiaCabecera.punto_de_llegada_direccion = m_guia.punto_de_llegada_direccion;
                obj_GuiaCabecera.punto_de_llegada_codigo_establecimiento_sunat = "0000";
                obj_GuiaCabecera.enviar_automaticamente_al_cliente = "false";
                obj_GuiaCabecera.formato_de_pdf = "";

                obj_GuiaCabecera.observaciones = "";

                m_guia.atencion_dni_1 =  m_guia.atencion_dni_1 == null ? "" : m_guia.atencion_dni_1;
                m_guia.atencion_dni_2 = m_guia.atencion_dni_2 == null ? "" : m_guia.atencion_dni_2;
                m_guia.agencia = m_guia.agencia == null ? "" : m_guia.agencia;
                m_guia.flete_x_pagar = m_guia.flete_x_pagar == null ? "" : m_guia.flete_x_pagar;

                if (m_guia.atencion_dni_1 != "")
                {
                    obj_GuiaCabecera.observaciones += "DNI: " + m_guia.atencion_dni_1 + " - " + m_guia.atencion_1 + "<br>";
                }
                if (m_guia.atencion_dni_2 != "")
                {
                    obj_GuiaCabecera.observaciones += "DNI: " + m_guia.atencion_dni_2 + " - " + m_guia.atencion_2 + "<br>";
                }

                obj_GuiaCabecera.observaciones += m_guia.observaciones + "<br>";

                if (m_guia.agencia != "")
                {
                    obj_GuiaCabecera.observaciones += m_guia.agencia + "<br>";
                }
                if (m_guia.flete_x_pagar != "")
                {
                    obj_GuiaCabecera.observaciones += m_guia.flete_x_pagar + "<br>";
                }


                Util util = new Util();
                foreach (GuiaTransferenciaDetalle detalle_guia in m_guia.lst_guiaDetalle)
                {
                    string lote_temp = detalle_guia.lote.ToString(); // util.quitarCerosIzquierda(detalle_guia.lote.ToString());

                    if (lote_temp == "")
                    {
                        lote_fecha = "";
                    }
                    else
                    {
                        lote_fecha = "  L: " + lote_temp;
                        //lote_fecha = lote_fecha + detalle_guia.codigo.ToString();             
                    }

                    if (detalle_guia.fecha_vencimiento > Convert.ToDateTime("2000-01-01")) /*DateTime.Now)*/
                    {
                        lote_fecha += "  F.V. " + detalle_guia.fecha_vencimiento.ToString("dd/MM/yyyy");
                    }

                    GuiaElectronicaDetalle item = new GuiaElectronicaDetalle();

                    item.unidad_de_medida = "NIU";
                    item.codigo = detalle_guia.codigo.ToString();
                    item.descripcion = detalle_guia.producto + lote_fecha.ToUpper();
                    item.cantidad = System.Convert.ToInt32(detalle_guia.cantidad.ToString("F0"));
                    list_GuiDetalle.Add(item);

                }

                respuesta = GeneraEstructuraGuia(obj_GuiaCabecera, list_GuiDetalle);

            }
            catch (Exception ex)
            {
                Util util_log = new Util();
                util_log.Escribir_Log("envioGuiaTrasnferenciaSunat," + ex.ToString());
            }


            return respuesta;
        }

        public string GeneraEstructuraGuia(GuiaElectronica objGuiaElectronica, List<GuiaElectronicaDetalle> detalleGuia)
        {
            objGuiaElectronica.cliente_denominacion = caracteres_guia(objGuiaElectronica.cliente_denominacion);
            objGuiaElectronica.transportista_denominacion = caracteres_guia(objGuiaElectronica.transportista_denominacion);
            objGuiaElectronica.conductor_nombre = caracteres_guia(objGuiaElectronica.conductor_nombre);
            objGuiaElectronica.conductor_apellidos = caracteres_guia(objGuiaElectronica.conductor_apellidos);
            objGuiaElectronica.punto_de_partida_direccion = caracteres_guia(objGuiaElectronica.punto_de_partida_direccion);
            objGuiaElectronica.punto_de_llegada_direccion = caracteres_guia(objGuiaElectronica.punto_de_llegada_direccion);
            objGuiaElectronica.observaciones = caracteres_guia(objGuiaElectronica.observaciones);

            objGuiaElectronica.observaciones = objGuiaElectronica.observaciones.Replace("BR>", "<br>");
            objGuiaElectronica.observaciones = objGuiaElectronica.observaciones.Replace("<<br>", "<br>");

            string respuesta = "";
            JObject rss;
            try
            {
                if ((objGuiaElectronica.sunat_envio_indicador == ""))
                {
                    rss = new JObject(new JProperty("operacion", objGuiaElectronica.operacion),
                          new JProperty("tipo_de_comprobante", objGuiaElectronica.tipo_de_comprobante),
                          new JProperty("serie", objGuiaElectronica.serie),
                          new JProperty("numero", objGuiaElectronica.numero),
                          new JProperty("cliente_tipo_de_documento", objGuiaElectronica.cliente_tipo_de_documento),
                          new JProperty("cliente_numero_de_documento", objGuiaElectronica.cliente_numero_de_documento),
                          new JProperty("cliente_denominacion", objGuiaElectronica.cliente_denominacion),
                          new JProperty("cliente_direccion", objGuiaElectronica.cliente_direccion),
                          new JProperty("cliente_email", objGuiaElectronica.cliente_email),
                          new JProperty("cliente_email_1", objGuiaElectronica.cliente_email_1),
                          new JProperty("cliente_email_2", objGuiaElectronica.cliente_email_2),
                          new JProperty("fecha_de_emision", objGuiaElectronica.fecha_de_emision),
                          new JProperty("observaciones", objGuiaElectronica.observaciones),
                          new JProperty("motivo_de_traslado", objGuiaElectronica.motivo_de_traslado),
                          new JProperty("motivo_de_traslado_otros_descripcion", objGuiaElectronica.motivo_de_traslado_otros_descripcion),
                          new JProperty("peso_bruto_total", objGuiaElectronica.peso_bruto_total),
                          new JProperty("peso_bruto_unidad_de_medida", objGuiaElectronica.peso_bruto_unidad_de_medida),
                          new JProperty("numero_de_bultos", objGuiaElectronica.numero_de_bultos),
                          new JProperty("tipo_de_transporte", objGuiaElectronica.tipo_de_transporte),
                          new JProperty("fecha_de_inicio_de_traslado", objGuiaElectronica.fecha_de_inicio_de_traslado),
                          new JProperty("transportista_documento_tipo", objGuiaElectronica.transportista_documento_tipo),
                          new JProperty("transportista_documento_numero", objGuiaElectronica.transportista_documento_numero),
                          new JProperty("transportista_denominacion", objGuiaElectronica.transportista_denominacion),
                          new JProperty("transportista_placa_numero", objGuiaElectronica.transportista_placa_numero),
                          new JProperty("conductor_documento_tipo", objGuiaElectronica.conductor_documento_tipo),
                          new JProperty("conductor_documento_numero", objGuiaElectronica.conductor_documento_numero),
                          new JProperty("conductor_nombre", objGuiaElectronica.conductor_nombre),
                          new JProperty("conductor_apellidos", objGuiaElectronica.conductor_apellidos),
                          new JProperty("conductor_numero_licencia", objGuiaElectronica.conductor_numero_licencia),
                          new JProperty("punto_de_partida_ubigeo", objGuiaElectronica.punto_de_partida_ubigeo),
                          new JProperty("punto_de_partida_direccion", objGuiaElectronica.punto_de_partida_direccion),
                          new JProperty("punto_de_partida_codigo_establecimiento_sunat", objGuiaElectronica.punto_de_partida_codigo_establecimiento_sunat),
                          new JProperty("punto_de_llegada_ubigeo", objGuiaElectronica.punto_de_llegada_ubigeo),
                          new JProperty("punto_de_llegada_direccion", objGuiaElectronica.punto_de_llegada_direccion),
                          new JProperty("punto_de_llegada_codigo_establecimiento_sunat", objGuiaElectronica.punto_de_llegada_codigo_establecimiento_sunat),
                          new JProperty("enviar_automaticamente_al_cliente", objGuiaElectronica.enviar_automaticamente_al_cliente),
                          new JProperty("formato_de_pdf", objGuiaElectronica.formato_de_pdf),
                          new JProperty("items", new JArray(from p in detalleGuia
                                                            select new JObject(new JProperty("unidad_de_medida", p.unidad_de_medida),
                                                            new JProperty("codigo", p.codigo),
                                                            new JProperty("descripcion", caracteres_guia(p.descripcion)),
                                                            new JProperty("cantidad", p.cantidad)))));
                }
                else
                {
                    rss = new JObject(new JProperty("operacion", objGuiaElectronica.operacion),
                          new JProperty("tipo_de_comprobante", objGuiaElectronica.tipo_de_comprobante),
                          new JProperty("serie", objGuiaElectronica.serie),
                          new JProperty("numero", objGuiaElectronica.numero),
                          new JProperty("cliente_tipo_de_documento", objGuiaElectronica.cliente_tipo_de_documento),
                          new JProperty("cliente_numero_de_documento", objGuiaElectronica.cliente_numero_de_documento),
                          new JProperty("cliente_denominacion", objGuiaElectronica.cliente_denominacion),
                          new JProperty("cliente_direccion", objGuiaElectronica.cliente_direccion),
                          new JProperty("cliente_email", objGuiaElectronica.cliente_email),
                          new JProperty("cliente_email_1", objGuiaElectronica.cliente_email_1),
                          new JProperty("cliente_email_2", objGuiaElectronica.cliente_email_2),
                          new JProperty("fecha_de_emision", objGuiaElectronica.fecha_de_emision),
                          new JProperty("observaciones", objGuiaElectronica.observaciones),
                          new JProperty("motivo_de_traslado", objGuiaElectronica.motivo_de_traslado),
                          new JProperty("motivo_de_traslado_otros_descripcion", objGuiaElectronica.motivo_de_traslado_otros_descripcion),
                          new JProperty("peso_bruto_total", objGuiaElectronica.peso_bruto_total),
                          new JProperty("peso_bruto_unidad_de_medida", objGuiaElectronica.peso_bruto_unidad_de_medida),
                          new JProperty("numero_de_bultos", objGuiaElectronica.numero_de_bultos),
                          new JProperty("tipo_de_transporte", objGuiaElectronica.tipo_de_transporte),
                          new JProperty("fecha_de_inicio_de_traslado", objGuiaElectronica.fecha_de_inicio_de_traslado),
                          new JProperty("transportista_documento_tipo", objGuiaElectronica.transportista_documento_tipo),
                          new JProperty("transportista_documento_numero", objGuiaElectronica.transportista_documento_numero),
                          new JProperty("transportista_denominacion", objGuiaElectronica.transportista_denominacion),
                          new JProperty("transportista_placa_numero", objGuiaElectronica.transportista_placa_numero),
                          new JProperty("sunat_envio_indicador", objGuiaElectronica.sunat_envio_indicador),
                          new JProperty("punto_de_partida_ubigeo", objGuiaElectronica.punto_de_partida_ubigeo),
                          new JProperty("punto_de_partida_direccion", objGuiaElectronica.punto_de_partida_direccion),
                          new JProperty("punto_de_partida_codigo_establecimiento_sunat", objGuiaElectronica.punto_de_partida_codigo_establecimiento_sunat),
                          new JProperty("punto_de_llegada_ubigeo", objGuiaElectronica.punto_de_llegada_ubigeo),
                          new JProperty("punto_de_llegada_direccion", objGuiaElectronica.punto_de_llegada_direccion),
                          new JProperty("punto_de_llegada_codigo_establecimiento_sunat", objGuiaElectronica.punto_de_llegada_codigo_establecimiento_sunat),
                          new JProperty("enviar_automaticamente_al_cliente", objGuiaElectronica.enviar_automaticamente_al_cliente),
                          new JProperty("formato_de_pdf", objGuiaElectronica.formato_de_pdf),
                          new JProperty("items", new JArray(from p in detalleGuia
                                                            select new JObject(new JProperty("unidad_de_medida", p.unidad_de_medida),
                                                            new JProperty("codigo", p.codigo),
                                                            new JProperty("descripcion", caracteres_guia(p.descripcion)),
                                                            new JProperty("cantidad", p.cantidad)))));

                }

                string NomDocumento = objGuiaElectronica.serie + "-" + objGuiaElectronica.numero;

                string _rss = rss.ToString().Replace("null,", "");
                _rss = _rss.Replace("null", "");
                _rss = _rss.Replace("        " + "\r\n", "");
                // Dim bytesDatos = System.Text.Encoding.UTF8.GetBytes(_rss)
                // Dim plainTextBytes As String = Convert.ToBase64String(bytesDatos)
                respuesta = guardarArchivoJson(NomDocumento, objGuiaElectronica.fecha_de_emision.Substring(6, 4) + objGuiaElectronica.fecha_de_emision.Substring(3, 2), _rss); // rss.ToString())
            }
            catch (Exception ex)
            {
                util.Escribir_Log("GeneraEstructuraGuia," + ex.ToString());
            }

            return respuesta;
        }

        private string guardarArchivoJson(string NomDocumento, string fechaEmision, string jsonDocumentos)
        {
            string ruta = rutaArcvhivos;
            var folderArchivos = ruta + @"\GuiasJson\" + fechaEmision;
            if (File.Exists(folderArchivos) == false)
            {
                Directory.CreateDirectory(folderArchivos + @"\enviados\");
                Directory.CreateDirectory(folderArchivos + @"\pendientes\");
                Directory.CreateDirectory(folderArchivos + @"\rechazados\");
            }

            string respuestaDeEnvio = "";

            //Descomentar para el envio
            string x_res = enviarJson(NomDocumento + ".json", jsonDocumentos.ToString());
            respuestaDeEnvio = x_res;

            var r = JsonConvert.DeserializeObject<Respuesta>(respuestaDeEnvio);
            var json_r_in = JsonConvert.SerializeObject(r, Formatting.Indented);
            JObject jsonObject = JObject.Parse(json_r_in);

            try
            {
                util.Escribir_Log("RespuestaEnvio:" + json_r_in.ToString());
            }
            catch (Exception ex)
            {
                util.Escribir_Log("Error_RespuestaEnvio," + ex.ToString());
            }

            // Acceder a los valores utilizando la clase JToken
            respuestaDeEnvio = jsonObject["nota_importante"].ToObject<string>();
            if (respuestaDeEnvio == null)
                respuestaDeEnvio = "";

            //string x_res = "Simulación de envío - ok";
            //respuestaDeEnvio = "https://www.nubefact.com/guia/05578718-6570-4a59-9f45-1f12a97628a0.pdf";

            if (respuestaDeEnvio != "")
            {
                folderArchivos = folderArchivos + @"\enviados\";
                Thread.Sleep(2000);
                string[] doc = NomDocumento.Split('-');
                respuestaDeEnvio = consultarGuiaElectronica(doc[0], doc[1]);
            }
            else
            {
                folderArchivos = folderArchivos + @"\pendientes\";
                respuestaDeEnvio = "La guía " + NomDocumento + " no ha podido ser enviado:" + x_res;
            }

            try
            {
                StreamWriter oSW = new StreamWriter(folderArchivos + NomDocumento + ".json");
                oSW.WriteLine(jsonDocumentos);
                oSW.Flush();
                oSW.Close();
            }
            catch (Exception ex)
            {
                //escribirMensajeGuias(ex.ToString());        
                util.Escribir_Log("guardarArchivoJson," + ex.ToString());
            }


            return respuestaDeEnvio;
        }

        private string enviarJson(string documento, string documento_json)
        {
            string respuesta = "";

            try
            {
                Uri uri = new Uri(rutaGuia);
                HttpWebRequest httpWebRequests = (HttpWebRequest)WebRequest.Create(rutaGuia);
                httpWebRequests.Headers.Add("Authorization", "Token token=" + tokenGuia);
                httpWebRequests.ContentType = "application/json";
                httpWebRequests.Method = WebRequestMethods.Http.Post;
                byte[] arrData = Encoding.UTF8.GetBytes(documento_json);
                httpWebRequests.ContentLength = arrData.Length;
                httpWebRequests.Expect = "application/json";

                using (var dataStream = httpWebRequests.GetRequestStream())
                {
                    dataStream.Write(arrData, 0, arrData.Length);
                }

                var response = (HttpWebResponse)httpWebRequests.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                respuesta = reader.ReadToEnd();

            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    respuesta = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                util.Escribir_Log("enviarJson," + ex.ToString());
                respuesta = "error en enviarJson";//return new StreamReader(ex.Response.GetResponseStream).ReadToEnd;
            }


            return respuesta;
        }

        private string caracteres_guia(string cadena)
        {
            string ncadena = cadena.ToUpper();
            ncadena = ncadena.Replace("¡", "Â¡");
            ncadena = ncadena.Replace("¢", "Â¢");
            ncadena = ncadena.Replace("£", "Â£");
            ncadena = ncadena.Replace("¤", "Â¤");
            ncadena = ncadena.Replace("¥", "Â¥");
            ncadena = ncadena.Replace("¦", "Â¦");
            ncadena = ncadena.Replace("§", "Â§");
            ncadena = ncadena.Replace("¨", "Â¨");
            ncadena = ncadena.Replace("©", "Â©");
            ncadena = ncadena.Replace("ª", "Âª");
            ncadena = ncadena.Replace("«", "Â«");
            ncadena = ncadena.Replace("¬", "Â¬");
            ncadena = ncadena.Replace("­", "Â­");
            ncadena = ncadena.Replace("®", "Â®");
            ncadena = ncadena.Replace("¯", "Â¯");
            ncadena = ncadena.Replace("°", "Â°");
            ncadena = ncadena.Replace("±", "Â±");
            ncadena = ncadena.Replace("²", "Â²");
            ncadena = ncadena.Replace("³", "Â³");
            ncadena = ncadena.Replace("´", "Â´");
            ncadena = ncadena.Replace("µ", "Âµ");
            ncadena = ncadena.Replace("¶", "Â¶");
            ncadena = ncadena.Replace("·", "Â·");
            ncadena = ncadena.Replace("¸", "Â¸");
            ncadena = ncadena.Replace("¹", "Â¹");
            ncadena = ncadena.Replace("º", "Âº");
            ncadena = ncadena.Replace("»", "Â»");
            ncadena = ncadena.Replace("¼", "Â¼");
            ncadena = ncadena.Replace("½", "Â½");
            ncadena = ncadena.Replace("¾", "Â¾");
            ncadena = ncadena.Replace("¿", "Â¿");
            ncadena = ncadena.Replace("À", "Ã€");
            ncadena = ncadena.Replace("Á", "A");
            ncadena = ncadena.Replace("Â", "A");
            ncadena = ncadena.Replace("Ã", "A");
            ncadena = ncadena.Replace("Ä", "A");
            ncadena = ncadena.Replace("Å", "A");
            ncadena = ncadena.Replace("Æ", "Ã†");
            ncadena = ncadena.Replace("Ç", "Ã‡");
            ncadena = ncadena.Replace("È", "E");
            ncadena = ncadena.Replace("É", "E");
            ncadena = ncadena.Replace("Ê", "E");
            ncadena = ncadena.Replace("Ë", "E");
            ncadena = ncadena.Replace("Ì", "I");
            ncadena = ncadena.Replace("Í", "Ã");
            ncadena = ncadena.Replace("Î", "I");
            ncadena = ncadena.Replace("Ï", "I");
            ncadena = ncadena.Replace("Ð", "Ã");
            ncadena = ncadena.Replace("Ñ", "Ã‘");
            ncadena = ncadena.Replace("Ò", "O");
            ncadena = ncadena.Replace("Ó", "O");
            ncadena = ncadena.Replace("Ô", "O");
            ncadena = ncadena.Replace("Õ", "O");
            ncadena = ncadena.Replace("Ö", "O");
            ncadena = ncadena.Replace("×", "Ã—");
            ncadena = ncadena.Replace("Ø", "Ã˜");
            ncadena = ncadena.Replace("Ù", "U");
            ncadena = ncadena.Replace("Ú", "U");
            ncadena = ncadena.Replace("Û", "U");
            ncadena = ncadena.Replace("Ü", "U");
            ncadena = ncadena.Replace("Ý", "Ã");
            ncadena = ncadena.Replace("Þ", "Ãž");
            ncadena = ncadena.Replace("ß", "ÃŸ");
            ncadena = ncadena.Replace("à", "a");
            ncadena = ncadena.Replace("á", "a");
            ncadena = ncadena.Replace("â", "a");
            ncadena = ncadena.Replace("ã", "a");
            ncadena = ncadena.Replace("ä", "a");
            ncadena = ncadena.Replace("å", "a");
            ncadena = ncadena.Replace("æ", "Ã¦");
            ncadena = ncadena.Replace("ç", "Ã§");
            ncadena = ncadena.Replace("è", "e");
            ncadena = ncadena.Replace("é", "e");
            ncadena = ncadena.Replace("ê", "e");
            ncadena = ncadena.Replace("ë", "e");
            ncadena = ncadena.Replace("ì", "i");
            ncadena = ncadena.Replace("í", "Ã­");
            ncadena = ncadena.Replace("î", "i");
            ncadena = ncadena.Replace("ï", "i");
            ncadena = ncadena.Replace("ð", "o");
            ncadena = ncadena.Replace("ñ", "Ã±");
            ncadena = ncadena.Replace("ò", "o");
            ncadena = ncadena.Replace("ó", "o");
            ncadena = ncadena.Replace("ô", "o");
            ncadena = ncadena.Replace("õ", "o");
            ncadena = ncadena.Replace("ö", "o");
            ncadena = ncadena.Replace("÷", "Ã·");
            ncadena = ncadena.Replace("ø", "Ã¸");
            ncadena = ncadena.Replace("ù", "u");
            ncadena = ncadena.Replace("ú", "u");
            ncadena = ncadena.Replace("û", "u");
            ncadena = ncadena.Replace("ü", "u");
            ncadena = ncadena.Replace("ý", "Ã½");
            ncadena = ncadena.Replace("þ", "Ã¾");
            ncadena = ncadena.Replace("ÿ", "Ã¿");
            ncadena = ncadena.Replace("°", " ");

            return ncadena;
        }

        private class Consulta
        {
            public string operacion { get; set; }
            public int tipo_de_comprobante { get; set; }
            public string serie { get; set; }
            public int numero { get; set; }
        }

        private class Respuesta
        {
            public string nota_importante { get; set; }
            public int tipo_de_comprobante { get; set; }
            public string serie { get; set; }
            public int numero { get; set; }
            public string enlace { get; set; }
            public bool aceptada_por_sunat { get; set; }
            public string sunat_description { get; set; }
            public string sunat_note { get; set; }
            public string sunat_responsecode { get; set; }
            public string sunat_soap_error { get; set; }
            public string pdf_zip_base64 { get; set; }
            public string xml_zip_base64 { get; set; }
            public string cdr_zip_base64 { get; set; }
            public string cadena_para_codigo_qr { get; set; }
            public string enlace_del_pdf { get; set; }
            public string enlace_del_xml { get; set; }
            public string enlace_del_cdr { get; set; }
        }


    }
}