using Authentication_Authorization.Models.Entities.Concrete;
using System.ComponentModel.DataAnnotations;

namespace Authentication_Authorization.Models.DTOs
{
    public class UserUpdateDTO
    {
        [Required(ErrorMessage ="Must to type into username")]
        [Display(Name = "User Name")]
        [MinLength(3,ErrorMessage ="Minimum length is 3..!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Must to type into password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Must to type into email adree")]
        [Display(Name = "Email Adress")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public UserUpdateDTO() { }
        public UserUpdateDTO(AppUser appUser)
        {
            UserName = appUser.UserName;
            Password = appUser.PasswordHash;
            Email = appUser.Email;
        }
    }
}
