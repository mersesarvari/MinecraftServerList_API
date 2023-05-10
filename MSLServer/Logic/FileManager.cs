using MSLServer.Models.Server;
using MSLServer.Models;
using System.Security.Cryptography;

namespace MSLServer.Logic
{

    public class FileManager : IFileManager
    {
        IServerThumbnailRepository thumbnailRepository;
        IServerLogoRepository logoRepository;
        public FileManager(IServerThumbnailRepository thumbnailRepository, IServerLogoRepository logoRepository)
        {
            this.thumbnailRepository = thumbnailRepository;
            this.logoRepository = logoRepository;
        }

        public void CreateThumbnail(Server server, CreateServerDTO serverDTO)
        {
            string thumbnailPath = Path.Combine(Resource.thumbnailDirectory, server.Id + Path.GetExtension(serverDTO.Thumbnail.FileName));
            using (Stream fileStream = new FileStream(thumbnailPath, FileMode.Create))
            {
                serverDTO.Thumbnail.CopyToAsync(fileStream);
                var extension = Path.GetExtension(thumbnailPath);
                var filename = server.Id + extension;

                var newThumbnail = new ServerThumbnail() { Name = server.Id, FullName = filename, Extension = extension, ServerId = server.Id };
                server.ThumbnailPath = newThumbnail.FullName;
                thumbnailRepository.Create(newThumbnail);
            }

            //Set the server Logo
            string logoPath = Path.Combine(Resource.logoDirectory, server.Id + Path.GetExtension(serverDTO.Logo.FileName));
            using (Stream fileStream = new FileStream(logoPath, FileMode.Create))
            {
                serverDTO.Logo.CopyToAsync(fileStream);
                var extension = Path.GetExtension(logoPath);
                var filename = server.Id + extension;

                var newLogo = new ServerLogo() { Name = server.Id, FullName = filename, Extension = extension, ServerId = server.Id };
                server.LogoPath = newLogo.FullName;
                logoRepository.Create(newLogo);
            }
        }

        public void ModifyThumbnail(Server server, CreateServerDTO serverDTO)
        {
            if (serverDTO.Thumbnail != null)
            {
                string thumbnailPath = Path.Combine(Resource.thumbnailDirectory, server.Id + Path.GetExtension(serverDTO.Thumbnail.FileName));
                using (Stream fileStream = new FileStream(thumbnailPath, FileMode.Create))
                {
                    serverDTO.Thumbnail.CopyToAsync(fileStream);
                    var extension = Path.GetExtension(thumbnailPath);
                    var filename = server.Id + extension;

                    var newThumbnail = new ServerThumbnail() { Name = server.Id, FullName = filename, Extension = extension, ServerId = server.Id };
                    server.ThumbnailPath = newThumbnail.FullName;
                    thumbnailRepository.Create(newThumbnail);
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

                    var newLogo = new ServerLogo() { Name = server.Id, FullName = filename, Extension = extension, ServerId = server.Id };
                    server.LogoPath = newLogo.FullName;
                    logoRepository.Create(newLogo);
                }
            }
        }
    }
}
