using Projet1_ASP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projet1_ASP.Controllers
{
    public class SuperUserController : Controller
    {
        SiteContext db = new SiteContext();

        [HttpGet]
        public ActionResult Connexion()
        {
            ViewBag.erreur = "";
            ViewBag.msg = "";
            return View();
        }

        [HttpPost]
        public ActionResult Connexion(string email, string password)
        {
            var x = db.superUsers.ToList();

            foreach (var i in x)
            {
                if (i.email == email && i.password == password)
                {
                    Session["id_sup"] = i.Id;
                    Session["alert"] = true;
                    return RedirectToAction("EspaceSuperUser");
                }
            }
            ViewBag.erreur = "is-invalid";
            ViewBag.msg = "Email ou mot de passe incorrect !";
            return View();
        }

        [HttpGet]
        public ActionResult Inscription()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Inscription(SuperUser superUser)
        {
            if (ModelState.IsValid)
            {
                
                db.superUsers.Add(superUser);
                db.SaveChanges();
                Session["alert"] = true;
                Session["id_sup"] = superUser.Id;
                return RedirectToAction("EspaceSuperUser");
            }
            return View();
        }

        public ActionResult EspaceSuperUser()
        {
            int id = Convert.ToInt32(Session["id_sup"]);
            SuperUser superUser = db.superUsers.Find(id);
            return View(superUser);
        }

        public ActionResult Deconnexion()
        {
            Session["id_sup"] = null;
            return RedirectToAction("Connexion");
        }

        [HttpGet]
        public ActionResult Modifier()
        {
            int id = Convert.ToInt32(Session["id_sup"]);
            SuperUser superUser = db.superUsers.Find(id);
            return View(superUser);
        }

        [HttpPost]
        public ActionResult Modifier(SuperUser superUser)
        {
            int id = Convert.ToInt32(Session["id_sup"]);
            SuperUser s = db.superUsers.Find(id);
            if (ModelState.IsValid)
            {
                s.nom = superUser.nom;
                s.prenom = superUser.prenom;
                s.email = superUser.email;
                s.password = superUser.password;
                s.confirmation = superUser.confirmation;
                db.SaveChanges();
                return View(superUser);
            }
            return View(s);
        }


        public PartialViewResult AfficherDetailsUser()
        {
            string word = Request.Form["search"];
            string rech = Request.Form["rech"];
            if (word == "")
            {
                word = "00000000000000";
            }
            if (rech == null)
            {
                rech = "description";
            }

            SearchModelUser sm = new SearchModelUser();

            sm.searchBy(rech, word);


            return PartialView("_AfficherDetailsUser", sm);
        }

        public ActionResult Get(int id)
        {

            SiteContext db = new SiteContext();
            Models.File file = db.files.Find(id);

            //If file exists....

            MemoryStream ms = new MemoryStream(file.Content, 0, 0, true, true);
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "inline;filename=" + file.Name);
            Response.Buffer = true;
            Response.Clear();
            Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.End();
            return new FileStreamResult(Response.OutputStream, "application/pdf");

        }
    }
}