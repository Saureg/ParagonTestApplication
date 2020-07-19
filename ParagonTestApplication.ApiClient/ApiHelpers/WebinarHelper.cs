namespace ParagonTestApplication.ApiClient.ApiHelpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ParagonTestApplication.ApiClient.ClientWrapper;
    using ParagonTestApplication.ApiClient.Models;
    using ParagonTestApplication.Models.ApiModels.Common;
    using ParagonTestApplication.Models.ApiModels.Series;
    using ParagonTestApplication.Models.ApiModels.Webinars;
    using ParagonTestApplication.Models.Common;

    /// <summary>
    /// Webinar API Helper.
    /// </summary>
    public class WebinarHelper
    {
        private readonly HttpClientWrapper client;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebinarHelper"/> class.
        /// </summary>
        /// <param name="client">HttpClientWrapper.</param>
        public WebinarHelper(HttpClientWrapper client)
        {
            this.client = client;
        }

        /// <summary>
        /// Get webinar.
        /// </summary>
        /// <param name="id">Webinar id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<Response<WebinarDto>> GetWebinar(int id)
        {
            var result = await this.client.GetAsync<Response<WebinarDto>>($"api/webinar/{id}");
            return result;
        }

        /// <summary>
        /// Get webinar (invalid parameters).
        /// </summary>
        /// <param name="id">Webinar id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<ValidationErrorResponse> GetWebinarWithInvalidData(string id)
        {
            var result = await this.client.GetAsync<ValidationErrorResponse>($"api/webinar/{id}");
            return result;
        }

        /// <summary>
        /// Get webinar list.
        /// </summary>
        /// <param name="webinarFilter">Webinar filter.</param>
        /// <param name="paginationFilter">Pagination filter.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<Response<PagedList<WebinarDto>>> GetWebinarList(
            WebinarFilter webinarFilter = null,
            PaginationFilter paginationFilter = null)
        {
            static Dictionary<string, string> GetProperties(object @object)
            {
                var propertyDictionary = new Dictionary<string, string>();
                var type = @object?.GetType();
                var properties = type?.GetProperties();
                if (properties == null)
                {
                    return propertyDictionary;
                }

                foreach (var property in properties)
                {
                    var propertyName = property.Name;
                    var propertyValue = property.GetValue(@object);
                    if (propertyValue != null)
                    {
                        propertyDictionary.Add(propertyName, propertyValue.ToString());
                    }
                }

                return propertyDictionary;
            }

            Dictionary<string, string> AddQuery(Dictionary<string, string> filterQueryDictionary, object filter)
            {
                if (filter == null)
                {
                    return filterQueryDictionary;
                }

                var dictionary = GetProperties(filter);
                foreach (var (key, value) in dictionary) filterQueryDictionary.Add(key, value);

                return filterQueryDictionary;
            }

            var queryDictionary = new Dictionary<string, string>();
            queryDictionary = AddQuery(queryDictionary, webinarFilter);
            queryDictionary = AddQuery(queryDictionary, paginationFilter);

            var result =
                await this.client.GetAsync<Response<PagedList<WebinarDto>>>(
                    $"{this.client.Client.BaseAddress}api/webinar",
                    queryDictionary);
            return result;
        }

        /// <summary>
        /// Create webinar.
        /// </summary>
        /// <param name="request">Webinar request.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<Response<WebinarDto>> CreateWebinar(CreateOrUpdateWebinarRequest request)
        {
            var result = await this.client.PostAsync<Response<WebinarDto>>("api/webinar", request);
            return result;
        }

        /// <summary>
        /// Create webinar (with invalid request).
        /// </summary>
        /// <param name="request">Webinar request.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<ValidationErrorResponse> CreateWebinarWithInvalidData(object request)
        {
            var result = await this.client.PostAsync<ValidationErrorResponse>("api/webinar", request);
            return result;
        }

        /// <summary>
        /// Update webinar.
        /// </summary>
        /// <param name="id">Webinar id.</param>
        /// <param name="request">Webinar request.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<Response<WebinarDto>> UpdateWebinar(int id, CreateOrUpdateWebinarRequest request)
        {
            var result = await this.client.PutAsync<Response<WebinarDto>>($"api/webinar/{id}", request);
            return result;
        }

        /// <summary>
        /// Update webinar (with invalid request).
        /// </summary>
        /// <param name="id">Webinar id.</param>
        /// <param name="request">Webinar request.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<ValidationErrorResponse> UpdateWebinarWithInvalidData(string id, object request)
        {
            var result = await this.client.PutAsync<ValidationErrorResponse>($"api/webinar/{id}", request);
            return result;
        }

        /// <summary>
        /// Delete webinar.
        /// </summary>
        /// <param name="id">Webinar id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<Response<WebinarDto>> DeleteWebinar(int id)
        {
            var result = await this.client.DeleteAsync<Response<WebinarDto>>($"api/webinar/{id}");
            return result;
        }

        /// <summary>
        /// Delete webinar (with invalid id).
        /// </summary>
        /// <param name="id">Webinar id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<ValidationErrorResponse> DeleteWebinarWithInvalidId(string id)
        {
            var result = await this.client.DeleteAsync<ValidationErrorResponse>($"api/webinar/{id}");
            return result;
        }

        /// <summary>
        /// Get series list.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<Response<IEnumerable<SeriesDto>>> GetSeries()
        {
            var result = await this.client.GetAsync<Response<IEnumerable<SeriesDto>>>("api/series");
            return result;
        }
    }
}