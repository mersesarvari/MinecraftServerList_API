using Microsoft.AspNetCore.Hosting.Server;
using MineStatLib;
using MSLServer.Data;
using MSLServer.Models;

namespace MSLServer.Logic
{
    public class ServerRepository : IServerRepository
    {
        ServerListDBContext context;

        public ServerRepository(ServerListDBContext _context)
        {
            context = _context;
        }
        public IList<Server> GetAll()
        {
            //SetServerListInformation(context.Servers.ToList());
            return context.Servers.ToList();
        }
        public Server GetById(string id)
        {
            return context.Servers.FirstOrDefault(x => x.Id == id);
        }
        public void Insert(string ip, string port, string ownerid)
        {
            var newserver = new Server() { Ip = ip, Publisherid = ownerid, Port = port };
            context.Servers.Add(newserver);
            context.SaveChanges();
        }

        public void Update(Server obj)
        {
            var old = GetById(obj.Id);
            old = obj;
            context.SaveChanges();
        }
        public void Delete(string id)
        {
            Server old = context.Servers.Find(id);
            context.Servers.Remove(old);
            context.SaveChanges();
        }

        public void AddThumbnail(string id)
        {
            var currentThumbnail = context.ServerThumbnails.FirstOrDefault(x => x.Name == id);
            var current = GetAll().FirstOrDefault(x => x.Id == id);
            current.ThumbnailPath = Resource.FilePath + currentThumbnail.FullName;
            context.SaveChanges();
        }

        public Server GetByIp(string ipaddress)
        {
            return context.Servers.FirstOrDefault(x => x.Ip == ipaddress);
        }

        public IList<Server> GetByStatus()
        {
            return context.Servers.Where(x => x.Status == true).ToList();
        }

        public IList<Server> GetByPlayerCount(int minplayers)
        {
            return context.Servers.Where(x => x.CurrentPlayers >= minplayers).ToList();
        }

        public void CheckServerStatus(string ip, string port)
        {
            var current = GetByIp(ip);
            MineStat ms = new MineStat(ip, ushort.Parse(port), 2, SlpProtocol.Json);
            if (ms.ServerUp)
            {
                current.Status = true;
                current.ServerVersion = ms.Version;
                current.CurrentPlayers = ms.CurrentPlayersInt;
                current.MaxPlayer = ms.MaximumPlayersInt;
            }
            else
            {
                current.Status = false;
            }
            context.SaveChanges();
        }

        public void CheckSpecificServersStatus(IList<Server> servers)
        {
            if (servers.Count() <= 20)
            {
                for (int i = 0; i < servers.Count(); i++)
                {
                    CheckServerStatus(servers[i].Ip, servers[i].Port);
                }
            }
        }

        public void CheckAllServerStatus()
        {
            var servers = GetAll();
            for (int i = 0; i < servers.Count(); i++)
            {
                CheckServerStatus(servers[i].Ip, servers[i].Port);
            }
        }
    }
}
