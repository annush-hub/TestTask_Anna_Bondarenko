using TimeTracking.Core.Base;
using TimeTracking.Core.Enums;

namespace TimeTracking.Core
{
    public class Employee : Entity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public required Sex Sex { get; set; } = Sex.Unknown;
        public required DateTime BirthDate { get; set; }

        public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();
    }
}
