using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Core;
using TimeTracking.Core.Enums;

namespace TimeTracking.Infrastructure
{
    public class SeedHelper
    {
        public static async Task SeedData(TimeTrackingContext context)
        {
            await SeedEmployees(context);
            await SeedProjects(context);
            await SeedActivities(context);
        }

        private static async Task SeedEmployees(TimeTrackingContext context)
        {
            if (context.Employees.Any()) return;

            var employees = new List<Employee>
            {
                new Employee
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Sex = Sex.Male,
                    BirthDate = new DateTime(1990, 1, 1)
                },
                new Employee
                {
                    FirstName = "Jane",
                    LastName = "Smith",
                    Sex = Sex.Female,
                    BirthDate = new DateTime(2001, 5, 15)
                },
                new Employee
                {
                    FirstName = "Alice",
                    LastName = "Johnson",
                    Sex = Sex.Female,
                    BirthDate = new DateTime(1992, 7, 20)
                }
            };

            foreach (var e in employees)
            {
                if ((!context.Employees.Any(emp => emp.FirstName == e.FirstName))
                        && (!context.Employees.Any(emp => emp.LastName == e.LastName)))
                {
                    await context.Employees.AddAsync(e);
                }
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedProjects(TimeTrackingContext context)
        {
            if (context.Projects.Any()) return;

            var projects = new List<Project>
            {
                new Project
                {
                    Name = "Project Alpha",
                    StartDate = DateTime.Now.AddMonths(-1),
                    EndDate = DateTime.Now.AddMonths(5)
                },
                new Project
                {
                    Name = "Project Beta",
                    StartDate = DateTime.Now.AddMonths(-2),
                    EndDate = DateTime.Now.AddMonths(3)
                },
                new Project
                {
                    Name = "Project Gamma",
                    StartDate = DateTime.Now.AddMonths(-3),
                    EndDate = DateTime.Now.AddMonths(2)
                },
                new Project
                {
                    Name = "Project Delta",
                    StartDate = DateTime.Now.AddMonths(-4),
                    EndDate = DateTime.Now.AddMonths(4)
                },
                new Project
                {
                    Name = "Project Epsilon",
                    StartDate = DateTime.Now.AddMonths(-6),
                    EndDate = DateTime.Now.AddMonths(6)
                }
            };

            foreach (var p in projects)
            {
                if (!context.Projects.Any(pr => pr.Name == p.Name))
                {
                    await context.Projects.AddAsync(p);
                }
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedActivities(TimeTrackingContext context)
        {
            if (!context.Projects.Any() || !context.Employees.Any()) return;

            var employees = context.Employees.ToList();
            var projects = context.Projects.ToList();

            var activities = new List<Activity>
            {
                new Activity
                {
                    EmployeeId = employees[0].Id,
                    ProjectId = projects[0].Id,
                    Role = Role.SoftwareEngineer,
                    ActivityType = ActivityType.RegularWork,
                    Date = DateTime.Now.AddDays(-5),
                    Hours = 8
                },
                new Activity
                {
                    EmployeeId = employees[0].Id,
                    ProjectId = projects[4].Id,
                    Role = Role.SoftwareArchitect,
                    ActivityType = ActivityType.Overtime,
                    Date = DateTime.Now.AddDays(-5),
                    Hours = 5
                },
                new Activity
                {
                    EmployeeId = employees[1].Id,
                    ProjectId = projects[1].Id,
                    Role = Role.TeamLead,
                    ActivityType = ActivityType.RegularWork,
                    Date = DateTime.Now.AddDays(-4),
                    Hours = 7
                },
                new Activity
                {
                    EmployeeId = employees[1].Id,
                    ProjectId = projects[2].Id,
                    Role = Role.BusinessAnalyst,
                    ActivityType = ActivityType.Overtime,
                    Date = DateTime.Now.AddDays(-4),
                    Hours = 6
                },
                new Activity
                {
                    EmployeeId = employees[2].Id,
                    ProjectId = projects[3].Id,
                    Role = Role.SoftwareEngineer,
                    ActivityType = ActivityType.RegularWork,
                    Date = DateTime.Now.AddDays(-6),
                    Hours = 8
                },
                new Activity
                {
                    EmployeeId = employees[2].Id,
                    ProjectId = projects[4].Id,
                    Role = Role.TeamLead,
                    ActivityType = ActivityType.Overtime,
                    Date = DateTime.Now.AddDays(-6),
                    Hours = 3
                },
                new Activity
                {
                    EmployeeId = employees[0].Id,
                    ProjectId = projects[2].Id,
                    Role = Role.SoftwareEngineer,
                    ActivityType = ActivityType.RegularWork,
                    Date = DateTime.Now.AddDays(-2),
                    Hours = 8
                }
            };

            foreach (var a in activities)
            {
                bool exists = await context.Activities.AnyAsync(act =>
                    act.EmployeeId == a.EmployeeId &&
                    act.ProjectId == a.ProjectId &&
                    act.Date.Date == a.Date.Date &&
                    act.Hours == a.Hours &&
                    act.Role == a.Role &&
                    act.ActivityType == a.ActivityType);

                if (!exists) 
                {
                    await context.Activities.AddAsync(a);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
