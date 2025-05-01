using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Core.Base;
using TimeTracking.Core.Enums;

namespace TimeTracking.Core
{
    public class Activity : Entity
    {
        public required Guid EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public required Role Role { get; set; }

        public required Guid ProjectId { get; set; }
        public Project? Project { get; set; }

        public required ActivityType ActivityType { get; set; }
        public required DateTime Date { get; set; }
        public required double Hours { get; set; }
    }
}
