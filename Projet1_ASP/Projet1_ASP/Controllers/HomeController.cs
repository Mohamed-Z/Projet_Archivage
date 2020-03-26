
using Projet1_ASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projet1_ASP.Controllers
{
    public class HomeController : Controller
    {
        SiteContext context = new SiteContext();

        public ActionResult Index()
        {
           

            return View();
        }
        [HttpPost]
        public ActionResult Index(Etudiant a)

        {
            //Session["etudiant"] = null;
            Etudiant x = context.etudiants.SingleOrDefault(p => p.email.Equals(a.email)&& p.password.Equals(a.password));

            if (x!=null) {

                Etudiant b = new Etudiant();
                b = x;

                //return View("espaceetudiant", "etudiant", x);
                return RedirectToAction("espaceetudiant", "etudiant",b);
            }

            return View();
        }

        
    }
}