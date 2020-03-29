using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Projet1_ASP.Models
{
    public class GroupeMembre
    {
        [Key]
        public int id_gm { get; set; }

        [ForeignKey("Groupe")]
        public Nullable<int> id_grp { get; set; }
        public virtual Groupe Groupe { get; set; }
        [ForeignKey("Etudiant")]
        public Nullable<int> id_et { get; set; }
        public virtual Etudiant Etudiant { get; set; }
        public string date { get; set; }
        public Boolean confirmed { get; set; }
        public String token { get; set; }
    }
}