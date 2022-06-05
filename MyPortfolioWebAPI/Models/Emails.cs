using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MyPortfolioWebAPI.Models
{
    public class Emails
    {
        [Key]
        public long ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

    }
}
