using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiAssignment.Models
{
    public class Managers
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public virtual Employee NormalEmployee { get; set; }
    
        public int ManagerId { get; set; }
        public virtual Employee Manager { get; set; }
    }
}
