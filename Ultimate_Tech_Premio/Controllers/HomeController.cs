using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ultimate_Tech_Premio.Models;

namespace Tech.Controllers
{
    public class HomeController : Controller
    {
        private tech_premioEntities2 db = new tech_premioEntities2();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Usuario objUser)
        {
            if (ModelState.IsValid)
            {
                var obj = db.Usuario.Where(a => a.cpf.Equals(objUser.cpf) && a.senha.Equals(objUser.senha)).FirstOrDefault();

                if (obj != null)
                {
                    Session["UserID"] = obj.Id;
                    Session["UserName"] = obj.nome.ToString();
                    Session["Permissao"] = obj.permissao.ToString();
                    ViewBag.ErroLogin = null;
                    return View("UserDashBoard");
                }

            }

            ViewBag.ErroLogin = "Usuario ou Senha Incorreto!!!";

            return View(objUser);
        }

        public ActionResult UserDashBoard()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Logout()
        {
            Session["UserID"] = null;
            Session["UserName"] = null;
            Session["Permissao"] = null;

            return RedirectToAction("Login");
        }
    }
}