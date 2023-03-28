﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSLServer.SecureServices;
using MSLServer.Models.User;

namespace MSLServer.Data.DBSeed
{
    public class UserDBSeed
    {
        public UserDBSeed(ModelBuilder mb)
        {
            LoadData(mb);
        }
        public static void LoadData(ModelBuilder mb)
        {
            var users = new List<User>()
            {
                new User()
                {

                    Id = "640db982-f8f1-4df1-a405-05103025bb03",
                    Email = "user@example.com",
                    Password = Secure.Encrypt("string"),                    
                    VerificationToken = null,
                    VerifiedAt = DateTime.Now,
                    Role = Roles.Admin
                },
                new User()
                {
                    Email = "merse.sarvari@gmail.com",
                    Password = Secure.Encrypt("proba"),
                    VerificationToken = null,
                    VerifiedAt = DateTime.Now,
                    Role = Roles.Admin
                }
            };
            mb.Entity<User>().HasData(users);
        }
    }
}
