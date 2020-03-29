
using Projet1_ASP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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



        public ActionResult inscription()
        {

            ViewBag.fil = new SelectList(context.filieres, "Id_filiere", "Nom_filiere");
          ViewBag.niv = new SelectList(context.niveaux, "id_Niveau", "nom_Niveau");
           // ViewBag.cycle = new SelectList(context.niveaus, "id_Cycle", "nom_Cycle");
            return View();
        }



        [HttpPost]
        public ActionResult inscription(String b)
        {
            
            Etudiant e = new Etudiant();

            e.cne =Convert.ToInt32( Request.Form["cne"]);
            DateTime dateTime = DateTime.Parse(Request.Form["date"]);
            
            e.email = Request.Form["email"];
            e.prenom = Request.Form["prenom"];
            e.nom = Request.Form["nom"];
            e.password = Request.Form["passe"];
            e.date_naiss = Request.Form["date"];
           
            e.id_fil = Convert.ToInt32( Request.Form["filiere"]);
            e.id_niv = Convert.ToInt32(Request.Form["niveau"]);
            var x = context.etudiants.SingleOrDefault(p => p.email.Equals(e.email));
            if (x != null)
            {
                return View("erreur");
            }
            else { 
            context.etudiants.Add(e);
            context.SaveChanges();

           

           //Session["etudiant"] = e;
            return RedirectToAction("index","Home",e);
            }
        }


        public ActionResult espaceetudiant() {
            ViewBag.fil = new SelectList(context.filieres, "Id_filiere", "Nom_filiere");
            ViewBag.niv = new SelectList(context.niveaux, "id_Niveau", "nom_Niveau");
            return View();
        }

        public ActionResult connexion()
        {
            return View();
        }

        public ActionResult erreur()
        {
            return View();
        }


        public ActionResult Archiver()
        {
            Etudiant etd = context.etudiants.Find(1);
            Session["connectedStudent"] = etd;
           

            ViewBag.Type = list;


           return View();
        }

        [HttpPost]
        public ActionResult Archiver(Models.File archive)
        {
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

                    if(ModelState.IsValid)
                    {
                        Etudiant et = (Etudiant)Session["connectedStudent"];
                        var grp = context.groupes.Where(g => g.id_admin == et.cne).Single();
                        archive.groupe_Id = grp.grp_id;
                        grp.type = archive.Type;
                        context.files.Add(archive);
                        context.SaveChanges();
                        return RedirectToAction("espaceetudiant");
                    }


                }
            }

            




            return View();
        }



        [HttpGet]
        public ActionResult InviterGroupe()
        {

            Etudiant et = context.etudiants.Find(1);
            Session["connectedStudent"] = et;

            var grp = context.groupes.Where(g => g.id_admin == et.cne).Single();

            if (grp == null)
            {
                return RedirectToAction("espaceetudiant");
            }
      
           var list = context.GroupeMembres.Where(g => g.id_grp == grp.grp_id).ToList();

            ViewBag.e = new SelectList(context.etudiants, "cne","nom");

            return View(list);
        }


        [HttpPost]
        public JsonResult AddToGroup(int? cne)
        {
            Etudiant et = (Etudiant)Session["connectedStudent"];
            var x = Request;
            var grp = context.groupes.Where(g => g.id_admin == et.cne).Single();
            var currentGroupMembers = context.GroupeMembres.Where(g => g.id_grp == grp.grp_id);
            if(currentGroupMembers.Count() > 4)
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

    }
}