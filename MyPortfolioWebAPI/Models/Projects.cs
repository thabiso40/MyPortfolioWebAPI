using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyPortfolioWebAPI.Models
{
    public class Projects
    {
        [Key]
        public long ID { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string GithubLink { get; set; } = string.Empty;
        public string URLLink { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        [NotMapped]
        public IFormFile ImageFile { get; set; } = null!;

        [NotMapped]
       public string ImageSrc { get; set; } = string.Empty;
    }
}
