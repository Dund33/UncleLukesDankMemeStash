using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UncleLukesDankMemeStash.Models
{
    public class Category : IDisplayable
    {
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [Display(Name = "Category")]
        [Localizable(true)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Image URL is required")]
        [Url]
        public string ImageURL { get; set; }
    }
}