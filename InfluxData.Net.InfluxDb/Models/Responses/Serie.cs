using System;
using System.Collections.Generic;
using System.Linq;

namespace InfluxData.Net.InfluxDb.Models.Responses
{
    /// <summary>
    /// A class representing a time series point for db reads/queries
    /// </summary>
    public class Serie
    {
        public Serie()
        {
            Tags = new Dictionary<string, string>();
        }

        private Serie(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public Dictionary<string, string> Tags { get; set; }

        public string[] Columns { get; set; }

        public object[][] Values { get; set; }
    }
}