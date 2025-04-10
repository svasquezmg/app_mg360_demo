using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Text;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Policy;
using iText.Layout.Element;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.Client;
using System.Security;
using static iText.Svg.SvgConstants;
using WebAppMontGroup.Entity;
using iText.Layout.Splitting;

namespace WebAppMontGroup.Models
{
    public class DataSharepoint
    {

        string siteUrl = "https://montgroupcompe.sharepoint.com/sites/DIRECION_TECNICA/";
        // Credenciales o token de autenticación
        string username = WebConfigurationManager.AppSettings["userSharepoint"];
        string password = WebConfigurationManager.AppSettings["passSharepoint"];


        public List<General> imgBase64_Producto(List<General> valores)
        {
            //string[] base64String = null;
            //List<string> listaImg64 = new List<string>();

            List<string> codigos = new List<string>();

            foreach (var cod in valores)
            {
                codigos.Add(cod.valor_1);
            }

            string campo = "CodEasy"; // Nombre del campo a filtrar

            using (ClientContext context = new ClientContext(siteUrl))
            {
                context.Credentials = new SharePointOnlineCredentials(username, GetSecureString(password));

                // Obtener la lista
                Microsoft.SharePoint.Client.List list = context.Web.Lists.GetByTitle("Directorio de Documento");

                // Crear la consulta
                string camlQuery = BuildCamlQueryWithOr(codigos.ToArray(), campo);
                CamlQuery query = new CamlQuery();
                query.ViewXml = camlQuery;

                // Ejecutar la consulta
                Microsoft.SharePoint.Client.ListItemCollection items = list.GetItems(query);
                context.Load(items);
                context.ExecuteQuery();


                string img64bString = "";
                // Procesar los resultados
                foreach (Microsoft.SharePoint.Client.ListItem item in items)
                {
                    int fila = 0;
                    img64bString = "";
                    context.Load(item, i => i.AttachmentFiles);
                    context.ExecuteQuery();

                    foreach (var attachment in item.AttachmentFiles)
                    {

                        var fileInfo = Microsoft.SharePoint.Client.File.OpenBinaryDirect(context, attachment.ServerRelativeUrl);
                        using (var memoryStream = new System.IO.MemoryStream())
                        {
                            fileInfo.Stream.CopyTo(memoryStream);
                            byte[] fileBytes = memoryStream.ToArray();

                            // Convertir a Base64
                             img64bString  = Convert.ToBase64String(fileBytes);
                            //listaImg64.Add(Convert.ToBase64String(fileBytes));
                            //Console.WriteLine($"Base64 del archivo {attachment.FileName}:");
                            //Console.WriteLine(base64String);
                        }

                        while (fila < valores.Count)
                        {
                            if (item.FieldValues["CodEasy"].ToString() == valores[fila].valor_1.ToString())
                            {
                                valores[fila].valor_3 = img64bString;
                                break;
                            }
                            fila++;
                        }

                    }
                    //string x = $"ID: {item.Id}, Título: {item["Title"]}";
                    //Console.WriteLine($"ID: {item.Id}, Título: {item["Title"]}");
                }
            }

            //base64String = listaImg64.ToArray();
            return valores;
        }


        private SecureString GetSecureString(string input)
        {
            var secureString = new SecureString();
            foreach (char c in input)
            {
                secureString.AppendChar(c);
            }
            return secureString;
        }


        private string BuildCamlQueryWithOr(string[] valores, string fieldName)
        {
            if (valores == null || valores.Length == 0)
                throw new ArgumentException("Debe proporcionar al menos un valor para filtrar.");

            // Iniciar la consulta CAML
            string caml = "<Where>";
            string caml2 = "";

            // Si hay más de un valor, usar <Or> para anidar las condiciones

            if (valores.Length > 3)
            {
                for (int i = 0; i < valores.Length; i++)
                {

                    if (i < valores.Length - 2)
                    {
                        caml += $@"
                        <Or>
                         <Eq><FieldRef Name='{fieldName}' /><Value Type='Text'>{valores[i]}</Value></Eq>";

                    }
                    else
                    {
                        caml2 += $@"
                         <Eq><FieldRef Name='{fieldName}' /><Value Type='Text'>{valores[i]}</Value></Eq>";
                    }

                }


                caml2 = $@"<Or> 
                            {caml2} 
                         </Or>";
                caml = caml + caml2;

                for (int i = 0; i < valores.Length; i++)
                {
                    if (i < valores.Length - 2)
                    {
                        caml += $@"
                              </Or>";

                    }

                }



            }
            else if (valores.Length > 1)
            {
                caml += "<Or>";
                for (int i = 0; i < valores.Length; i++)
                {
                    caml += $@"
                    <Eq>
                        <FieldRef Name='{fieldName}' />
                        <Value Type='Text'>{valores[i]}</Value>
                    </Eq>";
                    if (i == 0)
                        caml += "<Or>";
                }
                caml += "</Or></Or>";
            }
            else
            {
                // Si solo hay un valor, usar una sola condición <Eq>
                caml += $@"
                <Eq>
                    <FieldRef Name='{fieldName}' />
                    <Value Type='Text'>{valores[0]}</Value>
                </Eq>";
            }

            // Cerrar la consulta CAML
            caml += "</Where>";

            // Envolver en una vista completa
            return $@"
            <View>
                <Query>
                    {caml}
                </Query>
            </View>";
        }

        public async Task<string> imgBase64Producto(string valor)
        {

            string imgbase = "";
            string listName = "Directorio de Documento";
            string campo = "CodEasy"; // Nombre del campo a filtrar
            //string valor = "ValorABuscar";   // Valor que debe coincidir en el campo

            using (HttpClient client = new HttpClient())
            {
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                // Configurar autenticación básica
                var authToken = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{username}:{password}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

                // Construir la URL de la consulta
                string filterQuery = $"$filter={campo} eq '{valor}'";
                string requestUrl = $"{siteUrl}/_api/web/lists/getbytitle('{listName}')/items?{filterQuery}";

                try
                {
                    // Realizar la solicitud
                    HttpResponseMessage response = await client.GetAsync(requestUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Elementos encontrados: " + responseData);

                        // Procesar la respuesta JSON
                        dynamic items = Newtonsoft.Json.JsonConvert.DeserializeObject(responseData);
                        foreach (var item in items.value)
                        {
                            string x = $"ID: {item.ID}, Título: {item.Title}";
                            //Console.WriteLine($"ID: {item.ID}, Título: {item.Title}");
                        }
                    }
                    else
                    {
                        //Console.WriteLine("Error al obtener elementos: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    string msg2 = msg;
                }
            }

            return imgbase;
        }

    }
}