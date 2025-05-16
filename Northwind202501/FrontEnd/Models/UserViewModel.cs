using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace FrontEnd.Models
{
    public class UserViewModel
    {

        [Required]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }


        public bool  RememberLogin { get; set; }

        public string ? ReturnUrl { get; set; }
    }
}
