﻿using Microsoft.AspNetCore.Hosting.Server;
using MineStatLib;
using MSLServer.Data;
using MSLServer.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Metrics;
using MSLServer.Models.Server;

namespace MSLServer.Logic
{
    public class ServerRepository : IServerRepository
    {
        ServerListDBContext context;
        IServerThumbnailRepository thumbnailRepository;
        IServerLogoRepository logoRepository;

        public ServerRepository(ServerListDBContext _context, IServerThumbnailRepository thumbnailRepository, IServerLogoRepository logoRepository)
        {
            context = _context;
            this.thumbnailRepository = thumbnailRepository;
            this.logoRepository = logoRepository;
        }
        public IList<Server> GetAll()
        {
            
            var servers= context.Servers.ToList();
            return servers;
        }
        public IList<Server> GetAllOnline()
        {

            var servers = context.Servers.Where(x => x.Status == true).ToList();
            return servers;
        }
        public IList<Server> GetPremiumServers()
        {

            var servers = context.Servers.Where(x=>x.Premium==true && x.Status==true).ToList();
            return servers;
        }
        public Server GetById(string id)
        {
            if (context.Servers.First(x => x.Id == id) != null)
            {
                return context.Servers.First(x => x.Id == id);
            }
            else
            {
                throw new Exception("Server with that id doesnt exists");
            }
            
        }
        public void Insert(CreateServerDTO server)
        {
            try
            {
                
                var newServer = new Server()
                {
                    Publisherid = server.Publisherid,
                    Servername = server.Servername,
                    JavaIp = server.JavaIp==null?server.JavaIp:"",
                    JavaPort = server.JavaPort,
                    BedrockIp = server.BedrockIp == null ? server.BedrockIp : "",
                    BedrockPort = server.BedrockPort,
                    Country = server.Country,

                    ShortDescription = server.ShortDescription,
                    LongDescription = server.LongDescription,
                    Youtube = server.Youtube == null ? server.Youtube : "",
                    Discord = server.Discord == null ? server.Discord : "",
                    Website = server.Website == null ? server.Website : ""

                };
                //Set the server thumbnail
                string thumbnailPath = Path.Combine(Resource.thumbnailDirectory, newServer.Id + Path.GetExtension(server.Thumbnail.FileName));
                using (Stream fileStream = new FileStream(thumbnailPath, FileMode.Create))
                {
                    server.Thumbnail.CopyToAsync(fileStream);
                    var extension = Path.GetExtension(thumbnailPath);
                    var filename = newServer.Id + extension;

                    var newThumbnail = new ServerThumbnail() { Name = newServer.Id, FullName = filename, Extension = extension, ServerId = newServer.Id };
                    newServer.ThumbnailPath = newThumbnail.FullName;
                    thumbnailRepository.Create(newThumbnail);
                }

                //Set the server Logo
                string logoPath = Path.Combine(Resource.logoDirectory, newServer.Id + Path.GetExtension(server.Logo.FileName));
                using (Stream fileStream = new FileStream(logoPath, FileMode.Create))
                {
                    server.Logo.CopyToAsync(fileStream);
                    var extension = Path.GetExtension(logoPath);
                    var filename = newServer.Id + extension;

                    var newLogo = new ServerLogo() { Name = newServer.Id, FullName = filename, Extension = extension, ServerId = newServer.Id };
                    newServer.LogoPath = newLogo.FullName;
                    logoRepository.Create(newLogo);
                }
                
                //Scecking server status and if the server is valid
                context.Servers.Add(newServer);
                context.SaveChanges();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            

        }
        public void Update(ServerDTO obj)
        {
            
            var old = GetById(obj.Id);
            old.Servername = obj.Servername;
            if (obj.BedrockIp == null)
            {
                old.BedrockIp = "";
                old.BedrockIp = "";
            
            }
            else {
                old.BedrockIp = obj.BedrockIp;
                old.BedrockPort = obj.BedrockPort;
            }
            if (obj.JavaIp == null)
            {
                old.JavaIp = "";
                old.JavaPort = "";

            }
            else
            {
                old.JavaIp = obj.JavaIp;
                old.JavaPort = obj.JavaPort;
            }    
            old.Country = obj.Country;
            old.ShortDescription = obj.ShortDescription;
            old.LongDescription = obj.LongDescription;

            //Set the server thumbnail
            if (obj.Thumbnail != null)
            {
                string thumbnailPath = Path.Combine(Resource.thumbnailDirectory, obj.Id + Path.GetExtension(obj.Thumbnail.FileName));
                using (Stream fileStream = new FileStream(thumbnailPath, FileMode.Append))
                {
                    obj.Thumbnail.CopyToAsync(fileStream);
                    var extension = Path.GetExtension(thumbnailPath);
                    var filename = obj.Id + extension;

                    var newThumbnail = new ServerThumbnail() { Name = obj.Id, FullName = filename, Extension = extension, ServerId = obj.Id };
                    old.ThumbnailPath = newThumbnail.FullName;
                    thumbnailRepository.Create(newThumbnail);
                }
            }
            if(obj.Logo != null)
            {
                //Set the server Logo
                string logoPath = Path.Combine(Resource.logoDirectory, old.Id + Path.GetExtension(obj.Logo.FileName));
                using (Stream fileStream = new FileStream(logoPath, FileMode.Append))
                {
                    obj.Logo.CopyToAsync(fileStream);
                    var extension = Path.GetExtension(logoPath);
                    var filename = old.Id + extension;

                    var newLogo = new ServerLogo() { Name = old.Id, FullName = filename, Extension = extension, ServerId = old.Id };
                    old.LogoPath = newLogo.FullName;
                    logoRepository.Create(newLogo);
                }
            }
            old.Discord = obj.Discord;
            old.Youtube = obj.Youtube;
            old.Website = obj.Website;
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
            return context.Servers.FirstOrDefault(x => x.JavaIp == ipaddress || x.BedrockIp == ipaddress);
        }
        public IList<Server> GetByStatus()
        {
            return context.Servers.Where(x => x.Status == true).ToList();
        }
        public IList<Server> GetByPlayerCount(int minplayers)
        {
            return context.Servers.Where(x => x.CurrentPlayers >= minplayers).ToList();
        }
        public void CheckServerStatus(Server server)
        {
            
            if (server.BedrockIp != "")
            {
                var current = GetByIp(server.BedrockIp);
                MineStat ms = new MineStat(server.BedrockIp, ushort.Parse(server.BedrockPort), 2, SlpProtocol.Json);
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
            }
            if (server.JavaIp != "")
            {
                var current = GetByIp(server.JavaIp);
                MineStat ms = new MineStat(server.JavaIp, ushort.Parse(server.JavaPort), 2, SlpProtocol.Json);
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
            }

            context.SaveChanges();
        }
        public bool GetServerStatus(string hostname, string port)
        {
            MineStat ms = new MineStat(hostname, ushort.Parse(port), 2, SlpProtocol.Json);
            if (ms.ServerUp)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void CheckSpecificServersStatus(IList<Server> servers)
        {
            if (servers.Count() <= 20)
            {
                for (int i = 0; i < servers.Count(); i++)
                {
                    CheckServerStatus(servers[i]);
                }
            }
        }
        public void CheckAllServerStatus()
        {
            var servers = GetAll();
            for (int i = 0; i < servers.Count(); i++)
            {
                CheckServerStatus(servers[i]);
            }
        }
    }
}
