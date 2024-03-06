using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Common.Helpers
{
    public class RedisHelper
    {
        private readonly ConnectionMultiplexer _redis;

        public RedisHelper(IConfiguration configuration)
        {
            string redisConnectionString = configuration.GetConnectionString("RedisConnection");
            _redis = ConnectionMultiplexer.Connect(redisConnectionString);
        }

        public StackExchange.Redis.IDatabase GetDatabase(int db = -1)
        {
            return _redis.GetDatabase(db);
        }
    }

}
