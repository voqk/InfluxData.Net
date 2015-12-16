﻿using Newtonsoft.Json;
using System.Net;

namespace InfluxData.Net.InfluxDb.Infrastructure
{
    public static class InfluxDbResponseExtensions
    {
        public static T ReadAs<T>(this InfluxDbApiResponse response)
        {
            var @object = JsonConvert.DeserializeObject<T>(response.Body);

            return @object;
        }
    }
}