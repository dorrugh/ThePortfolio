using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ThePortfolio.ViewModels
{
    public class ProjectViewModel: EditImageViewModel
    {
        [DisplayName("Name")]
        public string ProjectName { get; set; }
        [DisplayName("Technologies Used")]
        public string TechnologiesUsed { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }
    }
}
