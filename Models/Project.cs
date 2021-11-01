using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace ThePortfolio.Models
{
    public class Project
    {
        [Key]
        [DisplayName("ID")]
        public int Id { get; set; }
        [DisplayName("Name")]
        public string ProjectName { get; set; }
        [DisplayName("Technologies Used")]
        public string TechnologiesUsed { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }
        [DisplayName("Project Picture")]
        public string ProjectPicture { get; set; }

    }
}
