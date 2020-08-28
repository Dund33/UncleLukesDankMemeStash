using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UncleLukesDankMemeStash.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Podaj prawidłową nazwę użytkownika")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Podaj prawidłowe hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
