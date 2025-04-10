using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebAppMontGroup.Entity;
using WebAppMontGroup.Models;

namespace WebAppMontGroup.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        //[HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {

            string imput_tipo = "";
            string imput_user = "";
            string imput_password = "";
            try
            {

                if (formCollection.Count > 0)
                {

                    Models.SecurityManager security_manager = new Models.SecurityManager();

                    if (security_manager.validaSesion() == false)
                    {
                        imput_tipo = formCollection["imput_tipo"].ToString();
                        imput_user = formCollection["imput_user"].ToString();
                        imput_password = formCollection["imput_password"].ToString();

                        ModelUsuario model_usuario = new ModelUsuario();
                        Usuario usuario = new Usuario();

                        if (imput_tipo == "1")
                        {
                            //string xx = security_manager.Decrypt("jSDCF5EN64DYZ8LC4HwuJA==");
                            //string te = security_manager.Encrypt("Clave124*");
                            string pwd_cifrado = security_manager.Encrypt(imput_password);
                           
                            usuario = model_usuario.loginRV("LGRV", imput_user, pwd_cifrado);

                            if (usuario.codigovendedor.ToString() == "")
                            {
                                string res_ad = model_usuario.validarUsuarioAD(imput_user, imput_password);
                                if (res_ad == "TRUE")
                                {
                                    usuario = model_usuario.loginRV("ACT-DIREC", imput_user, pwd_cifrado);
                                }
                            }

                            if (usuario.codigovendedor.ToString() != "")
                            {

                                DataTable dt_permisos = new DataTable();
                                dt_permisos = model_usuario.permisos(imput_user, "", "MG360");

                                var varMenu = dt_permisos.AsEnumerable()
                                 .Select(row => new
                                 {
                                     menu = row.Field<string>("MenuPrincipal")
                                 })
                                 .Distinct();

                                var varMenuBotones = dt_permisos.AsEnumerable()
                                 .Where(row => row.Field<string>("MenuBotonVisible") == "1")
                                 .Select(row => new
                                 {
                                     menuBoton = row.Field<string>("MenuPrincipal")
                                 })
                                 .Distinct();

                                string menus = "";
                                string menuBotones = "";
                                foreach (var m in varMenu)
                                {
                                    menus += m.menu.ToString() + ",";
                                }
                                foreach (var m in varMenuBotones)
                                {
                                    menuBotones += m.menuBoton.ToString() + ",";
                                }

                                List<Usuario> lista_vendedor_asociado = new List<Usuario>();
                                lista_vendedor_asociado = model_usuario.listarVendedorAsociado(usuario.codigovendedor);


                                System.Web.HttpContext.Current.Session["VendedorAsociado"] = lista_vendedor_asociado;
                                System.Web.HttpContext.Current.Session["SessionUsuario"] = usuario;
                                System.Web.HttpContext.Current.Session["TipoUsuario"] = imput_tipo;
                                System.Web.HttpContext.Current.Session["SessionUsuarioMenu"] = menus.TrimEnd(',');
                                System.Web.HttpContext.Current.Session["SessionUsuarioBotones"] = menuBotones.TrimEnd(',');
                                System.Web.HttpContext.Current.Session["SessionUsuarioPermiso"] = dt_permisos;

                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                System.Web.HttpContext.Current.Session["ErrorIntentoLogin"] = "ERROR";
                                return RedirectToAction("Index", "Login");
                            }

                        }
                        else if (imput_tipo == "2")
                        {
                            usuario = model_usuario.loginAdministrativo(imput_user, imput_password);
                            if (usuario.idusuario != 0)
                            {
                                DataTable dt_permisos = new DataTable();
                                dt_permisos = model_usuario.permisos(imput_user, "", "MG360");

                                var varMenu = dt_permisos.AsEnumerable()
                                 .Select(row => new
                                 {
                                     menu = row.Field<string>("MenuPrincipal")
                                 })
                                 .Distinct();

                                var varMenuBotones = dt_permisos.AsEnumerable()
                                 .Where(row => row.Field<string>("MenuBotonVisible") == "1")
                                 .Select(row => new
                                 {
                                     menuBoton = row.Field<string>("MenuPrincipal")
                                 })
                                 .Distinct();

                                string menus = "";
                                string menuBotones = "";
                                foreach (var m in varMenu)
                                {
                                    menus += m.menu.ToString() + ",";
                                }
                                foreach (var m in varMenuBotones)
                                {
                                    menuBotones += m.menuBoton.ToString() + ",";
                                }

                                System.Web.HttpContext.Current.Session["SessionUsuario"] = usuario;
                                System.Web.HttpContext.Current.Session["TipoUsuario"] = imput_tipo;
                                System.Web.HttpContext.Current.Session["SessionUsuarioMenu"] = menus.TrimEnd(',');
                                System.Web.HttpContext.Current.Session["SessionUsuarioBotones"] = menuBotones.TrimEnd(',');
                                System.Web.HttpContext.Current.Session["SessionUsuarioPermiso"] = dt_permisos;

                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                System.Web.HttpContext.Current.Session["ErrorIntentoLogin"] = "ERROR";
                                return RedirectToAction("Index", "Login");
                            }
                        }
                        else
                        {
                            System.Web.HttpContext.Current.Session["ErrorIntentoLogin"] = "ERROR";
                            return RedirectToAction("Index", "Login");
                        }
                    }
                    ViewData["ErrorIntentoLogin"] = System.Web.HttpContext.Current.Session["ErrorIntentoLogin"] as string;
                }

            }
            catch (Exception ex)
            {
                Util u = new Util(); ;
                u.Escribir_Log(imput_user + "-" + imput_password + "-" + ex.ToString());
            }
            Session.Abandon();
            return View();
        }


        // GET: Login/Details/5
        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }


    }
}
