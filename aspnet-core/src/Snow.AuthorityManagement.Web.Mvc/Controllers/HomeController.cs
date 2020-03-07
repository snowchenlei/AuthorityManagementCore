using Snow.AuthorityManagement.Common.Conversion;
using Snow.AuthorityManagement.Common.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Snow.AuthorityManagement.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            this._logger = logger;
        }

        // GET: Home
        public ActionResult Index()
        {
            //string path =
            //    @"D:\Cl\git\AuthorityManagementCore\src\Snow.AuthorityManagement.Data/bin/Debug/netcoreapp2.1/Snow.AuthorityManagement.Data.dll";
            //byte[] fileData = System.IO.File.ReadAllBytes(path);
            //Assembly assembly = Assembly.Load(fileData);
            //Type type = assembly.GetType("Snow.AuthorityManagement.Data.AuthorityManagementContext");
            //PropertyInfo[] modelTypes = type.GetProperties().Where(p => p.GetMethod.IsVirtual).ToArray();
            ViewBag.Name = HttpContext.User.Claims.SingleOrDefault(t => t.Type == ClaimTypes.Name)?.Value;
            ViewBag.Title = "Snow Cms";
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