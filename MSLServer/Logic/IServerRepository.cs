using MSLServer.Models;

namespace MSLServer.Logic
{
    public interface IServerRepository
    {
        IList<Server> GetAll();
        Server GetById(string id);
        Server GetByIp(string ipaddress);
        IList<Server> GetByStatus();
        //public IList<Server> GetByOwner(Guid ownerid);
        public IList<Server> GetByPlayerCount(int minplayers);
        void Insert(string ipaddress, string port, string ownerid);
        void Update(Server obj);
        void Delete(string id);

        public void SetServerListInformation(IList<Server> servers);
        public void GetServerInformation(string ip, string port);


    }
}