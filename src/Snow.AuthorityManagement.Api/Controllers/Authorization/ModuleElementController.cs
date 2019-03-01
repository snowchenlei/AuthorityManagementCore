using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cl.AuthorityManagement.Common;
using Cl.AuthorityManagement.Entity;
using Cl.AuthorityManagement.Enum;
using Cl.AuthorityManagement.IServices;
using Cl.AuthorityManagement.Model;
using Cl.AuthorityManagement.Model.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cl.AuthorityManagement.Api.Controllers.Authorization
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleElementController : ApiBaseController
    {
        private readonly IMapper Mapper = null;
        private readonly IModuleElementServices ModuleElementServices = null;
        public ModuleElementController(
            IMapper mapper,
            IModuleElementServices moduleElementServices)
        {
            Mapper = mapper;
            ModuleElementServices = moduleElementServices;
        }

        [HttpGet]
        [Route("list")]
        public IActionResult Load(int pageIndex, int pageSize, string sort,
            OrderType order, string name, DateTime startTime, DateTime endTime)
        {
            PageHelper.GetPageIndex(ref pageIndex);
            PageHelper.GetPageSize(ref pageSize);
            IQueryable<ModuleElement> tempElements = ModuleElementServices.LoadEntities(e => true);

            #region 查询
            if (!String.IsNullOrEmpty(name))
            {
                tempElements = tempElements.Where(u => u.Name.Contains(name.Trim()));
            }
            if (startTime > new DateTime(1970, 1, 1) && startTime != endTime)
            {
                tempElements = tempElements.Where(u => u.AddTime > startTime);
            }
            if (endTime > startTime)
            {
                tempElements = tempElements.Where(u => u.AddTime < endTime);
            }
            #endregion

            #region 排序
            if ("AddTime".Equals(sort, StringComparison.InvariantCultureIgnoreCase))
            {
                tempElements = Sort(tempElements, e => e.AddTime, order).ThenBy(e => e.ID);
            }
            else if ("Sort".Equals(sort, StringComparison.InvariantCultureIgnoreCase))
            {
                tempElements = Sort(tempElements, e => e.Sort, order).ThenBy(e => e.ID);
            }
            else
            {
                tempElements = Sort(tempElements, u => u.ID, order);
            }
            #endregion
            var roles = ModuleElementServices
                .LoadPageEntities(pageIndex, pageSize, tempElements);
            int totalCount = roles.Count();
            int pageCount = PageHelper.GetPageCount(totalCount, pageSize);
            return Ok(new
            {
                total = totalCount,
                rows = roles.Select(u => new
                {
                    Id = u.ID,
                    u.DomId,
                    u.Attr,
                    u.Class,
                    u.Icon,
                    u.Remark,
                    u.Script,
                    u.Type,
                    u.Sort,
                    u.Name,
                    u.Action,
                    u.AddTime
                })
            });
        }

        [HttpPost]
        [Route("add")]
        public ActionResult Add(ModuleElementEdit moduleElementEdit)
        {
            ModuleElement moduleElement = Mapper.Map<ModuleElement>(moduleElementEdit);
            moduleElement = ModuleElementServices.AddEntity(moduleElement);
            return Ok(new Result<int>
            {
                State = 1,
                Message = "添加成功",
                Data = moduleElement.ID
            });
        }

        [HttpPost]
        [Route("edit")]
        public ActionResult Edit(ModuleElementEdit moduleElementEdit)
        {
            ModuleElement moduleElement = ModuleElementServices
                .LoadFirst(r => r.ID == moduleElementEdit.ID.Value);
            if (moduleElement == null)
            {
                return NotFound(new Result
                {
                    State = 0,
                    Message = "修改的角色不存在"
                });
            }
            moduleElement = Mapper.Map(moduleElementEdit, moduleElement);
            if (ModuleElementServices.EditEntity(moduleElement))
            {
                return Ok(new Result
                {
                    State = 1,
                    Message = "修改成功"
                });
            }
            else
            {
                return BadRequest(new Result
                {
                    State = 0,
                    Message = "修改失败"
                });
            }
        }

        [HttpPost]
        [Route("delete")]
        public ActionResult Delete(int id)
        {
            ModuleElement moduleElement = ModuleElementServices
                .LoadFirst(u => u.ID == id);
            if (moduleElement == null)
            {
                return NotFound(new Result
                {
                    State = 0,
                    Message = "模块元素不存在"
                });
            }
            ModuleElementServices.DeleteEntity(moduleElement);
            return Ok(new Result
            {
                State = 1,
                Message = "删除成功"
            });

        }
    }
}

