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
using Evisou.Web;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Web;
using System.Text;

namespace Evisou.Web.AdminApplication.Areas.Account.ApiControllers
{

    //[RoutePrefix("api/account/user")]
    [WebApiPermission(EnumBusinessPermission.AccountManage_User)]
    public class UserController : AdminApiControllerBase
    {
        [HttpGet]
        public HttpResponseMessage Get([FromUri] UserInquiryDTO userInquiryDTO)
        {

            TransactionalInformation transaction = new TransactionalInformation();
            var allUsers = this.AccountService.GetUserList(null);
            IEnumerable<User> filterUsers = allUsers;
            if (!string.IsNullOrEmpty(userInquiryDTO.LoginName))
                filterUsers = filterUsers.Where(c => c.LoginName.Contains(userInquiryDTO.LoginName));
            if (!string.IsNullOrEmpty(userInquiryDTO.Mobile))
                filterUsers = filterUsers.Where(c => c.Mobile == userInquiryDTO.Mobile);
            if(!string.IsNullOrEmpty(userInquiryDTO.Email))
                filterUsers = filterUsers.Where(c => c.Email.Contains(userInquiryDTO.Email));
            if (userInquiryDTO.IsActive!=null)
                filterUsers = filterUsers.Where(c => c.IsActive == userInquiryDTO.IsActive);
            if (userInquiryDTO.RoleIds!=null)
                filterUsers = filterUsers.Where(c => {
                    if (c.Roles.Where(r => userInquiryDTO.RoleIds.Contains(r.ID)).ToList().Count != 0)
                        return true;
                    else
                        return false;
                });
            
            int start = (userInquiryDTO.CurrentPageNumber - 1) * userInquiryDTO.PageSize;
            var sortDirection = userInquiryDTO.SortDirection;
            var sortExpression = userInquiryDTO.SortExpression;



            if (userInquiryDTO.PageSize > 0)
            {
                filterUsers = filterUsers.Skip(start).Take(userInquiryDTO.PageSize);
            }
            
            UserApiModels userWebApi = new UserApiModels();
            List<UserDTO> UserList = new List<UserDTO>();

            foreach (User user in filterUsers)
            {
                UserList.Add(new UserDTO
                {
                    ID = user.ID,
                    LoginName = user.LoginName,
                    Email = user.Email,
                    Mobile = user.Mobile,
                    IsActive = user.IsActive,
                    Roles = StringUtil.CutString(string.Join(",", user.Roles.Select(r => r.Name)), 40),
                    RoleIds = user.Roles.Select(r => r.ID).ToList()
                });
            };


            // Func<UserDTO, string> orderingFunction = c => "LoginName";
            Func<UserDTO, string> orderingFunction = (
                c => sortExpression.Contains("LoginName") ? c.LoginName :
                sortExpression.Contains("Email") ? c.Email :
                sortExpression.Contains("Mobile") ? c.Email :
                sortExpression.Contains("IsActive") ? c.IsActive.ToString() :
                ""

            );
            //  Func<UserDTO, string> orderingFunction = Func<UserDTO,sortExpression>;
            IEnumerable<UserDTO> Result= new List<UserDTO>();
            
            switch (sortDirection)
            {
                case "ASC":
                    
                     Result = UserList.OrderBy(orderingFunction);
                    break;

                case "DESC":
                     Result = UserList.OrderByDescending(orderingFunction);
                    break;

                default:
                    Result = UserList;
                    break;

            }
            UserDTO UserDto = new UserDTO();
            List<RoleDTO> RoleList = new List<RoleDTO>();
            this.AccountService.GetRoleList(null).ToList().ForEach(c => RoleList.Add(new RoleDTO { ID = c.ID, Name = c.Name, Info = c.Info }));
            UserDto.RoleList = RoleList;
            userWebApi.User = UserDto;

            userWebApi.Users = Result;
            userWebApi.TotalRecords = allUsers.Count();
            userWebApi.ReturnMessage = transaction.ReturnMessage;
            userWebApi.ReturnStatus = transaction.ReturnStatus;

            return Request.CreateResponse(HttpStatusCode.OK, userWebApi);
        }

        [HttpGet]
        public HttpResponseMessage GetUser([FromUri] int UserID)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            var model = this.AccountService.GetUser(UserID);

            UserApiModels userWebApi = new UserApiModels();
            userWebApi.User = new UserDTO
            {
                LoginName = model.LoginName,
                Email = model.Email,
                Mobile = model.Mobile,
                IsActive = model.IsActive,
                Roles = StringUtil.CutString(string.Join(",", model.Roles.Select(r => r.Name)), 40),
                RoleIds = model.Roles.Select(r => r.ID).ToList()
            };

            List<RoleDTO> RoleList = new List<RoleDTO>();
            this.AccountService.GetRoleList(null).ToList().ForEach(c => RoleList.Add(new RoleDTO { ID = c.ID, Name = c.Name, Info = c.Info }));
            userWebApi.User.RoleList = RoleList;

