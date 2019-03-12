using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Snow.AuthorityManagement.Common.Conversion;

namespace Snow.AuthorityManagement.Common.Extension
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.LoadAsync();
            session.Set(key,Encoding.Default.GetBytes(Serialization.SerializeObject(value)));
        }

        public static T Get<T>(this ISession session, string key)
        {
            session.LoadAsync();
           if( session.TryGetValue(key, out byte[] bytes))
            {
                return Serialization.DeserializeObject<T>(Encoding.Default.GetString(bytes));
            }
            else
            {
                return default(T);
            }
        }
    }
}
