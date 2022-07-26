using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TelerikFileUpload_MVC.Models
{
    public class Attachments
    {
        public int id { get; set; }
        public int vid { get; set; }

        [Required(ErrorMessage ="Required!")]
        [Display(Name ="File Name")]
        public string fileName { get; set; }

        [Display(Name = "File Description")]
        public string fileDesc { get; set; }

        [Display(Name = "File Path")]
        public string filePath { get; set; }

        public bool status { get; set; }
    }
}