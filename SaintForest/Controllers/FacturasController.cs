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
    public class FacturasController : Controller
    {
        private EntidadesSaintForest db = new EntidadesSaintForest();

        //
        // GET: /Facturas/

        public ActionResult Index()
        {
            ViewBag.IsCliente = false;
            int Rol = -1;
            int IdCliente = 0;
            IEnumerable<Facturas> facturas;
            try
            {
                Rol = (int)Session["IdRol"];
                IdCliente = (int)Session["id"];
                ViewBag.Rol = Rol;
                ViewBag.IsCliente = new UserModel().IsCliente(Rol);

            }
            catch (System.InvalidCastException)
            {
                ViewBag.Rol = Rol;
            }
            catch (System.NullReferenceException)
            {
                ViewBag.Rol = Rol;
            }
            if(Rol!=1){
            facturas = db.Facturas.Include(f => f.EstadosDeLaFactura).Include(f => f.SiNo).Include(f => f.Usuarios);
         
            }else{
                facturas = db.Facturas.Where(F => F.Cliente == IdCliente).Include(f => f.EstadosDeLaFactura).Include(f => f.SiNo).Include(f => f.Usuarios);
            }
                return View(facturas.ToList());
        }

        //
        // GET: /Facturas/Details/5

        public ActionResult Details(int id = 0)
        {
            Facturas facturas = db.Facturas.Find(id);
            if (facturas == null)
            {
                return HttpNotFound();
            }
            return View(facturas);
        }

        //
        // GET: /Facturas/Create

        public ActionResult Create()
        {
            ViewBag.Estado = new SelectList(db.EstadosDeLaFactura, "Id", "EstadoFactura");
            ViewBag.Cerrada = new SelectList(db.SiNo, "Id", "Nombre");
            ViewBag.Cliente = new SelectList(db.Usuarios, "Id", "Nombres");
            return View();
        }

        //
        // POST: /Facturas/Create

        [HttpPost]
        public ActionResult Create(Facturas facturas)
        {
            if (ModelState.IsValid)
            {
                db.Facturas.Add(facturas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Estado = new SelectList(db.EstadosDeLaFactura, "Id", "EstadoFactura", facturas.Estado);
            ViewBag.Cerrada = new SelectList(db.SiNo, "Id", "Nombre", facturas.Cerrada);
            ViewBag.Cliente = new SelectList(db.Usuarios, "Id", "Nombres", facturas.Cliente);
            return View(facturas);
        }

        //
        // GET: /Facturas/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Facturas facturas = db.Facturas.Find(id);
            if (facturas == null)
            {
                return HttpNotFound();
            }
            ViewBag.Estado = new SelectList(db.EstadosDeLaFactura, "Id", "EstadoFactura", facturas.Estado);
            ViewBag.Cerrada = new SelectList(db.SiNo, "Id", "Nombre", facturas.Cerrada);
            ViewBag.Cliente = new SelectList(db.Usuarios, "Id", "Nombres", facturas.Cliente);
            return View(facturas);
        }

        //
        // POST: /Facturas/Edit/5

        [HttpPost]
        public ActionResult Edit(Facturas facturas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(facturas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Estado = new SelectList(db.EstadosDeLaFactura, "Id", "EstadoFactura", facturas.Estado);
            ViewBag.Cerrada = new SelectList(db.SiNo, "Id", "Nombre", facturas.Cerrada);
            ViewBag.Cliente = new SelectList(db.Usuarios, "Id", "Nombres", facturas.Cliente);
            return View(facturas);
        }

        //
        // GET: /Facturas/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Facturas facturas = db.Facturas.Find(id);
            if (facturas == null)
            {
                return HttpNotFound();
            }
            return View(facturas);
        }

        //
        // POST: /Facturas/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Facturas facturas = db.Facturas.Find(id);
            db.Facturas.Remove(facturas);
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