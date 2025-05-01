using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TimeTracking.Core.Enums;

namespace TimeTracking.Application.DTOs
{
    public class CreateActivityDto
    {
        public Guid EmployeeId { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime Date { get; set; }
        public double Hours { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ActivityType ActivityType { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Role Role { get; set; }
    }

}
