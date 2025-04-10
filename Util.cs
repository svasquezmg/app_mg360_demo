using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using WebAppMontGroup.Entity;
using System.Text;

namespace WebAppMontGroup
{
    public class Util
    {

        public void Escribir_Log(string mensajeLog)
        {
            try
            {
                string rutaArcvhivos = ConfigurationManager.AppSettings["RutaArcvhivos"];

                string folderArchivos = rutaArcvhivos + @"\log_service.txt";
                if (File.Exists(folderArchivos) == false)
                {
                    File.Create(folderArchivos);
                }

                using (StreamWriter sw = File.AppendText(folderArchivos))
                {
                    sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " " + mensajeLog);
                }

            }
            catch
            {
                //Silenciosa
            }
        }

        public  string EnvioDeCorreoCpanel(string correo_destino, string asunto, string correo)
        {

            string mensaje = "1";

            MailMessage _Correo = new MailMessage();
            string _correo_salida;

            SmtpClient smtp = new SmtpClient();
            // smtp.Credentials = New NetworkCredential(_correo_salida, "mgroup")
            smtp.Host = System.Configuration.ConfigurationManager.AppSettings["HostCpanel"];
            smtp.Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PortCpanel"]);
            // smtp.Host = "smtp.gmail.com"
            // smtp.Port = 587
            smtp.EnableSsl = false; // True

            _correo_salida = System.Configuration.ConfigurationManager.AppSettings["EmailCpanel"];
            smtp.Credentials = new NetworkCredential(_correo_salida, System.Configuration.ConfigurationManager.AppSettings["PassCpanel"]);

            _Correo.From = new MailAddress(_correo_salida, "Notificaciones Mont Group", System.Text.Encoding.UTF8);
            _Correo.To.Add(correo_destino);
            _Correo.Subject = asunto;
            _Correo.Body = DecodeBase64(correo);
            _Correo.IsBodyHtml = true;
            _Correo.Priority = MailPriority.Normal;

            try
            {
                ServicePointManager.ServerCertificateValidationCallback = (object s, X509Certificate certificate, X509Chain chai, SslPolicyErrors sslPolicyErrors) => true;
                smtp.Send(_Correo);
            }
            catch (Exception ex)
            {
                Escribir_Log(ex.ToString());
                //mensaje = ex.ToString();
                mensaje = "0";
            }
            //string mensajes = mensaje;
            _Correo.Dispose();

            return mensaje;
        }


        public  string  DecodeBase64( string value)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(value);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        

        }

        public  string EncodeBase64( string value)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(value);
            return System.Convert.ToBase64String(plainTextBytes);
        }



    }
}