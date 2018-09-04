using AutoMapper;
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
using Microsoft.AspNet.Identity;

namespace Saned.ArousQatar.Api.Controllers
{
    [RoutePrefix("api/favorite")]
    public class FavoriteController : ApiControllerBase
    {
        public FavoriteController(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {

        }

        public FavoriteController()
        {

        }
        [HttpPost]
        [Route("GetAll")]
        [Attributes.Authorize(Roles = "User")]
        public async Task<IHttpActionResult> GetAll(RouteViewModel route)
        {
            string userName = User.Identity.GetUserName();
            ApplicationUser u = await GetApplicationUser(userName);
            if (u == null)
            {
                ModelState.AddModelError("", "You Need To Login");
                return BadRequest(ModelState);
            }

            IHttpActionResult response;
            int currentPage = route.PageNumber.Value;
            int currentPageSize = route.PageSize.Value;
            try
            {
                var allfavorite = await _unitOfWork.Favorites.GetAllAsync(userName, (currentPage - 1) * currentPageSize, currentPageSize);
                var allfavoriteVm = Mapper.Map<List<FavoriteDto>, List<FavoriteViewModel>>(allfavorite);

                response = Ok(allfavoriteVm);
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



        // GET: /api/favorite/5
        //[ResponseType(typeof(Favorite))]
        [Route("{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSingle(int id)
        {
            try
            {
                var favorite = await (_unitOfWork.Favorites.GetSingleAsync(id));
                // ReSharper disable once PossibleInvalidOperationException
                if (favorite == null)
                    return NotFound();

                var favoriteVm = Mapper.Map<FavoriteDto, FavoriteViewModel>(favorite);

                return Ok(favoriteVm);

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



        // POST: /api/favorite/update/5?
        //[ResponseType(typeof(void))]
        [HttpPost]
        [Route("add")]
        [Attributes.Authorize(Roles = "User")]
        public async Task<IHttpActionResult> AddFavorite(FavoriteViewModel model)
        {
            int i = 0;
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

                Favorite favorite = new Favorite()
                {
                    AdvertismentId = model.AdvertismentId,
                    ApplicationUserId = model.ApplicationUserId
                };
                var oldfav = await _unitOfWork.Favorites.GetSingleAsyncByUser(model.AdvertismentId, model.ApplicationUserId);
                if (oldfav == null)
                {
                    _unitOfWork.Favorites.Add(favorite);
                    i= await _unitOfWork.CommitAsync();

                }

                response = Ok(i);
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

        // DELETE: /api/favorite/delete/5
        //[ResponseType(typeof(Favorite))]
        [HttpPost]
        [Route("delete/{id:int}")]
        //[Attributes.Authorize(Roles = "User")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            IHttpActionResult response;

            try
            {
                string userName = User.Identity.GetUserName();
                ApplicationUser u = await GetApplicationUser(userName);
                var favorite = await (_unitOfWork.Favorites.GetSingleAsyncByUser(id,u.Id));
                if (favorite == null)
                {
                    return Ok("0");
                }

                _unitOfWork.Favorites.Delete(favorite);

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