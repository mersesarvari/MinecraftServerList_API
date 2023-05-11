﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MSLServer.Models.Server
{
    [Table("Server")]
    public class Server
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string? Registration { get; set; }
        public string? Publisherid { get; set; }

        #region ServerDetails
        [Required]
        public string Servername { get; set; }
        public string? JavaIp { get; set; } = "";
        public string? JavaPort { get; set; } = "";
        public string? BedrockIp { get; set; } = "";
        public string? BedrockPort { get; set; } = "";
        #endregion
        #region Social
        public string? Youtube { get; set; } = "";
        public string? Discord { get; set; } = "";
        public string? Website { get; set; } = "";
        #endregion
        [Required]
        public string? Country { get; set; } = "";
        [Required]
        public string? ShortDescription { get; set; } = "";
        [Required]
        public string? LongDescription { get; set; }
        public bool Status { get; set; }
        public int CurrentPlayers { get; set; }
        public int MaxPlayer { get; set; }
        public string? ServerVersion { get; set; }
        

        public bool Premium { get; set; }
        public DateTime? PremiumExpiration { get; set; }

        public string? ThumbnailPath { get; set; }
        public string? LogoPath { get; set; }
        public string? ThumbnailId { get; set; }
        public string? LogoId { get; set; }



        public Server()
        {
            Id = Guid.NewGuid().ToString();
            //Servername = string.Empty;
            //ServerVersion = string.Empty;


            Registration = DateTime.Now.ToString();
            ThumbnailPath = Id + ".mp4";
            LogoPath = Id + ".webp";
        }
        public Server(string id)
        {
            Id = id;
            Servername = string.Empty;
            ServerVersion = string.Empty;


            Registration = DateTime.Now.ToString();
            ThumbnailPath = Id + ".mp4";
            LogoPath = Id + ".webp";
        }
    }
}