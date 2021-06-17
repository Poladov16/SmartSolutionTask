using SmartSTask.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSTask.Models.ViewModels
{
    public class ManageTaskViewModel
    {
        public int TaskId { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Deadline")]
        public DateTime Deadline { get; set; }

        public int Status { get; set; }

        public string StatusName { get; set; }

        public int  TaskUserId { get; set; }

        public virtual IEnumerable<User> AssignedUsers { get; set; }
    }
}
