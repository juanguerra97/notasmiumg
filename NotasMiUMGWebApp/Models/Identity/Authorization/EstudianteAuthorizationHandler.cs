using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using jguerra.notasmiumg;

namespace NotasMiUmg.Authorization
{

    /// <summary>
    /// Handler para saber si un usuario puede modificar un registro de estudiante
    /// </summary>
    public class EstudianteAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Estudiante>
    {

        private UserManager<IdentityUser> _userManager;

        public EstudianteAuthorizationHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        protected override Task
            HandleRequirementAsync(AuthorizationHandlerContext context,
                                   OperationAuthorizationRequirement requirement,
                                   Estudiante resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }


            if (requirement.Name != Constants.CrudEstudianteOperationName)
            {
                return Task.CompletedTask;
            }
            
            if (resource.UsuarioEstudiante.Id == _userManager.GetUserId(context.User))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

    }
}
