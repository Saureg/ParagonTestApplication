using System.Threading.Tasks;
using ParagonTestApplication.ApiTests.Common;
using ParagonTestApplication.ApiTests.Models;
using ParagonTestApplication.Models.ApiModels.Common;
using ParagonTestApplication.Models.ApiModels.Webinars;

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

        public async Task<BadRequestResponse> GetWebinarWithBadRequestResponse(string id)
        {
            var result = await _client.GetAsync<BadRequestResponse>($"api/webinar/{id}");
            return result;
        }

        public async Task<Response<WebinarDto>> CreateWebinar(CreateOrUpdateWebinarRequest request)
        {
            var result = await _client.PostAsync<Response<WebinarDto>>("api/webinar", request);
            return result;
        }

        public async Task DeleteWebinar(int id)
        {
            await _client.DeleteAsync($"api/webinar/{id}");
        }
    }
}