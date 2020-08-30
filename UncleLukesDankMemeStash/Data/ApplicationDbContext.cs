using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UncleLukesDankMemeStash.Areas.Identity;
using UncleLukesDankMemeStash.Models;

namespace UncleLukesDankMemeStash.Data
{
    public class ApplicationDbContext : IdentityDbContext<MemeAuthor>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Meme> Memes { get; set; }
        public DbSet<Category> Category { get; set; }
    }
}