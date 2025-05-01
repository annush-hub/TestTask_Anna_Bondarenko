using TimeTracking.Core.Base;
using TimeTracking.Core.Enums;

namespace TimeTracking.Core
{
    public class Project : Entity
    {
        public required string Name { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }

        public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();
    }
}
