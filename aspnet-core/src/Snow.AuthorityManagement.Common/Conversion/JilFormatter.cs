using System;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cl.AuthorityManagement.Common
{
    public class JilFormatter : MediaTypeFormatter
    {
        private readonly Options mJilOptions;

        /// <summary>  
        /// Jil Json序列化器  
        /// </summary>  
        public JilFormatter()
        {
            mJilOptions = new Options(
                dateFormat: DateTimeFormat.MillisecondsSinceUnixEpoch,
                excludeNulls: true,
                includeInherited: true,
                serializationNameFormat: SerializationNameFormat.CamelCase);
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            SupportedEncodings.Add(new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true));
            SupportedEncodings.Add(new UnicodeEncoding(bigEndian: false, byteOrderMark: true, throwOnInvalidBytes: true));
        }
        public override bool CanReadType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return true;
        }
        public override bool CanWriteType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return true;
        }
        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            return Task.FromResult<object>(this.DeserializeFromStream(type, readStream));
        }
        private object DeserializeFromStream(Type type, Stream readStream)
        {
            try
            {
                using (var reader = new StreamReader(readStream))
                {
                    MethodInfo method = typeof(JSON).GetMethod("Deserialize", new Type[] { typeof(TextReader), typeof(Options) });
                    MethodInfo generic = method.MakeGenericMethod(type);
                    return generic.Invoke(this, new object[] { reader, mJilOptions });
                }
            }
            catch
            {
                return null;
            }
        }
        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content, TransportContext transportContext)
        {
            var streamWriter = new StreamWriter(writeStream);
            JSON.Serialize(value, streamWriter, mJilOptions);
            streamWriter.Flush();
            return Task.FromResult(writeStream);
        }
    }
}
