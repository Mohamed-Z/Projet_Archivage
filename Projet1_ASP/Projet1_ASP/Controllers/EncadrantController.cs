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
        public ActionResult Connexion(string email,string password)
        {
            Encadrant encadrant = new Encadrant();

            var x = db.encadrants.Where(p => p.email == email && p.password == password);
            
            foreach(var i in x)
            {
                encadrant = i;
            }
            if (encadrant == null)
            {
                return View();
            }
            return RedirectToAction("EspaceEncadrant",encadrant);
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
                db.encadrants.Add(encadrant);
                db.SaveChanges();
                return RedirectToAction("EspaceEncadrant", encadrant);
            }
            return View();
        }

        public ActionResult EspaceEncadrant(Encadrant encadrant)
        {
            return View();
        }
    }
}