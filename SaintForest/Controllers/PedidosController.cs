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
    public class PedidosController : Controller
    {
        private EntidadesSaintForest db = new EntidadesSaintForest();

        //
        // GET: /Pedidos/

        public ActionResult Index()
        {
            int Rol = -1;
            int IdCliente = 0;
            IEnumerable<Pedidos> pedidos;
            try
            {
                Rol = (int)Session["IdRol"];
                IdCliente = (int)Session["id"];
                ViewBag.Rol = Rol;

            }
            catch (System.InvalidCastException)
            {
                ViewBag.Rol = Rol;
            }
            catch (System.NullReferenceException)
            {
                ViewBag.Rol = Rol;
            }
            if (Rol != 1)
            {
                pedidos = db.Pedidos.Include(p => p.EstadosDelPedido).Include(p => p.Facturas).Include(p => p.SiNo).Include(p => p.Usuarios);
            }
            else
            {
                pedidos = db.Pedidos.Where(P => P.IdCliente == IdCliente).Include(p => p.EstadosDelPedido).Include(p => p.Facturas).Include(p => p.SiNo).Include(p => p.Usuarios);

            }
            return View(pedidos.ToList());
        }


        //
        // GET: /Pedidos/Details/5

        public ActionResult Details(int id = 0)
        {
            Pedidos pedidos = db.Pedidos.Find(id);

            if (pedidos == null)
            {
                return HttpNotFound();
            }
            return View(pedidos);
        }

        //
        // GET: /Pedidos/Create

        public ActionResult Create()
        {
            int IdRol = 0;
            ViewBag.IsCliente = false;
            try
            {
                IdRol = ((int)Session["IdRol"]);
                ViewBag.IsCliente = new UserModel().IsCliente(IdRol);
            }
            catch (NullReferenceException)
            {
                // return RedirectToAction("../Home/LogOut");  
            }
            catch (InvalidCastException)
            {
                //return RedirectToAction("../Home/LogOut");
            }

            ViewBag.Estado = new SelectList(db.EstadosDelPedido, "Id", "Nombre", 1);
            ViewBag.IdFactura = new SelectList(db.Facturas, "Id", "Id");
            ViewBag.Cerrado = new SelectList(db.SiNo, "Id", "Nombre", 2);
            ViewBag.IsCLiente = false;
            if (IdRol == 1)
            {
                ViewBag.IsCLiente = true;
                ViewBag.IdCliente = new SelectList(db.Usuarios, "Id", "Apellidos", (int)Session["Id"]);

            }
            else
            {
                ViewBag.IdCliente = new SelectList(db.Usuarios, "Id", "Nombres");

            }
           
            return View();
        }

        //
        // POST: /Pedidos/Create

        [HttpPost]
        public ActionResult Create(Pedidos pedidos)//Crea el pedido cuando me envia el objeto Pedido por post
        {
            
            if (ModelState.IsValid)
            {
             // Antes de crear una Facturas debes consultar si existe ya un factura para que le el pedido se guarde con esa factura

                DateTime FechaActual=new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day);
                DateTime FechaActual2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,23,59,59);
                var Facturas = db.Facturas.Where(F=>F.Cliente==pedidos.IdCliente && (F.FechaFactura>=FechaActual && F.FechaFactura<=FechaActual2));
               
                if (Facturas.Count() == 0)
                {
                   
                    Facturas Factura = new Facturas();//Creo una factura
                    Factura.FechaFactura = DateTime.Now;
                    Factura.Estado = 1;
                    Factura.Cliente = (int)pedidos.IdCliente;
                    Factura.TotalFactura = (decimal)pedidos.ValorTotal; ;
                    Factura.Cerrada = 2;
                    db.Facturas.Add(Factura);
                    db.SaveChanges();
                }
                    Facturas fac = (from F in db.Facturas
                                    orderby F.Id descending
                                    select F).FirstOrDefault();
                    pedidos.IdFactura = fac.Id;
                    ViewBag.Id = pedidos.Id;
                    pedidos.Cerrado = 2;//no esta Cerrado
                    pedidos.Estado = 1;//Pendiente
                    pedidos.FechaPedido = DateTime.Now;
                    fac.TotalFactura = ActualizarFactura(fac.Id);
                    db.Entry(fac).State = EntityState.Modified;
                    db.Pedidos.Add(pedidos);
                    db.SaveChanges();
                  
                
                return RedirectToAction("Index");
                   
            }

            ViewBag.Estado = new SelectList(db.EstadosDelPedido, "Id", "Nombre", pedidos.Estado);
            ViewBag.IdFactura = new SelectList(db.Facturas, "Id", "Id", pedidos.IdFactura);
            ViewBag.Cerrado = new SelectList(db.SiNo, "Id", "Nombre", pedidos.Cerrado);
            ViewBag.IdCliente = new SelectList(db.Usuarios, "Id", "Nombres", pedidos.IdCliente);
            return View(pedidos);
        }

        public decimal ActualizarFactura(int? IdFactura)
        {

            
            var orders = from Pedidoss in db.Pedidos
                           where Pedidoss.IdFactura == IdFactura
                           select new { Pedidoss.ValorTotal };

            decimal ValorTotalFactura = 0;
            var Sum = orders.ToList().Select(c => c.ValorTotal).Sum();
            ValorTotalFactura = (decimal)Sum;

            return ValorTotalFactura;
          
        }
      

        //
        // GET: /Pedidos/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Pedidos pedidos = db.Pedidos.Find(id);
            if (pedidos == null)
            {
                return HttpNotFound();
            }
            ViewBag.Estado = new SelectList(db.EstadosDelPedido, "Id", "Nombre", pedidos.Estado);
            ViewBag.IdFactura = new SelectList(db.Facturas, "Id", "Id", pedidos.IdFactura);
            ViewBag.Cerrado = new SelectList(db.SiNo, "Id", "Nombre", pedidos.Cerrado);
            int IdRol = 0;
            ViewBag.IsCliente = false;
            try
            {
                IdRol = ((int)Session["IdRol"]);
                ViewBag.IsCliente = new UserModel().IsCliente(IdRol);
            }
            catch (NullReferenceException)
            {
                // return RedirectToAction("../Home/LogOut");  
            }
            catch (InvalidCastException)
            {
                //return RedirectToAction("../Home/LogOut");
            }
            ViewBag.IdCliente = new SelectList(db.Usuarios, "Id", "Nombres", pedidos.IdCliente);
            return View(pedidos);
        }

        //
        // POST: /Pedidos/Edit/5

        [HttpPost]
        public ActionResult Edit(Pedidos pedidos)
        {
            if (ModelState.IsValid)
            {

                db.Entry(pedidos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Estado = new SelectList(db.EstadosDelPedido, "Id", "Nombre", pedidos.Estado);
            ViewBag.IdFactura = new SelectList(db.Facturas, "Id", "Id", pedidos.IdFactura);
            ViewBag.Cerrado = new SelectList(db.SiNo, "Id", "Nombre", pedidos.Cerrado);
            ViewBag.IdCliente = new SelectList(db.Usuarios, "Id", "Nombres", pedidos.IdCliente);
            return View(pedidos);
        }

        //
        // GET: /Pedidos/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Pedidos pedidos = db.Pedidos.Find(id);
            if (pedidos == null)
            {
                return HttpNotFound();
            }
            return View(pedidos);
        }

        //
        // POST: /Pedidos/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Pedidos pedidos = db.Pedidos.Find(id);
            db.Pedidos.Remove(pedidos);
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