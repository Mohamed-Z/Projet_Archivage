
using Projet1_ASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projet1_ASP.Controllers
{
    public class EtudiantController : Controller
    {
        // GET: Etudiant
        SiteContextt context = new SiteContextt();



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

        public ActionResult erreur()
        {
            return View();
        }






    }
}