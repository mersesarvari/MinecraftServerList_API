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
                    Id="ce5607a6-fae2-4232-8ecc-535ce64ee40a",
                    Ip="mc.hypixel.net",
                    Port="25565",
                    Publisherid="640db982-f8f1-4df1-a405-05103025bb03",
                },
                new Server() {
                    Id="3ca5e108-50da-4624-816f-9c1a9ec1a11c",
                    Ip="mc.nazmox.net",
                    Port="25565",
                    Publisherid="640db982-f8f1-4df1-a405-05103025bb03",
                },
                new Server() {
                    Id="81f58175-8e09-466c-8196-0dcb144de1a7",
                    Ip="sm.opblocks.com",
                    Port="25565",
                    Publisherid="640db982-f8f1-4df1-a405-05103025bb03",
                },


            };
            mb.Entity<Server>().HasData(servers);
        }
    }
}
