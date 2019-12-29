using Newtonsoft.Json;
using Student.Web.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Student.Web.Utility
{
    public class ApiHelper
    {
        public static async Task<T> GetAsync<T>(string url, string token)
        {
            using (var httpClient = new HttpClient())
            {
                if (!string.IsNullOrEmpty(token) && token != "Google")
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                }
                using (var response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(apiResponse);
                }
            }
        }

        public static async Task<T> PostAsync<T>(string url, string token, PostObject model)
        {
            using (var httpClient = new HttpClient())
            {
                var formContent = new FormUrlEncodedContent(JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(model.PostData)));
                httpClient.DefaultRequestHeaders.Accept.Clear();
                if (!string.IsNullOrEmpty(token) && token != "Google")
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                using (var response = await httpClient.PostAsync(url, formContent))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(apiResponse);
                }
            }
        }

        public static async Task<T> PutAsync<T>(string url, string token, PostObject model)
        {
            using (var httpClient = new HttpClient())
            {
                var formContent = new FormUrlEncodedContent(JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(model.PostData)));
                httpClient.DefaultRequestHeaders.Accept.Clear();
                if (!string.IsNullOrEmpty(token) && token != "Google")
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                using (var response = await httpClient.PutAsync(url, formContent))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(apiResponse);
                }
            }
        }

        public static async Task<T> DeleteAsync<T>(string url, string token)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                if (!string.IsNullOrEmpty(token) && token != "Google")
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                using (var response = await httpClient.DeleteAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(apiResponse);
                }
            }
        }
    }
}
