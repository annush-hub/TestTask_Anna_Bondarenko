using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Application.DTOs
{
    public class TimeTrackingEntryDto
    {
        public DateTime Date { get; set; }
        public string Role { get; set; }
        public string Project { get; set; }
        public string ActivityType { get; set; }
        public double Hours { get; set; }
    }
}
