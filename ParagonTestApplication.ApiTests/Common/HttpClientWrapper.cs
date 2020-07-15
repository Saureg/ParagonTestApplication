using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParagonTestApplication.ApiTests.Helpers;

namespace ParagonTestApplication.ApiTests.Common
{
    public class HttpClientWrapper
    {
        public HttpClientWrapper(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public async Task<T> GetAsync<T>(string url)
        {
            var response = await Client.GetAsync(url);

            var responseText = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(responseText);
            return data;
        }

        public async Task GetAsync(string url)
        {
            var response = await Client.GetAsync(url);
        }

        public async Task<T> PostAsync<T>(string url, object body)
        {
            var response = await Client.PostAsync(url, new JsonContent(body));

            response.EnsureSuccessStatusCode();

            var responseText = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(responseText);
            return data;
        }

        public async Task PostAsync(string url, object body)
        {
            var response = await Client.PostAsync(url, new JsonContent(body));

            response.EnsureSuccessStatusCode();
        }

        public async Task<T> PutAsync<T>(string url, object body)
        {
            var response = await Client.PutAsync(url, new JsonContent(body));

            response.EnsureSuccessStatusCode();

            var respnoseText = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(respnoseText);
            return data;
        }

        public async Task DeleteAsync(string url)
        {
            var response = await Client.DeleteAsync(url);

            response.EnsureSuccessStatusCode();
        }
    }
}