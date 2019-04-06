using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductSearch.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string code { get; set; }
        public string Name { get; set; }
        public bool AllowToEdit { get; set; }
    }
}