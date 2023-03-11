using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MSLServer.Data;
using MSLServer.Models;

namespace MSLServer.Logic
{
    public class ServerLogoRepository : IServerLogoRepository
    {
        ServerListDBContext db;
        public ServerLogoRepository(ServerListDBContext db)
        {
            this.db = db;
        }
        public void Create(ServerLogo item)
        {
            var count = db.Servers.Count();
            var currentserver = db.Servers.FirstOrDefault(x => x.Id == item.ServerId);
            if (db.ServerLogos.Where(x => x.Id == item.Id).Count() > 0)
            {
                db.ServerLogos.Remove(item);
            }
            item.ServerId = item.Name;
            currentserver.ThumbnailPath = item.FullName;
            db.ServerLogos.Add(item);
            db.SaveChanges();
        }

        public void Delete(string id)
        {
            var currentserver = db.Servers.FirstOrDefault(x => x.Id == id);
            currentserver.ThumbnailPath = string.Empty;
            db.ServerLogos.Remove(ReadByServerId(id));
            db.SaveChanges();
        }
        public ServerLogo Read(string id)
        {
            if (db == null)
            {
                throw new Exception("There is no data in database");
            }
            else
            {
                return db.ServerLogos.FirstOrDefault(t => t.Id == id);
            }
        }
        public ServerLogo ReadByServerId(string serverid)
        {
            if (db == null)
            {
                throw new Exception("There is no data in database");
            }
            else
            {
                return db.ServerLogos.FirstOrDefault(t => t.Name == serverid);
            }
        }
        public IList<ServerLogo> ReadAll()
        {
            if (db == null)
            {
                throw new Exception("There is no data in that table");
            }
            else
            {
                return db.ServerLogos.ToList();
            }

        }
    }
}
