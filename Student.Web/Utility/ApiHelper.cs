using Newtonsoft.Json;
using Student.Web.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Student.Web.Utility
{
    public class ApiHelper
    {
        public static async Task<T> GetAsync<T>(string url)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(apiResponse);
                }
            }
        }

        public static async Task<T> PostAsync<T>(string url, PostObject model)
        {
            using (var httpClient = new HttpClient())
            {
                string json = JsonConvert.SerializeObject(model.PostData);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsJsonAsync(url, model.PostData))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(apiResponse);
                }
            }
        }

        public static async Task<T> PutAsync<T>(string url, PostObject model)
        {
            using (var httpClient = new HttpClient())
            {
                string json = JsonConvert.SerializeObject(model.PostData);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                using (var response = await httpClient.PutAsJsonAsync(url, model.PostData))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(apiResponse);
                }
            }
        }

        public static async Task<T> DeleteAsync<T>(string url)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(apiResponse);
                }
            }
        }
    }
}
