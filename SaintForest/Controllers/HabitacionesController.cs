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
    public class HabitacionesController : Controller
    {
        private EntidadesSaintForest db = new EntidadesSaintForest();

        //
        // GET: /Habitaciones/

        public ActionResult Index(string TipoHabitacion, string PrecioMe, string PrecioMa, string EstadoLimpieza)
        {
            int IdRol = 0;
            ViewBag.IsCliente = false;
            ViewBag.IsMucama = false;
            try
            {
                IdRol = ((int)Session["IdRol"]);
                ViewBag.IsCliente = new UserModel().IsCliente(IdRol);
                ViewBag.IsMucama = new UserModel().IsMucama(IdRol);
            }
            catch (NullReferenceException)
            {
                // return RedirectToAction("../Home/LogOut");  
            }
            catch (InvalidCastException)
            {
                //return RedirectToAction("../Home/LogOut");
            }
            if (IdRol == 1)
            {
                ViewBag.IsCLiente = true;

            }


            var query = from ha in db.Habitaciones
                        select ha;
            if (!string.IsNullOrEmpty(TipoHabitacion))
            {
                int TipoHabi;

                TipoHabi = (int.Parse(TipoHabitacion));
                query = query.Where(h => h.TipoHabitacion == TipoHabi);
            }
            if (!string.IsNullOrEmpty(PrecioMa))
            {
                ViewBag.Ma = PrecioMa;
                PrecioMa = PrecioMa + "0000";
                int Preci = int.Parse(PrecioMa);
                query = query.Where(h => h.Precio <= Preci);
            }

            if (!string.IsNullOrEmpty(PrecioMe))
            {
                ViewBag.Me = PrecioMe;
                PrecioMe = PrecioMe + "0000";
                int Preci = int.Parse(PrecioMe);
                query = query.Where(h => h.Precio >= Preci);
            }
            if (!string.IsNullOrEmpty(EstadoLimpieza))
            {

                int EstadoLi = int.Parse(EstadoLimpieza);
                query = query.Where(h => h.EstadoLimpieza == EstadoLi);
            }
            //query = query.Where(h => h.TipoHabitacion == 1);
            //  habitaciones.Where(h => h.Precio >= (decimal)800000);
            //query= query.Where(h => h.Precio >= (decimal)860000);
            ViewBag.TipoHabitacion = new SelectList(db.TipoHabitaciones, "Id", "Nombre");
            ViewBag.EstadoLimpieza = new SelectList(db.EstadoLimpieza, "Id", "Nombre");
            return View(query.ToList());
        }

        // GET: /Habitaciones/Details/5

        public ActionResult Details(int id = 0)
        {
            Habitaciones habitaciones = db.Habitaciones.Find(id);
            if (habitaciones == null)
            {
                return HttpNotFound();
            }
            return View(habitaciones);
        }

        //
        // GET: /Habitaciones/Create

        public ActionResult Create()
        {
            ViewBag.Disponilidad = new SelectList(db.Disponibilidad, "Id", "Nombre");
            ViewBag.EstadoActivo = new SelectList(db.EstadoActivo, "Id", "Nombre");
            ViewBag.EstadoLimpieza = new SelectList(db.EstadoLimpieza, "Id", "Nombre");
            ViewBag.TipoHabitacion = new SelectList(db.TipoHabitaciones, "Id", "Nombre");
            return View();
        }

        //
        // POST: /Habitaciones/Create

        [HttpPost]
        public ActionResult Create(Habitaciones habitaciones)
        {
            if (ModelState.IsValid)
            {
                db.Habitaciones.Add(habitaciones);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Disponilidad = new SelectList(db.Disponibilidad, "Id", "Nombre", habitaciones.Disponilidad);
            ViewBag.EstadoActivo = new SelectList(db.EstadoActivo, "Id", "Nombre", habitaciones.EstadoActivo);
            ViewBag.EstadoLimpieza = new SelectList(db.EstadoLimpieza, "Id", "Nombre", habitaciones.EstadoLimpieza);
            ViewBag.TipoHabitacion = new SelectList(db.TipoHabitaciones, "Id", "Nombre", habitaciones.TipoHabitacion);
            return View(habitaciones);
        }

        //
        // GET: /Habitaciones/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Habitaciones habitaciones = db.Habitaciones.Find(id);
            if (habitaciones == null)
            {
                return HttpNotFound();
            }
            int IdRol = 0;
            ViewBag.IsCliente = false;
            ViewBag.IsMucama = false;

            try
            {
                IdRol = ((int)Session["IdRol"]);
                ViewBag.IsCliente = new UserModel().IsCliente(IdRol);//es CLiente?
                ViewBag.IsMucama = new UserModel().IsMucama(IdRol);//Es Mucama?
            }
            catch (NullReferenceException)
            {
                // return RedirectToAction("../Home/LogOut");  
            }
            catch (InvalidCastException)
            {
                //return RedirectToAction("../Home/LogOut");
            }
            ViewBag.Precio = habitaciones.Precio;
            ViewBag.Disponilidad = new SelectList(db.Disponibilidad, "Id", "Nombre", habitaciones.Disponilidad);
            ViewBag.EstadoActivo = new SelectList(db.EstadoActivo, "Id", "Nombre", habitaciones.EstadoActivo);
            ViewBag.EstadoLimpieza = new SelectList(db.EstadoLimpieza, "Id", "Nombre", habitaciones.EstadoLimpieza);
            ViewBag.TipoHabitacion = new SelectList(db.TipoHabitaciones, "Id", "Nombre", habitaciones.TipoHabitacion);
            return View(habitaciones);
        }

        //
        // POST: /Habitaciones/Edit/5

        [HttpPost]
        public ActionResult Edit(Habitaciones habitaciones, FormCollection Fc, string EstadoLimpieza)
        {

            if (ModelState.IsValid)
            {

                if (!string.IsNullOrEmpty(EstadoLimpieza))
                {
                    int EstadoLim = int.Parse(EstadoLimpieza);
                    habitaciones.EstadoLimpieza = EstadoLim;
                }
                db.Entry(habitaciones).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Disponilidad = new SelectList(db.Disponibilidad, "Id", "Nombre", habitaciones.Disponilidad);
            ViewBag.EstadoActivo = new SelectList(db.EstadoActivo, "Id", "Nombre", habitaciones.EstadoActivo);
            ViewBag.EstadoLimpieza = new SelectList(db.EstadoLimpieza, "Id", "Nombre", habitaciones.EstadoLimpieza);
            ViewBag.TipoHabitacion = new SelectList(db.TipoHabitaciones, "Id", "Nombre", habitaciones.TipoHabitacion);
            return View(habitaciones);
        }

        //
        // GET: /Habitaciones/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Habitaciones habitaciones = db.Habitaciones.Find(id);
            if (habitaciones == null)
            {
                return HttpNotFound();
            }
            return View(habitaciones);
        }

        //
        // POST: /Habitaciones/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Habitaciones habitaciones = db.Habitaciones.Find(id);
            db.Habitaciones.Remove(habitaciones);
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