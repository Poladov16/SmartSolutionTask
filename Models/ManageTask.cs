using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSTask.Models
{
    public class ManageTask
    {
        [Key]
        public int TaskId { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Deadline")]
        public DateTime Deadline { get; set; }

        public int Status { get; set; }

        [NotMapped]
        public string StatusName { get; set; }

        public virtual IEnumerable<User> AssignedUsers { get; set; }

        //[Display(Name = "User")]
        //public int UserId { get; set; }//icraatçı
    }
}
