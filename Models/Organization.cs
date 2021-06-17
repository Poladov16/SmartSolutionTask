using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSTask.Models
{
    public class Organization
    {
        [Key]
        public int OrganizationId { get; set; }

        [Required(ErrorMessage = "Please Enter Organization Name..")]
        public string OrganizationName { get; set; }

        public string OrganizationPhone { get; set; }

        public string OrganizationAddress { get; set; }

        public virtual IEnumerable<User> Users { get; set; }
    }
}
