using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UncleLukesDankMemeStash.Areas.Identity;

namespace UncleLukesDankMemeStash.Models
{
    public class Meme : IDisplayable
    {
        [DisplayName("Komentarz")] public string Comment { get; set; }

        [DisplayName("ID Kategorii")]
        [ForeignKey("Category")]
        public int CategoryID { get; set; }

        [DisplayName("Kategoria")] public Category Category { get; set; }

        [DisplayName("Użytkownik")]
        [ForeignKey("MemeAuthor")]
        public string UserID { get; set; }

        [DisplayName("Użytkownik")] public MemeAuthor User { get; set; }

        [Key] public int ID { get; set; }

        [Required] [DisplayName("Tytuł")] public string Title { get; set; }

        [Required] [Url] public string ImageURL { get; set; }
    }
}