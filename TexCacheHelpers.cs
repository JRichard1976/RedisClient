using StackExchange.Redis;
using System.Threading.Channels;

namespace RedisClient
{
    internal static class TexCacheHelpers
    {
        static readonly ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(
                new ConfigurationOptions
                {
                    EndPoints = { "localhost:6379" }
                });
        
        
        public static Task SetValue(string Key, string Value)
        {
            var db = redis.GetDatabase();
            db.StringSet(Key, Value);
            return Task.CompletedTask;

        }
        public static Task<string> GetValue(string Key)
        {
            var db = redis.GetDatabase();
            var res = db.StringGet (Key);
            return Task.FromResult(res.ToString());

        }
    }
}