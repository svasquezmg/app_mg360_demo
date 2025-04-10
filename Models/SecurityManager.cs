using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebAppMontGroup.Entity;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace WebAppMontGroup.Models
{
    public class SecurityManager
    {
        readonly string password = "MG*P455W0rd?";

        public bool validaSesion()
        {

            bool estado_session = true;
            if (System.Web.HttpContext.Current.Session["SessionUsuario"] == null)
            {
                estado_session = false;
            }
            return estado_session;
        }

        public bool validaAcceso(string access_pass)
        {
            string psw = ConfigurationManager.AppSettings["access_pass"];
            bool estado_acceso = false;
            if (psw == access_pass)
            {
                estado_acceso = true;
            }

            return estado_acceso;
        }

        public bool validaAccesoPagina(string pagina, string accionPermiso)
        {

            ModelUsuario m_usuario = new ModelUsuario();
            Usuario usuario = new Usuario();

            bool estado_permiso = false;

            usuario = System.Web.HttpContext.Current.Session["SessionUsuario"] as Usuario;
            string tipo_usuario = System.Web.HttpContext.Current.Session["TipoUsuario"].ToString();
            //if (tipo_usuario == "2")
            //{
            DataTable dtPermisoPagina = new DataTable();
            dtPermisoPagina = m_usuario.permisosPagina(usuario.usuario, "", "MG360", pagina);
            if (dtPermisoPagina.Rows.Count > 0)
            {
                if (accionPermiso == "R")
                {
                    if (dtPermisoPagina.Rows[0]["lectura"].ToString() == "1")
                    {
                        estado_permiso = true;
                    }
                }
                else if (accionPermiso == "E")
                {
                    if (dtPermisoPagina.Rows[0]["escritura"].ToString() == "1")
                    {
                        estado_permiso = true;
                    }
                }
                else if (accionPermiso == "D")
                {
                    if (dtPermisoPagina.Rows[0]["eliminacion"].ToString() == "1")
                    {
                        estado_permiso = true;
                    }
                }
            }
            //}
            /*else if (tipo_usuario == "1")
            {
                estado_permiso = true;
            }*/
            return estado_permiso;
        }

        public string Encrypt(string plainText)
        {
            if (plainText == null)
            {
                return null;
            }
            // Get the bytes of the string
            var bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            passwordBytes = SHA512.Create().ComputeHash(passwordBytes);

            var bytesEncrypted = Encrypt(bytesToBeEncrypted, passwordBytes);

            return Convert.ToBase64String(bytesEncrypted);
        }

        public string Decrypt(string encryptedText)
        {
            if (encryptedText == null)
            {
                return null;
            }
            // Get the bytes of the string
            var bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            passwordBytes = SHA512.Create().ComputeHash(passwordBytes);

            var bytesDecrypted = Decrypt(bytesToBeDecrypted, passwordBytes);

            return Encoding.UTF8.GetString(bytesDecrypted);
        }

        private byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }

                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        private byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }

                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }


        public string codUserConsulta()
        {
            string user = "'XXX'";
            string tipo_usuario = System.Web.HttpContext.Current.Session["TipoUsuario"].ToString();
            if (tipo_usuario == "1")
            {
                Usuario usuario = new Usuario();
                usuario = System.Web.HttpContext.Current.Session["SessionUsuario"] as Usuario;
                List<Usuario> lst_usuario = new List<Usuario>();
                lst_usuario = System.Web.HttpContext.Current.Session["VendedorAsociado"] as List<Usuario>;

                user = "'" + usuario.codigovendedor + "'";
                if (lst_usuario != null)
                {
                    foreach (var usr in lst_usuario)
                    {
                        user += ",'" + usr.codigovendedor + "'";
                    }
                }
            }
            else if (tipo_usuario == "2")
            {
                user = "ALL";
            }

            return user;
        }
        public bool validaAccesoUserData(string codPedido) {

            bool accesoConsulta = false;
            string tipo_usuario = System.Web.HttpContext.Current.Session["TipoUsuario"].ToString();
            var usuario = System.Web.HttpContext.Current.Session["SessionUsuario"] as Usuario;
            var lst_usuario = System.Web.HttpContext.Current.Session["VendedorAsociado"] as List<Usuario>;

            if (tipo_usuario == "1")
            {
                if (usuario.codigovendedor == codPedido)
                {
                    accesoConsulta = true;
                }
                else
                {
                    foreach (var iUsuario in lst_usuario)
                    {
                        if (iUsuario.codigovendedor == codPedido)
                        {
                            accesoConsulta = true;
                            break;
                        }
                    }
                }
            }
            else if (tipo_usuario == "2")
            {
                accesoConsulta = true;
            }

            return accesoConsulta;
        }


    }

}