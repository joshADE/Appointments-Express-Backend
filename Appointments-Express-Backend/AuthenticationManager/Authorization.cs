using Appointments_Express_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.AuthenticationManager
{
    public static class Authorization
    {
        public static bool UserHasPermission(AppointmentDBContext appointmentDBContext, int userId, int storeId, string permissionName)
        {

            var permission = appointmentDBContext.Permissions.FirstOrDefault(p => p.name == permissionName);
            var roleIds = appointmentDBContext.UserStoreRoles
                            .Where(usr => usr.userId == userId && usr.storeId == storeId)
                            .Select(usr => usr.roleId);

            if (permission == null || !roleIds.Any())
            {
                return false;
            }

            return appointmentDBContext.RolePermissions.Any(rp => rp.permissionId == permission.id && roleIds.Any(roleId => roleId == rp.roleId));

        }


        public static string GetUserId(ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims.FirstOrDefault(x => x.Type.Equals("id", StringComparison.InvariantCultureIgnoreCase));
            return userIdClaim.Value;
        }
    }
}
