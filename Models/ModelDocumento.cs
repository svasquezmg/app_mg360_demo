using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using WebAppMontGroup.Entity;
namespace WebAppMontGroup.Models
{
    public class ModelDocumento
    {

        public DataTable getDocumento(string documento, string usr)
        {

            DataTable dt = new DataTable();

            WebServiceEasy.Service_EasySoapClient Service_Easy = new WebServiceEasy.Service_EasySoapClient();

            string x_usuario = WebConfigurationManager.AppSettings["uWebServ"];
            string x_password = WebConfigurationManager.AppSettings["pWebServ"];

            DataTable dtCliente = new DataTable();
            dtCliente.TableName = "dtClientes";
            dt = Service_Easy.documento(x_usuario, x_password, documento,usr);

            return dt;
        }

        public DataTable getDetalleDocumento(string documento, string yymm, string usr)
        {

            DataTable dt = new DataTable();

            WebServiceEasy.Service_EasySoapClient Service_Easy = new WebServiceEasy.Service_EasySoapClient();

            string x_usuario = WebConfigurationManager.AppSettings["uWebServ"];
            string x_password = WebConfigurationManager.AppSettings["pWebServ"];

            //   DataTable dtCliente = new DataTable();
            //   dtCliente.TableName = "dtClientes";
            dt = Service_Easy.documentoDetalle(x_usuario, x_password, documento, yymm, usr);

            return dt;
        }

        public DataTable getDocumentoPlanilla(string documento)
        {
            //getDataxPlanillaOperacionDocumento
            DataTable dt = new DataTable();

            WebServiceEasy.Service_EasySoapClient Service_Easy = new WebServiceEasy.Service_EasySoapClient();

            string x_usuario = WebConfigurationManager.AppSettings["uWebServ"];
            string x_password = WebConfigurationManager.AppSettings["pWebServ"];

            //   DataTable dtCliente = new DataTable();
            //   dtCliente.TableName = "dtClientes";
            dt = Service_Easy.getDataxPlanillaOperacionDocumento(x_usuario, x_password, "", "", "", documento);

            return dt;
        }


        public DataTable Vendedores()
        {
            DataTable dt = new DataTable();
            WebServiceEasy.Service_EasySoapClient Service_Easy = new WebServiceEasy.Service_EasySoapClient();

            string x_usuario = WebConfigurationManager.AppSettings["uWebServ"];
            string x_password = WebConfigurationManager.AppSettings["pWebServ"];

            dt = Service_Easy.vendedor(x_usuario, x_password);
            return dt;
        }
        public DataTable GetDocumentosFiltrados(string fechaInicio, string fechaFin, string tipoDocumento,
                           string serie, string vendedor, string cliente,
                           string cancelado, string anulado, string articulo, string lote, string usr)
        {
            DataTable dt = new DataTable();
            try
            {
                WebServiceEasy.Service_EasySoapClient Service_Easy = new WebServiceEasy.Service_EasySoapClient();

                // Obtener usuario y contraseña del WebService
                string x_usuario = WebConfigurationManager.AppSettings["uWebServ"];
                string x_password = WebConfigurationManager.AppSettings["pWebServ"];


                DateTime fechaInicioParsed = Convert.ToDateTime(fechaInicio);
                DateTime fechaFinParsed = Convert.ToDateTime(fechaFin);

                //// Validar fechaInicio
                //if (!DateTime.TryParseExact(fechaInicio, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaInicioParsed))
                //{
                //    // Si la fecha de inicio no es válida, asignar la fecha de hoy menos un año
                //    fechaInicioParsed = DateTime.Now.AddYears(-1);
                //}

                //// Validar fechaFin
                //if (!DateTime.TryParseExact(fechaFin, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaFinParsed))
                //{
                //    // Si la fecha de fin no es válida, asignar la fecha de hoy
                //    fechaFinParsed = DateTime.Now;
                //}

                // Si tipoDocumento ya es un string, no necesitas hacer un Join
                string tipoDoc = tipoDocumento ?? "";
                string codVendedor = vendedor ?? "";
                string codCliente = cliente ?? "";

                // Verificar los campos Cancelado y Anulado
                string canceladoFormat = cancelado ?? "";
                string anuladoFormat = anulado ?? "";

                // Llamar al servicio SOAP con los filtros formateados
                dt = Service_Easy.documentos(
                    x_usuario,
                    x_password,
                    Convert.ToDateTime(fechaInicioParsed),  // Fecha_Inicio en formato DateTime
                    Convert.ToDateTime(fechaFinParsed),     // Fecha_Fin en formato DateTime
                    tipoDoc,            // TipoDoc: (vacío para todos)s
                    serie,
                    codVendedor,        // Cod_Vendedor
                    codCliente,         // Cod_Cliente
                    canceladoFormat,    // Cancelado = "" (todos), "C", " "
                    anuladoFormat,      // Anulado = "" (todos), "=", "<>"
                     "",               // Excluir_Cod_Vendedor (si aplica)
                     "",               // Errado = "" (todos), "=", "<>"
                     "",               // conGuia = ""
                     usr
                );

            }
            catch (Exception ex)
            {
                // Manejar la excepción y registrar el error para futura referencia.
                Console.WriteLine($"Error al obtener documentos filtrados en GetDocumentosFiltrados-Model: {ex.Message}");
                // Opcionalmente, puedes lanzar la excepción para que sea manejada en otro nivel
            }

            return dt;

        }

