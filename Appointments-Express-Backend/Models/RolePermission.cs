using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.Models
{
    public class RolePermission
    {
        public int roleId { get; set; }
        public int permissionId { get; set; }

        [ForeignKey("roleId")]
        public virtual Role role { get; set; }

        [ForeignKey("permissionId")]
        public virtual Permission permission { get; set; }
    }
}
