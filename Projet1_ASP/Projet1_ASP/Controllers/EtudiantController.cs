
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
          //  ViewBag.type = new SelectList(context.types, "id_type", "nom_type");



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
                            

                            ViewBag.file = "votre file est chargé avec succées";
                        return View("espaceetudiant", et);
                    }


                        if (grp == null)
                        {
                            ViewBag.file = "vous n avez pas le droit vous etes pas un admin";
                            return View("Archiver", et);
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
            ViewBag.e = new SelectList(context.etudiants.Where(x=>x.Filiere.Id_filiere==et.id_fil && x.id_cyc == et.id_cyc && x.id_niv==et.id_niv), "cne","nom");
       
            return View(list);

            }


        public ActionResult Crrer_Groupe(Groupe g)
        {
            ViewBag.type = new SelectList(context.types, "id_type", "nom_type");
            return View();
        }




                [HttpPost]
                public JsonResult AddToGroup(int? cne)
                {
                    Etudiant et = (Etudiant)Session["connectedStudent"];
                    var x = Request;
                    Groupe grp = (Groupe)Session["groupe"];
                    var currentGroupMembers = context.GroupeMembres.Where(g => g.id_grp == grp.grp_id);
                    if(currentGroupMembers.Count() > 4)
                    {
                        return Json("full", JsonRequestBehavior.AllowGet);
                    }
                    var selectedStudent = currentGroupMembers.Where(e => e.id_et == cne).Count();

            var grpmembretypeexiste = context.GroupeMembres.Where(l => l.id_et == cne);
            

                    if (selectedStudent == 0)
                    {DateTime localDate = DateTime.Now;
                        GroupeMembre groupe = new GroupeMembre
                        {
                            id_et = cne,
                            id_grp = grp.grp_id,
                            
                         date = Convert.ToString(localDate),

            };
               bool typeexiste = false;

                foreach (GroupeMembre k in grpmembretypeexiste) {
                    if (k.Groupe.id_tp==grp.id_tp && k.confirmed==true) {
                        typeexiste = true;
                    } }

                if (typeexiste == false)
                {
                    context.GroupeMembres.Add(groupe);
                    context.SaveChanges();
                    return Json("added", JsonRequestBehavior.AllowGet);
                }

                else if(typeexiste == true){

                    return Json("autregroupe", JsonRequestBehavior.AllowGet);
                }
                       
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
    
    //********* Tache_Archive_Detaisl *********//

        //Variable globales
        List<Models.File> liste_fichiers = new List<Models.File>();
        List<String> liste_types = new List<string>();
        List<Encadrant> liste_encadrants = new List<Encadrant>();
        List<List<Etudiant>> liste_groupes = new List<List<Etudiant>>();
        List<int> x = new List<int>();

        //Action_Archive_Details
        public ActionResult Archive_Details()
        {
            //variables
            Models.File f;
            Encadrant e;
            Etudiant et;
            int id_encadrant, id_typ;
            List<int> liste_id_etudiants = new List<int>();
            List<Etudiant> liste_etudiants = new List<Etudiant>();


            //code
            //recuperation des id des groupes ou lesquelles apaprtient un etudiant pour les utiliser afin de determiner les rapports
            //et les encaadrants
            Etudiant etud = (Etudiant)Session["connectedStudent"];

            x = (List<int>)(context.GroupeMembres.Where(g => g.id_et == etud.cne).Select(g => g.id_grp)).Cast<int>().ToList();
            foreach (int i in x)
            {
                f = (Models.File)context.files.Where(p => p.groupe_Id == i).FirstOrDefault();
                liste_fichiers.Add(f);
                //recuperation de l'id de l'encadrants lequel encadre le groupe i
                id_encadrant = context.groupes.Where(p => p.grp_id == i).Select(p => p.id_enc).Single().GetValueOrDefault();
                // y = (from gr in context.groupes where gr.grp_id==1 select new {gr.id_enc}).SingleOrDefault();
                //recuperation de l'encadrant
                e = (Encadrant)context.encadrants.Where(l => l.Id == id_encadrant).FirstOrDefault();
                liste_encadrants.Add(e);
                //recuperation des id des etudiants du meme groupe
                liste_id_etudiants = (context.GroupeMembres.Where(m => m.id_grp == i).Select(m => m.id_et)).Cast<int>().ToList();

                //recuperation des etudiants 
                foreach (int j in liste_id_etudiants)
                {
                    et = (Etudiant)context.etudiants.Where(n => n.cne == j).FirstOrDefault();
                    liste_etudiants.Add(et);
                }

                liste_groupes.Add(liste_etudiants.ToList());

                //liste des types
                id_typ = context.groupes.Where(o => o.grp_id == i).Select(o => o.id_tp).Single().GetValueOrDefault();
                liste_types.Add((String)(context.types.Where(t => t.id_type == id_typ).Select(t => t.nom_type).Single()));
                //Vider les listes qui sont besoin 
                liste_id_etudiants.Clear();
                liste_etudiants.Clear();

            }

            //instanciation du ViewModele
            ArchiveDetailsViewModel archive = new ArchiveDetailsViewModel
            {
                liste_rapports = liste_fichiers,
                liste_encadrant = liste_encadrants,
                liste_groupes_etudiants = liste_groupes,
                liste_type = liste_types
            };
            return View(archive);
        }

        [HttpGet]
        public ActionResult GetFile(int ID)
        {

            //Recuperer le rapport
            Models.File file = context.files.Find(ID);

            //If file exists....

            MemoryStream ms = new MemoryStream(file.Content, 0, 0, true, true);
            Response.ContentType = file.Type;
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
}
