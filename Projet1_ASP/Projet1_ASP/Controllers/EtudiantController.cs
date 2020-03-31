
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
                    string path = Path.Combine(Server.MapPath("~/Images"),Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    ////// base de données
                    byte[] imageBytes = null;  
                    BinaryReader reader = new BinaryReader(file.InputStream);
                    imageBytes = reader.ReadBytes((int)file.ContentLength);

                    
                /*    var length = file.InputStream.Length; //Length: 103050706
                    MemoryStream target = new MemoryStream();
                    file.InputStream.CopyTo(target); // generates problem in this line
                    byte[] data = target.ToArray();*/
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

            Session["connectedStudent"] = x;


            if (x != null)
            {

                Etudiant b = new Etudiant();
                b = x;
                
                return View("espaceetudiant", x);
            }

            return View();
        }
      
        //espace etudiant 

        public ActionResult espaceetudiant(Etudiant x) {
            ViewBag.niv = "";
            ViewBag.file = "";
           
              
            ViewBag.fil = new SelectList(context.filieres, "Id_filiere", "Nom_filiere");
            ViewBag.cycle = new SelectList(context.cycles, "id_Cycle", "nom_Cycle");
           

            return View(x);
        }
        [HttpPost]
        public ActionResult espaceetudiant()
        {
            ViewBag.file = "";


           // context.SaveChanges();

            return View();
        }

// a traiter 

        public ActionResult Archiver()
        {
          

            ViewBag.Type = list;


           return View();
        }

        [HttpPost]
        public ActionResult Archiver(Models.File archive)
        {   Etudiant et = (Etudiant)Session["connectedStudent"];

           // ViewData["type"] = new SelectList(list);

             GroupeMembre membre =context.GroupeMembres.SingleOrDefault(x => x.id_et == et.cne);

             File existfile = context.files.SingleOrDefault(p => p.groupe_Id == membre.id_grp);
            
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
                    var grp = context.groupes.SingleOrDefault(g => g.id_admin == et.cne);
                    if (ModelState.IsValid)
                    {
                        if (existfile == null)
                        {
                            archive.groupe_Id = grp.grp_id;
                        grp.type = archive.Type;
                        context.files.Add(archive);
                        context.SaveChanges();

                            ViewBag.file = "vous n'avez pas encore mettez aucun file";
                        }
                       

                        if (grp == null)
                        {
                            ViewBag.file = "vous n avez pas le droit vous etes pas un admin";
                            return View("espaceetudiant", et);
                        }
                        
                            
                        
                        else
                        {
                            ViewBag.file = "vous avez le droit d instancier votre fichier qu'une seule fois";

                            return View("espaceetudiant", et);

                        } 
                    }


                }
            }

            




            return View();
        }



        [HttpGet]
        public ActionResult InviterGroupe()
        {

            Etudiant et = (Etudiant) Session["connectedStudent"] ;
           
            var grp = context.groupes.Where(g => g.id_admin == et.cne).Single();

            if (grp == null)
            {
                return RedirectToAction("espaceetudiant",et);
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
            var grp = context.groupes.SingleOrDefault(g => g.id_admin == et.cne);
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