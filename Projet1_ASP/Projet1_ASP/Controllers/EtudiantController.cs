
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

            Session["connectedStudent"] = x;


            if (x != null)
            {

                Etudiant b = new Etudiant();
                b = x;
                ViewBag.type = new SelectList(context.types, "id_type", "nom_type");
                return View("espaceetudiant", x);
            }

            return View();
        }

        //espace etudiant 
        [HttpGet]
        public ActionResult espaceetudiant(Etudiant x) {
            ViewBag.erreur = "";
            ViewBag.niv = "";
            ViewBag.file = "";
            ViewBag.fil = new SelectList(context.filieres, "Id_filiere", "Nom_filiere");
            ViewBag.cycle = new SelectList(context.cycles, "id_Cycle", "nom_Cycle");
            ViewBag.type = new SelectList(context.types, "id_type", "nom_type");



            return View(x);

}
        [HttpPost]
        public ActionResult espaceetudiant()
        {
            
            ViewBag.erreur = "";
            ViewBag.niv = "";
             ViewBag.file = "";
          
            ViewBag.type = new SelectList(context.types, "id_type", "nom_type");
            
            Etudiant e = (Etudiant)Session["connectedStudent"];
            //if vous avez deja initialiser un groupe de ce type
            var groupedetudiant = context.GroupeMembres.Where(x => x.id_et == e.cne);
            if (groupedetudiant != null) {
                foreach (var mem in groupedetudiant)
                {
                    var typedegroupemembre = context.groupes.SingleOrDefault(x => x.grp_id == mem.id_grp);
                    if (typedegroupemembre != null )
                    {
                        if (typedegroupemembre.id_tp == Convert.ToInt32(Request.Form["type"]) && mem.confirmed==true )
                        {
                            Groupe grp = (Groupe)typedegroupemembre;
                            Session["groupe"] = grp;
                            ViewBag.erreur = "vous etes deja un membre de ce type de projet et vous avez confirmer ca";
                            return RedirectToAction("inviterGroupe");
                        }
                    }

                }

            } 

            //sinon
                Groupe g = new Groupe();
                Random rnd = new Random();
                var list = context.encadrants.ToList();
                int r = rnd.Next(list.Count);
                g.id_tp = Convert.ToInt32(Request.Form["type"]);
                g.id_enc = list[r].Id;
            context.encadrants.SingleOrDefault(x => x.Id == g.id_enc).nbr_grp = g.grp_id;
                context.groupes.Add(g);
                Session["groupe"] = g;


             
              /*  GroupeMembre groupemembre = new GroupeMembre();
                groupemembre.id_grp = g.grp_id;
                groupemembre.id_et = e.cne;
                DateTime localDate = DateTime.Now;
                groupemembre.date = Convert.ToString(localDate);
                context.GroupeMembres.Add(groupemembre);*/
                context.SaveChanges();

                return RedirectToAction("inviterGroupe");
           
        }
      

            // a traiter 

            public ActionResult Archiver()
        {
            ViewBag.accord = "";

          

           return View();
        }


        [HttpPost]
        public ActionResult Archiver(Models.File archive, HttpPostedFileBase file)
        {
            ViewBag.accord = "";
            ViewBag.type = new SelectList(context.types, "id_type", "nom_type");
            Etudiant et = (Etudiant)Session["connectedStudent"];

            // ViewData["type"] = new SelectList(list);
            Groupe grp = (Groupe)Session["groupe"];
            GroupeMembre membre = context.GroupeMembres.SingleOrDefault(x => x.id_et == et.cne && x.id_grp==grp.grp_id);
            var mmbrdecegroupe = context.GroupeMembres.Where(x => x.id_grp == grp.grp_id);
            foreach(GroupeMembre k in mmbrdecegroupe)
            {
                if (k.confirmed == false)
                {
                    ViewBag.accord = "veuillez verifier que tous les membre sont accepter ou refuser de rejoindrer le groupe";
                    return View("archiver");

                }

            }


            File existfile = context.files.SingleOrDefault(p => p.groupe_Id == membre.id_grp);

            if (Request.Files.Count > 0)
            {
                //var file = Request.Files[0];
                
                if (file != null && file.ContentLength > 0)
                {

                    var memoryStream = new MemoryStream();
                    file.InputStream.CopyTo(memoryStream);
                    archive.Content = memoryStream.ToArray();
                    archive.Name = file.FileName;
                    archive.Length = file.ContentLength;
                    


                    
                        if (existfile == null)
                        {
                            archive.groupe_Id = grp.grp_id;
                            archive.Type = grp.Type.nom_type ;
                            context.files.Add(archive);
                            context.SaveChanges();
                            

                            ViewBag.file = "vous n'avez pas encore mettez aucun file";
                        return View("espaceetudiant", et);
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










            return View();
        }

       
           [HttpGet]
            public ActionResult InviterGroupe(Groupe g)
            {

            
            Etudiant et = (Etudiant) Session["connectedStudent"] ;
            Groupe grp = (Groupe)Session["groupe"];
           
            var list= context.GroupeMembres.Where(x => x.id_grp ==grp.grp_id).ToList();
            ViewBag.e = new SelectList(context.etudiants, "cne","nom");
       
            return View(list);

            }
            

       

            

                [HttpPost]
                public JsonResult AddToGroup(int? cne)
                {
                    Etudiant et = (Etudiant)Session["connectedStudent"];
                    var x = Request;
                    Groupe grp = (Groupe)Session["groupe"];
                    var currentGroupMembers = context.GroupeMembres.Where(g => g.id_et == et.cne);
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