
using Projet1_ASP.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using File = Projet1_ASP.Models.File;

namespace Projet1_ASP.Controllers
{
    public class EtudiantController : Controller
    {
        List<SelectListItem> list = new List<SelectListItem>
                {
                    new SelectListItem{ Text="Mini-Projet", Value = "Mini-Projet", Selected = true },
                    new SelectListItem{ Text="PFE", Value = "PFE" },
                    new SelectListItem{ Text="Stage d'initiation", Value = "Stage d'initiation" },
                    new SelectListItem{ Text="Stage d'application", Value = "Stage d'application" },
                };


        // GET: Etudiant
        SiteContext context = new SiteContext();


       // inscription 
        public ActionResult inscription()
        {
            ViewBag.niv = "";

            ViewBag.fil = new SelectList(context.filieres, "Id_filiere", "Nom_filiere");
          ViewBag.cycle = new SelectList(context.cycles, "id_Cycle", "nom_Cycle");
       
            return View();
        }


        public JsonResult Js(int h)
        {
         
            context.Configuration.ProxyCreationEnabled = false;

            return Json(context.niveaux.Where(x => x.code_cyc == h), JsonRequestBehavior.AllowGet);

        }


        [HttpPost]

        public ActionResult inscription(Etudiant e, HttpPostedFileBase file)
        {
            
            
          

            if (ModelState.IsValid == false)
            {
                ViewBag.fil = new SelectList(context.filieres, "Id_filiere", "Nom_filiere");
                ViewBag.cycle = new SelectList(context.cycles, "id_Cycle", "nom_Cycle");
                ViewBag.niv = Request.Form["niveau"];
                return View("inscription");
            }


            if (file != null && file.ContentLength > 0)
                try
                {
                    byte[] imageBytes = null;  
                    BinaryReader reader = new BinaryReader(file.InputStream);
                    imageBytes = reader.ReadBytes((int)file.ContentLength);

                    e.photo = imageBytes;
                   

                }
                catch (Exception ex)
                {
                   ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    return View("erreur");
                }

            context.etudiants.Add(e);
            
            context.SaveChanges();

            return RedirectToAction("index","Home",e);
            
        }


       

        //connexion




        public ActionResult connexion()
        {
            return View();
        }


     
        [HttpPost]
        public ActionResult connexion(Etudiant a)
        {
            Etudiant x = context.etudiants.SingleOrDefault(p => p.email.Equals(a.email) && p.password.Equals(a.password));

            


            if (x != null)
            {

                Etudiant b = new Etudiant();
                b = x;
                ViewBag.type = new SelectList(context.types, "id_type", "nom_type");
                Session["connectedStudent"] = x;
                return RedirectToAction("espaceetudiant");
            }

            return View();
        }

        //espace etudiant 
        [HttpGet]
        public ActionResult espaceetudiant() {

            Etudiant etd = Logged();


            if (etd == null)
            {
                return RedirectToAction("connexion");
            }

            ViewBag.erreur = "";
            ViewBag.niv = "";
            ViewBag.file = "";
            ViewBag.fil = new SelectList(context.filieres, "Id_filiere", "Nom_filiere");
            ViewBag.cycle = new SelectList(context.cycles, "id_Cycle", "nom_Cycle");
            ViewBag.type = new SelectList(context.types, "id_type", "nom_type");



            return View(etd);

}

        public ActionResult Archiver(int? id)
        {

            if (id == null)
                return RedirectToAction("Groupes");

            Etudiant etd = Logged();


            if (etd == null)
            {
                return RedirectToAction("connexion");
            }

            var grp = context.groupes.Where(g => g.id_admin == etd.cne && g.grp_id == id).Single();

            if (grp == null)
            {
                return RedirectToAction("espaceetudiant");
            }


            ViewBag.Type = list;
            ViewBag.idgrp = grp.grp_id;


            return View();
        }

