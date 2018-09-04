using AutoMapper;
using Microsoft.AspNet.Identity;
using Saned.ArousQatar.Api.Infrastructure.Core;
using Saned.ArousQatar.Api.Models;
using Saned.ArousQatar.Data.Core;
using Saned.ArousQatar.Data.Core.Dtos;
using Saned.ArousQatar.Data.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using System.Web.Http;

namespace Saned.ArousQatar.Api.Controllers
{
    [RoutePrefix("api/comment")]
    public class CommentController : ApiControllerBase
    {
        public CommentController(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {

        }

        public CommentController()
        {

        }

        [HttpPost]
        [Route("GetPagedComments")]
        public async Task<IHttpActionResult> GetPagedComments(ViewModel model)
        {
            IHttpActionResult response;
            try
            {
                int totalCount = 0;
                var allcomment = await _unitOfWork.Comments.GetPagedComments(model.PageNumber, model.PageSize, model.Filter);
                totalCount = await _unitOfWork.Complaints.CountAsync();

                PaginationSet<CommentDto> pagedSet = new PaginationSet<CommentDto>()
                {
                    Items = allcomment,
                    Page = model.PageNumber,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling((decimal)totalCount / model.PageSize)
                };

                //   var allcommentVm = Mapper.Map<List<CommentDto>, List<CommentViewModel>>(allcomment);

                //foreach (var comment in allcommentVm)
                //{
                //    var replys = await _unitOfWork.Comments.GetAllReplysAsync(comment.Id);
                //    comment.Replys = Mapper.Map<List<CommentDto>, List<CommentViewModel>>(replys);
                //}

                response = Ok(pagedSet);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                LogError(ex);
                response = NotFound();
            }
            catch (DbUpdateException ex)
            {
                LogError(ex);
                response = BadRequest(ex.InnerException?.Message);
            }
            catch (Exception ex)
            {
                LogError(ex);
                response = InternalServerError(ex);
            }

            return response;
        }

        [HttpPost]
        [Route("GetAll")]
        public async Task<IHttpActionResult> GetAll(ViewModel model)
        {
            IHttpActionResult response;
            try
            {
                var allcomment = await _unitOfWork.Comments.GetAllAsync(model.Id, model.PageNumber, model.PageSize, model.Filter);

                //   var allcommentVm = Mapper.Map<List<CommentDto>, List<CommentViewModel>>(allcomment);

                //foreach (var comment in allcommentVm)
                //{
                //    var replys = await _unitOfWork.Comments.GetAllReplysAsync(comment.Id);
                //    comment.Replys = Mapper.Map<List<CommentDto>, List<CommentViewModel>>(replys);
                //}

                response = Ok(allcomment);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                LogError(ex);
                response = NotFound();
            }
            catch (DbUpdateException ex)
            {
                LogError(ex);
                response = BadRequest(ex.InnerException?.Message);
            }
            catch (Exception ex)
            {
                LogError(ex);
                response = InternalServerError(ex);
            }

            return response;
        }

        [HttpPost]
        [Route("GetAllAds")]
        public async Task<IHttpActionResult> GetAllAds(ViewModel model)
        {
            IHttpActionResult response;
            try
            {
                int totalCount = 0;
                var allcomment = await _unitOfWork.Comments.GetAllAsync(model.Id, model.PageNumber, model.PageSize, model.Filter);
                totalCount = allcomment[0].OverAllCount;
                PaginationSet<CommentDto> pagedSet = new PaginationSet<CommentDto>()
                {
                    Items = allcomment,
                    Page = model.PageNumber,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling((decimal)totalCount / model.PageSize)
                };

                response = Ok(pagedSet);
            }
            catch (Exception ex)
            {
                LogError(ex);
                response = InternalServerError(ex);
            }

            return response;
        }
        [HttpPost]
        [Route("GetCommentForAd")]
        public async Task<IHttpActionResult> GetAllForadmin(ViewModel model)
        {
            IHttpActionResult response;
            try
            {
                int totalCount = 0;
                var allcomment = await _unitOfWork.Comments.GetAllAsync(model.Id, model.PageNumber, model.PageSize, model.Filter);
                totalCount = allcomment[0].OverAllCount;


                PaginationSet<CommentDto> pagedSet = new PaginationSet<CommentDto>()
                {
                    Items = allcomment,
                    Page = model.PageNumber,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling((decimal)totalCount / model.PageSize)
                };

                response = Ok(pagedSet);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                LogError(ex);
                response = NotFound();
            }
            catch (DbUpdateException ex)
            {
                LogError(ex);
                response = BadRequest(ex.InnerException?.Message);
            }
            catch (Exception ex)
            {
                LogError(ex);
                response = InternalServerError(ex);
            }

            return response;
        }



        // GET: /api/comment/5
        //[ResponseType(typeof(Comment))]
        [Route("{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSingle(int id)
        {
            try
            {
                var comment = await (_unitOfWork.Comments.GetSingleDtoAsync(id));
                // ReSharper disable once PossibleInvalidOperationException
                if (comment == null)
                    return NotFound();

                var commentVm = Mapper.Map<CommentDto, CommentViewModel>(comment);
                var replys = await _unitOfWork.Comments.GetAllReplysAsync(id);
                commentVm.Replys = Mapper.Map<List<CommentDto>, List<CommentViewModel>>(replys);

                return Ok(commentVm);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                LogError(ex);
                return NotFound();
            }
            catch (DbUpdateException ex)
            {
                LogError(ex);
                return BadRequest(ex.InnerException?.Message);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return InternalServerError(ex);
            }


        }

        // POST: /api/comment/Add
        [HttpPost]
        [Route("Add")]
        public async Task<IHttpActionResult> AddComment(CommentViewModel model)
        {
            IHttpActionResult response;
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                string userName = User.Identity.GetUserName();
                ApplicationUser u = await GetApplicationUser(userName);
                if (u == null)
                {
                    ModelState.AddModelError("", "You Need To Login");
                    return BadRequest(ModelState);
                }
                model.ApplicationUserId = u.Id;
                var comment = Mapper.Map<CommentViewModel, Comment>(model);


                _unitOfWork.Comments.Add(comment);

                await _unitOfWork.CommitAsync();

                model.UserFirstName = userName;
                model.UserFullName = u.Name;
                model.Id = comment.Id;
                response = Ok(model);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                LogError(ex);
                response = NotFound();
            }
            catch (Exception ex)
            {
                LogError(ex);
                response = InternalServerError(ex);
            }
            return response;
        }

        // DELETE: /api/comment/delete/5
        //[ResponseType(typeof(Comment))]
        [HttpDelete]
        [Authorize]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            IHttpActionResult response;

            try
            {
                var comment = await (_unitOfWork.Comments.GetSingleAsync(id));
                if (comment == null)
                    return NotFound();
                _unitOfWork.Comments.Delete(comment);
                await _unitOfWork.CommitAsync();
                response = Ok();
            }
            catch (Exception ex)
            {
                LogError(ex);
                response = InternalServerError(ex);
            }

            return response;
        }


        [HttpPost]
        [Route("GetPagedCommentsAdmin")]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> GetPagedCommentsAdmin(ViewModel model)
        {
            IHttpActionResult response;
            try
            {
                int totalCount = 0;
                var allcomment = await _unitOfWork.Comments.GetPagedComments(model.PageNumber, model.PageSize, model.Filter);
                totalCount = await _unitOfWork.Comments.CountAsync();

                PaginationSet<CommentDto> pagedSet = new PaginationSet<CommentDto>()
                {
                    Items = allcomment,
                    Page = model.PageNumber,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling((decimal)totalCount / model.PageSize)
                };

                //   var allcommentVm = Mapper.Map<List<CommentDto>, List<CommentViewModel>>(allcomment);

                //foreach (var comment in allcommentVm)
                //{
                //    var replys = await _unitOfWork.Comments.GetAllReplysAsync(comment.Id);
                //    comment.Replys = Mapper.Map<List<CommentDto>, List<CommentViewModel>>(replys);
                //}

                response = Ok(pagedSet);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                LogError(ex);
                response = NotFound();
            }
            catch (DbUpdateException ex)
            {
                LogError(ex);
                response = BadRequest(ex.InnerException?.Message);
            }
            catch (Exception ex)
            {
                LogError(ex);
                response = InternalServerError(ex);
            }

            return response;
        }

    }
}