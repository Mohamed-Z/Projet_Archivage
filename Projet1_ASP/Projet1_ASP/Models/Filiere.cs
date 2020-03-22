using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projet1_ASP.Models
{
    public class Filiere
    {
        public virtual ICollection<Etudiant> etudiants { get; set; }

        public Filiere()
        {
            this.etudiants = new HashSet<Etudiant>();
        }

        [Key]
        public int Id_filiere { get; set; }
        public string Nom_filiere { get; set; }

    }
}