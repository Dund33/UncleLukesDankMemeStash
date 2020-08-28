using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UncleLukesDankMemeStash.Models
{
    public class Category: IDisplayable
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [DisplayName("Kategoria")]
        public string Title { get; set; }
        [DisplayName("Opis")]
        public string Description { get; set; }
        [Required]
        [Url]
        public string ImageURL { get; set; }
    }
}
