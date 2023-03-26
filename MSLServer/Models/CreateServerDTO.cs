using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MSLServer.Models
{
    public class CreateServerDTO
    {
        public string? Publisherid { get; set; }

        #region ServerDetails
        public string Servername { get; set; }
        public string? JavaIp { get; set; }
        public string? JavaPort { get; set; }
        public string? BedrockIp { get; set; }
        public string? BedrockPort { get; set; }
        #endregion
        #region Social
        public string? Youtube { get; set; }
        public string? Discord { get; set; }
        public string? Website { get; set; }
        #endregion
        public string? Country { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public IFormFile Thumbnail { get; set; }
        public IFormFile Logo { get; set; }
    }
}
