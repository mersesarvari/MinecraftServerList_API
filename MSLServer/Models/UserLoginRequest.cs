using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MSLServer.Models
{
    [Table("UserLoginRequest")]
    public class UserLoginRequest
    {
        [Required, EmailAddress]
        public string Email{ get; set; }

        [Required]
        public string Password { get; set; }


    }
}
