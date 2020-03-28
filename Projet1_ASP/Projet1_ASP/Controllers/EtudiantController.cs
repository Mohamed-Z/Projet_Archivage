
using Projet1_ASP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Projet1_ASP.Controllers
{
    public class EtudiantController : Controller
    {
        // GET: Etudiant
        SiteContext context = new SiteContext();



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
            
            
          /*  e.id_fil = Convert.ToInt32( Request.Form["filiere"]);
            e.id_cyc = Convert.ToInt32(Request.Form["cycle"]);
           string nomniveau = Request.Form["niveau"];
            var x = context.niveaux.SingleOrDefault(p => p.nom_Niveau.Equals(nomniveau));
             e.id_niv =x.id_Niveau;
             */

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
                    string path = Path.Combine(Server.MapPath("~/Images"),Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    ////// base de données
                    var length = file.InputStream.Length; //Length: 103050706
                    MemoryStream target = new MemoryStream();
                    file.InputStream.CopyTo(target); // generates problem in this line
                    byte[] data = target.ToArray();
                    e.photo = data;
                   

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


        public ActionResult espaceetudiant() {
            ViewBag.fil = new SelectList(context.filieres, "Id_filiere", "Nom_filiere");
            ViewBag.niv = new SelectList(context.cycles, "id_Cycle", "nom_Cycle");
            return View();
        }







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

                //return View("espaceetudiant", "etudiant", x);
                return RedirectToAction("espaceetudiant", "etudiant", b);
            }
            return View();
        }
        public ActionResult erreur()
        {
            return View();
        }

        public ActionResult test()
        {
            return View();
        }




    }
}