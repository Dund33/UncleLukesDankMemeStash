using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace UncleLukesDankMemeStash.Areas.Identity
{
    public class MemeAuthor : IdentityUser
    {
        [Required] public string FirstName { get; set; }

        [Required] public string LastName { get; set; }

        [Required] public bool Admin { get; set; }
    }
}