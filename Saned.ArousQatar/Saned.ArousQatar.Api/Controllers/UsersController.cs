using AutoMapper;
using Saned.ArousQatar.Api.Infrastructure.Core;
using Saned.ArousQatar.Api.Models;
using Saned.ArousQatar.Data.Core;
using Saned.ArousQatar.Data.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Saned.ArousQatar.Data.Core.Dtos;
using Saned.ArousQatar.Data.Persistence.Repositories;
using Saned.ArousQatar.Data.Persistence;

namespace Saned.ArousQatar.Api.Controllers
{
    [RoutePrefix("api/Users")]
    public class UsersController : ApiControllerBase
    {
        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
        private readonly AuthRepository _repo = null;
        private new readonly IUnitOfWork _unitOfWork;
        public UsersController()
        {
            _repo = new AuthRepository();
            _unitOfWork = new UnitOfWork();
        }
        public UsersController(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {

        }


        [HttpPost]
        [Authorize]
        [Route("saveimage")]
        public async Task<IHttpActionResult> SaveImage(UserImageViewModel model)
        {
            string userName = User.Identity.GetUserName();
            ApplicationUser u = await GetApplicationUser(userName);
            if (u == null)
            {
                return BadRequest();
            }
            else
            {
                if (!string.IsNullOrEmpty(model.ImageFilename) && !string.IsNullOrEmpty(model.ImageBase64))
                {
                    var randomImage = SaveImage(model.ImageFilename, model.ImageBase64);

                    u.PhotoUrl = randomImage;
                    await _unitOfWork.User.ChangeImage(u.Id, randomImage);

                    return Ok(randomImage);
                }
                else
                {
                    return BadRequest();
                }
            }
        }


        [Route("GetAllUser")]
        [HttpPost]
        // [Attributes.Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> GetUsers([FromBody] PagingViewModel viewModel)
        {
            IHttpActionResult response;
            try
            {
                int currentPage = viewModel.PageNumber;
                int currentPageSize = viewModel.PageSize;

                var alluser = await _unitOfWork.User.GetWithFilterAsync(
                viewModel.PageNumber,
                viewModel.PageSize,
                viewModel.Filter,
                 role: "User"
                );

                if (alluser.Count == 0)
                    return Ok(new PaginationSet<UserData>());
                int totalCount = alluser[0].OverAllCount;



                PaginationSet<UserData> pagedSet = new PaginationSet<UserData>()
                {
                    Items = alluser,
                    Page = currentPage,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling((decimal)totalCount / currentPageSize)
                };

                response = Ok(pagedSet);
            }
            catch (DbUpdateConcurrencyException ex)
            {
              LogError( ex );
                response = NotFound();
            }
            catch (DbUpdateException ex)
            {
              LogError( ex );
                response = BadRequest(ex.InnerException?.Message);
            }
            catch (Exception ex)
            {
               LogError( ex );
                response = InternalServerError(ex);
            }

            return response;

        }
        
        [Route("GetUsers")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllPaging()
        {
            IHttpActionResult response = null;
            try
            {

                var users = await _unitOfWork.User.GetAllAsync();


                response = Ok(users);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                LogError(ex);
                response = NotFound();
            }
            catch (DbUpdateException ex)
            {
                LogError(ex);
                response = BadRequest(ex.InnerException.Message);
            }

            catch (Exception ex)
            {
                LogError(ex);
                response = InternalServerError(ex);
            }

            return response;
        }

        public async Task<IHttpActionResult> Get()
        {
            return Ok(await _unitOfWork.User.GetAllAsync());
        }

        public async Task<IHttpActionResult> GetUserById(string id)
        {
            return Ok(await _unitOfWork.User.GetSingleAsync(id));
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> DeleteUserById(string id)
        {
            try
            {
                int result = await _unitOfWork.User.DeleteUser(id.ToString());
                if (result==1)
                    return Ok();
                else
                    return Ok(new { error = "لا يمكن حذف مستخدم مرتبط باعلانات " });


            }
            catch (Exception ex)
            {
                LogError(ex);
                return Ok(new { error = "لا يمكن حذف مستخدم مرتبط باعلانات"});
            }
        }

        //[Route("update")]
        //[Attributes.Authorize(Roles = "Administrator")]
        //public async Task<IHttpActionResult> EditShelterAdmin(UserViewModel userViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {


        //        //CheckUser
        //        var userbyid = await _repo.FindUserById(userViewModel.Id);
        //        if (userbyid != null)
        //        {
        //            if (userbyid.Email.Trim().ToLower() != userViewModel.Email.Trim().ToLower())
        //            {
        //                var userbyemail = await _repo.FindUser(userViewModel.Email);
        //                if (userbyemail != null)
        //                {
        //                    ModelState.AddModelError("Email", "Email Must Be Unique");
        //                    return BadRequest(ModelState);
        //                }
        //            }

        //            userbyid.CityId = userViewModel.CityId;
        //            userbyid.IsApprove = true;
        //            userbyid.UserName = userViewModel.UserName;
        //            userbyid.PhoneNumber = userViewModel.PhoneNumber;
        //            userbyid.Email = userViewModel.Email;
        //            userbyid.Name = userViewModel.Name;

        //            IdentityResult result = await _repo.EditShelterAdmin(userbyid,userViewModel.Password);

        //            IHttpActionResult errorResult = GetErrorResult(result);
        //            if (errorResult != null)
        //                return errorResult;


        //            return Ok(userbyid.Id);

        //        }
        //        else
        //        {
        //            ModelState.AddModelError("Account", "Account Not Found");
        //            return BadRequest(ModelState);
        //        }

        //        //





        //    }
        //    else
        //    {
        //        return BadRequest(ModelState);
        //    }


        //}


        //[Route("save")]
        //[Attributes.Authorize(Roles = "Administrator")]
        //public async Task<IHttpActionResult> AddShelterAdmin(UserViewModel userViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        ApplicationUser u;
        //        // Check Email
        //        var userbyemail = await _repo.FindUser(userViewModel.Email);
        //        if (userbyemail != null)
        //        {
        //            ModelState.AddModelError("Email", "Email Must Be Unique");
        //            return BadRequest(ModelState);
        //        }

        //        IdentityResult result = await _repo.AddShelterAdmin(userViewModel.UserName, userViewModel.Password,
        //            userViewModel.Role, userViewModel.Name, userViewModel.Email, userViewModel.PhoneNumber,
        //            userViewModel.CityId);

        //        IHttpActionResult errorResult = GetErrorResult(result);

        //        if (errorResult != null)
        //        {
        //            return errorResult;
        //        }
        //        else
        //        {
        //            u = await _repo.FindUser(userViewModel.UserName, userViewModel.Password);
        //        }
        //        return Ok(u.Id);


        //    }
        //    else
        //    {
        //        return BadRequest(ModelState);
        //    }


        //}

        //[Route("GetFilterdUsers")]
        //[HttpPost]
        //public async Task<IHttpActionResult> GetFilterdUsers([FromBody] PagingViewModel viewModel)
        //{
        //    return Ok(await _unitOfWork.ApplicationUsers.GetWithFilterAsync(viewModel.PageNumber, viewModel.PageSize));
        //}

        //[Route("GetAll")]
        //[HttpPost]
        //[Attributes.Authorize(Roles = "Administrator")]
        //public async Task<IHttpActionResult> GetUsersInShelterROle([FromBody] PagingViewModel viewModel)
        //{
        //    IHttpActionResult response;
        //    try
        //    {
        //        int currentPage = viewModel.PageNumber;
        //        int currentPageSize = viewModel.PageSize;

        //        var alluser = await _unitOfWork.ApplicationUsers.GetWithFilterAsync(
        //        viewModel.PageNumber,
        //        viewModel.PageSize,
        //        viewModel.Filter,
        //         role: "Shelter"
        //        );

        //        if (alluser.Count == 0)
        //            return Ok(new PaginationSet<AppUserData>());
        //        int totalCount = alluser[0].OverallCount;



        //        PaginationSet<AppUserData> pagedSet = new PaginationSet<AppUserData>()
        //        {
        //            Items = alluser,
        //            Page = currentPage,
        //            TotalCount = totalCount,
        //            TotalPages = (int)Math.Ceiling((decimal)totalCount / currentPageSize)
        //        };

        //        response = Ok(pagedSet);
        //    }
        //    catch (DbUpdateConcurrencyException ex)
        //    {
        //        //LogError( ex );
        //        response = NotFound();
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        // LogError( ex );
        //        response = BadRequest(ex.InnerException?.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        //LogError( ex );
        //        response = InternalServerError(ex);
        //    }

        //    return response;

        //}


        //[Route("GetShelterAdmin/{id}")]
        //[Attributes.Authorize(Roles = "Administrator")]
        //public async Task<IHttpActionResult> GetShelterAdmin(string id)
        //{
        //    IHttpActionResult response;
        //    try
        //    {
        //        int? shelterid = null;
        //        if (id != "0")
        //            shelterid = int.Parse(id.Trim());

        //        var allcategory = await _unitOfWork.ApplicationUsers.GetShelterAdmin(shelterid);


        //        response = Ok(allcategory);
        //    }
        //    catch (DbUpdateConcurrencyException ex)
        //    {
        //        //LogError( ex );
        //        response = NotFound();
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        // LogError( ex );
        //        response = BadRequest(ex.InnerException?.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        //LogError( ex );
        //        response = InternalServerError(ex);
        //    }

        //    return response;
        //}

        //[Route("EditUser")]
        //[Attributes.Authorize(Roles = "User")]
        //[HttpPost]
        //public async Task<IHttpActionResult> EditUser(EditUserViewModel userViewModel)
        //{
        //    IHttpActionResult response;
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //            return BadRequest(ModelState);
        //        string userName = User.Identity.GetUserName();
        //        ApplicationUser u = await GetApplicationUser(userName);
        //        if (u == null)
        //            return BadRequest("You need login");
        //        userViewModel.Id = u.Id;

        //        var bo = Mapper.Map<EditUserViewModel, RegisterUserData>(userViewModel);
        //        IdentityResult result = await _repo.EditUser(bo);
        //        if (result == IdentityResult.Success)
        //        {
        //            response = Ok(bo.Id);
        //            return response;
        //        }
        //        else
        //        {
        //            response = BadRequest();
        //            return response;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        LogError(e);
        //        response = InternalServerError(e);

        //    }
        //    return response;
        //}


        #region admin area 
        [Route("GetAllUserAdmin")]
        [HttpPost]
        [Attributes.Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> GetAllUserAdmin([FromBody] PagingViewModel viewModel)
        {
            IHttpActionResult response;
            try
            {
                int currentPage = viewModel.PageNumber;
                int currentPageSize = viewModel.PageSize;

                var alluser = await _unitOfWork.User.GetWithFilterAsync(
                viewModel.PageNumber,
                viewModel.PageSize,
                viewModel.Filter,
                 role: "User"
                );

                if (alluser.Count == 0)
                    return Ok(new PaginationSet<UserData>());
                int totalCount = alluser[0].OverAllCount;



                PaginationSet<UserData> pagedSet = new PaginationSet<UserData>()
                {
                    Items = alluser,
                    Page = currentPage,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling((decimal)totalCount / currentPageSize)
                };

                response = Ok(pagedSet);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                LogError( ex );
                response = NotFound();
            }
            catch (DbUpdateException ex)
            {
                LogError( ex );
                response = BadRequest(ex.InnerException?.Message);
            }
            catch (Exception ex)
            {
                LogError( ex );
                response = InternalServerError(ex);
            }

            return response;

        }

        #endregion




    }
}