using System;
using System.Collections.Generic;
using System.Text;
using Anc.Auditing.Anc.Auditing;
using Anc.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Anc.Auditing
{
    public class JsonNetAuditSerializer : IAuditSerializer, ITransientDependency
    {
        protected AncAuditingOptions Options;

        public JsonNetAuditSerializer(IOptions<AncAuditingOptions> options)
        {
            Options = options.Value;
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, GetSharedJsonSerializerSettings());
        }

        private static readonly object SyncObj = new object();
        private static JsonSerializerSettings _sharedJsonSerializerSettings;

        private JsonSerializerSettings GetSharedJsonSerializerSettings()
        {
            if (_sharedJsonSerializerSettings == null)
            {
                lock (SyncObj)
                {
                    if (_sharedJsonSerializerSettings == null)
                    {
                        _sharedJsonSerializerSettings = new JsonSerializerSettings
                        {
                            ContractResolver = new AuditingContractResolver(Options.IgnoredTypes)
                        };
                    }
                }
            }

            return _sharedJsonSerializerSettings;
        }
    }
}