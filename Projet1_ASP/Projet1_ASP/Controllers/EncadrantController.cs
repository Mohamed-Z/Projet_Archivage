using Projet1_ASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projet1_ASP.Controllers
{
    public class EncadrantController : Controller
    {
        SiteContext db = new SiteContext();

        [HttpGet]
        public ActionResult Connexion()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Connexion(string email, string password)
        {
            var x = db.encadrants.ToList();

            foreach (var i in x)
            {
                if (i.email == email && i.password == password)
                {
                    Session["id"] = i.Id;

                    return RedirectToAction("EspaceEncadrant");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Inscription()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Inscription(Encadrant encadrant)
        {
            if (ModelState.IsValid)
            {
                encadrant.nbr_grp = 0;
                db.encadrants.Add(encadrant);
                db.SaveChanges();
                Session["id"] = encadrant.Id;
                return RedirectToAction("EspaceEncadrant");
            }
            return View();
        }

        public ActionResult EspaceEncadrant()
        {
            int id = Convert.ToInt32(Session["id"]);
            Encadrant encadrant = db.encadrants.Find(id);
            return View(encadrant);
        }
    }
}