using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Core.Specifications
{
    public class ActivityByEmployeeAndDateSpecification : BaseSpecification<Activity>
    {
        public ActivityByEmployeeAndDateSpecification(Guid personId, DateTime date)
            : base(a => a.EmployeeId == personId && a.Date.Date == date.Date)
        {
            AddInclude(a => a.Project);
            AddInclude(a => a.Employee);
        }
    }

}