        [HttpPost]
        public ActionResult Archiver(File archive)
        {

            Etudiant etd = Logged();


            if (etd == null)
            {
                return RedirectToAction("connexion");
            }

            var grp2 = context.groupes.Where(g => g.id_admin == etd.cne).Single();

            if (grp2 == null)
            {
                return RedirectToAction("Groupes");
            }

            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {

                    var memoryStream = new MemoryStream();
                    file.InputStream.CopyTo(memoryStream);
                    archive.Content = memoryStream.ToArray();
                    archive.Name = file.FileName;
                    archive.Length = file.ContentLength;

                    if (ModelState.IsValid)
                    {
                        var grp = context.groupes.Where(g => g.id_admin == etd.cne).Single();
                        archive.groupe_Id = grp.grp_id;
                        archive.Type = grp.Type.nom_type;
                        context.files.Add(archive);
                        context.SaveChanges();
                        return RedirectToAction("espaceetudiant");
                    }


                }
            }





            ViewBag.idgrp = grp2.grp_id;
            return View();
        }


        [HttpGet]
        public ActionResult InviterGroupe(int? id)
        {
            Etudiant etd = Logged();


            if (etd == null)
            {
                return RedirectToAction("connexion");
            }

            var member = context.GroupeMembres.Where(m => m.id_et == etd.cne && m.id_grp == id).Single();
            if (member == null)
            {
                return RedirectToAction("Groupes");
            }

            var list = member.Groupe.GroupeMembres.ToList();
            ViewBag.e = new SelectList(context.etudiants, "cne", "nom");
            ViewBag.idgrp = member.Groupe.grp_id;

            return View(list);

        }


        [HttpPost]
        public JsonResult AddToGroup(int? cne)
        {
            Etudiant etd = Logged();


            if (etd == null)
            {
                return Json("forbidden 403", JsonRequestBehavior.AllowGet);
            }

            var x = Request;
            var grp = context.groupes.Where(g => g.id_admin == etd.cne).Single();
            var currentGroupMembers = context.GroupeMembres.Where(g => g.id_grp == grp.grp_id);
            if (currentGroupMembers.Count() > 4)
            {
                return Json("full", JsonRequestBehavior.AllowGet);
            }
            var selectedStudent = currentGroupMembers.Where(e => e.id_et == cne).Count();

            if (selectedStudent == 0)
            {
                GroupeMembre groupe = new GroupeMembre
                {
                    id_et = cne,
                    id_grp = grp.grp_id
                };
                context.GroupeMembres.Add(groupe);
                context.SaveChanges();
                return Json("added", JsonRequestBehavior.AllowGet);
            }

            return Json("deja", JsonRequestBehavior.AllowGet);
        }


        public ActionResult Groupes()
        {
            Etudiant etd = Logged();


            if (etd == null)
            {
                return RedirectToAction("connexion");
            }


            List<Groupe> list = new List<Groupe>();
            List<GroupeMembre> listGM = context.GroupeMembres.Where(g => g.id_et == etd.cne && g.confirmed == true).ToList();

            ViewBag.Invitation = context.GroupeMembres.Where(gm => gm.id_et == etd.cne && gm.confirmed == false).ToList();

            foreach (var gm in listGM)
            {
                list.Add(gm.Groupe);
            }


            return View(list);
        }


        public ActionResult Groupe(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Groupes");
            }

            Etudiant etd = Logged();
            if (etd == null)
            {
                return RedirectToAction("connexion");
            }

            var member = context.GroupeMembres.Where(m => m.id_et == etd.cne && m.id_grp == id).Single();
            if(member == null)
            {
                return RedirectToAction("Groupes");
            }
           
