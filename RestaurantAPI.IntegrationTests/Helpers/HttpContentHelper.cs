using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace RestaurantAPI.IntegrationTests.Helpers
{
    public static class HttpContentHelper
    {
        public static HttpContent ToJsonHttpContent(this object obj)
        {
            var json = JsonSerializer.Serialize(obj);

            var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            return httpContent;
        }
    }
}
