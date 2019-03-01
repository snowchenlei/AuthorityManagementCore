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

namespace Cl.AuthorityManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ApiBaseController
    {
        private readonly IRoleServices RoleServices = null;
        private readonly IModuleServices ModuleServices = null;
        private readonly IRoleModuleElementServices RoleModuleElementServices = null;
        public RoleController(
            IRoleServices roleServices,
            IModuleServices moduleServices,
            IRoleModuleElementServices roleModuleElementServices)
        {
            RoleServices = roleServices;
            ModuleServices = moduleServices;
            RoleModuleElementServices = roleModuleElementServices;
        }

        [HttpGet]
        [Route("list")]
        public IActionResult Load(int pageIndex, int pageSize, string sort,
            OrderType order, string roleName, DateTime startTime, DateTime endTime)
        {
            PageHelper.GetPageIndex(ref pageIndex);
            PageHelper.GetPageSize(ref pageSize);
            int totalCount;
            var tempRoles = RoleServices.LoadEntities(r => true);
            #region 查询
            if (!String.IsNullOrEmpty(roleName))
            {
                tempRoles = tempRoles.Where(r => r.Name.Contains(roleName.Trim()));
            }
            if (startTime > new DateTime(1970, 1, 1) && startTime != endTime)
            {
                tempRoles = tempRoles.Where(r => r.AddTime > startTime);
            }
            if (endTime > startTime)
            {
                tempRoles = tempRoles.Where(r => r.AddTime < endTime);
            }
            #endregion

            #region 排序
            if ("AddTime".Equals(sort, StringComparison.InvariantCultureIgnoreCase))
            {
                tempRoles = Sort(tempRoles, r => r.AddTime, order).ThenBy(r => r.ID);
            }
            else if ("Sort".Equals(sort, StringComparison.InvariantCultureIgnoreCase))
            {
                tempRoles = Sort(tempRoles, r => r.Sort, order).ThenBy(r => r.ID);
            }
            else
            {
                tempRoles = Sort(tempRoles, r => r.ID, order);
            }
            #endregion

            //var roles = RoleServices
            //    .LoadPageEntities(pageIndex, pageSize, role);
            var roles = RoleServices.LoadPageEntities(pageIndex, pageSize, tempRoles);
            totalCount = tempRoles.Count();

            int pageCount = PageHelper.GetPageCount(totalCount, pageSize);
            return Ok(new
            {
                total = totalCount,
                rows = roles.Select(u => new
                {
                    Id = u.ID,
                    u.Sort,
                    u.Name,
                    u.AddTime
                })
            });
        }

        [HttpPost]
        [Route("add")]
        public ActionResult Add(RoleEdit roleEdit)
        {
                Role role = Mapper.Map<Role>(roleEdit);
                //role.IsDelete = 0;
                RoleServices.AddEntity(role);
                return Ok(new Result<int>
                {
                    State = 1,
                    Message = "添加成功",
                    Data = role.ID
                });
        }

        [HttpPut]
        [Route("edit")]
        public ActionResult Edit(RoleEdit roleEdit)
        {
                Role role = RoleServices
                    .LoadFirst(r => r.ID == roleEdit.ID.Value);
                if (role == null)
                {
                    return NotFound(new Result
                    {
                        State = 0,
                        Message = "修改的角色不存在"
                    });
                }
                role = Mapper.Map(roleEdit, role);
                if (RoleServices.EditEntity(role))
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
                        State = 1,
                        Message = "修改失败"
                    });
                }
        }

        [HttpDelete]
        [Route("delete")]
        public ActionResult Delete(int id)
        {
            if (RoleServices.Delete(id))
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

        [HttpPost]
        [Route("set_modules")]
        public ActionResult SetModules(int firstID, string secondID)
        {
            string[] tempIDs = secondID.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            int[] moduleIDs = Array.ConvertAll(tempIDs, s => Convert.ToInt32(s));

            ReturnDescription description = RoleServices
                 .SetRoleModule(firstID, moduleIDs);
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

        [HttpPost]
        [Route("set_elements")]
        public ActionResult SetElements(int roleID, string elementID, int moduleID)
        {
            string[] tempIDs = elementID.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            int[] elementIDs = Array.ConvertAll(tempIDs, s => Convert.ToInt32(s));

            ReturnDescription description = RoleServices
                 .SetRoleModuleElements(roleID, elementIDs, moduleID);
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