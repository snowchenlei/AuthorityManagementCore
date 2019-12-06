using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Snow.Extensions.Json.Abstractions;

namespace Snow.Extensions.Json.SystemText
{
    public class SystemTextJson : IJson
    {
        public object Deserialize(string json)
        {
            return JsonSerializer.Deserialize(json, typeof(Object));
        }

        public TModel Deserialize<TModel>(string json)
        {
            return JsonSerializer.Deserialize<TModel>(json);
        }

        public string Serialize(object model)
        {
            return JsonSerializer.Serialize(model);
        }

        public string Serialize<TModel>(TModel model)
        {
            return JsonSerializer.Serialize<TModel>(model);
        }
    }
}