using StackExchange.Redis;

namespace RedisClient
{
    public static class RedisService
    {
        public static IServiceCollection AddArangoDBServices(this IServiceCollection service)
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("redis://default:redispw@localhost:49153");
            IDatabase db = redis.GetDatabase();

            return service;
        }
    }
}