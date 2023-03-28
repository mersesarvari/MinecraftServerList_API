using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MSLServer.Models.User
{
    public class UserInfoDTO
    {
        public string Email { get; set; }

        public string Token { get; set; }
    }
}
