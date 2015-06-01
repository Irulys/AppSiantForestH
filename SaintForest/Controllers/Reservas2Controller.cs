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
    public class Reservas2Controller : Controller
    {
        private EntidadesSaintForest db = new EntidadesSaintForest();

        //
        // GET: /Reservas2/

        public ActionResult Index()
        {
            var reservas = db.Reservas.Include(r => r.EstadoDeLaReserva).Include(r => r.Habitaciones).Include(r => r.Usuarios);
            return View(reservas.ToList());
        }

        //
        // GET: /Reservas2/Details/5

        public ActionResult Details(int id = 0)
        {
            Reservas reservas = db.Reservas.Find(id);
            if (reservas == null)
            {
                return HttpNotFound();
            }
            return View(reservas);
        }

        //
        // GET: /Reservas2/Create

        public ActionResult Create()
        {
            ViewBag.Estado = new SelectList(db.EstadoDeLaReserva, "Id", "Estado");
            ViewBag.IdHabitacion = new SelectList(db.Habitaciones, "Id", "NombreHabitacion");
            ViewBag.IdCliente = new SelectList(db.Usuarios, "Id", "Nombres");
            return View();
        }

        //
        // POST: /Reservas2/Create

        [HttpPost]
        public ActionResult Create(Reservas reservas)
        {
            if (ModelState.IsValid)
            {
                db.Reservas.Add(reservas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Estado = new SelectList(db.EstadoDeLaReserva, "Id", "Estado", reservas.Estado);
            ViewBag.IdHabitacion = new SelectList(db.Habitaciones, "Id", "NombreHabitacion", reservas.IdHabitacion);
            ViewBag.IdCliente = new SelectList(db.Usuarios, "Id", "Nombres", reservas.IdCliente);
            return View(reservas);
        }

        //
        // GET: /Reservas2/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Reservas reservas = db.Reservas.Find(id);
            if (reservas == null)
            {
                return HttpNotFound();
            }
            ViewBag.Estado = new SelectList(db.EstadoDeLaReserva, "Id", "Estado", reservas.Estado);
            ViewBag.IdHabitacion = new SelectList(db.Habitaciones, "Id", "NombreHabitacion", reservas.IdHabitacion);
            ViewBag.IdCliente = new SelectList(db.Usuarios, "Id", "Nombres", reservas.IdCliente);
            return View(reservas);
        }

        //
        // POST: /Reservas2/Edit/5

        [HttpPost]
        public ActionResult Edit(Reservas reservas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Estado = new SelectList(db.EstadoDeLaReserva, "Id", "Estado", reservas.Estado);
            ViewBag.IdHabitacion = new SelectList(db.Habitaciones, "Id", "NombreHabitacion", reservas.IdHabitacion);
            ViewBag.IdCliente = new SelectList(db.Usuarios, "Id", "Nombres", reservas.IdCliente);
            return View(reservas);
        }

        //
        // GET: /Reservas2/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Reservas reservas = db.Reservas.Find(id);
            if (reservas == null)
            {
                return HttpNotFound();
            }
            return View(reservas);
        }

        //
        // POST: /Reservas2/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservas reservas = db.Reservas.Find(id);
            db.Reservas.Remove(reservas);
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