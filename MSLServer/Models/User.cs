using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MSLServer.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Registration { get; set; }

        [Required]
        [Column(TypeName = "varchar(200)")]
        public string UserName { get; set; }

        [Required]
        [Column(TypeName = "varchar(200)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Email { get; set; }


        public string Password { get; set; }

        public User(string UserName, string email, string password)
        {
            if (Id == "" || Id == null)
            {
                Id = Guid.NewGuid().ToString();
            }
            Registration = DateTime.Now.ToString();
            this.UserName = UserName;
            Email = email;
            Password = password;

        }
    }
}
