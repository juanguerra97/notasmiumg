using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace NotasMiUmg.Authorization
{

    public static class CrudOperations
    {
        public static OperationAuthorizationRequirement CrudCarrera =
          new OperationAuthorizationRequirement { Name = Constants.CrudCarreraOperationName };
        public static OperationAuthorizationRequirement CrudPensum =
          new OperationAuthorizationRequirement { Name = Constants.CrudPensumOperationName };
        public static OperationAuthorizationRequirement CrudCurso =
          new OperationAuthorizationRequirement { Name = Constants.CrudCursoOperationName };
        public static OperationAuthorizationRequirement CrudPensumCurso =
          new OperationAuthorizationRequirement { Name = Constants.CrudPensumCursoOperationName };
        public static OperationAuthorizationRequirement CrudEstudiante =
          new OperationAuthorizationRequirement { Name = Constants.CrudEstudianteOperationName };
        public static OperationAuthorizationRequirement CrudNota =
          new OperationAuthorizationRequirement { Name = Constants.CrudNotaOperationName };
    
    }

    /// <summary>
    /// Nombres de las operaciones que se pueden hacer en una tabla
    /// </summary>
    public class Constants
    {
        public static readonly string CrudCarreraOperationName = "CrudCarrera";
        public static readonly string CrudPensumOperationName = "CrudPensum";
        public static readonly string CrudCursoOperationName = "CrudCurso";
        public static readonly string CrudPensumCursoOperationName = "CrudPensumCurso";
        public static readonly string CrudEstudianteOperationName = "CrudEstudiante";
        public static readonly string CrudNotaOperationName = "CrudNota";

    }

    /// <summary>
    /// Nombres de los roles de usuarios
    /// </summary>
    public class RoleNameConstants
    {
        public static readonly string RolAdministrador = "ADMIN";
        public static readonly string RolEstudiante = "ESTUDIANTE";
    }

}
