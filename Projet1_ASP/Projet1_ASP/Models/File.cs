using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Projet1_ASP.Models
{
    public class File
    {
        [Key]
        [ForeignKey("Groupe")]
        public int groupe_Id { get; set; }

        public string Name { get; set; }
        public string Type { get; set; }
        public int Length { get; set; }
        public byte[] Content { get; set; }
        public string sujet { get; set; }
        public string description { get; set; }

        
        public virtual Groupe Groupe { get; set; }

    }
}