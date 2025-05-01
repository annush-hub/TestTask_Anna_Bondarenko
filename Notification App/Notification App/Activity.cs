using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification_App
{
    public class Activity
    {
        public Guid Id { get; set; }
        public string EmployeeFullName { get; set; }
        public string ProjectName { get; set; }
        public string Role { get; set; }
        public string ActivityType { get; set; }
        public DateTime Date { get; set; }
        public double Hours { get; set; }
    }
}
