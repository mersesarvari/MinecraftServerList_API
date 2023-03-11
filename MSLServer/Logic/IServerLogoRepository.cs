using MSLServer.Models;

namespace MSLServer.Logic
{
    public interface IServerLogoRepository
    {
        void Create(ServerLogo item);
        void Delete(string id);
        ServerLogo Read(string id);
        IList<ServerLogo> ReadAll();
        ServerLogo ReadByServerId(string serverid);
    }
}