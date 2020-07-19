namespace ParagonTestApplication.ApiClient.ClientWrapper
{
    using System.Net.Http;
    using System.Text;
    using Newtonsoft.Json;

    /// <summary>
    /// JsonContent.
    /// </summary>
    public class JsonContent : StringContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonContent"/> class.
        /// </summary>
        /// <param name="obj">Object.</param>
        public JsonContent(object obj)
            : base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
        {
        }
    }
}