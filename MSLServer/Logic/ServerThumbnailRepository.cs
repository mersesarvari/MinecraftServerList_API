using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MSLServer.Data;
using MSLServer.Models;
using Newtonsoft.Json.Serialization;

namespace MSLServer.Logic
{
    public class ServerThumbnailRepository : IServerThumbnailRepository
    {
        ServerListDBContext db;
        public ServerThumbnailRepository(ServerListDBContext db)
        {
            this.db = db;
        }
        public void Create(ServerThumbnail item)
        {
            if (db.ServerThumbnails.Where(x => x.Id == item.Id).Count() > 0)
            {
                db.ServerThumbnails.Remove(item);
            }
            item.ServerId = item.Name;
            //currentserver.ThumbnailPath = item.FullName;
            db.ServerThumbnails.Add(item);
            db.SaveChanges();
        }
        public void Modify(ServerThumbnail item)
        {
            if (db.ServerThumbnails.Where(x => x.Id == item.Id).Count() > 0)
            {
                db.ServerThumbnails.Remove(item);
            }
            item.ServerId = item.Name;
            //currentserver.ThumbnailPath = item.FullName;
            db.ServerThumbnails.Add(item);
            db.SaveChanges();
        }

        public void Delete(string id)
        {
            var thumbnail = db.ServerThumbnails.FirstOrDefault(x => x.Id == id);
            var server = db.Servers.FirstOrDefault(x => x.ThumbnailId == id);
            File.Delete();
            server.ThumbnailId = string.Empty;
            server.ThumbnailPath = string.Empty;
            
            db.ServerThumbnails.Remove(ReadByServerId(id));
            db.SaveChanges();
        }
        public void DeleteByServer(string serverid)
        {
            var server = db.Servers.FirstOrDefault(x => x.Id == serverid);
            var thumbnail = db.ServerThumbnails.FirstOrDefault(x => x.Id == server.ThumbnailId);
            db.ServerThumbnails.Remove(thumbnail);
            server.ThumbnailId = string.Empty;
            server.ThumbnailPath = string.Empty;
            db.SaveChanges();
        }
        public ServerThumbnail Read(string id)
        {
            if (db == null)
            {
                throw new Exception("There is no data in database");
            }
            else
            {
                return db.ServerThumbnails.FirstOrDefault(t => t.Id == id);
            }
        }
        public ServerThumbnail ReadByServerId(string serverid)
        {
            if (db == null)
            {
                throw new Exception("There is no data in database");
            }
            else
            {
                var a = db.ServerThumbnails;
                return db.ServerThumbnails.FirstOrDefault(t => t.Name == serverid);
            }
        }
        public IList<ServerThumbnail> ReadAll()
        {
            if (db == null)
            {
                throw new Exception("There is no data in that table");
            }
            else
            {
                return db.ServerThumbnails.ToList();
            }

        }
    }
}
