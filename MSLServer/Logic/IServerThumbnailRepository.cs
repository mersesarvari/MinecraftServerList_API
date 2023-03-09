using MSLServer.Models;

namespace MSLServer.Logic
{
    public interface IServerThumbnailRepository
    {
        void Create(ServerThumbnail item);
        void Delete(string id);
        ServerThumbnail Read(string id);
        IList<ServerThumbnail> ReadAll();
        ServerThumbnail ReadByServerId(string serverid);
    }
}