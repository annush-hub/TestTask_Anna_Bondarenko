using Newtonsoft.Json;
using System.Text;
using TimeTracking.Core;

namespace Time_Tracking_App.Services
{
    public class ActivityService
    {
        private readonly HttpClient _httpClient;
        private readonly string _reportingAppUrl = "http://localhost:5001/activity"; 

        public ActivityService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateActivityAsync(Activity activity)
        {
            await SendActivityToReportingAppAsync(activity);
        }

        private async Task SendActivityToReportingAppAsync(Activity activity)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(activity), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_reportingAppUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error sending activity to Reporting App: {response.StatusCode}");
                }
                else
                {
                    Console.WriteLine("Activity sent to Reporting App successfully.");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }

}
