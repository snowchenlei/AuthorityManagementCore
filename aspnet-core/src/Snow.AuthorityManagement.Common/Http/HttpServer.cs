using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace Snow.AuthorityManagement.Common.Http
{
    public static  class HttpServer
    {
        #region Http请求
        #region Get请求
        #region 同步
        public static string HttpGet(string url, IDictionary<string, string> paras,
            IDictionary<string, string> headers = null)
        {
            string para = string.Join("&", paras.Select(p => $"{p.Key}={p.Value}"));
            return HttpGet(url, para, headers);
        }

        public static string HttpGet(string url, string para,
            IDictionary<string, string> headers = null)
        {
            return HttpGet($"{url}?{para}", headers);
        }

        public static string HttpGet(string url,
            IDictionary<string, string> headers = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("请求地址不能为空");
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "application/json, */*";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            //request.ContentType = "text/html;charset=utf-8";
            if (headers!=null && headers.Count >0)
            {
                foreach (KeyValuePair<string,string> item in headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }

            return HttpGet(request);
        }

        public static string HttpGet(HttpWebRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("请求描述不能为空");
            }
            Stream responseStream = null;
            StreamReader streamReader = null;
            try
            {
                HttpWebResponse response = (HttpWebResponse)(request.GetResponse());
                using (responseStream = response.GetResponseStream())
                using (streamReader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    return streamReader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                {
                    throw ex;
                }

                HttpWebResponse response = (HttpWebResponse)ex.Response;
                if (response.StatusCode >= HttpStatusCode.BadRequest)
                {
                    using (responseStream = response.GetResponseStream())
                    using (streamReader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        throw new WebException(streamReader.ReadToEnd());
                    }
                }
                throw ex;
            }
            finally
            {
                if (responseStream != null)
                {
                    responseStream.Dispose();
                }
                if (streamReader != null)
                {
                    streamReader.Dispose();
                }
            }
        }
        #endregion

        #region 异步
        public static async Task<string> HttpGetAsync(string url, IDictionary<string, string> paras,
            IDictionary<string, string> headers = null)
        {
            string para = string.Join("&", paras.Select(p => $"{p.Key}={p.Value}"));
            return await HttpGetAsync(url, para, headers);
        }

        public static async Task<string> HttpGetAsync(string url, string para,
            IDictionary<string, string> headers = null)
        {
            return await HttpGetAsync($"{url}?{para}", headers);
        }

        public static async Task<string> HttpGetAsync(string url,
            IDictionary<string, string> headers = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("请求地址不能为空");
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "text/html;charset=utf-8";
            request.Method = "Get";
            if (headers != null && headers.Count > 0)
            {
                foreach (KeyValuePair<string, string> item in headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }

            return await HttpGetAsync(request);
        }

        public static async Task<string> HttpGetAsync(HttpWebRequest request)
        {
            Stream responseStream = null;
            StreamReader streamReader = null;
            try
            {
                HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync());
                using (responseStream = response.GetResponseStream())
                using (streamReader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    return await streamReader.ReadToEndAsync();
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
            finally
            {
                if (responseStream != null)
                {
                    responseStream.Dispose();
                }
                if (streamReader != null)
                {
                    streamReader.Dispose();
                }
            }
        }
        #endregion
        #endregion

        #region Post请求
        #region 同步
        public static string HttpPost(string url, IDictionary<string, object> paras,
            IDictionary<string, string> headers = null)
        {
            string para = string.Join("&", paras.Select(p => $"{p.Key}={p.Value}"));
            return HttpPost(url, para, headers);
        }

        public static string HttpPost(string url, string para,
            IDictionary<string, string> headers = null)
        {
            byte[] data = Encoding.UTF8.GetBytes(para);
            return HttpPost(url, data, headers);
        }

        public static string HttpPost(string url, byte[] data,
            IDictionary<string, string> headers = null)
        {
            return HttpPost(url, "application/x-www-form-urlencoded", data, headers);
        }

        public static string HttpPost(string url, string contentType, 
            string para, IDictionary<string, string> headers = null)
        {
            byte[] data = Encoding.UTF8.GetBytes(para);
            return HttpPost(url, contentType, data, headers);
        }
        
        public static string HttpPost(string url, string contentType, 
            byte[] data, IDictionary<string, string> headers = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = contentType;
            request.ContentLength = data.Length;
            if(headers!= null && headers.Count > 0)
            {
                foreach (KeyValuePair<string, string> item in headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }

            Stream requestStream = null;
            try
            {
                using (requestStream = request.GetRequestStream())
                {
                    requestStream.Write(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (requestStream != null)
                {
                    requestStream.Dispose();
                }
            }
            return HttpPost(request);
        }

        public static string HttpPost(HttpWebRequest request)
        {
            Stream responseStream = null;
            StreamReader streamReader = null;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (responseStream = response.GetResponseStream())
                using (streamReader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    return streamReader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                HttpWebResponse response = (HttpWebResponse)ex.Response;
                if (response.StatusCode >= HttpStatusCode.Ambiguous
                    && response.StatusCode < HttpStatusCode.BadRequest)
                {

                }
                if (response.StatusCode >= HttpStatusCode.BadRequest)
                {
                    using (responseStream = response.GetResponseStream())
                    using (streamReader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        throw new WebException(streamReader.ReadToEnd());
                    }
                }
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (responseStream != null)
                {
                    responseStream.Dispose();
                }
                if (streamReader != null)
                {
                    streamReader.Dispose();
                }
            }
        }
        #endregion

        #region 异步
        public static async Task<string> HttpPostAsync(string url, IDictionary<string, string> paras,
            IDictionary<string, string> headers = null)
        {
            string para = string.Join("&", paras.Select(p => $"{p.Key}={p.Value}"));
            return await HttpPostAsync(url, para, headers);
        }

        public static async Task<string> HttpPostAsync(string url, string para,
            IDictionary<string, string> headers = null)
        {
            byte[] data = Encoding.UTF8.GetBytes(para);
            return await HttpPostAsync(url, data, headers);
        }

        public static async Task<string> HttpPostAsync(string url,byte[] data,
            IDictionary<string, string> headers = null)
        {
            return await HttpPostAsync(url, "application/x-www-form-urlencoded", data, headers);
        }

        public static async Task<string> HttpPostAsync(string url, string contentType, 
            string para, IDictionary<string, string> headers = null)
        {
            byte[] data = Encoding.UTF8.GetBytes(para);
            return await HttpPostAsync(url, contentType, data, headers);
        }

        public static async Task<string> HttpPostAsync(string url, string contentType, 
            byte[] data, IDictionary<string, string> headers = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = contentType;
            request.ContentLength = data.Length;
            if(headers != null && headers.Count > 0)
            {
                foreach (KeyValuePair<string, string> item in headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }                
            }

            Stream requestStream = null;
            try
            {
                using (requestStream = await request.GetRequestStreamAsync())
                {
                    await requestStream.WriteAsync(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (requestStream != null)
                {
                    requestStream.Dispose();
                }
            }
            return await HttpPostAsync(request);
        }

        public static async Task<string> HttpPostAsync(HttpWebRequest request)
        {
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            Stream responseStream = null;
            StreamReader streamReader = null;
            try
            {
                using (responseStream = response.GetResponseStream())
                using (streamReader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    return await streamReader.ReadToEndAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (responseStream != null)
                {
                    responseStream.Dispose();
                }
                if (streamReader != null)
                {
                    streamReader.Dispose();
                }
            }
        }
        #endregion
        #endregion
        #endregion
    }
}
