using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projet1_ASP.Models
{
    public class SuperUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string nom { get; set; }

        [Required]
        public string prenom { get; set; }

        [RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$", ErrorMessage = "Format de l'email n'est pa correcte")]
        public string email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Mot de passe faible")]
        public string password { get; set; }

        [Compare("password")]
        public string confirmation { get; set; }
    }
}