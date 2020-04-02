﻿using Projet1_ASP.Models;
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
            foreach (Groupe groupe in list_groupe)
            {
                List<GroupeMembre> list_gm = db.GroupeMembres.Where(x => x.id_grp == groupe.grp_id).ToList();
                List<Etudiant> list_etudiant = new List<Etudiant>();
                foreach (GroupeMembre gm in list_gm)
                {
                    Etudiant etudiant = db.etudiants.Find(gm.id_et);
                    list_etudiant.Add(etudiant);
                }
                list_grp_id.Add(groupe.grp_id);
                list_grps.Add(list_etudiant);
            }
            ViewBag.listids = list_grp_id;
            ViewBag.listgrp = list_grps;
            return View(encadrant);
        }
    }
}