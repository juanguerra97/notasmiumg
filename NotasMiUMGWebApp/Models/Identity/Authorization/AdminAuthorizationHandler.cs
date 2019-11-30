using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace NotasMiUmg.Authorization
{
    public class AdminAuthorizationHandler :
        AuthorizationHandler<OperationAuthorizationRequirement>
    {
        protected override Task
            HandleRequirementAsync(AuthorizationHandlerContext context,
                                   OperationAuthorizationRequirement requirement)
        {
            if (context.User == null)
            {
                return Task.CompletedTask;
            }

            if (requirement.Name != Constants.CrudCarreraOperationName &&
                requirement.Name != Constants.CrudPensumOperationName &&
                requirement.Name != Constants.CrudCursoOperationName &&
                requirement.Name != Constants.CrudPensumCursoOperationName)
            {
                return Task.CompletedTask;
            }

            if (context.User.IsInRole(RoleNameConstants.RolAdministrador))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}