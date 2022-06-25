using GooglePlaceDetailClient.Model;
using Newtonsoft.Json;

namespace GooglePlaceDetailClient
{
    public interface IGooglePlaceDetail
    {
        GooglePlaceDetailResponse GetPlaceDetail(string place_id);
    }
    public class PlaceDetailFuncs : IGooglePlaceDetail
    {
        public GooglePlaceDetailResponse GetPlaceDetail(string place_id)
        {
            var Details = new HttpClient().GetAsync($"https://maps.googleapis.com/maps/api/place/details/json?place_id={place_id}&key=AIzaSyBoI8mRu2B4RZEfCjSE61WjBS8G1dUFu-w").GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<GooglePlaceDetailResponse>(Details.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }
    }
    public static class PlaceDetailClient
    {
        public static IServiceCollection AddGooglePlaceDetailClient(this IServiceCollection service)
        {
            service.AddSingleton<IGooglePlaceDetail, PlaceDetailFuncs>();
            return service;
        }
    }
}