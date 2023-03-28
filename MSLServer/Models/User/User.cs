using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MSLServer.Models.User
{
    public enum Roles
    {
        User,
        Vip,
        Moderator,
        Admin,
    }
    [Table("User")]
    public class User
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Email { get; set; }

        public string? VerificationToken { get; set; }
        public DateTime? VerifiedAt { get; set; }

        public string? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }

        public Roles Role { get; set; }


        public string Password { get; set; }

        public User(string id)
        {
            Id = id;
        }
        public User()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
