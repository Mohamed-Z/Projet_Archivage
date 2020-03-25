using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Projet1_ASP.Models
{
    public class Etudiant
    {
        public virtual ICollection<GroupeMembre> GroupeMembres { get; set; }

        public Etudiant()
        {
            this.GroupeMembres = new HashSet<GroupeMembre>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cne { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public string date_naiss { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        [ForeignKey("Filiere")]
        public Nullable<int> id_fil { get; set; }

        [ForeignKey("Cycle")]
        public Nullable<int> id_cyc { get; set; }

        [ForeignKey("Niveau")]
        public Nullable<int> id_niv { get; set; }

        public byte[] photo { get; set; }

        
        public virtual Filiere Filiere { get; set; }
        public virtual Cycle Cycle { get; set; }
        public virtual Niveau Niveau { get; set; }
    }
}