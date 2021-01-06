using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUD_App.Entity.Entity;
using CRUD_App.General.Datatable;
using CRUD_App.General.Resources.Services;
using CRUD_App.General.Response;
using CRUD_App.Services.Interfaces;
using CRUD_App.Web.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;


namespace CRUD_App.API
{
    
    [Route("api/Admin/[controller]")]
    [ApiController]
    [Authorize(Policy = Policies.Admin)]
    public class UserDetailController : ControllerBase
    {
        #region Properties
        private readonly IUserMasterService _UserService;
        private readonly IConfiguration _configuration;
        private readonly IObjectResponseHandler<UserMasterEntity> _ObjectResponse;
        private readonly IDataTableResponseHandler<UserMasterEntity> _DatatableResponse;
        private readonly IListResponseHandler<UserMasterEntity> _ListResponse;
        private readonly IResourceService _messageService;
        #endregion

        #region Constructor
        public UserDetailController(IUserMasterService UserService, IConfiguration configuration, IListResponseHandler<UserMasterEntity> ListResponse,
            IObjectResponseHandler<UserMasterEntity> ObjectResponse, IDataTableResponseHandler<UserMasterEntity> DatatableResponse, IResourceService messageService)
        {
            _UserService = UserService;
            _configuration = configuration;
            _ObjectResponse = ObjectResponse;
            _ListResponse = ListResponse;
            _DatatableResponse = DatatableResponse;
            _messageService = messageService;
        }
        #endregion

        #region Method

        /// <summary>
        /// Description -  To get all UserDetail with dynamic search and without search.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="405">Method Not Allowed</response>
        /// <response code="415">Unsupported Media Type</response>
        /// <response code="409">Conflict (Already Exist)</response>
        /// <response code="500">Internal Server</response>
        [HttpPost]
        [Route("GetAllUser")]
        public async Task<IActionResult> GetAllUser(DataTableAjaxPostModel model)
        {
            var token = (string)this.Request.Headers["Authorization"];
            token = token.Replace("Bearer", "").Trim();
            var loggedInUser = UserHelper.GetLoggedInUser(token);
            string UserId = loggedInUser.UserId.ToString();

            var searchBy = "";
            if (model != null)
                searchBy = (model.search != null) ? model.search.value : null;

            IEnumerable<UserMasterEntity> AllUser = null;

            var response = _DatatableResponse;

            var recordsTotal = 0;
            var recordsFiltered = 0;
            if (model != null)
            {
                dynamic result = await _UserService.GetPageSoretedFilteredListFromSP(UserId, model);
                AllUser = result.AllUser;
                recordsTotal = result.TotalRecord;
                recordsFiltered = result.TotalCount;
                response = _DatatableResponse.Create(model.draw, recordsTotal, recordsFiltered, AllUser, _messageService.GetString("GetAllList", new Parameters { Name = "@count", Value = recordsTotal.ToString() }));

            }
            else
            {
                response = null;
                AllUser = await _UserService.GetAllUser();
            }

            if (response != null)
                return response.ToHttpResponse();
            else
                return Ok(AllUser);

        }

        /// <summary>
        /// Description - To get User Capacity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ///   /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="405">Method Not Allowed</response>
        /// <response code="415">Unsupported Media Type</response>
        /// <response code="409">Conflict (Already Exist)</response>
        /// <response code="500">Internal Server</response>
        [HttpGet, Route("GetUserDetailById/{id}")]
        public async Task<IActionResult> GetUserDetailById([FromRoute] int id)
        {
            var response = _ObjectResponse;
            var OUser = new UserMasterEntity();

            OUser = await _UserService.GetUserById(id);
            if (OUser != null)
            {
                response = _ObjectResponse.Create(OUser, _messageService.GetString("GetById"));
                return response.ToHttpResponse();
            }
            else
            {
                response = _ObjectResponse.Create(OUser, _messageService.GetString("GetById"));
                return response.ToHttpResponse(404);

            }

        }

        /// <summary>
        /// Description - To Create/Update User Capacity - If id is zero then Create else Update
        /// </summary>
        /// <param name="oUser"></param>
        /// <returns></returns>
        ///   /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="405">Method Not Allowed</response>
        /// <response code="415">Unsupported Media Type</response>
        /// <response code="409">Conflict (Already Exist)</response>
        /// <response code="500">Internal Server</response>
        [HttpPost]
        [Route("SaveUpdateUser")]
        public async Task<IActionResult> SaveUpdateUser([FromBody] UserMasterEntity oUser)
        {
            var response = _ObjectResponse;
            var _oUser = new UserMasterEntity();

            var token = (string)this.Request.Headers["Authorization"];
            token = token.Replace("Bearer", "").Trim();
            var loggedInUser = UserHelper.GetLoggedInUser(token);


            // Start : Added by Prasad for avoid duplication
            var IsExist = _UserService.IsUserExist(oUser);
            if (IsExist)
            {
                response = _ObjectResponse.Create(oUser, _messageService.GetString("Exist"));
                return response.ToHttpResponseConflict();
            }
            // End : Added  by Prasad for avoid duplication
            if (oUser.UserId == 0)
            {
                // oUser.CreatedBy = loggedInUser.UserId;
                _oUser = await _UserService.AddUser(oUser);
                response = _ObjectResponse.Create(_oUser, _messageService.GetString("Save"));
            }
            else
            {
                //oUser.UpdatedBy = loggedInUser.UserId;
                _oUser = await _UserService.UpdateUser(oUser);
                response = _ObjectResponse.Create(_oUser, _messageService.GetString("Update"));
            }


            //if (response != null)
            //    return response.ToHttpResponse();
            //else if (_oUser != null)
            //    return Ok(_oUser);
            //else
            //    return BadRequest();
            return response.ToHttpResponse();
        }

        /// <summary>
        /// Description - To delete User Capacity by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ///   /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="405">Method Not Allowed</response>
        /// <response code="415">Unsupported Media Type</response>
        /// <response code="409">Conflict (Already Exist)</response>
        /// <response code="500">Internal Server</response>
        [HttpDelete, Route("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var response = _ObjectResponse;
            UserMasterEntity entityUser = new UserMasterEntity();
            var token = (string)this.Request.Headers["Authorization"];
            token = token.Replace("Bearer", "").Trim();
            var loggedInUser = UserHelper.GetLoggedInUser(token);
            entityUser.IsDeleted = true;
            entityUser.UserId = id;
            entityUser.DeletedBy = loggedInUser.UserId;
            var _oUser = await _UserService.UpdateUser(entityUser);

            if (_oUser != null)
            {
                response = _ObjectResponse.Create(_oUser, _messageService.GetString("Delete"));
                //response.Success = true;
                //response.NotificationType = "Success";
                return response.ToHttpResponse();
            }
            else
            {
                response = _ObjectResponse.Create(_oUser, _messageService.GetString("DeleteFailed"));
                return response.ToHttpResponse(404);
                // return NotFound();
            }

        }
        #endregion
    }
}