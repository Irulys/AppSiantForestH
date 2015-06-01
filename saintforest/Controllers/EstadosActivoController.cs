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
    public class EstadosActivoController : Controller
    {
        private EntidadesSaintForest db = new EntidadesSaintForest();

        //
        // GET: /EstadosActivo/

        public ActionResult Index()
        {
            return View(db.EstadoActivo.ToList());
        }

        //
        // GET: /EstadosActivo/Details/5

        public ActionResult Details(int id = 0)
        {
            EstadoActivo estadoactivo = db.EstadoActivo.Find(id);
            if (estadoactivo == null)
            {
                return HttpNotFound();
            }
            return View(estadoactivo);
        }

        //
        // GET: /EstadosActivo/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /EstadosActivo/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EstadoActivo estadoactivo)
        {
            if (ModelState.IsValid)
            {
                db.EstadoActivo.Add(estadoactivo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(estadoactivo);
        }

        //
        // GET: /EstadosActivo/Edit/5

        public ActionResult Edit(int id = 0)
        {
            EstadoActivo estadoactivo = db.EstadoActivo.Find(id);
            if (estadoactivo == null)
            {
                return HttpNotFound();
            }
            return View(estadoactivo);
        }

        //
        // POST: /EstadosActivo/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EstadoActivo estadoactivo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estadoactivo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(estadoactivo);
        }

        //
        // GET: /EstadosActivo/Delete/5

        public ActionResult Delete(int id = 0)
        {
            EstadoActivo estadoactivo = db.EstadoActivo.Find(id);
            if (estadoactivo == null)
            {
                return HttpNotFound();
            }
            return View(estadoactivo);
        }

        //
        // POST: /EstadosActivo/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EstadoActivo estadoactivo = db.EstadoActivo.Find(id);
            db.EstadoActivo.Remove(estadoactivo);
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