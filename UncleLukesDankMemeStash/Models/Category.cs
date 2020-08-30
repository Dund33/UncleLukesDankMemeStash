using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UncleLukesDankMemeStash.Models
{
    public class Category : IDisplayable
    {
        [DisplayName("Opis")] public string Description { get; set; }

        [Key] public int ID { get; set; }

        [Required] [DisplayName("Kategoria")] public string Title { get; set; }

        [Required] [Url] public string ImageURL { get; set; }
    }
}