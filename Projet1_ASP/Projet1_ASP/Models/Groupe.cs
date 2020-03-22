using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Projet1_ASP.Models
{
    public class Groupe
    {
        public virtual ICollection<Etudiant> etudiants { get; set; }

        public Groupe()
        {
            this.etudiants = new HashSet<Etudiant>();
        }

        [Key]
        public int grp_id { get; set; }
        public string type { get; set; }

        [ForeignKey("Encadrant")]
        public Nullable<int> id_enc { get; set; }
        public virtual Encadrant Encadrant { get; set; }

        public virtual File File { get; set; }
    }
}