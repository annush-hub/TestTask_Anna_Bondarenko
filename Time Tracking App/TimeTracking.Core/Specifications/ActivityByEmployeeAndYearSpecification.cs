using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Core.Specifications
{
    public class ActivityByEmployeeAndYearSpecification : BaseSpecification<Activity>
    {
        public ActivityByEmployeeAndYearSpecification(Guid employeeId, int year)
            : base(a => a.EmployeeId == employeeId && a.Date.Year == year)
        {
            AddInclude(a => a.Project);
            AddInclude(a => a.Employee);
        }
    }
}
