using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ultimate_Tech_Premio.Helper;
using Ultimate_Tech_Premio.Models;

namespace Ultimate_Tech_Premio.Controllers
{
    public class UsuarioController : Controller
    {
        private tech_premioEntities2 db = new tech_premioEntities2();

        // GET: Usuario
        public ActionResult Index()
        {
            return View(db.Usuario.ToList());
        }

        // GET: Usuario/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,nome,cpf,email,telefone,senha,ativo,permissao")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                var resultUser = db.Usuario.Where(c => c.cpf == usuario.cpf).ToList();
                
                if (resultUser.Count > 0)
                {
                    ModelState.AddModelError(string.Empty,"Usuario já cadastrado!!!");

                    return View();
                }

                usuario.permissao = Enum.EnumPermissao.USER.ToString();
                usuario.ativo = true;
                db.Usuario.Add(usuario);
                db.SaveChanges();
                ViewBag.Cadastro = "Cadastro Relizado Com Sucesso!!!";
                return RedirectToAction("Login", "Home");
            }

            return View(usuario);
        }

        public ActionResult CreateAdm()
        {
            List<string> Permissao = new List<string>() { "TEC", "ADM", "USER" };
            ViewBag.Permissao = Permissao;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAdm(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                if (usuario.permissao == "USER")
                    usuario.permissao = Enum.EnumPermissao.USER.ToString();

                if (usuario.permissao == "TEC")
                    usuario.permissao = Enum.EnumPermissao.TEC.ToString();

                if (usuario.permissao == "ADM")
                    usuario.permissao = Enum.EnumPermissao.ADM.ToString();

                usuario.ativo = true;
                db.Usuario.Add(usuario);
                db.SaveChanges();
                ViewBag.Cadastro = "Cadastro Relizado Com Sucesso!!!";
                return RedirectToAction("Index", "Usuario");
            }

            return View(usuario);
        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<string> Permissao = new List<string>() { "TEC", "ADM", "USER" };
            ViewBag.Permissao = Permissao;
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,nome,cpf,email,telefone,senha,ativo,permissao")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        // GET: Usuario/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuario usuario = db.Usuario.Find(id);
            db.Usuario.Remove(usuario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
