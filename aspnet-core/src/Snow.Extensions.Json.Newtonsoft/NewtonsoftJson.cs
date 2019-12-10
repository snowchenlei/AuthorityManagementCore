using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Snow.Extensions.Json.Abstractions;

namespace Snow.Extensions.Json.Newtonsoft
{
    public class NewtonsoftJson : IJson
    {
        public object Deserialize(string json)
        {
            return JsonConvert.DeserializeObject(json);
        }

        public TModel Deserialize<TModel>(string json)
        {
            return JsonConvert.DeserializeObject<TModel>(json);
        }

        public string Serialize(object model)
        {
            return JsonConvert.SerializeObject(model);
        }

        public string Serialize<TModel>(TModel model)
        {
            return JsonConvert.SerializeObject(model);
        }
    }
}