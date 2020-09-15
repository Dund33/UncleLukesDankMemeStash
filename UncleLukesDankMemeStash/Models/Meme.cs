using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UncleLukesDankMemeStash.Areas.Identity;

namespace UncleLukesDankMemeStash.Models
{
    public class Meme : IDisplayable
    {
        [Display(Name = "Comment")] 
        public string Comment { get; set; }

        [Display(Name = "Category")]
        [ForeignKey("Category")]
        public int CategoryID { get; set; }

        [Display(Name = "Category")] 
        public Category Category { get; set; }

        [Display(Name = "User")]
        [ForeignKey("MemeAuthor")]
        public string UserID { get; set; }

        [Display(Name = "User")] 
        public MemeAuthor User { get; set; }

        [Key] 
        public int ID { get; set; }

        [Required(ErrorMessage = "Meme title is required")] 
        [Display( Name = "Title")] 
        public string Title { get; set; }

        [Required(ErrorMessage = "Image URL is required")]
        [Url] 
        public string ImageURL { get; set; }
    }
}