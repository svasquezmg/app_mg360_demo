using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebAppMontGroup.Entity;
using WebAppMontGroup.Models;

namespace WebAppMontGroup.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            Models.SecurityManager security_manager = new Models.SecurityManager();

            if (security_manager.validaSesion() == false)
            {
                return RedirectToAction("Index", "Login");
            }

            ViewData["TipoUsuario"] = Session["VendedorAsociado"] as string;

            return View();
        }
  

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //[HttpPost]
        //public ActionResult ValidaLogin(FormCollection formCollection) //string imput_tipo, string imput_user, string imput_password)
        //{
        //    Models.SecurityManager security_manager = new Models.SecurityManager();

        //    if (security_manager.validaSesion() == false)
        //    {
        //        string imput_tipo = formCollection["imput_tipo"].ToString();
        //        string imput_user = formCollection["imput_user"];
        //        string imput_password = formCollection["imput_password"];

        //        ModelUsuario model_usuario = new ModelUsuario();

        //        //string te = security_manager.Encrypt("UsuaioDemo1");
        //        string pwd_cifrado = security_manager.Encrypt(imput_password);


        //        if (imput_tipo == "1")
        //        {
        //            Usuario usuario = new Usuario();
        //            usuario = model_usuario.loginRV(imput_user, pwd_cifrado);
        //            if (usuario.codigovendedor.ToString() != "")
        //            {
        //                List<Usuario> lista_vendedor_asociado = new List<Usuario>();
        //                lista_vendedor_asociado = model_usuario.listarVendedorAsociado(usuario.codigovendedor);
        //                System.Web.HttpContext.Current.Session["VendedorAsociado"] = lista_vendedor_asociado;
        //                System.Web.HttpContext.Current.Session["SessionUsuario"] = usuario;
        //                System.Web.HttpContext.Current.Session["TipoUsuario"] = imput_tipo;

        //                return RedirectToAction("Index");
        //                //return View();
        //            }
        //            else
        //            {
        //                System.Web.HttpContext.Current.Session["ErrorIntentoLogin"] = "ERROR";
        //                return RedirectToAction("Index", "Login");
        //            }

        //        }
        //        else if (imput_tipo == "2")
        //        {
        //            System.Web.HttpContext.Current.Session["ErrorIntentoLogin"] = "ERROR";
        //            return RedirectToAction("Index", "Login");
        //        }
        //        else
        //        {
        //            System.Web.HttpContext.Current.Session["ErrorIntentoLogin"] = "ERROR";
        //            return RedirectToAction("Index", "Login");
        //        }

        //        /* List<Usuario> lista_usuario = new List<Usuario>();
        //        List<Usuario> lista_vendedor_asociado = new List<Usuario>();
        //        lista_usuario = model_usuario.listarUSuario(39);
        //        lista_vendedor_asociado = model_usuario.listarVendedorAsociado(lista_usuario[0].codigo_vendedor);

        //        System.Web.HttpContext.Current.Session["SessionUsuario"] = lista_usuario;
        //        ViewData["Usuario"] = System.Web.HttpContext.Current.Session["SessionUsuario"] as List<Usuario>;

        //        System.Web.HttpContext.Current.Session["VendedorAsociado"] = lista_vendedor_asociado;
        //        ViewData["VendedorAsociado"] = System.Web.HttpContext.Current.Session["VendedorAsociado"] as List<Usuario>;*/
        //    }

        //    return RedirectToAction("Index");
        //    //return View();
        //}

    }
}