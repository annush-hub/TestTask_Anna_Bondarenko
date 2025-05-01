using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Core.Specifications
{
    public class ActivityByEmployeeAndWeekSpecification : BaseSpecification<Activity>
    {
        public ActivityByEmployeeAndWeekSpecification(Guid employeeId, int year, int weekNumber)
            : base(a => a.EmployeeId == employeeId &&
                        a.Date.Year == year &&
                        GetWeekNumber(a.Date) == weekNumber)
        {
            AddInclude(a => a.Project);
            AddInclude(a => a.Employee);
        }

        public static int GetWeekNumber(DateTime date)
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            return culture.Calendar.GetWeekOfYear(date, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }
    }

}
