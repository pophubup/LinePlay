using GoogleTextSerachClient.Model;
using Newtonsoft.Json;

namespace GoogleTextSerachClient
{
    public class PlaceDetailFuncs
    {
        public GoogleTextSearchResponse GetPlaceDetail(string Address, string keyword)
        {
            var Details = new HttpClient().GetAsync($"https://maps.googleapis.com/maps/api/place/textsearch/json?query={Address} {keyword}&radius=1000&key=AIzaSyBoI8mRu2B4RZEfCjSE61WjBS8G1dUFu-w").GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<GoogleTextSearchResponse>(Details.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }
    }
    public static class TextSearchClient
    {
        public static IServiceCollection AddGoogleTextSearchClient(this IServiceCollection service)
        {
            service.AddScoped<PlaceDetailFuncs>();
            return service;
        }
    }
}