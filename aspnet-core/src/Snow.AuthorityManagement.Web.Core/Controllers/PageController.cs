using Anc.Application.Services.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Snow.AuthorityManagement.Application.Dto;
using Snow.AuthorityManagement.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Snow.AuthorityManagement.Web.Core.Controllers
{
    [ApiController]
    public class PageController : ControllerBase
    {
        private readonly IMapper _mapper;

        public PageController(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// 创建分页地址
        /// </summary>
        /// <typeparam name="T">分页请求参数类型</typeparam>
        /// <param name="parameters">分页请求参数</param>
        /// <param name="actionName">分页地址名</param>
        /// <param name="uriType">地址类型</param>
        /// <param name="pageCount"></param>
        /// <returns>地址</returns>
        protected string CreateUri<T>(T parameters, string actionName, PaginationResourceUriType uriType, int pageCount = 0) where T : PagedAndSortedInputDto, new()
        {
            string url;
            switch (uriType)
            {
                case PaginationResourceUriType.PreviousPage:
                    T previousParamaters = new T();
                    previousParamaters = _mapper.Map(parameters, previousParamaters);
                    previousParamaters.PageIndex = previousParamaters.PageIndex > pageCount
                        ? pageCount
                        : previousParamaters.PageIndex - 1;

                    url = Url.Link(actionName, previousParamaters);
                    break;

                case PaginationResourceUriType.NextPage:
                    T nextParamaters = new T();
                    nextParamaters = _mapper.Map(parameters, nextParamaters);
                    nextParamaters.PageIndex++;
                    url = Url.Link(actionName, nextParamaters);
                    break;

                case PaginationResourceUriType.CurrentPage:
                default:
                    url = Url.Link(actionName, parameters);
                    break;
            }

            string[] results = url.Split('?');
            string newParameters = string.Join("&", results[1].Split('&').Select(p =>
            {
                var kv = p.Split('=');
                var k = Regex.Replace(kv[0], "([a-z])([A-Z])", "$1_$2").ToLower();
                var v = kv[1];
                return $"{k}={v}";
            }));
            url = results[0] + "?" + newParameters;//ParameterHelper.ReplaceParameters(url, "_");
            return url;
        }

        /// <summary>
        /// 分页返回
        /// </summary>
        /// <typeparam name="TInput">输入类型</typeparam>
        /// <typeparam name="TOutput">响应类型</typeparam>
        /// <typeparam name="T">响应具体类型</typeparam>
        /// <param name="parameters">输入参数</param>
        /// <param name="actionName">分页地址名</param>
        /// <param name="output">响应参数</param>
        /// <returns></returns>
        protected IActionResult Return<TInput, TOutput, T>(TInput parameters, string actionName, TOutput output)
            where TInput : PagedAndSortedInputDto, new()
            where TOutput : PagedResultDto<T>
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }

            string previousLink = output.HasPrevious
                ? CreateUri(parameters, actionName, PaginationResourceUriType.PreviousPage, output.PageCount)
                : String.Empty;
            string nextLink = output.HasNext
                ? CreateUri(parameters, actionName, PaginationResourceUriType.NextPage)
                : String.Empty;
            var meta = new
            {
                TotalCount = output.TotalCount,
                PageIndex = output.PageIndex,
                PageSize = output.PageSize,
                PageCount = output.PageCount,
                PreviousLink = previousLink,
                NextLink = nextLink
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta, new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            }));
            return Ok(output);
        }
    }
}