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

        // GET: Chamado
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

            return View();

        }

        // GET: Chamado/Details/5
        public ActionResult Details(int? id)
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

        // GET: Chamado/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Chamado/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Chamado chamado)
        {
            if (ModelState.IsValid)
            {
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

        // GET: Chamado/Edit/5
        public ActionResult Edit(int? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Chamado chamado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chamado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chamado);
        }

        public ActionResult Aceitar(int? id)
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
                if (Session["UserID"] != null)
                {
                    var idTec = (int)Session["UserID"];

                    Usuario userTec = db.Usuario.Find(idTec);

                    chamado.Em_Andamento = true;
                    chamado.Tecnico_nome = userTec.nome;
                    chamado.Tecnico1 = userTec.Id;
                }

                db.Entry(chamado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chamado);
        }

        public ActionResult Fechar(int? id)
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

        // POST: Chamado/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Fechar(Chamado chamado)
        {
            if (ModelState.IsValid)
            {
                chamado.status = Enum.EnumChamado.FECHADO.ToString();
                chamado.Em_Andamento = false;
                chamado.avaliado = false;
                chamado.Tecnico1 = (int)Session["UserID"];
                db.Entry(chamado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chamado);
        }


        // GET: Chamado/Delete/5
        public ActionResult Delete(int? id)
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

        // GET: Chamado/Edit/5
        public ActionResult Avaliar(int? id)
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

            List<string> solucionadoSN = new List<string>() {"SIM","NÃO"};
            ViewBag.solucionadoSN = solucionadoSN;

            List<int> list = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            ViewBag.Notas = list;

            return View(chamado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Avaliar(Chamado chamado)
        {
            if (ModelState.IsValid)
            {
                chamado.avaliado = true;
                if (chamado.solucionado_avaliacao == "SIM")
                    chamado.solucionado_nota = 10;
                else
                    chamado.solucionado_nota = 0;

                chamado.Em_Andamento = false;
                db.Entry(chamado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chamado);
        }

        // POST: Chamado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Chamado chamado = db.Chamado.Find(id);
            db.Chamado.Remove(chamado);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Reabrir(int? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reabrir(Chamado chamado)
        {
            if (ModelState.IsValid)
            {
                chamado.reaberto = true;
                chamado.status = Enum.EnumChamado.ABERTO.ToString();
                chamado.Em_Andamento = false;
                db.Entry(chamado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chamado);
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
