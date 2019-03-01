using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cl.AuthorityManagement.Common;
using Cl.AuthorityManagement.Common.Encryption;
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
    public class UserController : ApiBaseController
    {
        private readonly IMapper Mapper = null;
        private readonly IRoleServices RoleServices = null;
        private readonly IModuleServices ModuleServices = null;
        private readonly IUserInfoServices UserInfoServices = null;
        private readonly IModuleElementServices ModuleElementServices = null;
        private readonly IUserInfoModuleElementServices UserInfoModuleElementServices = null;
        public UserController(
            IMapper mapper,
            IRoleServices roleServices,
            IModuleServices moduleServices,
            IUserInfoServices userInfoServices,
            IModuleElementServices moduleElementServices,
            IUserInfoModuleElementServices userInfoModuleElementServices)
        {
            Mapper = mapper;
            RoleServices = roleServices;
            ModuleServices = moduleServices;
            UserInfoServices = userInfoServices;
            ModuleElementServices = moduleElementServices;
            UserInfoModuleElementServices = userInfoModuleElementServices;
        }

        [HttpGet]
        [Route("list")]
        public IActionResult Load(int pageIndex, int pageSize, string sort,
            OrderType order, string phoneNumber, string userName, DateTime startTime, DateTime endTime)
        {
            PageHelper.GetPageIndex(ref pageIndex);
            PageHelper.GetPageSize(ref pageSize);

            var tempUsers = UserInfoServices.LoadEntities(u => true);
            #region 查询
            if (!String.IsNullOrEmpty(phoneNumber))
            {
                tempUsers = tempUsers.Where(u => u.PhoneNumber == phoneNumber.Trim());
            }
            if (!String.IsNullOrEmpty(userName))
            {
                tempUsers = tempUsers.Where(u => u.UserName == userName.Trim());
            }
            if (startTime > new DateTime(1970, 1, 1) && startTime != endTime)
            {
                tempUsers = tempUsers.Where(u => u.AddTime > startTime);
            }
            if (endTime > startTime)
            {
                tempUsers = tempUsers.Where(u => u.AddTime < endTime);
            }
            #endregion

            #region 排序
            if ("AddTime".Equals(sort, StringComparison.InvariantCultureIgnoreCase))
            {
                tempUsers = Sort(tempUsers, u => u.AddTime, order).ThenBy(u => u.ID);
            }
            else
            {
                tempUsers = Sort(tempUsers, u => u.ID, order);
            }
            #endregion
            int totalCount = tempUsers.Count();
            var users = UserInfoServices
                .LoadPageEntities(pageIndex, pageSize, tempUsers);

            int pageCount = PageHelper.GetPageCount(totalCount, pageSize);
            return Ok(new
            {
                total = totalCount,
                rows = users.Select(u => new
                {
                    Id = u.ID,
                    u.UserName,
                    u.Name,
                    u.PhoneNumber,
                    u.Password,
                    u.AddTime,
                    u.IsCanUse
                })
            });
        }

        [HttpPost]
        [Route("add")]
        public ActionResult Add(UserEdit userEdit)
        {
            UserInfo user = Mapper.Map<UserInfo>(userEdit);
            user.Password = Md5Encryption.Encrypt(Md5Encryption.Encrypt(user.Password, Md5EncryptionType.Strong));
            user = UserInfoServices.AddEntity(user);

            //LoggerHelper.Operate(new OperateLog
            //{
            //    CreateUser_Id = UserInfo.ID,
            //    OperateType = (int)OperateType.Add,
            //    Remark = $"{UserInfo.Name}添加了一个用户{userEdit.Name}"
            //});
            return Ok(new Result<int>
            {
                State = 1,
                Message = "添加成功",
                Data = user.ID
            });
        }

        [HttpPost]
        [Route("edit")]
        public ActionResult Edit(UserEdit userEdit)
        {
            UserInfo user = UserInfoServices
                    .LoadFirst(u => u.ID == userEdit.ID.Value);
            if (user == null)
            {
                return NotFound(new Result
                {
                    State = 0,
                    Message = "修改的用户不存在"
                });
            }
            user = Mapper.Map(userEdit, user);
            if (UserInfoServices.EditEntity(user))
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

        [HttpPost]
        [Route("delete")]
        public ActionResult Delete(int id)
        {
            if (UserInfoServices.Delete(id))
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
        [Route("set_roles")]
        public ActionResult SetRoles(int firstID, string secondID)
        {
            string[] tempIds = secondID.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            int[] roleIds = Array.ConvertAll(tempIds, s => Convert.ToInt32(s));

            ReturnDescription description = UserInfoServices
                .SetUserRole(firstID, roleIds);

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
        [Route("set_modules")]
        public ActionResult SetModules(int firstID, string secondID)
        {
            string[] tempIds = secondID.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            int[] moduleIDs = Array.ConvertAll(tempIds, s => Convert.ToInt32(s));

            ReturnDescription description = UserInfoServices
                 .SetUserModule(firstID, moduleIDs);
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
        public ActionResult SetElements(int userID, string elementID, int moduleID)
        {
            string[] tempIds = elementID.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            int[] elementIds = Array.ConvertAll(tempIds, s => Convert.ToInt32(s));

            ReturnDescription description = UserInfoServices.SetUserModuleElements(userID, elementIds, moduleID);
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