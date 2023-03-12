using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MSLServer.Models
{
    [Table("ResetPasswordRequest")]
    public class ResetPasswordRequest
    {

        [Required]
        public string Token{ get; set; }

        [Required, MinLength(6, ErrorMessage ="Your password must be minimum 6 character")]
        public string Password { get; set; }

        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }

    }
}
