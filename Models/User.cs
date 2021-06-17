using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SmartSTask.Models
{
    public class User : IdentityUser<int>
    {
        [Required(ErrorMessage = "Please Enter Username..")]
        [Display(Name = "Name")]
        public string FirstName { get; set; }

        [Display(Name = "Surname")]
        public string LastName { get; set; }

        public bool IsAdmin { get; set; }

        public int OrganizationId { get; set; }

        public virtual Organization Organization { get; set; }

        public int? TaskId { get; set; }

        public virtual ManageTask Task { get; set; }
    }
}
