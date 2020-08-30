using System.ComponentModel.DataAnnotations;
using UncleLukesDankMemeStash.Areas.Identity;

namespace UncleLukesDankMemeStash.Models
{
    public class RegisterModel : MemeAuthor
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}