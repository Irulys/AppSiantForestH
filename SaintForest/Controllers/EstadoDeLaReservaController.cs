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
    public class EstadoDeLaReservaController : Controller
    {
        private EntidadesSaintForest db = new EntidadesSaintForest();

        //
        // GET: /EstadoDeLaReserva/

        public ActionResult Index()
        {
            return View(db.EstadoDeLaReserva.ToList());
        }

        //
        // GET: /EstadoDeLaReserva/Details/5

        public ActionResult Details(int id = 0)
        {
            EstadoDeLaReserva estadodelareserva = db.EstadoDeLaReserva.Find(id);
            if (estadodelareserva == null)
            {
                return HttpNotFound();
            }
            return View(estadodelareserva);
        }

        //
        // GET: /EstadoDeLaReserva/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /EstadoDeLaReserva/Create

        [HttpPost]
        public ActionResult Create(EstadoDeLaReserva estadodelareserva)
        {
            if (ModelState.IsValid)
            {
                db.EstadoDeLaReserva.Add(estadodelareserva);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(estadodelareserva);
        }

        //
        // GET: /EstadoDeLaReserva/Edit/5

        public ActionResult Edit(int id = 0)
        {
            EstadoDeLaReserva estadodelareserva = db.EstadoDeLaReserva.Find(id);
            if (estadodelareserva == null)
            {
                return HttpNotFound();
            }
            return View(estadodelareserva);
        }

        //
        // POST: /EstadoDeLaReserva/Edit/5

        [HttpPost]
        public ActionResult Edit(EstadoDeLaReserva estadodelareserva)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estadodelareserva).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(estadodelareserva);
        }

        //
        // GET: /EstadoDeLaReserva/Delete/5

        public ActionResult Delete(int id = 0)
        {
            EstadoDeLaReserva estadodelareserva = db.EstadoDeLaReserva.Find(id);
            if (estadodelareserva == null)
            {
                return HttpNotFound();
            }
            return View(estadodelareserva);
        }

        //
        // POST: /EstadoDeLaReserva/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            EstadoDeLaReserva estadodelareserva = db.EstadoDeLaReserva.Find(id);
            db.EstadoDeLaReserva.Remove(estadodelareserva);
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