using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppMontGroup.Entity;
using WebAppMontGroup.Models;

namespace WebAppMontGroup.Controllers
{
    public class PedidoAprController : Controller
    {
        SecurityManager security_manager = new SecurityManager();



        /* Vista Aprobacion Precio */
        public ActionResult AprobacionPrecio()
        {
            if (!security_manager.validaAccesoPagina("PedidoApr/AprobacionPrecio", "R"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        /* Vista Aprobacion Credito */
        public ActionResult AprobacionCredito()
        {
            if (!security_manager.validaAccesoPagina("PedidoApr/AprobacionCredito", "R"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        /* Sirve para listar el pedido general */
        [HttpGet]
        public JsonResult listaPedidosApr(int? anio, int? mes, string estado, string responsable, string tipo)
        {
            ModelPedidoApr model_pedido_apr = new ModelPedidoApr();

            // Verificar si la sesión es válida
            if (!security_manager.validaSesion())
            {
                return Json(new { error = "Sesión no válida" }, JsonRequestBehavior.AllowGet);
            }

            var pedidos = model_pedido_apr.listaPedido(anio ?? DateTime.Now.Year, mes ?? DateTime.Now.Month, estado, responsable, tipo);

            if (pedidos != null && pedidos.Any())
            {
                return Json(new
                {
                    idPedido = pedidos.Select(p => p.IdPedido),
                    pedido = pedidos.Select(p => p.CodigoPedido), // Agregar pedido
                    fechaRegistro = pedidos.Select(p => p.FechaRegistro.ToString("yyyy-MM-dd")),
                    coa = pedidos.Select(p => p.Coa),
                    cliente = pedidos.Select(p => p.Cliente),
                    tipoPago = pedidos.Select(p => p.CodigoTipoPago), // Agregar tipo de pago
                    vendedor = pedidos.Select(p => p.Vendedor), // Agregar vendedor
                    FechaEntrega = pedidos.Select(p => p.FechaEntrega.ToString("yyyy-MM-dd")),
                    codCategoriaCliente = pedidos.Select(p => p.CodCategoriaCliente),
                    total = pedidos.Select(p => p.Total)
                }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new { error = "No se encontraron pedidos" }, JsonRequestBehavior.AllowGet);
            }
        }


        /* Traer mas detalle del pedido para los labels del pedido */
        public ActionResult ObtenerPedidoPorId(int idPedido, string responsable, string estado, string proviene)
        {
            try
            {
                ModelPedido m_pedido = new ModelPedido();
                ModelPedidoApr m_pedidoapr = new ModelPedidoApr();
                var pedido = m_pedido.PedidoPorId(idPedido);
                var pedido_detapr = m_pedidoapr.listaPedidoDetalleApr(idPedido, responsable, estado);
                /* var pedido_aprobaciones = m_pedidoapr.listaPedidoApr(idPedido, "PRO"); */
                var pedido_log = m_pedidoapr.lista_Pedido_log(idPedido, "", proviene);

                if (pedido == null)
                {
                    return Json(new { success = false, message = "Pedido no encontrado" }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    success = true,
                    idPedido = pedido.IdPedido,
                    codigoPedido = pedido.CodigoPedido,
                    fechaRegistro = pedido.FechaRegistro.ToString("yyyy-MM-dd"),
                    coa = pedido.Coa,
                    cliente = pedido.Cliente,
                    tipoPago = pedido.NombreTipoPago,
                    vendedor = pedido.Vendedor,
                    fechaEntrega = pedido.FechaEntrega.ToString("yyyy-MM-dd") ?? "",
                    total = pedido.Total,
                    codCategoriaCliente = pedido.CodCategoriaCliente,
                    observacionPrecio = pedido.ObservacionPrecio,
                    observacionCredito = pedido.ObservacionCredito,
                    seguimientoCredito = pedido.SeguimientoCredito,
                    pedidoDetalle = pedido_detapr.Select(d => new
                    {
                        idPedidoDetalle = d.IdPedidoDetalle,
                        codigo = d.Articulo,
                        articulo = d.ArticuloDesc,
                        cantidad = d.Cantidad,
                        precio = d.Precio,
                        subTotal = d.SubTotal,
                        descuento = d.Descuento,
                        responsable = d.UsuarioResponsable,
                        decision = d.Decision,
                        observacion = d.Observacion
                    }).ToList(),

                    pedidoLog = pedido_log.Select(log => new
                    {
                        accion = log.Accion,
                        usuario = log.Usuario,
                        fecha = log.Fecha.ToString("MMM dd, HH:mm"),
                        obs = log.Obs

                    }).ToList()
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        /* Para validar lo de aprobaciones y guardar tambien en el pedido log*/
        [HttpPost]
        public JsonResult crudAprobar(Pedido pedido)
        {
            bool acceso = false;
            Usuario user = null;

            try
            {
                // Validar acceso para la página "Pedido/Aprobacion"
                if (security_manager.validaAcceso("") || security_manager.validaAccesoPagina("PedidoApr/Aprobacion", "E"))
                {
                    acceso = true;
                    user = (Usuario)Session["SessionUsuario"];
                    pedido.UsuarioActualizacion = user?.usuario;
                }

                if (!acceso)
                {
                    return Json(new { success = false, message = "Acceso denegado." });
                }
            }
            catch
            {
                return Json(new { success = false, message = "Error al validar el acceso." });
            }

            if (!security_manager.validaSesion())
            {
                return Json(new { success = false, message = "Sesión no válida." });
            }

            try
            {
                ModelPedidoApr model = new ModelPedidoApr();

                // Insertar aprobación
                int result = model.insert_Aprobacion(pedido);

                // Registrar log del pedido
                int result2 = model.insert_Pedido_Log(pedido);

                if (result > 0 && result2 > 0)
                {
                    return Json(new { success = true, message = "Pedido aprobado correctamente." });
                }
                else
                {
                    return Json(new { success = false, message = "Error al aprobar el pedido." });
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        [HttpGet]
        public JsonResult listaPromocionesTodos()
        {
            ModelPromocion model_promocion = new ModelPromocion();

            // Verificar si la sesión es válida
            if (!security_manager.validaSesion())
            {
                return Json(new { error = "Sesión no válida" }, JsonRequestBehavior.AllowGet);
            }

            var promociones = model_promocion.listaPreciosPromocionesGeneral();

            if (promociones != null && promociones.Any())
            {
                return Json(new
                {
                    idpromo = promociones.Select(p => p.idPromocion),
                    tipoPromocion = promociones.Select(p => p.tipoPromocion), // Solo mandamos los tipos de promoción
                    categoriaCliente = promociones.Select(p => p.categoriaCliente),
                    codigoProducto = promociones.Select(p => p.codigoProducto),
                    descripcionProducto = promociones.Select(p => p.descripcionProducto),
                    bonificacion = promociones.Select(p => p.bonificacion),
                    cantidad_desde = promociones.Select(p => p.cantidad_desde),
                    cantidad_hasta = promociones.Select(p => p.cantidad_hasta),
                    fechaInicio = promociones.Select(p => p.fechaInicio.ToString("yyyy-MM-dd")),
                    fechaFin = promociones.Select(p => p.fechaFin.ToString("yyyy-MM-dd"))
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { error = "No se encontró una promoción válida" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult crudPromociones(Promocion promocion)
        {
            ModelPromocion model_promocion = new ModelPromocion();
            ModelGeneral m_auditoria = new ModelGeneral();
            string descripcion = "";
            var usuario = Session["SessionUsuario"] as Usuario;
            // Aquí, capturamos el ID del usuario logueado
            int usuarioId = usuario.idusuario; // Suponiendo que la propiedad del ID se llama "idusuario"

            if (promocion == null || string.IsNullOrEmpty(promocion.accion))
            {
                return Json(new { success = false, message = "Los datos de la promoción son inválidos o falta la acción." });
            }

            try
            {
                int result = model_promocion.crud_promocion(promocion);

                if (result > 0)
                {
                    // Descripción personalizada para la auditoría
                    descripcion = $"Se realizó la acción '{promocion.accion}' en la promoción con título '{promocion.tipoPromocion}' y fecha de inicio '{promocion.fechaInicio}'.";

                    // Guardar auditoría con la acción específica
                    int resultAuditoria = m_auditoria.LOG_AUDITORIA("Formulario Promoción", promocion.accion, usuarioId, descripcion);

                    return Json(new { success = true, message = "Promoción procesada correctamente." });
                }
                else
                {
                    return Json(new { success = false, message = "Error al procesar la promoción." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        /*  PROGRAMAR SEGUIR */
        /* [HttpPost]
        public JsonResult crudPreciosFijos(ProductoPrecio precioxcategoria)
        {
            
        } */




        // GET: PedidoApr
        public ActionResult Index()
        {
            return View();
        }

        // GET: PedidoApr/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PedidoApr/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PedidoApr/Create
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

        // GET: PedidoApr/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PedidoApr/Edit/5
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

        // GET: PedidoApr/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PedidoApr/Delete/5
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
