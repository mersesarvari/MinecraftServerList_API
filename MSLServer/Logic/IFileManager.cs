using MSLServer.Models;
using MSLServer.Models.Server;

namespace MSLServer.Logic
{
    public interface IFileManager
    {
        void CreateThumbnail(Server server, CreateServerDTO serverDTO);
        void ModifyThumbnail(Server server, ServerDTO serverDTO);
    }
}