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

        [Required]
        public int cne { get; set; }
        [Required]
        public string nom { get; set; }
        [Required]
        public string prenom { get; set; }
        [Required]
        public string date_naiss { get; set; }
        [Required]
        [RegularExpression(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))"+@"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$")]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        [ForeignKey("Filiere")]
        public Nullable<int> id_fil { get; set; }
        [Required]
        [ForeignKey("Cycle")]
        public Nullable<int> id_cyc { get; set; }
        [Required]
        [ForeignKey("Niveau")]
        public Nullable<int> id_niv { get; set; }
        
        public byte[] photo { get; set; }

        
        public virtual Filiere Filiere { get; set; }
        public virtual Cycle Cycle { get; set; }
        public virtual Niveau Niveau { get; set; }
    }
}