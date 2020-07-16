using System.Collections.Generic;
using System.Threading.Tasks;
using ParagonTestApplication.ApiTests.Common;
using ParagonTestApplication.ApiTests.Models;
using ParagonTestApplication.Models.ApiModels.Common;
using ParagonTestApplication.Models.ApiModels.Series;
using ParagonTestApplication.Models.ApiModels.Webinars;
using ParagonTestApplication.Models.Common;

namespace ParagonTestApplication.ApiTests.Helpers
{
    public class WebinarHelper
    {
        private readonly HttpClientWrapper _client;

        public WebinarHelper(HttpClientWrapper client)
        {
            _client = client;
        }

        public async Task<Response<WebinarDto>> GetWebinar(int id)
        {
            var result = await _client.GetAsync<Response<WebinarDto>>($"api/webinar/{id}");
            return result;
        }

        public async Task<ValidationErrorResponse> GetWebinarWithInvalidData(string id)
        {
            var result = await _client.GetAsync<ValidationErrorResponse>($"api/webinar/{id}");
            return result;
        }

        public async Task<Response<PagedList<WebinarDto>>> GetWebinarList(WebinarFilter webinarFilter = null,
            PaginationFilter paginationFilter = null)
        {
            static Dictionary<string, string> GetProperties(object @object)
            {
                var propertyDictionary = new Dictionary<string, string>();
                var type = @object?.GetType();
                var properties = type?.GetProperties();
                if (properties == null) return propertyDictionary;
                foreach (var property in properties)
                {
                    var propertyName = property.Name;
                    var propertyValue = property.GetValue(@object);
                    if (propertyValue != null) propertyDictionary.Add(propertyName, propertyValue.ToString());
                }

                return propertyDictionary;
            }

            Dictionary<string, string> AddQuery(Dictionary<string, string> filterQueryDictionary, object filter)
            {
                if (filter == null) return filterQueryDictionary;

                var dictionary = GetProperties(filter);
                foreach (var (key, value) in dictionary) filterQueryDictionary.Add(key, value);

                return filterQueryDictionary;
            }

            var queryDictionary = new Dictionary<string, string>();
            queryDictionary = AddQuery(queryDictionary, webinarFilter);
            queryDictionary = AddQuery(queryDictionary, paginationFilter);

            var result =
                await _client.GetAsync<Response<PagedList<WebinarDto>>>($"{_client.Client.BaseAddress}api/webinar",
                    queryDictionary);
            return result;
        }

        public async Task<Response<WebinarDto>> CreateWebinar(CreateOrUpdateWebinarRequest request)
        {
            var result = await _client.PostAsync<Response<WebinarDto>>("api/webinar", request);
            return result;
        }

        public async Task<ValidationErrorResponse> CreateWebinarWithInvalidData(object request)
        {
            var result = await _client.PostAsync<ValidationErrorResponse>("api/webinar", request);
            return result;
        }

        public async Task<Response<WebinarDto>> UpdateWebinar(int id, CreateOrUpdateWebinarRequest request)
        {
            var result = await _client.PutAsync<Response<WebinarDto>>($"api/webinar/{id}", request);
            return result;
        }

        public async Task<ValidationErrorResponse> UpdateWebinarWithInvalidData(string id, object request)
        {
            var result = await _client.PutAsync<ValidationErrorResponse>($"api/webinar/{id}", request);
            return result;
        }

        public async Task<Response<WebinarDto>> DeleteWebinar(int id)
        {
            var result = await _client.DeleteAsync<Response<WebinarDto>>($"api/webinar/{id}");
            return result;
        }

        public async Task<ValidationErrorResponse> DeleteWebinarWithInvalidId(string id)
        {
            var result = await _client.DeleteAsync<ValidationErrorResponse>($"api/webinar/{id}");
            return result;
        }

        public async Task<Response<IEnumerable<SeriesDto>>> GetSeries()
        {
            var result = await _client.GetAsync<Response<IEnumerable<SeriesDto>>>("api/series");
            return result;
        }
    }
}