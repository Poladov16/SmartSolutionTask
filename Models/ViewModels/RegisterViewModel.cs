using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSTask.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Name")]
        public string FirstName { get; set; }

        [Display(Name = "Surname")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter Email...")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter Password...")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please Enter Organization Name..")]
        public string OrganizationName { get; set; }

        public string OrganizationPhone { get; set; }

        public string OrganizationAddress { get; set; }


    }
}
