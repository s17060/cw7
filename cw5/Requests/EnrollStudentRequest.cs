using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cw5.Requests
{
    public class EnrollStudentRequest
    {
        [Required(ErrorMessage ="First Name required")]
        [MaxLength(255)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name required")]
        [MaxLength(255)]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Index Number required")]
        [RegularExpression("^s[0-9]+$")]
        public string IndexNumber { get; set; }
        [Required(ErrorMessage = "Birth Date required")]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Studies required")]
        [MaxLength(255)]
        public string Studies { get; set; }
    }
}
