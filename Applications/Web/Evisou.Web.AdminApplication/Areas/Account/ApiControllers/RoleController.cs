using Evisou.Account.Contract;
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
    public class RoleController : AdminApiControllerBase
    {
        public HttpResponseMessage Get()
        {
            TransactionalInformation transaction = new TransactionalInformation();
            var roles = this.AccountService.GetRoleList();
            RoleApiModels RoleWebApi = new RoleApiModels();
            List<RoleDTO> RoleList = new List<RoleDTO>();
            foreach (Role role in roles)
            {
                RoleList.Add(new RoleDTO
                {
                    ID = role.ID,
                    Name = role.Name
                });
            };
            RoleWebApi.ReturnMessage = transaction.ReturnMessage;
            RoleWebApi.ReturnStatus = transaction.ReturnStatus;
            RoleWebApi.Roles = RoleList;
            return Request.CreateResponse(HttpStatusCode.OK, RoleWebApi);
        }
    }
}
