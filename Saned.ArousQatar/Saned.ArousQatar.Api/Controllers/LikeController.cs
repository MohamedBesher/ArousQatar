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

namespace Saned.ArousQatar.Api.Controllers
{
    [RoutePrefix("api/like")]
    public class LikeController : ApiControllerBase
    {
        public LikeController(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {

        }

        public LikeController()
        {

        }
        [Route("{id:int}/all")]
        public async Task<IHttpActionResult> GetAll(int id)
        {
            IHttpActionResult response;
            try
            {
                var alllike = await _unitOfWork.Likes.GetAllAsync(id);
                var alllikeVm = Mapper.Map<List<LikeDto>, List<LikeViewModel>>(alllike);

                response = Ok(alllikeVm);
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
        // POST: /api/like/update/5?
        //[ResponseType(typeof(void))]
        [HttpPost]
        [Route("add")]
        [Attributes.Authorize(Roles = "User")]
        public async Task<IHttpActionResult> AddLike(LikeViewModel model)
        {
            int id = 0;
            IHttpActionResult response;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userName = User.Identity.GetUserName();
            ApplicationUser u = await GetApplicationUser(userName);
            if (u == null)
            {
                ModelState.AddModelError("", "You Need To Login");
                return BadRequest(ModelState);
            }
            model.ApplicationUserId = u.Id;
            try
            {
                Like like = new Like()
                {
                    AdvertismentId = model.AdvertismentId,
                    ApplicationUserId = model.ApplicationUserId
                };

                var oldlike = await _unitOfWork.Likes.GetSingleAsyncByUser(model.AdvertismentId, model.ApplicationUserId);
                if (oldlike == null)
                {
                    _unitOfWork.Likes.Add(like);
                    id = await _unitOfWork.CommitAsync();
                }

                var advertisment = await (_unitOfWork.Advertisements.GetSingleAsync(model.AdvertismentId));
                if (advertisment != null)
                {
                    advertisment.NumberOfLikes = advertisment.NumberOfLikes + 1;
                    _unitOfWork.Advertisements.Edit(advertisment);
                    await _unitOfWork.CommitAsync();
                }


                response = Ok(id);
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

        // DELETE: /api/like/delete/5
        //[ResponseType(typeof(Like))]
        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> DeleteLike(int id)
        {
            IHttpActionResult response;

            try
            {
                string userName = User.Identity.GetUserName();
                ApplicationUser u = await GetApplicationUser(userName);

                var like = await (_unitOfWork.Likes.GetSingleAsyncByUser(id,u.Id));
                if (like == null)
                {
                    return Ok("0");
                }

                _unitOfWork.Likes.Delete(like);

                await _unitOfWork.CommitAsync();

                response = Ok("1");
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