using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.Extensions.Json.Abstractions
{
    public interface IJson
    {
        string Serialize(object model);
        string Serialize<TModel>(TModel model);

        object Deserialize(string json);
        TModel Deserialize<TModel>(string json);
    }
}