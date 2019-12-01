
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using jguerra.notasmiumg;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace NotasMiUmg.Authorization
{
    public class NotaAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Nota>
    {

        private UserManager<IdentityUser> _userManager;

        public NotaAuthorizationHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        protected override Task
            HandleRequirementAsync(AuthorizationHandlerContext context,
                                   OperationAuthorizationRequirement requirement,
                                   Nota resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }


            if (requirement.Name != Constants.CrudNotaOperationName)
            {
                return Task.CompletedTask;
            }

            if (resource.Estudiante.UsuarioEstudiante.Id == _userManager.GetUserId(context.User))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }


    }
}
