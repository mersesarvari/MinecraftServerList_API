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
                new Server("ce5607a6-fae2-4232-8ecc-535ce64ee40a") {
                    Servername="Hypixel",

                    JavaIp="mc.hypixel.net",
                    JavaPort="25565",
                    BedrockIp="mc.hypixel.net",
                    BedrockPort="25565",
                    Publisherid="640db982-f8f1-4df1-a405-05103025bb03",
                    ShortDescription="Short description",
                    LongDescription="This is the longer version of the description of the server. ",
                    Country="United States",
                    Premium=true
                    
                    
                },
                new Server("3ca5e108-50da-4624-816f-9c1a9ec1a11c") {
                    Servername="NazmoxMC",
                    
                    JavaIp="mc.nazmox.net",
                    JavaPort="25565",
                    BedrockIp="mc.nazmox.net",
                    BedrockPort="25565",
                    Publisherid="640db982-f8f1-4df1-a405-05103025bb03",
                    ShortDescription="Short description",
                    LongDescription="This is the longer version of the description of the server. ",
                    Country="United States",
                    Premium=true
                },
                new Server("81f58175-8e09-466c-8196-0dcb144de1a7") {
                    Servername="OPblocks",
                    
                    JavaIp="sm.opblocks.com",
                    JavaPort="25565",
                    BedrockIp="sm.opblocks.com",
                    BedrockPort="25565",
                    Publisherid="640db982-f8f1-4df1-a405-05103025bb03",
                    ShortDescription="Short description",
                    LongDescription="This is the longer version of the description of the server. ",
                    Country="United States",
                    Premium=false
                },
                new Server("8421e8d1-2e4e-4c07-a039-9bcd41a66b28") {
                    Servername="Manacube",

                    JavaIp="lobby.manacube.com",
                    JavaPort="25565",
                    BedrockIp="lobby.manacube.com",
                    BedrockPort="25565",
                    Publisherid="640db982-f8f1-4df1-a405-05103025bb03",
                    ShortDescription="Short description",
                    LongDescription="This is the longer version of the description of the server. ",
                    Country="United States",
                    Premium=false
                },
                new Server("d17f9f98-d633-4ac6-8bc8-edc6ad7f37e1") {
                    Servername="Foundationcraft",

                    JavaIp="play.foundationcraft.com",
                    JavaPort="25565",
                    BedrockIp="play.foundationcraft.com",
                    BedrockPort="25565",
                    Publisherid="640db982-f8f1-4df1-a405-05103025bb03",
                    ShortDescription="Short description",
                    LongDescription="This is the longer version of the description of the server. ",
                    Country="United States",
                    Premium=false
                },
                new Server("ab937e0d-2a17-42db-8847-455500d3c6ce") {
                    Servername="Crackedmc",

                    JavaIp="crankedmc.com",
                    JavaPort="25565",
                    BedrockIp="crankedmc.com",
                    BedrockPort="25565",
                    Publisherid="640db982-f8f1-4df1-a405-05103025bb03",
                    ShortDescription="Short description",
                    LongDescription="This is the longer version of the description of the server. ",
                    Country="United States",
                    Premium=false
                },


            };
            mb.Entity<Server>().HasData(servers);
        }
    }
}
