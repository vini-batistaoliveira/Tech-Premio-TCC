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

        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                if (Session["Permissao"].ToString() == "ADM")
                {

                    if (Session["UserId"] != null)
                    {

                        return View(db.Usuario.ToList());

                    }
                    else
                    {
                        return RedirectToAction("Erro", "Home");
                    }

                }
                else
                {
                    return RedirectToAction("ErroPermissao", "Home");
                }
            }
            else
            {
                return RedirectToAction("Erro", "Home");
            }


        }

        public ActionResult Details(int? id)
        {
            if (Session["UserId"] != null)
            {
                if (Session["Permissao"].ToString() == "ADM")
                {

                    if (Session["UserId"] != null)
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
                    else
                    {
                        return RedirectToAction("Erro", "Home");
                    }

                }
                else
                {
                    return RedirectToAction("ErroPermissao", "Home");
                }
            }
            else
            {
                return RedirectToAction("Erro", "Home");
            }

        }

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
                    ModelState.AddModelError(string.Empty, "Usuario já cadastrado!!!");

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
            if (Session["UserId"] != null)
            {
                if (Session["Permissao"].ToString() == "ADM")
                {
                    List<string> Permissao = new List<string>() { "TEC", "ADM", "USER" };
                    ViewBag.Permissao = Permissao;

                    return View();

                }
                else
                {
                    return RedirectToAction("ErroPermissao", "Home");
                }
            }
            else
            {
                return RedirectToAction("Erro", "Home");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAdm(Usuario usuario)
        {
            if (Session["UserId"] != null)
            {
                if (Session["Permissao"].ToString() == "ADM")
                {
                    List<string> Permissao = new List<string>() { "TEC", "ADM", "USER" };
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
                else
                {
                    return RedirectToAction("ErroPermissao", "Home");
                }
            }
            else
            {
                return RedirectToAction("Erro", "Home");
            }
        }

        public ActionResult Edit(int? id)
        {
            if (Session["UserId"] != null)
            {
                if (Session["Permissao"].ToString() == "ADM")
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
                else
                {
                    return RedirectToAction("ErroPermissao", "Home");
                }
            }
            else
            {
                return RedirectToAction("Erro", "Home");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,nome,cpf,email,telefone,senha,ativo,permissao")] Usuario usuario)
        {
            if (Session["UserId"] != null)
            {
                if (Session["Permissao"].ToString() == "ADM")
                {
                    if (ModelState.IsValid)
                    {
                        var user = db.Usuario.Find(usuario.Id);

                        if (usuario.senha == null)
                        {
                            usuario.senha = user.senha;
                        }

                        if(usuario.permissao == null)
                        {
                            usuario.permissao = user.permissao;
                        }

                        db.Entry(usuario).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return View(usuario);

                }
                else
                {
                    return RedirectToAction("ErroPermissao", "Home");
                }
            }
            else
            {
                return RedirectToAction("Erro", "Home");
            }

        }

        public ActionResult Delete(int? id)
        {
            if (Session["UserId"] != null)
            {
                if (Session["Permissao"].ToString() == "ADM")
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
                else
                {
                    return RedirectToAction("ErroPermissao", "Home");
                }
            }
            else
            {
                return RedirectToAction("Erro", "Home");
            }


        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["UserId"] != null)
            {
                if (Session["Permissao"].ToString() == "ADM")
                {
                    Usuario usuario = db.Usuario.Find(id);
                    db.Usuario.Remove(usuario);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("ErroPermissao", "Home");
                }
            }
            else
            {
                return RedirectToAction("Erro", "Home");
            }

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
