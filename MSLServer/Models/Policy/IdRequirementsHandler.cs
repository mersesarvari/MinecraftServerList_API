using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSLServer.Models.Policy
{
    public class IdRequirementsHandler : AuthorizationHandler<IdRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IdRequirement requirement)
        {
            if (!context.User.HasClaim(claim => claim.Type == "_id"))
            {
                return Task.CompletedTask;
            }
            return Task.CompletedTask;
            /*
            if (!int.TryParse(context.User.FindFirst(c => c.Type == "id").Value, out string id))
            {
                return Task.CompletedTask;
            }
            if (id > requirement.Id)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
            */

        }
    }
}
