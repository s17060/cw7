using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cw5.Requests
{
    public class PromoteStudentRequest
    {
        [Required(ErrorMessage = "Studies required")]
        [MaxLength(255)]
        public string Studies { get; set; }
        [Required(ErrorMessage = "Semester required")]
        [RegularExpression("^[1-9]$")]
        public int Semester { get; set; }
    }
}
