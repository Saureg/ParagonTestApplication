using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
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

        public async Task<T> GetAsync<T>(string url, Dictionary<string, string> queryDictionary = null)
        {
            if (queryDictionary != null)
            {
                var uriBuilder = new UriBuilder(url);
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                foreach (var (key, value) in queryDictionary) query[key] = value;

                uriBuilder.Query = query.ToString()!;
                url = uriBuilder.ToString();
            }

            var response = await Client.GetAsync(url);

            var responseText = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(responseText);
            return data;
        }

        public async Task<T> PostAsync<T>(string url, object body)
        {
            var response = await Client.PostAsync(url, new JsonContent(body));

            var responseText = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(responseText);
            return data;
        }

        public async Task<T> PutAsync<T>(string url, object body)
        {
            var response = await Client.PutAsync(url, new JsonContent(body));

            var responseText = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(responseText);
            return data;
        }

        public async Task<T> DeleteAsync<T>(string url)
        {
            var response = await Client.DeleteAsync(url);

            var responseText = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(responseText);
            return data;
        }
    }
}