        public DataTable GetDocumentosDetalleFiltrados(string fechaInicio, string fechaFin, string tipoDocumento,
                           string serie, string vendedor, string cliente,
                           string cancelado, string anulado, string articulo, string lote, string usr)
        {
            DataTable dt = new DataTable();
            try
            {
                WebServiceEasy.Service_EasySoapClient Service_Easy = new WebServiceEasy.Service_EasySoapClient();

                string x_usuario = WebConfigurationManager.AppSettings["uWebServ"];
                string x_password = WebConfigurationManager.AppSettings["pWebServ"];
                DateTime fechaInicioParsed = Convert.ToDateTime(fechaInicio);
                DateTime fechaFinParsed = Convert.ToDateTime(fechaFin);


                //if (!DateTime.TryParseExact(fechaInicio, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaInicioParsed) || fechaInicio is null)
                //{
                //    // Si la fecha de inicio no es válida, asignar la fecha de hoy menos un año
                //    fechaInicioParsed = DateTime.Now.AddYears(-1);
                //}

                //// Validar fechaFin
                //if (!DateTime.TryParseExact(fechaFin, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaFinParsed) || fechaFin is null)
                //{
                //    // Si la fecha de fin no es válida, asignar la fecha de hoy
                //    fechaFinParsed = DateTime.Now;
                //}

                // Si tipoDocumento ya es un string, no necesitas hacer un Join
                string tipoDoc = tipoDocumento ?? "";
                string codVendedor = vendedor ?? "";
                string codCliente = cliente ?? "";

                // Verificar los campos Cancelado y Anulado
                string canceladoFormat = cancelado ?? "";
                string anuladoFormat = anulado ?? "";
                string articuloFormat = articulo ?? "";
                string LoteFormat = lote ?? "";

                // Llamar al servicio SOAP con los filtros formateados
                dt = Service_Easy.documentos_detalle(
                    x_usuario,
                    x_password,
                    Convert.ToDateTime(fechaInicioParsed),  // Fecha_Inicio en formato DateTime
                    Convert.ToDateTime(fechaFinParsed),     // Fecha_Fin en formato DateTime
                    tipoDoc,            // TipoDoc: (vacío para todos)s
                    serie,
                    codVendedor,        // Cod_Vendedor
                    codCliente,         // Cod_Cliente
                    anuladoFormat,      // Anulado = "" (todos), "=", "<>"
                    articuloFormat,
                    LoteFormat,
                    "",                 // Excluir_Cod_Vendedor (si aplica)
                    "SI",                  // conGuia = "SI"
                     usr
                );

            }
            catch (Exception ex)
            {
                // Manejar la excepción y registrar el error para futura referencia.
                Console.WriteLine($"Error al obtener documentos filtrados en GetDocumentosFiltrados-Model: {ex.Message}");
                // Opcionalmente, puedes lanzar la excepción para que sea manejada en otro nivel
            }

            return dt;

        }

    }
}