            userWebApi.ReturnMessage = transaction.ReturnMessage;
            userWebApi.ReturnStatus = transaction.ReturnStatus;

            // this.ViewBag.RoleIds = new SelectList(roles, "ID", "Name", string.Join(",", model.Roles.Select(r => r.ID)));
            return Request.CreateResponse(HttpStatusCode.OK, userWebApi);
        }

        [HttpGet]
        public HttpResponseMessage ExportUser([FromUri]  List<int> UserIDs, [FromUri] string DataType)
        {
           
            HttpResponseMessage response = new HttpResponseMessage();
            response=this.AccountService.UserDataExport(UserIDs, DataType);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody] UserDTO userDTO)
        {
            var model = new User();
            model.Password = "111111";
            model.Password = Encrypt.MD5(model.Password);
            model.LoginName = userDTO.LoginName;
            model.Email = userDTO.Email;
            model.Mobile = userDTO.Mobile;
            model.IsActive = userDTO.IsActive;
            model.RoleIds = userDTO.IDs;
            UserApiModels userWebApi = new UserApiModels();
            TransactionalInformation transaction = new TransactionalInformation();
            try
            {
                this.AccountService.SaveUser(model);
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
                userWebApi.ReturnMessage = transaction.ReturnMessage;
                userWebApi.ReturnStatus = transaction.ReturnStatus;
                userWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse<UserApiModels>(HttpStatusCode.BadRequest, userWebApi);
                return badResponse;
            }

            userWebApi.IsAuthenicated = true;
            userWebApi.ReturnStatus = transaction.ReturnStatus;
            userWebApi.ReturnMessage.Add("注册成功");



            return Request.CreateResponse(HttpStatusCode.OK, userWebApi);
        }

        [HttpPut]
        public HttpResponseMessage Put([FromBody] UserDTO userDTO)
        {
            var model = this.AccountService.GetUser(userDTO.ID);
            model.Password = userDTO.Password;
            model.Password = Encrypt.MD5(model.Password);
            model.Email = userDTO.Email;
            model.Mobile = userDTO.Mobile;
            model.IsActive = userDTO.IsActive;
            model.RoleIds = userDTO.IDs;
            UserApiModels userWebApi = new UserApiModels();
            TransactionalInformation transaction = new TransactionalInformation();
            try
            {
                this.AccountService.SaveUser(model);
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
                userWebApi.ReturnMessage = transaction.ReturnMessage;
                userWebApi.ReturnStatus = transaction.ReturnStatus;
                userWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse<UserApiModels>(HttpStatusCode.BadRequest, userWebApi);
                return badResponse;
            }

            userWebApi.IsAuthenicated = true;
            userWebApi.ReturnStatus = transaction.ReturnStatus;
            userWebApi.ReturnMessage.Add("修改成功");



            return Request.CreateResponse(HttpStatusCode.OK, userWebApi);
           
        }

        [HttpPatch]
        public HttpResponseMessage Patch([FromBody] List<UserDTO> Users)
        {
            
            UserApiModels userWebApi = new UserApiModels();
            TransactionalInformation transaction = new TransactionalInformation();
            try
            {
                Users.ForEach(c =>
                {
                    var model = this.AccountService.GetUser(c.ID);
                    model.LoginName = c.LoginName;
                    model.Email = c.Email;
                    model.Mobile = c.Mobile;
                    model.IsActive = c.IsActive;
                    model.RoleIds = c.RoleIds; //model.Roles.Select(r=>r.ID).ToList();
                    this.AccountService.SaveUser(model);
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
                userWebApi.ReturnMessage = transaction.ReturnMessage;
                userWebApi.ReturnStatus = transaction.ReturnStatus;
                userWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse<UserApiModels>(HttpStatusCode.BadRequest, userWebApi);
                return badResponse;
            }

            userWebApi.IsAuthenicated = true;
            userWebApi.ReturnStatus = transaction.ReturnStatus;
            userWebApi.ReturnMessage.Add("修改成功");



            return Request.CreateResponse(HttpStatusCode.OK, userWebApi);
        }

        [HttpDelete]
        public HttpResponseMessage Delete([FromUri] List<int> UserID)
        {
            UserApiModels userWebApi = new UserApiModels();
            TransactionalInformation transaction = new TransactionalInformation();
            try
            {
                this.AccountService.DeleteUser(UserID);
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
                userWebApi.ReturnMessage = transaction.ReturnMessage;
                userWebApi.ReturnStatus = transaction.ReturnStatus;
                userWebApi.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse<UserApiModels>(HttpStatusCode.BadRequest, userWebApi);
                return badResponse;
            }

            userWebApi.IsAuthenicated = true;
            userWebApi.ReturnStatus = transaction.ReturnStatus;
            userWebApi.ReturnMessage.Add("删除成功");
            return Request.CreateResponse(HttpStatusCode.OK, userWebApi);
        }
    }
}
