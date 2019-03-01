using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    public class ModuleController : ApiBaseController
    {
        private readonly IMapper Mapper = null;
        private IModuleServices ModuleServices = null;
        public ModuleController(
            IMapper mapper, 
            IModuleServices moduleServices)
        {
            Mapper = mapper;
            ModuleServices = moduleServices;
        }

        [HttpGet]
        [Route("list")]
        public IActionResult Modules(int pageIndex,int pageSize, string sort,
            OrderType order, string moduleName, int parentId, DateTime startTime, DateTime endTime)
        {
            PageHelper.GetPageIndex(ref pageIndex);
            PageHelper.GetPageSize(ref pageSize);

            var tempModules = ModuleServices.LoadEntities(m => true);

            #region 查询
            if (!String.IsNullOrEmpty(moduleName))
            {
                tempModules = tempModules.Where(u => u.Name.Contains(moduleName.Trim()));
            }
            if (parentId > 0)
            {
                tempModules = tempModules.Where(u => u.Parent.ID == parentId);
            }
            if (startTime > new DateTime(1970, 1, 1) && startTime != endTime)
            {
                tempModules = tempModules.Where(u => u.AddTime > startTime);
            }
            if (endTime > startTime)
            {
                tempModules = tempModules.Where(u => u.AddTime < endTime);
            }
            #endregion

            #region 排序
            if ("AddTime".Equals(sort, StringComparison.InvariantCultureIgnoreCase))
            {
                tempModules = Sort(tempModules, r => r.AddTime, order).ThenBy(r => r.ID);
            }
            else if ("Sort".Equals(sort, StringComparison.InvariantCultureIgnoreCase))
            {
                tempModules = Sort(tempModules, r => r.Sort, order).ThenBy(r => r.ID);
            }
            else
            {
                tempModules = Sort(tempModules, r => r.ID, order);
            }
            #endregion

            int totalCount = tempModules.Count();
            var modules = ModuleServices
                .LoadPageEntities(pageIndex, pageSize, tempModules);

            int pageCount = PageHelper.GetPageCount(totalCount, pageSize);
            return Ok(new Result<Object>
            {
                State = 1,
                Message = "获取成功",
                Data = new {
                    total = totalCount,
                    rows = modules.Select(m => new
                    {
                        ID = m.ID,
                        m.Name,
                        m.Url,
                        m.IconName,
                        m.Sort,
                        ParentID = m.Parent == null ? 0 : m.Parent.ID,
                        ParentName = m.Parent == null ? String.Empty : m.Parent.Name,
                        m.AddTime
                    })
                }
            });
        }

        [HttpGet]
        [Route("parent_list")]
        public IActionResult Modules()
        {
            var modules = ModuleServices
                .LoadEntities(m => m.Parent == null)
                .Select(m => new
                {
                    m.ID,
                    m.Name
                });
            return Ok(new Result<object>
            {
                State = 1,
                Message = "获取成功",
                Data = modules
            });
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add(ModuleEdit moduleEdit)
        {
            Module module = Mapper.Map<Module>(moduleEdit);
            if (moduleEdit.ParentID.HasValue && moduleEdit.ParentID.Value > 0)
            {
                module.Parent = ModuleServices
                    .LoadFirst(m => m.ID == moduleEdit.ParentID);
            }
            module.AddTime = DateTime.Now;
            module = ModuleServices.AddEntity(module);
            return Ok(new Result<int>
            {
                State = 1,
                Message = "添加成功",
                Data = module.ID
            });
        }

        [HttpPut]
        [Route("edit")]
        public IActionResult Edit(ModuleEdit moduleEdit)
        {
            Module parent = null;
            if (moduleEdit.ParentID.HasValue)
            {
                parent = ModuleServices
                    .LoadFirst(m => m.ID == moduleEdit.ParentID);
            }
            Module module = ModuleServices
                .LoadFirst(u => u.ID == moduleEdit.ID.Value);
            if (module == null)
            {
                return NotFound(new Result
                {
                    State = 0,
                    Message = "修改的用户不存在"
                });
            }
            module = Mapper.Map(moduleEdit, module);
            module.Name = moduleEdit.Name?.Trim();
            module.Url = moduleEdit.Url?.Trim();
            module.IconName = moduleEdit.IconName?.Trim();
            module.Sort = moduleEdit.Sort;
            if (parent != null)
            {
                module.Parent = parent;
            }
            if (ModuleServices.EditEntity(module))
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

        [HttpDelete]
        [Route("delete")]
        public IActionResult Delete(int id)
        {
            Module module = ModuleServices
                .LoadFirst(u => u.ID == id);
            if(module == null)
            {
                return NotFound(new Result
                {
                    State = 0,
                    Message = "用户不存在"
                });
            }
            if (!ModuleServices
                .IsExists(m => m.Parent.ID == module.ID))
            {
                if (ModuleServices.DelectModule(module))
                {
                    return Ok(new Result
                    {
                        State = 1,
                        Message = "删除成功"
                    });
                }
                else
                {
                    return BadRequest(new Result
                    {
                        State = 0,
                        Message = "删除失败"
                    });
                }
            }
            else
            {
                return BadRequest(new Result
                {
                    State = 0,
                    Message = "请先删除子元素后再试"
                });
            }
        }        

        [HttpPost]
        [Route("set_elements")]
        public ActionResult SetModuleElements(int firstID, string secondID)
        {
            string[] tempIDs = secondID.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            int[] elementIDs = Array.ConvertAll(tempIDs, s => Convert.ToInt32(s));

            ReturnDescription description = ModuleServices.SetModuleElements(firstID, elementIDs);
            if (description.Flag)
            {
                return Ok(new Result
                {
                    State = 1,
                    Message = description.Message
                });
            }
            else
            {
                return BadRequest(new Result
                {
                    State = 0,
                    Message = description.Message
                });
            }
        }
    }
}