using System.ComponentModel.DataAnnotations;

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