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
    public class ReservasController : Controller
    {
        private EntidadesSaintForest db = new EntidadesSaintForest();

        //
        // GET: /Reservas/

        public ActionResult Index()
        {
            var reservas = from R in db.Reservas
                           select R;

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
            if (IdRol == 1)
            {
                ViewBag.IsCLiente = true;
                int IdClinte = int.Parse(Session["Id"].ToString());
                reservas = reservas.Where(R => R.IdCliente == IdClinte);
            }

            return View(reservas.ToList());
        }

        //
        // GET: /Reservas/Details/5

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
        // GET: /Reservas/Create

        public ActionResult Create(string id)
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
            if (IdRol == 1)
            {
                ViewBag.IsCLiente = true;
                if (string.IsNullOrEmpty(id))
                {
                    return RedirectToAction("../Shared/Error.cstml");
                }
                else
                {
                    ViewBag.habi = id;
                    ViewBag.IdHabitacion = new SelectList(db.Habitaciones, "Id", "NombreHabitacion",int.Parse(id));
                    ViewBag.IdCliente = new SelectList(db.Usuarios.Where(U => U.IdRol == 1), "Id", "Nombres", int.Parse(Session["Id"].ToString()));
                    ViewBag.Estado = new SelectList(db.EstadosDeLaFactura, "Id", "EstadoFactura");

                }

            }
            else 
            {
                ViewBag.IdHabitacion = new SelectList(db.Habitaciones, "Id", "NombreHabitacion");
                ViewBag.IdCliente = new SelectList(db.Usuarios.Where(U => U.IdRol == 1), "Id", "Nombres");
                ViewBag.Estado = new SelectList(db.EstadosDeLaFactura, "Id", "EstadoFactura");
               
            }

            ViewBag.Estado = new SelectList(db.EstadosDeLaFactura, "Id", "EstadoFactura",1);
          
            return View();
        }


        //
        // POST: /Reservas/Create

        [HttpPost]
        public ActionResult Create(Reservas reservas, string id)
        {
            reservas.Estado = 1;// Por defecto
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
            if (IdRol == 1)
            {
                ViewBag.IsCLiente = true;
                if (string.IsNullOrEmpty(id))
                {

                    return RedirectToAction("../Shared/Error.cstml");

                }
               

            }
            ViewBag.IdHabitacion = new SelectList(db.Habitaciones, "Id", "NombreHabitacion", reservas.IdHabitacion);
            ViewBag.IdCliente = new SelectList(db.Usuarios.Where(U => U.IdRol == 1), "Id", "Nombres", reservas.IdCliente);
            ViewBag.Estado = new SelectList(db.EstadosDeLaFactura, "Id", "EstadoFactura", 1);
            if (ModelState.IsValid)
            {

                var query = from R in db.Reservas
                            where R.IdHabitacion == reservas.IdHabitacion
                            select R;

                ViewBag.Resultados = query.Count();
                ViewBag.PuedeReserava = true;
                foreach (Reservas R in query.ToList())
                {
                    ViewBag.PuedeReserava = PuedeReservar(R.FechaInical, R.FechaFinal, reservas.FechaInical, reservas.FechaFinal);
                    if (!ViewBag.PuedeReserava)
                    {
                        ViewBag.PuedeReseravar = "La habitacion que intentas Reservar se encuentra ocupada, en este lapso de tiempo lapso (" + R.FechaInical + "-" + R.FechaFinal + " )";
                        break;
                    }
                }
                if (ViewBag.PuedeReserava)
                {
                    var Habitacion = db.Habitaciones.Find(reservas.IdHabitacion);
                    var Cliente = db.Usuarios.Find(reservas.IdCliente);
                    Habitacion.NombreHabitacion = Cliente.Nombres + " " + Cliente.Apellidos;
                    Habitacion.Disponilidad = 2;
                    db.Entry(Habitacion).State = EntityState.Modified;
                    db.Reservas.Add(reservas);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }


        }
        private static bool PuedeReservar(DateTime FechaIniciaHabitacion, DateTime FechaFinalHabitacion, DateTime FechaIniciaCliente, DateTime FechaFinalCliente)
        {
            bool PuedeHacerReservas = true;
            Console.WriteLine("Fecha I Habitacion: " + FechaIniciaHabitacion + " Fecha F Habitacion: " + FechaFinalHabitacion + "\n" + "Fecha I Cliente: " + FechaIniciaCliente + " Fecha F Cliente: " + FechaFinalCliente);
            DateTime Fecha0 = new DateTime(2000, 01, 01);
            TimeSpan HorasIniHabi = FechaIniciaHabitacion.Subtract(Fecha0);
            TimeSpan HorasIniCliente = FechaIniciaCliente.Subtract(Fecha0);
            TimeSpan HorasFinHabi = FechaFinalHabitacion.Subtract(Fecha0);
            TimeSpan HorasFinCliente = FechaFinalCliente.Subtract(Fecha0);
            Console.WriteLine("Inicial Habitacion: " + HorasIniHabi.TotalDays + "    Fin Habitacion: " + HorasFinHabi.TotalDays +
                "\nInicial Cliente: " + HorasIniCliente.TotalDays + "     Final CLiente: " + HorasFinCliente.TotalDays);
            if (HorasIniCliente <= HorasIniHabi && HorasFinCliente >= HorasFinHabi)
            {
                PuedeHacerReservas = false;


            }
            if (HorasFinCliente.TotalHours < HorasIniHabi.TotalHours || HorasIniCliente.TotalHours > HorasFinHabi.TotalHours)
            {
            }
            else
            {


                PuedeHacerReservas = false;

            }
            return PuedeHacerReservas;
        }


        //
        // GET: /Reservas/Edit/5

        public ActionResult Edit(int id = 0)
        {

            Reservas reservas = db.Reservas.Find(id);

            if (reservas == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdHabitacion = new SelectList(db.Habitaciones, "Id", "NombreHabitacion", reservas.IdHabitacion);
            ViewBag.IdCliente = new SelectList(db.Usuarios, "Id", "Nombres", reservas.IdCliente);
            return View(reservas);
        }

        //
        // POST: /Reservas/Edit/5

        [HttpPost]
        public ActionResult Edit(Reservas reservas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdHabitacion = new SelectList(db.Habitaciones, "Id", "NombreHabitacion", reservas.IdHabitacion);
            ViewBag.IdCliente = new SelectList(db.Usuarios, "Id", "Nombres", reservas.IdCliente);
            return View(reservas);
        }

        //
        // GET: /Reservas/Delete/5

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
        // POST: /Reservas/Delete/5

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