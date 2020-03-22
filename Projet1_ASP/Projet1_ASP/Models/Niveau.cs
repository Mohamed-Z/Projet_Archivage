using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projet1_ASP.Models
{
    public class Niveau
    {
        public virtual ICollection<Etudiant> etudiants { get; set; }

        public Niveau()
        {
            this.etudiants = new HashSet<Etudiant>();
        }

        [Key]
        public int id_Niveau { get; set; }
        public string nom_Niveau { get; set; }
    }
}