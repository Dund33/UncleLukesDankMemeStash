using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UncleLukesDankMemeStash.Areas.Identity;

namespace UncleLukesDankMemeStash.Models
{
    public class RegisterModel: MemeAuthor
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
