﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MSLServer.Models
{
    [Table("Server")]
    public class Server
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Registration { get; set; }

        public string Publisherid { get; set; }
        public string Servername { get; set; }
        public string Ip { get; set; }
        public string Port { get; set; }
        public bool Status { get; set; }
        public int CurrentPlayers { get; set; }
        public int MaxPlayer { get; set; }
        public string ServerVersion { get; set; }
        //public string Gamemode { get; set; }
        //public string Modt { get; set; }
        //public long Latency { get; set; }

        public string ThumbnailPath { get; set; }

        public Server()
        {
            if (Id == "" || Id == null)
            {
                Id = Guid.NewGuid().ToString();
            }
            ThumbnailPath = string.Empty;
            ServerVersion = string.Empty;
            Registration = DateTime.Now.ToString();
        }
    }
}
