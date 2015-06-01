using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SaintForest.Models;
using System.Web.Security;

namespace SaintForest.Controllers
{
    public class UsuariosController : Controller
    {
        private EntidadesSaintForest db = new EntidadesSaintForest();

        //
        // GET: /Usuarios/

        public ActionResult Index()
        {
            var usuarios = db.Usuarios.Include(u => u.EstadoActivo).Include(u => u.Roles);
            return View(usuarios.ToList());
        }

        //
        // GET: /Usuarios/Details/5

        public ActionResult Details(int id = 0)
        {
            Usuarios usuarios = db.Usuarios.Find(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            return View(usuarios);
        }

        //
        // GET: /Usuarios/Create

        public ActionResult Create()
        {
            ViewBag.Estado = new SelectList(db.EstadoActivo, "Id", "Nombre");
            ViewBag.IdRol = new SelectList(db.Roles, "Id", "NombreRol");
            return View();
        }

        //
        // POST: /Usuarios/Create

        [HttpPost]
        public ActionResult Create(Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                db.Usuarios.Add(usuarios);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Estado = new SelectList(db.EstadoActivo, "Id", "Nombre", usuarios.Estado);
            ViewBag.IdRol = new SelectList(db.Roles, "Id", "NombreRol", usuarios.IdRol);
            return View(usuarios);
        }

        //
        // GET: /Usuarios/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Usuarios usuarios = db.Usuarios.Find(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            ViewBag.Estado = new SelectList(db.EstadoActivo, "Id", "Nombre", usuarios.Estado);
            ViewBag.IdRol = new SelectList(db.Roles, "Id", "NombreRol", usuarios.IdRol);
            return View(usuarios);
        }

        //
        // POST: /Usuarios/Edit/5

        [HttpPost]
        public ActionResult Edit(Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuarios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Estado = new SelectList(db.EstadoActivo, "Id", "Nombre", usuarios.Estado);
            ViewBag.IdRol = new SelectList(db.Roles, "Id", "NombreRol", usuarios.IdRol);
            return View(usuarios);
        }

        //
        // GET: /Usuarios/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Usuarios usuarios = db.Usuarios.Find(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            return View(usuarios);
        }

        //
        // POST: /Usuarios/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuarios usuarios = db.Usuarios.Find(id);
            db.Usuarios.Remove(usuarios);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        ///_------------------------Mis Metodos--------------------------------------------//

        public ActionResult LogIn()
        {

            return View();
        }

        public ActionResult Registrar()
        {

            return View();
        }

        [HttpPost] //Aqui solo se registran Clientes
        public ActionResult Registrar(Usuarios usuarios)
        {
            usuarios.IdRol = 1;
            usuarios.Estado = 1;
            usuarios.Contraseña = "Milano";
            if (ModelState.IsValid)
            {
                db.Usuarios.Add(usuarios);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception E) {
                    return Content(
                         string.Format("{0}, Valor de la contraseña {1}", E.Message,usuarios.Contraseña)); 
                }
                    return RedirectToAction("LogIn");
            }

            ViewBag.IdRol = new SelectList(db.Roles, "Id", "NombreRol", usuarios.IdRol);
            UserModel Entrante = new UserModel();
            Entrante.Email = usuarios.Email;
            Entrante.Contrasena = usuarios.Contraseña;
            LogIn(Entrante);
            return View(usuarios);
        }

        [HttpPost]//Log In De todo El mundo 
        public ActionResult LogIn(SaintForest.Models.UserModel User)
        {
            string Result = "";
            if (ModelState.IsValid)
            {

                if (Isvalid(User.Email, User.Contrasena))
                {
                    var Usuario = db.Usuarios.FirstOrDefault(a => a.Email == User.Email);
                    Session["IdRol"] = Usuario.IdRol;
                    Session["Rol"] = Usuario.Roles.NombreRol;
                    Session["Nombre"] = Usuario.Nombres + " " + Usuario.Apellidos;
                    Session["id"] = Usuario.Id;
                    FormsAuthentication.SetAuthCookie(User.Email, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    Result = "Datos incorrectos";
                    ModelState.AddModelError("", "Incorrecto");
                }

            }

            ViewBag.Result = Result;
            return View(User);
        }
        //Salidad De Todo El mundo
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        //Compruebo Datos 
        private bool Isvalid(string Email, string Contrasena)
        {
            bool valido = false;

            var User = db.Usuarios.FirstOrDefault(u => u.Email == Email);
            if (User != null)
            {
                if (User.Contraseña == Contrasena)
                {
                    valido = true;
                }
            }

            return valido;
        }
    }
}