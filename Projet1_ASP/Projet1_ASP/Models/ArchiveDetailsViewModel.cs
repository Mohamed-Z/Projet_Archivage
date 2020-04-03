using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Projet1_ASP.Models;

namespace Projet1_ASP.Models
{
    public class ArchiveDetailsViewModel
    {
       // public List<int,string> liste { get; set; }
        public List<File> liste_rapports { get; set; }
        public List<String> liste_type { get; set; }
        public List<Encadrant> liste_encadrant { get; set; }
        public List<List<Etudiant>> liste_groupes_etudiants { get; set; }
    }
}
