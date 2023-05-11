using MSLServer.Models.Server;
using MSLServer.Models;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Tls;

namespace MSLServer.Logic
{

    public class FileManager : IFileManager
    {
        public FileManager()
        {
        }

        public void CreateThumbnail(Server server, CreateServerDTO serverDTO)
        {
            string thumbnailPath = Path.Combine(Resource.thumbnailDirectory, server.Id + Path.GetExtension(serverDTO.Thumbnail.FileName));
            using (Stream fileStream = new FileStream(thumbnailPath, FileMode.Create))
            {
                serverDTO.Thumbnail.CopyToAsync(fileStream);
                var extension = Path.GetExtension(thumbnailPath);
                var filename = server.Id + extension;

                server.ThumbnailPath = filename;
            }

            //Set the server Logo
            string logoPath = Path.Combine(Resource.logoDirectory, server.Id + Path.GetExtension(serverDTO.Logo.FileName));
            using (Stream fileStream = new FileStream(logoPath, FileMode.Create))
            {
                serverDTO.Logo.CopyToAsync(fileStream);
                var extension = Path.GetExtension(logoPath);
                var filename = server.Id + extension;

                server.LogoPath = filename;
            }
        }

        public void ModifyThumbnail(Server server ,ServerDTO serverDTO)
        {
            if (serverDTO.Thumbnail != null)
            {
                string thumbnailPath = Path.Combine(Resource.thumbnailDirectory, server.Id + Path.GetExtension(serverDTO.Thumbnail.FileName));
                using (Stream fileStream = new FileStream(thumbnailPath, FileMode.Create))
                {
                    serverDTO.Thumbnail.CopyToAsync(fileStream);
                    var extension = Path.GetExtension(thumbnailPath);
                    var filename = server.Id + extension;

                    server.ThumbnailPath = filename;
                }
            }
            if (serverDTO.Logo != null)
            {
                //Set the server Logo
                string logoPath = Path.Combine(Resource.logoDirectory, server.Id + Path.GetExtension(serverDTO.Logo.FileName));
                using (Stream fileStream = new FileStream(logoPath, FileMode.Create))
                {
                    serverDTO.Logo.CopyToAsync(fileStream);
                    var extension = Path.GetExtension(logoPath);
                    var filename = server.Id + extension;

                    server.LogoPath = filename;
                }
            }
        }
        
        public void DeleteFile(Server server, FileType type) {
            if (type == FileType.thumbnail)
            {
                if (File.Exists(server.ThumbnailPath))
                {
                    File.Delete(server.ThumbnailPath);
                }
            }
            if (type == FileType.logo)
            {
                if (File.Exists(server.LogoPath))
                {
                    File.Delete(server.LogoPath);
                }
            }
        }
    }
}
