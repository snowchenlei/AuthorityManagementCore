using Snow.AuthorityManagement.Common.Conversion;
using Snow.AuthorityManagement.Common.Http;
using Snow.AuthorityManagement.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Snow.AuthorityManagement.Web.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Name = "至尊";
            return View();
        }

        public string Weather()
        {
            string url = "http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=js";
            string result = HttpServer.HttpGet(url);
            // 请求到的原始string需要处理一下才能解析
            result = result.Split('=')[1].Trim().TrimEnd(';');
            // 解析json字符串
            JObject jobj = JObject.Parse(result);
            // 国家
            string country = jobj["country"]?.ToString();
            // 省份
            string province = jobj["province"]?.ToString();
            // 城市
            string city = jobj["city"]?.ToString();

            result = HttpServer.HttpGet("http://wthrcdn.etouch.cn/weather_mini?city=" + HttpUtility.UrlEncode(city.Replace("市", "")));

            return "";
        }

        public ActionResult Welcome()
        {
            return View();
        }
    }
}