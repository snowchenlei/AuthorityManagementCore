using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.Extensions.Json.Abstractions
{
    public class NullJson : IJson
    {
        public object Deserialize(string json)
        {
            return default;
        }

        public TModel Deserialize<TModel>(string json)
        {
            return default;
        }

        public string Serialize(object model)
        {
            return default;
        }

        public string Serialize<TModel>(object model)
        {
            return default;
        }
    }
}