using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace UncleLukesDankMemeStash.Areas.Identity
{
    public class MemeAuthor : IdentityUser
    {
        [Required] public string FirstName { get; set; }

        [Required] public string LastName { get; set; }

        [Required] public bool Admin { get; set; }
    }
}