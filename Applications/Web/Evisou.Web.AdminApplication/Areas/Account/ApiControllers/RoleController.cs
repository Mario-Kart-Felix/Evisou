using Evisou.Account.Contract;
using Evisou.Framework.Utility;
using Evisou.Web.AdminApplication.Areas.Account.WebApiModels;
using Evisou.Web.AdminApplication.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Evisou.Web.AdminApplication.Areas.Account.ApiControllers
{
    [WebApiPermission(EnumBusinessPermission.AccountManage_Role)]
    public class RoleController : AdminApiControllerBase
    {
        [HttpGet]
        public HttpResponseMessage Get([FromUri] RoleInquiryDTO roleInquiryDTO)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            RoleApiModels roleWebApi = new RoleApiModels();
            try
            {
                var businessPermissionList = EnumHelper.GetItemValueList<EnumBusinessPermission>();
                List<BusinessPermission> BusinessPermissions = new List<BusinessPermission>();
                businessPermissionList.ToList().ForEach(c =>
                {
                    BusinessPermissions.Add(new BusinessPermission
                    {
                        Name = c.Value.ToString(),
                        Value = c.Key
                    });
                });


                var roles = this.AccountService.GetRoleList(null);
                IEnumerable<Role> filterRoles = roles;
                if (!string.IsNullOrEmpty(roleInquiryDTO.Name))
                    filterRoles = filterRoles.Where(c => c.Name.Contains(roleInquiryDTO.Name));

                List<RoleDTO> RoleList = new List<RoleDTO>();

                int start = (roleInquiryDTO.CurrentPageNumber - 1) * roleInquiryDTO.PageSize;
                var sortDirection = roleInquiryDTO.SortDirection;
                var sortExpression = roleInquiryDTO.SortExpression;
                if (roleInquiryDTO.PageSize > 0)
                    filterRoles = filterRoles.Skip(start).Take(roleInquiryDTO.PageSize);

                filterRoles.ToList().ForEach(c =>
                {
                    RoleList.Add(new RoleDTO
                    {
                        ID = c.ID,
                        Name = c.Name,
                        Info = c.Info,
                        BusinessPermissionString= StringUtil.CutString(string.Join(",", c.BusinessPermissionList.Select(r => Evisou.Framework.Utility.EnumHelper.GetEnumTitle(r))), 40)
                    });
                });

                Func<RoleDTO, string> orderingFunction = (
                    c => sortExpression.Contains("Name") ? c.Name :
                    sortExpression.Contains("Info") ? c.Info :
                    ""
                );
                IEnumerable<RoleDTO> Result = new List<RoleDTO>();
                switch (sortDirection)
                {
                    case "ASC":

                        Result = RoleList.OrderBy(orderingFunction);
                        break;

                    case "DESC":
                        Result = RoleList.OrderByDescending(orderingFunction);
                        break;

                    default:
                        Result = RoleList;
                        break;

                }
                roleWebApi.TotalRecords = roles.Count();
                roleWebApi.Roles = Result;
                roleWebApi.Role.BusinessPermissions = BusinessPermissions;
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            if (transaction.ReturnStatus == false)
            {
                roleWebApi.ReturnMessage = transaction.ReturnMessage;
                roleWebApi.ReturnStatus = transaction.ReturnStatus;
                roleWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, roleWebApi);
                return badResponse;
            }


            roleWebApi.ReturnMessage = transaction.ReturnMessage;
            roleWebApi.ReturnStatus = transaction.ReturnStatus;
            return Request.CreateResponse(HttpStatusCode.OK, roleWebApi);
        }

        [HttpGet]
        public HttpResponseMessage GetRole([FromUri] int RoleID)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            RoleApiModels roleWebApi = new RoleApiModels();
            try
            {
                var model = this.AccountService.GetRole(RoleID);
                roleWebApi.Role = new RoleDTO
                {
                    Name = model.Name,
                    Info = model.Info,
                    BusinessPermissionValues = model.BusinessPermissionList.Select(p => (int)p).ToList()
                };
                List<BusinessPermission> BusinessPermissions = new List<BusinessPermission>();
                EnumHelper.GetItemValueList<EnumBusinessPermission>().ToList().ForEach(c =>
                {
                    BusinessPermissions.Add(new BusinessPermission
                    {
                        Name = c.Value.ToString(),
                        Value = c.Key
                    });
                });
                roleWebApi.Role.BusinessPermissions = BusinessPermissions;
                transaction.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            if (transaction.ReturnStatus == false)
            {
                roleWebApi.ReturnMessage = transaction.ReturnMessage;
                roleWebApi.ReturnStatus = transaction.ReturnStatus;
                roleWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, roleWebApi);
                return badResponse;
            }
            
            roleWebApi.ReturnMessage = transaction.ReturnMessage;
            roleWebApi.ReturnStatus = transaction.ReturnStatus;
            return Request.CreateResponse(HttpStatusCode.OK, roleWebApi);
        }

        [HttpGet]
        public HttpResponseMessage ExportRole([FromUri]  List<int> RoleIDs, [FromUri] string DataType)
        {

            HttpResponseMessage response = new HttpResponseMessage();
            response = this.AccountService.RoleDataExport(RoleIDs, DataType);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody] RoleDTO roleDTO)
        {
           
            Role model = new Role();
            model.Name = roleDTO.Name;
            model.Info = roleDTO.Info;
            model.BusinessPermissionString = string.Join(",", roleDTO.BusinessPermissionValues.ToArray());
            RoleApiModels roleWebApi = new RoleApiModels();
            TransactionalInformation transaction = new TransactionalInformation();
            try
            {
                this.AccountService.SaveRole(model);
                transaction.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            if (transaction.ReturnStatus == false)
            {
                roleWebApi.ReturnMessage = transaction.ReturnMessage;
                roleWebApi.ReturnStatus = transaction.ReturnStatus;
                roleWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, roleWebApi);
                return badResponse;
            }

            roleWebApi.IsAuthenicated = true;
            roleWebApi.ReturnStatus = transaction.ReturnStatus;
            roleWebApi.ReturnMessage.Add("添加成功");
            return Request.CreateResponse(HttpStatusCode.OK, roleWebApi);
        }

        [HttpPut]
        public HttpResponseMessage Put([FromBody] RoleDTO roleDTO)
        {
            var model = this.AccountService.GetRole(roleDTO.ID);
            model.Name = roleDTO.Name;
            model.Info = roleDTO.Info;
            model.BusinessPermissionString = string.Join(",", roleDTO.BusinessPermissionValues.ToArray());
            RoleApiModels roleWebApi = new RoleApiModels();
            TransactionalInformation transaction = new TransactionalInformation();
            try
            {
                this.AccountService.SaveRole(model);
                transaction.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }

            if (transaction.ReturnStatus == false)
            {
                roleWebApi.ReturnMessage = transaction.ReturnMessage;
                roleWebApi.ReturnStatus = transaction.ReturnStatus;
                roleWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, roleWebApi);
                return badResponse;
            }

            roleWebApi.IsAuthenicated = true;
            roleWebApi.ReturnStatus = transaction.ReturnStatus;
            roleWebApi.ReturnMessage.Add("修改成功");



            return Request.CreateResponse(HttpStatusCode.OK, roleWebApi);

        }

        [HttpPatch]
        public HttpResponseMessage Patch([FromBody] List<RoleDTO> Roles)
        {

            RoleApiModels roleWebApi = new RoleApiModels();
            TransactionalInformation transaction = new TransactionalInformation();
            try
            {
                Roles.ForEach(c =>
                {
                    var model = this.AccountService.GetRole(c.ID);
                    model.Name = c.Name;
                    model.Info = c.Info;
                    model.BusinessPermissionString = string.Join(",", c.BusinessPermissionValues.ToArray());
                    this.AccountService.SaveRole(model);
                });

                transaction.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }

            if (transaction.ReturnStatus == false)
            {
                roleWebApi.ReturnMessage = transaction.ReturnMessage;
                roleWebApi.ReturnStatus = transaction.ReturnStatus;
                roleWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, roleWebApi);
                return badResponse;
            }

            roleWebApi.IsAuthenicated = true;
            roleWebApi.ReturnStatus = transaction.ReturnStatus;
            roleWebApi.ReturnMessage.Add("修改成功");



            return Request.CreateResponse(HttpStatusCode.OK, roleWebApi);
        }
        
        [HttpDelete]
        public HttpResponseMessage Delete([FromUri] List<int> RoleID)
        {
           
            RoleApiModels roleWebApi = new RoleApiModels();
            TransactionalInformation transaction = new TransactionalInformation();
            try
            {
                this.AccountService.DeleteRole(RoleID);
                transaction.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(ex.Message);
            }

            if (transaction.ReturnStatus == false)
            {
                roleWebApi.ReturnMessage = transaction.ReturnMessage;
                roleWebApi.ReturnStatus = transaction.ReturnStatus;
                roleWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse(HttpStatusCode.BadRequest, roleWebApi);
                return badResponse;
            }

            roleWebApi.IsAuthenicated = true;
            roleWebApi.ReturnStatus = transaction.ReturnStatus;
            roleWebApi.ReturnMessage.Add("删除成功");
            return Request.CreateResponse(HttpStatusCode.OK, roleWebApi);
        }

        
    }
}
