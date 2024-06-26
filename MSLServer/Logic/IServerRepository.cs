﻿using MSLServer.Models;
using MSLServer.Models.Server;

namespace MSLServer.Logic
{
    public interface IServerRepository
    {
        void AddThumbnail(string id);
        void CheckAllServerStatus();
        void CheckServerStatus(Server server);
        bool GetServerStatus(string hostname, string port);
        void CheckSpecificServersStatus(IList<Server> servers);
        void Delete(string id);
        IList<Server> GetAll();
        IList<Server> GetAllOnline();
        public IList<Server> GetPremiumServers();
        Server GetById(string id);
        Server GetByIp(string ipaddress);
        IList<Server> GetByPlayerCount(int minplayers);
        IList<Server> GetByStatus();
        void Insert(CreateServerDTO server);
        void Update(ServerDTO obj);
    }
}