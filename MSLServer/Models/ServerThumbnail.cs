using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MSLServer.Models
{
    [Table("ServerLogos")]
    public class ServerThumbnail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Extension { get; set; }
        [Required]
        public string FullName { get; set; }

        public string ServerId { get; set; }

        public ServerThumbnail()
        {
            Id = Guid.NewGuid().ToString();
        }

    }
}
