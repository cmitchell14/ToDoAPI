using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ToDoAPI.API.Models
{
    public class ToDoViewModel
    {
        [Key]
        public int TodoId { get; set; }

        [Required]
        public string Action { get; set; }

        [Required] 
        public bool Done { get; set; }

        public int CategoryId { get; set; }

        public virtual CategoryViewModel Category { get; set; }
    }

    public class CategoryViewModel
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "*** 50 character max")]
        public string Name { get; set; }

        [StringLength(100, ErrorMessage = "*** 100 character max")]
        public string Description { get; set; }
    }
}