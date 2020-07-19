namespace ParagonTestApplication.ApiClient.ClientWrapper
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;
    using Newtonsoft.Json;

    /// <summary>
    /// HttpClientWrapper.
    /// </summary>
    public class HttpClientWrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientWrapper"/> class.
        /// </summary>
        /// <param name="client">HttpClient.</param>
        public HttpClientWrapper(HttpClient client)
        {
            this.Client = client;
        }

        /// <summary>
        /// Gets httpClient.
        /// </summary>
        public HttpClient Client { get; }

        /// <summary>
        /// Get.
        /// </summary>
        /// <typeparam name="T">Response type.</typeparam>
        /// <param name="url">Uri.</param>
        /// <param name="queryDictionary">Quesy dictionary.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<T> GetAsync<T>(string url, Dictionary<string, string> queryDictionary = null)
        {
            if (queryDictionary != null)
            {
                var uriBuilder = new UriBuilder(url);
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                foreach (var (key, value) in queryDictionary) query[key] = value;

                uriBuilder.Query = query.ToString() !;
                url = uriBuilder.ToString();
            }

            var response = await this.Client.GetAsync(url);

            var responseText = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(responseText);
            return data;
        }

        /// <summary>
        /// Post.
        /// </summary>
        /// <typeparam name="T">Response type.</typeparam>
        /// <param name="url">Uri.</param>
        /// <param name="body">Body.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<T> PostAsync<T>(string url, object body)
        {
            var response = await this.Client.PostAsync(url, new JsonContent(body));

            var responseText = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(responseText);
            return data;
        }

        /// <summary>
        /// Put.
        /// </summary>
        /// <typeparam name="T">Response type.</typeparam>
        /// <param name="url">Uri.</param>
        /// <param name="body">Body.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<T> PutAsync<T>(string url, object body)
        {
            var response = await this.Client.PutAsync(url, new JsonContent(body));

            var responseText = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(responseText);
            return data;
        }

        /// <summary>
        /// Delete.
        /// </summary>
        /// <typeparam name="T">Response type.</typeparam>
        /// <param name="url">Uri.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<T> DeleteAsync<T>(string url)
        {
            var response = await this.Client.DeleteAsync(url);

            var responseText = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(responseText);
            return data;
        }
    }
}