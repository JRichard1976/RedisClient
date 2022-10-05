
using RedisClient;
using StackExchange.Redis;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

internal  class Program
{
    static readonly ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(
                 new ConfigurationOptions
                 {
                     EndPoints = { "localhost:6379" }
                 });
    static async Task Main(string[] args)
    {
        //var test =  await TexCache.Connect();
        string EXPIRED_KEYS_CHANNEL = "__keyevent@0__:*";
        ISubscriber subscriber = redis.GetSubscriber();
        subscriber.Subscribe(EXPIRED_KEYS_CHANNEL, async (channel, key) =>
        {
            var Res = await TexCacheHelpers.GetValue($"{key}");
            Console.WriteLine($"{channel}: {key} {Res}");
            
        }
        );
        Console.WriteLine("Listening for events...");
        for (int K=0; K<10; K++)
        {
            await TexCacheHelpers.SetValue("Bassetti2.Tes." + K, "Test3" + K);
            
            
            var Res = await TexCacheHelpers.GetValue("Bassetti.Tes." + K.ToString());

            if (Res == "")
                Console.WriteLine("Don't exist");
            else
                Console.WriteLine(Res.ToString());

        }
        
        Console.ReadKey();
    }
   
}