using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SaintForest.Models;

namespace SaintForest.Controllers
{
    public class DetallesPedidosController : Controller
    {
        private EntidadesSaintForest db = new EntidadesSaintForest();

        //
        // GET: /DetallesPedidos/

        public ActionResult Index(int? Id)
        {
            var detallepedido = db.DetallePedido.Where(dt => dt.IdPedido == Id).Include(d => d.Pedidos).Include(d => d.Productos);
            return View(detallepedido.ToList());
        }

        //
        // GET: /DetallesPedidos/Details/5

        public ActionResult Details(int id = 0)
        {
            DetallePedido detallepedido = db.DetallePedido.Find(id);
            if (detallepedido == null)
            {
                return HttpNotFound();
            }
            return View(detallepedido);
        }

        //
        // GET: /DetallesPedidos/Create

        public ActionResult Create(int Id)
        {
            ViewBag.Id = Id;
            ViewBag.IdPedido = new SelectList(db.Pedidos, "Id", "Id");
            ViewBag.IdProducto = new SelectList(db.Productos, "Id", "Nombre");
            return View();
        }

        //
        // POST: /DetallesPedidos/Create

        [HttpPost]
        public ActionResult Create(DetallePedido detallepedido)
        {
            if (ModelState.IsValid)
            {
                var Producto = db.Productos.Find(detallepedido.IdProducto);
                decimal Iva = 0, Ico = 0, TotalIco = 0, TotalIva = 0, ValorTotal = 0;
                int Cantidad = 0;
                Cantidad = detallepedido.Cantidad;
                Iva = DivisionSobre100((decimal)Producto.Iva * Producto.Valor);
                Ico = DivisionSobre100((decimal)Producto.Ico * Producto.Valor);
                TotalIco = (decimal)(Ico * Cantidad);
                TotalIva = (decimal)(Iva * Cantidad);
                ValorTotal = (decimal)(TotalIco + TotalIva + (Producto.Valor * Cantidad));
                detallepedido.Ico = Ico;
                detallepedido.Iva = Iva;
                detallepedido.TotalIco = TotalIco;
                detallepedido.TotalIva = TotalIva;
                detallepedido.ValorUnitario = Producto.Valor;
                detallepedido.ValorTotal = ValorTotal;

                var PedidoActualizar = db.Pedidos.Find(detallepedido.IdPedido);
                PedidoActualizar.ValorTotal = (ActualizarValorDelPedido(detallepedido.IdPedido)+ValorTotal);
               

                db.Entry(PedidoActualizar).State = EntityState.Modified;
               
                db.DetallePedido.Add(detallepedido);
                db.SaveChanges();
                return RedirectToAction("../Pedidos/Index");
            }

            ViewBag.IdPedido = new SelectList(db.Pedidos, "Id", "Id", detallepedido.IdPedido);
            ViewBag.IdProducto = new SelectList(db.Productos, "Id", "Nombre", detallepedido.IdProducto);
            return View(detallepedido);
        }
        //para El calculo del IVA e ICO
        public decimal DivisionSobre100(decimal Numero)
        {
            if (Numero == 0)
            {
                return 0;
            }
            else
            {
                return Numero / 100;
            }
        }
        public decimal ActualizarValorDelPedido(int IdPedido)
        {
            //EntidadesSaintForest db = new EntidadesSaintForest();

            var Detalles = from detallePedido in db.DetallePedido
                           where detallePedido.IdPedido==IdPedido
                           select new { detallePedido.ValorTotal };

            decimal ValorTotal = 0;
            var Sum = Detalles.ToList().Select(c=>c.ValorTotal).Sum();
            ValorTotal = (decimal) Sum;

            return ValorTotal;


        }
        //
        // GET: /DetallesPedidos/Edit/5

        public ActionResult Edit(int id = 0)
        {
            DetallePedido detallepedido = db.DetallePedido.Find(id);
            if (detallepedido == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdPedido = new SelectList(db.Pedidos, "Id", "Id", detallepedido.IdPedido);
            ViewBag.IdProducto = new SelectList(db.Productos, "Id", "Nombre", detallepedido.IdProducto);
            return View(detallepedido);
        }

        //
        // POST: /DetallesPedidos/Edit/5

        [HttpPost]
        public ActionResult Edit(DetallePedido detallepedido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detallepedido).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdPedido = new SelectList(db.Pedidos, "Id", "Id", detallepedido.IdPedido);
            ViewBag.IdProducto = new SelectList(db.Productos, "Id", "Nombre", detallepedido.IdProducto);
            return View(detallepedido);
        }

        //
        // GET: /DetallesPedidos/Delete/5

        public ActionResult Delete(int id = 0)
        {
            DetallePedido detallepedido = db.DetallePedido.Find(id);
            if (detallepedido == null)
            {
                return HttpNotFound();
            }
            return View(detallepedido);
        }

        //
        // POST: /DetallesPedidos/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            DetallePedido detallepedido = db.DetallePedido.Find(id);
            db.DetallePedido.Remove(detallepedido);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}