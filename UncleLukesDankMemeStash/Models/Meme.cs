using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.CodeAnalysis;
using UncleLukesDankMemeStash.Areas.Identity;
using UncleLukesDankMemeStash.Resources;

namespace UncleLukesDankMemeStash.Models
{
    public class Meme : IDisplayable
    {
        [Display(Name = "Comment", ResourceType = typeof(MemeLocalization))]
        public string Comment { get; set; }

        [Display(Name = "Category", ResourceType = typeof(MemeLocalization))]
        [ForeignKey("Category")]
        public int CategoryID { get; set; }

        [Display(Name = "Category", ResourceType = typeof(MemeLocalization))]
        public Category Category { get; set; }

        [Display(Name = "User", ResourceType = typeof(MemeLocalization))]
        [ForeignKey("MemeAuthor")]
        public string UserID { get; set; }

        [Display(Name = "User", ResourceType = typeof(MemeLocalization))]
        public MemeAuthor User { get; set; }

        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Meme title is required")]
        [Display(Name = "Title", ResourceType = typeof(MemeLocalization))]
        public string Title { get; set; }

        [Required(ErrorMessage = "Image URL is required")]
        [Url]
        [Display(Name = "URL")]
        public string ImageURL { get; set; }
    }
}