using Microsoft.EntityFrameworkCore;
using MSLServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSLServer.Data.DBSeed
{
    public class ServerDBSeed
    {
        public ServerDBSeed(ModelBuilder mb)
        {
            LoadData(mb);
        }
        public static void LoadData(ModelBuilder mb)
        {
            var servers = new List<Server>()
            {
                new Server() {
                    Servername="Hypixel",
                    Id="ce5607a6-fae2-4232-8ecc-535ce64ee40a",
                    Ip="mc.hypixel.net",
                    Port="25565",
                    Publisherid="640db982-f8f1-4df1-a405-05103025bb03",
                    ThumbnailPath=" 5d2c7e10-58f8-46ce-8cc3-a27bc7d5f44c.mp4"
                },
                new Server() {
                    Servername="NazmoxMC",
                    Id="3ca5e108-50da-4624-816f-9c1a9ec1a11c",
                    Ip="mc.nazmox.net",
                    Port="25565",
                    Publisherid="640db982-f8f1-4df1-a405-05103025bb03",
                    ThumbnailPath=" 81f58175-8e09-466c-8196-0dcb144de1a7.mp4"
                },
                new Server() {
                    Servername="OPblocks",
                    Id="81f58175-8e09-466c-8196-0dcb144de1a7",
                    Ip="sm.opblocks.com",
                    Port="25565",
                    Publisherid="640db982-f8f1-4df1-a405-05103025bb03",
                    ThumbnailPath="81f58175-8e09-466c-8196-0dcb144de1a7.mp4"
                },
                new Server() {
                    Servername="Manacube",
                    Ip="lobby.manacube.com",
                    Port="25565",
                    Publisherid="640db982-f8f1-4df1-a405-05103025bb03",
                    ThumbnailPath="3ca5e108-50da-4624-816f-9c1a9ec1a11c.mp4"
                },
                new Server() {
                    Servername="Foundationcraft",
                    Ip="play.foundationcraft.com",
                    Port="25565",
                    Publisherid="640db982-f8f1-4df1-a405-05103025bb03",
                    ThumbnailPath="5bf14395-8b88-4583-8e6f-8daaeb52ca2d.mp4"
                },
                new Server() {
                    Servername="Crackedmc",
                    Ip="crankedmc.com",
                    Port="25565",
                    Publisherid="640db982-f8f1-4df1-a405-05103025bb03",
                    ThumbnailPath="3ca5e108-50da-4624-816f-9c1a9ec1a11c.mp4"
                },


            };
            mb.Entity<Server>().HasData(servers);
        }
    }
}
