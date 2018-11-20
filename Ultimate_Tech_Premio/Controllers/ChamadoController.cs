using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ultimate_Tech_Premio.Models;

namespace Ultimate_Tech_Premio.Controllers
{
    public class ChamadoController : Controller
    {
        private tech_premioEntities2 db = new tech_premioEntities2();

        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {

                if (Session["Permissao"].ToString() == "TEC" || Session["Permissao"].ToString() == "ADM")
                {
                    var chamado = db.Chamado.ToList();
                    return View(chamado);
                }

                else if (Session["Permissao"].ToString() == "USER")
                {
                    var idUsuario = (int)Session["UserID"];
                    var chamado = db.Chamado.Where(c => c.UsuarioId == idUsuario);
                    return View(chamado.ToList());
                }
            }

            return RedirectToAction("Erro", "Home");

        }

        public ActionResult Details(int? id)
        {
            if (Session["UserId"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Chamado chamado = db.Chamado.Find(id);
                if (chamado == null)
                {
                    return HttpNotFound();
                }
                return View(chamado);
            }
            else
            {
                return RedirectToAction("Erro", "Home");
            }
        }

        public ActionResult Create()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Erro", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Chamado chamado)
        {
            if (Session["UserId"] != null)
            {
                if (ModelState.IsValid)
                {
                    chamado.reaberto = false;
                    chamado.Em_Andamento = false;
                    chamado.nome_usuario = Session["UserName"].ToString();
                    chamado.status = Enum.EnumChamado.ABERTO.ToString();
                    chamado.UsuarioId = (int)Session["UserID"];
                    chamado.data_chamado = DateTime.Now;
                    db.Chamado.Add(chamado);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(chamado);
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
                if (Session["Permissao"].ToString() == "TEC" || Session["Permissao"].ToString() == "ADM")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Chamado chamado = db.Chamado.Find(id);
                    if (chamado == null)
                    {
                        return HttpNotFound();
                    }

                    return View(chamado);
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
        public ActionResult Edit(Chamado chamado)
        {
            if (Session["UserId"] != null)
            {
                if (Session["Permissao"].ToString() == "TEC" || Session["Permissao"].ToString() == "ADM")
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(chamado).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return View(chamado);

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

        public ActionResult Aceitar(int? id)
        {
            if (Session["UserId"] != null)
            {
                if (Session["Permissao"].ToString() == "TEC" || Session["Permissao"].ToString() == "ADM")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Chamado chamado = db.Chamado.Find(id);
                    if (chamado == null)
                    {
                        return HttpNotFound();
                    }

                    if (ModelState.IsValid)
                    {
                        var idTec = (int)Session["UserID"];
                        Usuario userTec = db.Usuario.Find(idTec);
                        chamado.Em_Andamento = true;
                        chamado.Tecnico_nome = userTec.nome;
                        chamado.Tecnico1 = userTec.Id;
                        db.Entry(chamado).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return View(chamado);
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

        public ActionResult Fechar(int? id)
        {
            if (Session["UserId"] != null)
            {
                if (Session["Permissao"].ToString() == "TEC" || Session["Permissao"].ToString() == "ADM")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Chamado chamado = db.Chamado.Find(id);
                    if (chamado == null)
                    {
                        return HttpNotFound();
                    }
                    return View(chamado);
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
        public ActionResult Fechar(Chamado chamado)
        {
            if (Session["UserId"] != null)
            {
                if (Session["Permissao"].ToString() == "TEC" || Session["Permissao"].ToString() == "ADM")
                {
                    if (ModelState.IsValid)
                    {
                        Chamado cham = db.Chamado.Find(chamado.Id);
                        cham.reaberto = chamado.reaberto;
                        cham.status = Enum.EnumChamado.FECHADO.ToString();
                        cham.Em_Andamento = false;
                        cham.avaliado = false;
                        cham.Tecnico1 = (int)Session["UserID"];
                        cham.retorno_tec = chamado.retorno_tec;
                        db.Entry(cham).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return View(chamado);
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
                    Chamado chamado = db.Chamado.Find(id);
                    if (chamado == null)
                    {
                        return HttpNotFound();
                    }
                    return View(chamado);
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

        // GET: Chamado/Edit/5
        public ActionResult Avaliar(int? id)
        {
            if (Session["UserId"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Chamado chamado = db.Chamado.Find(id);
                if (chamado == null)
                {
                    return HttpNotFound();
                }
                List<string> solucionadoSN = new List<string>() { "SIM", "NÃO" };
                ViewBag.solucionadoSN = solucionadoSN;

                List<int> list = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                ViewBag.Notas = list;
                return View(chamado);
            }
            else
            {
                return RedirectToAction("Erro", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Avaliar(Chamado chamado)
        {
            if (Session["UserId"] != null)
            {
                if (ModelState.IsValid)
                {
                    Chamado cham = db.Chamado.Find(chamado.Id);
                    cham.solucionado_avaliacao = chamado.solucionado_avaliacao;
                    cham.sugestao_avaliacao = chamado.sugestao_avaliacao;
                    cham.avaliado = true;

                    if (chamado.solucionado_avaliacao == "SIM")
                        cham.solucionado_nota = 10;
                    else
                        cham.solucionado_nota = 0;

                    cham.Em_Andamento = false;
                    db.Entry(cham).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(chamado);

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
                    Chamado chamado = db.Chamado.Find(id);
                    db.Chamado.Remove(chamado);
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

        public ActionResult Reabrir(int? id)
        {
            if (Session["UserId"] != null)
            {
                if (Session["Permissao"].ToString() == "TEC" || Session["Permissao"].ToString() == "ADM")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Chamado chamado = db.Chamado.Find(id);
                    if (chamado == null)
                    {
                        return HttpNotFound();
                    }
                    return View(chamado);
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
        public ActionResult Reabrir(Chamado chamado)
        {
            if (Session["UserId"] != null)
            {
                if (Session["Permissao"].ToString() == "TEC" || Session["Permissao"].ToString() == "ADM")
                {
                    if (ModelState.IsValid)
                    {
                        Chamado cham = db.Chamado.Find(chamado.Id);

                        cham.reaberto = true;
                        cham.status = Enum.EnumChamado.ABERTO.ToString();
                        cham.Em_Andamento = false;
                        cham.motivo_reaberto = chamado.motivo_reaberto;
                        db.Entry(cham).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
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
