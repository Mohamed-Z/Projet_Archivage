using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projet1_ASP.Models
{
    public class Cycle
    {
        public virtual ICollection<Etudiant> etudiants { get; set; }

        public Cycle()
        {
            this.etudiants = new HashSet<Etudiant>();
        }

        [Key]
        public int id_Cycle { get; set; }
        public string nom_Cycle { get; set; }
    }
}