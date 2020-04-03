using Projet1_ASP.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
            ViewBag.erreur = "";
            ViewBag.msg = "";
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
                    Session["alert"] = true;
                    return RedirectToAction("EspaceEncadrant");
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
        public ActionResult Inscription(Encadrant encadrant)
        {
            if (ModelState.IsValid)
            {
                encadrant.nbr_grp = 0;
                db.encadrants.Add(encadrant);
                db.SaveChanges();
                 Session["alert"] = true;
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

        public ActionResult Deconnexion()
        {
            Session["id"] = null;
            return RedirectToAction("Connexion");
        }

        [HttpGet]
        public ActionResult Modifier()
        {
            int id = Convert.ToInt32(Session["id"]);
            Encadrant encadrant = db.encadrants.Find(id);
            return View(encadrant);
        }

        [HttpPost]
        public ActionResult Modifier(Encadrant encadrant)
        {
            int id = Convert.ToInt32(Session["id"]);
            Encadrant e = db.encadrants.Find(id);
            if (ModelState.IsValid)
            {
                e.nom = encadrant.nom;
                e.prenom = encadrant.prenom;
                e.email = encadrant.email;
                e.password = encadrant.password;
                e.confirmation = encadrant.confirmation;
                db.SaveChanges();
                return View(encadrant);
            }
            return View(e);
        }

        public ActionResult Groupes()
        {
            int id = Convert.ToInt32(Session["id"]);
            Encadrant encadrant = db.encadrants.Find(id);
            List<Groupe> list_groupe = db.groupes.Where(x => x.id_enc == id).ToList();
            List<List<Etudiant>> list_grps = new List<List<Etudiant>>();
            List<int> list_grp_id = new List<int>();
            List<string> list_type = new List<string>();
            foreach (Groupe groupe in list_groupe)
            {
                List<GroupeMembre> list_gm = db.GroupeMembres.Where(x => x.id_grp == groupe.grp_id).ToList();
                List<Etudiant> list_etudiant = new List<Etudiant>();
                foreach (GroupeMembre gm in list_gm)
                {
                    if (gm.confirmed != false)
                    {
                        Etudiant etudiant = db.etudiants.Find(gm.id_et);
                        list_etudiant.Add(etudiant);
                    }
                }
                list_type.Add(groupe.Type.nom_type);
                list_grp_id.Add(groupe.grp_id);
                list_grps.Add(list_etudiant);
            }
            ViewBag.listids = list_grp_id;
            ViewBag.listgrp = list_grps;
            ViewBag.listtps = list_type;
            return View(encadrant);
        }

        public PartialViewResult AfficherDetails()
        {
            string word = Request.Form["search"];
            string rech = Request.Form["rech"];
            if (word == "") {
                word = "00000000000000";
            }
            if (rech == null)
            {
                rech = "description";
            }
            int id = Convert.ToInt32(Session["id"]);
            Encadrant encadrant = db.encadrants.Find(id);
            /*
            if (rech == "sujet")
            {
                var x = db.groupes.Where(g => g.id_enc == id).ToList();
                foreach(Groupe i in x)
                {
                    var y = db.GroupeMembres.Where(p => p.id_grp == i.grp_id).ToList();
                    Models.Type type = db.types.Find(i.id_tp);
                    File f = db.files.Where(p => p.groupe_Id == i.grp_id && p.sujet.StartsWith(rech)).SingleOrDefault();
                }
            }
            */


            SearchModel sm = new SearchModel(id);

            sm.searchBy(rech, word);
            

            return PartialView("_AfficherDetails",sm);
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