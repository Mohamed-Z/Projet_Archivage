using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projet1_ASP.Models
{
    public class Encadrant
    {
        public virtual ICollection<Groupe> groupes { get; set; }

        public Encadrant()
        {
            this.groupes = new HashSet<Groupe>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string nom { get; set; }
        public string prenom { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int nbr_grp { get; set; }
    }
}