            ViewBag.IsAdmin = etd.cne == member.Groupe.id_admin;
            return View(member.Groupe);
        }

        public ActionResult AcceptInvit(string id)
        {

            Etudiant et = Logged();
            if (et == null)
            {
                return RedirectToAction("connexion");
            }

            var memberGroup = context.GroupeMembres.Where(g => g.id_et == et.cne && g.token == id).Single();
            if (memberGroup != null)
            {
                memberGroup.confirmed = true;
                memberGroup.token = "";
                context.SaveChanges();
            }


            return RedirectToAction("Groupes");
        }

        public ActionResult RefuseInvit(string id)
        {

            Etudiant et = Logged();
            if (et == null)
            {
                return RedirectToAction("connexion");
            }

            var memberGroup = context.GroupeMembres.Where(g => g.id_et == et.cne && g.token == id).Single();
            if (memberGroup != null)
            {
                context.GroupeMembres.Remove(memberGroup);
                context.SaveChanges();
            }
            return RedirectToAction("Groupes");
        }


        [HttpGet]

        public ActionResult NewGroup()
        {
            Etudiant etd = Logged();


            if (etd == null)
            {
                return RedirectToAction("connexion");
            }

           ViewBag.id_tp = new SelectList(context.types.ToList(), "id_type", "nom_type");
            return View();
        }

        [HttpPost]
        public ActionResult NewGroup(Groupe grp)
        {

            var enc = GetIdEncadrant();
            var etu = Logged();

            if (enc != null)
            {
                grp.id_enc = enc.Id;
                grp.id_admin = etu.cne;
            }

            if (ModelState.IsValid)
            {

                Guid g = Guid.NewGuid();
                string GuidString = Convert.ToBase64String(g.ToByteArray());

                context.groupes.Add(grp);
                context.SaveChanges();
                GroupeMembre members = new GroupeMembre()
                {
                    confirmed = true,
                    id_et = grp.id_admin,
                    id_grp = grp.grp_id,
                    token = GuidString
                };

                context.GroupeMembres.Add(members);
                context.SaveChanges();

                return RedirectToAction("Groupes");
            }


            ViewBag.id_tp = new SelectList(context.types.ToList(), "id_type", "nom_type");
            return View();
        }

        public Etudiant Logged()
        {
            return Session["connectedStudent"] as Etudiant;
        }


        Encadrant GetIdEncadrant()
        {

            return context.encadrants.Where(c => c.groupes.Count() < 4).Single();
        }

        public ActionResult notification()
        {
            ViewBag.erreur = "";
            return View();
        }

        [HttpPost]
        public ActionResult notification(string k)
        { Etudiant et= (Etudiant)Session["connectedStudent"];
            ViewBag.erreur = "";
            if (Request.Form["valider"] != null) {

                int idgroupevalider = Convert.ToInt32(Request.Form["valider"]);
                Groupe thatsone = context.groupes.SingleOrDefault(x => x.grp_id == idgroupevalider);
                var allrequests = context.GroupeMembres.Where(x =>  x.id_et == et.cne);
                foreach(var req in allrequests)
                {
                    if (req.Groupe.Type.id_type == thatsone.Type.id_type && req.confirmed == true) {
                        ViewBag.erreur = "vous avez deja confirmer ce type de projet veuillez refuser ";
                        return View("notification");
                        
                    }                }
                context.GroupeMembres.SingleOrDefault(x => x.id_grp == idgroupevalider && x.id_et==et.cne).confirmed = true;
                context.SaveChanges();
                RedirectToAction("connexion");

            }
            if (Request.Form["refuser"] != null) {
                int idgrouperefuser = Convert.ToInt32(Request.Form["refuser"]);
                GroupeMembre mustdelet = context.GroupeMembres.SingleOrDefault(x => x.id_grp == idgrouperefuser && x.id_et == et.cne);
                context.GroupeMembres.Remove(mustdelet);
                context.SaveChanges();
                return View("notification");
            }
            

            return View();
        }
        //information
        public ActionResult byget()
        {
            Etudiant etd = Logged();


            if (etd == null)
            {
                return RedirectToAction("connexion");
            }

            ViewBag.type = new SelectList(context.types, "id_type", "nom_type");
            Etudiant x =(Etudiant) Session["connectedStudent"];
            return View("espaceetudiant",x);
        }
        //deconnexion
            public ActionResult Deconnexion()
    {
            Session["connectedStudent"] = null;
            Session["groupe"] = null;
        return RedirectToAction("Connexion");
    }
 }
}