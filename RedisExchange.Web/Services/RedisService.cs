using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchange.Web.Services
{
    public class RedisService
    {
        private readonly string _redisHost;
        private readonly string _redisPort;
        private ConnectionMultiplexer _redis;

        public IDatabase _db { get; set; }
        public RedisService(IConfiguration configuration)
        {
            _redisHost = configuration["Redis:Host"];
            _redisPort = configuration["Redis:Port"];
        }

        public void Connect()
        {
            var configString = $"{_redisHost}:{_redisPort}";

            // redis ile bağlantı kuruluyor.
            _redis = ConnectionMultiplexer.Connect(configString);

        }


        public IDatabase GetDb(int db)
        {
            // db id ye göre geriye db döner
            return _redis.GetDatabase(db);
        }

    }
}
