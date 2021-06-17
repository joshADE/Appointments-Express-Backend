using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.Models
{
    public class UserStoreRole
    {
        public int userId { get; set; }
        public int storeId { get; set; }
        public int roleId { get; set; }

        [ForeignKey("userId")]
        public virtual User user { get; set; }

        [ForeignKey("storeId")]
        public virtual Store store { get; set; }

        [ForeignKey("roleId")]
        public virtual Role role { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime createdAt { get; set; }
    }
}
