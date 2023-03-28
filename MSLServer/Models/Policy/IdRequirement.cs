using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSLServer.Models.Policy
{
    public class IdRequirement : IAuthorizationRequirement
    {
        public string Id { get; set; }
        public IdRequirement(string id)
        {
            Id = id;
        }
    }
}
