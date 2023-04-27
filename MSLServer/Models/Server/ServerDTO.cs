using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MSLServer.Models.Server
{
    public class ServerDTO
    {
        public string? Id { get; set; }
        public string? Servername { get; set; }
        public string? JavaIp { get; set; }
        public string? JavaPort { get; set; }
        public string? BedrockIp { get; set; }
        public string? BedrockPort { get; set; }
        public string? Youtube { get; set; }
        public string? Discord { get; set; }
        public string? Website { get; set; }
        public string? Country { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public IFormFile? Thumbnail { get; set; }
        public IFormFile? Logo { get; set; }
    }
}
