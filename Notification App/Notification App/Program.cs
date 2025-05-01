using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Notification_App;

namespace NotificationApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string url = "http://localhost:5001";  

            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(url + "/activity/"); 
            listener.Start();
            Console.WriteLine("Listening for activity data on " + url + "/activity/");

            while (true)
            {               
                HttpListenerContext context = await listener.GetContextAsync();
                HttpListenerRequest request = context.Request;

                if (request.HttpMethod == "POST")
                {
                    string body;
                    using (StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding))
                    {
                        body = reader.ReadToEnd();
                    }

                    Activity activity = JsonConvert.DeserializeObject<Activity>(body);
                    ProcessActivity(activity);

                    HttpListenerResponse response = context.Response;
                    string responseString = "{\"message\": \"Activity received and processed\"}";
                    byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                    response.ContentLength64 = buffer.Length;
                    await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                    response.Close();
                }
            }
        }

        private static void ProcessActivity(Activity activity)
        {
            Console.WriteLine($"New activity received for {activity.EmployeeFullName} on {activity.Date}");
            Console.WriteLine($"Project: {activity.ProjectName}, Role: {activity.Role}, Type: {activity.ActivityType}, Hours: {activity.Hours}");

            SimulateEmailSend(activity);
        }

        private static void SimulateEmailSend(Activity activity)
        {
            Console.WriteLine($"Sending email: Notification about activity for {activity.EmployeeFullName}");
            Console.WriteLine($"Subject: New activity in {activity.ProjectName}");
            Console.WriteLine($"Body: {activity.EmployeeFullName} worked {activity.Hours} hours on {activity.ProjectName} ({activity.ActivityType}) as a {activity.Role}");
        }
    }
    
}
