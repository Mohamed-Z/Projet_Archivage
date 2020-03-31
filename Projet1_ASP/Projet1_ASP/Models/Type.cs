using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projet1_ASP.Models
{
    public class Type
    {
        public virtual ICollection<Groupe> groupes { get; set; }

        public Type()
        {
            this.groupes = new HashSet<Groupe>();
        }

        [Key]
        public int id_type { get; set; }

        public String nom_type { get; set; }
    }
}