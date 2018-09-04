using AutoMapper;
using Saned.ArousQatar.Api.Infrastructure.Core;
using Saned.ArousQatar.Api.Infrastructure.Extensions;
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
    [RoutePrefix("api/advertisementPrice")]
    public class AdvertismentPriceController : ApiControllerBase
    {
        public AdvertismentPriceController()
        {

        }
        public AdvertismentPriceController(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {

        }

        // GET: /api/advertismentPrice/search/1/4?filter=??
        [Route("all")]
        [HttpPost]
        public async Task<IHttpActionResult> GetAllFilterd(RouteViewModel route)
        {
            IHttpActionResult response = null;
            try
            {
                int currentPage = route.PageNumber.Value;
                int currentPageSize = route.PageSize.Value;
                int totalCount = 0;


                var alladvertismentPrice =
                    await
                        _unitOfWork.AdvertisementPrice.GetAllAsync((currentPage - 1) * currentPageSize, currentPageSize,
                            route.Filter ?? "", false);

                totalCount = alladvertismentPrice[0].OverAllCount;

                var alladvertismentPriceVm = Mapper.Map<List<AdvertisementPriceDto>, List<AdvertismentPriceViewModel>>(alladvertismentPrice);


                PaginationSet<AdvertismentPriceViewModel> pagedSet = new PaginationSet<AdvertismentPriceViewModel>()
                {
                    Items = alladvertismentPriceVm,
                    Page = currentPage,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling((decimal)totalCount / currentPageSize)
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
                response = BadRequest(ex.InnerException.Message);
            }

            catch (Exception ex)
            {
                LogError(ex);
                response = InternalServerError(ex);
            }

            return response;
        }

        [Route("allAdmin")]
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IHttpActionResult> GetAllFilterdAdmin(RouteViewModel route)
        {
            IHttpActionResult response = null;
            try
            {
                int currentPage = route.PageNumber.Value;
                int currentPageSize = route.PageSize.Value;
                int totalCount = 0;


                var alladvertismentPrice =
                    await
                        _unitOfWork.AdvertisementPrice.GetAllAsync((currentPage - 1) * currentPageSize, currentPageSize,
                            route.Filter ?? "", false);

                totalCount = alladvertismentPrice[0].OverAllCount;

                var alladvertismentPriceVm = Mapper.Map<List<AdvertisementPriceDto>, List<AdvertismentPriceViewModel>>(alladvertismentPrice);


                PaginationSet<AdvertismentPriceViewModel> pagedSet = new PaginationSet<AdvertismentPriceViewModel>()
                {
                    Items = alladvertismentPriceVm,
                    Page = currentPage,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling((decimal)totalCount / currentPageSize)
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
                response = BadRequest(ex.InnerException.Message);
            }

            catch (Exception ex)
            {
                LogError(ex);
                response = InternalServerError(ex);
            }

            return response;
        }

        [Route("GetAll")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            IHttpActionResult response = null;
            try
            {
                var advertismentPrice = await _unitOfWork.AdvertisementPrice.GetAllAsync();
                response = Ok(advertismentPrice);
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

        [Route("archieve/all")]
        [HttpPost]
        public async Task<IHttpActionResult> GetAllArchieve(RouteViewModel route)
        {
            IHttpActionResult response = null;
            try
            {
                int currentPage = route.PageNumber ?? 1;
                int currentPageSize = route.PageSize ?? 10;
                int totalCount = 0;


                var alladvertisment = await _unitOfWork.AdvertisementPrice.GetAllAsync((currentPage - 1) * currentPageSize, currentPageSize, route.Filter ?? "", true);
                totalCount = alladvertisment[0].OverAllCount;

                var alladvertismentVm = Mapper.Map<List<AdvertisementPriceDto>, List<AdvertismentPriceViewModel>>(alladvertisment);


                PaginationSet<AdvertismentPriceViewModel> pagedSet = new PaginationSet<AdvertismentPriceViewModel>()
                {
                    Items = alladvertismentVm,
                    Page = currentPage,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling((decimal)totalCount / currentPageSize)
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
                response = BadRequest(ex.InnerException.Message);
            }

            catch (Exception ex)
            {
                LogError(ex);
                response = InternalServerError(ex);
            }

            return response;
        }

        // GET: /api/advertismentPrice/5
        //[ResponseType(typeof(AdvertismentPrice))]
        [Route("{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSingle(int id)
        {
            try
            {
                var advertismentPrice = await (_unitOfWork.AdvertisementPrice.GetSingleAsync(id));
                // ReSharper disable once PossibleInvalidOperationException
                if (advertismentPrice == null || advertismentPrice.IsArchieved.Value)
                    return NotFound();

                var advertismentPriceVm = Mapper.Map<AdvertismentPrice, AdvertismentPriceViewModel>(advertismentPrice);

                return Ok(advertismentPriceVm);

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


        [Route("GetSingleAdmin/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> GetSingleAdmin(int id)
        {
            try
            {
                var advertismentPrice = await (_unitOfWork.AdvertisementPrice.GetSingleAsync(id));
                // ReSharper disable once PossibleInvalidOperationException
                if (advertismentPrice == null || advertismentPrice.IsArchieved.Value)
                    return NotFound();

                var advertismentPriceVm = Mapper.Map<AdvertismentPrice, AdvertismentPriceViewModel>(advertismentPrice);

                return Ok(advertismentPriceVm);

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

        // POST: /api/advertismentPrice/update/5?
        //[ResponseType(typeof(void))]
        [HttpPost]
        [Route("update")]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> UpdateAdvertismentPrice(AdvertismentPriceViewModel model)
        {
            IHttpActionResult response;
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (model.Id == 0)
                {

                    #region Adding

                    AdvertismentPrice advertismentPrice = new AdvertismentPrice();
                    // the only thing left
                    advertismentPrice.Update(model);


                    bool isDuplicateDaysCount = _unitOfWork.AdvertisementPrice.IsDuplicateDaysCount(advertismentPrice.Period);
                    if (isDuplicateDaysCount)
                    {
                        return Ok(new { error = "الرجاء اضافة باقة بعدد ايام مختلف" });
                    }

                    //Don't Miss Here File Upload

                    _unitOfWork.AdvertisementPrice.Add(advertismentPrice);

                    #endregion
                }
                else
                {
                    #region Update

                    var advertismentPrice = await (_unitOfWork.AdvertisementPrice.GetSingleAsync(model.Id));

                    // ReSharper disable once PossibleInvalidOperationException
                    if (advertismentPrice == null || advertismentPrice.IsArchieved.Value)
                    {
                        return NotFound();
                    }

                    advertismentPrice.Update(model);

                    bool isDuplicateDaysCount = _unitOfWork.AdvertisementPrice.IsDuplicateDaysCount(advertismentPrice.Period, advertismentPrice.Id);
                    if (isDuplicateDaysCount)
                    {
                        return Ok(new { error = "الرجاء اضافة باقة بعدد ايام مختلف" });
                    }

                    //Dont Miss Here File Upload

                    _unitOfWork.AdvertisementPrice.Edit(advertismentPrice);

                    #endregion

                }
                await _unitOfWork.CommitAsync();

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

        // DELETE: /api/advertismentPrice/delete/5
        //[ResponseType(typeof(AdvertismentPrice))]
        [HttpDelete]
        [Route("delete/{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            IHttpActionResult response;

            try
            {
                var advertismentPrice = await (_unitOfWork.AdvertisementPrice.GetSingleAsync(id));

                if (advertismentPrice == null)
                {
                    return NotFound();
                }

                //check if there is relations
                if (await _unitOfWork.AdvertisementPrice.IsRelatedWithAdvertisements(id))
                    return Ok(new { isDeleted = false, errorMessage = "الباقة المراده مرتبطة بإعلانات" });

                _unitOfWork.AdvertisementPrice.Delete(advertismentPrice);

                await _unitOfWork.CommitAsync();
                response = Ok(new { isDeleted = true });
            }
            catch (Exception ex)
            {
                LogError(ex);
                response = InternalServerError(ex);
            }

            return response;
        }

        // Archive: /api/advertismentPrice/archive/5
        //[ResponseType(typeof(AdvertismentPrice))]
        [HttpPost]
        [Route("archive/{id:int}")]
        public async Task<IHttpActionResult> Archive(int id)
        {
            IHttpActionResult response;

            try
            {
                var advertismentPrice = await (_unitOfWork.AdvertisementPrice.GetSingleAsync(id));

                if (advertismentPrice == null)
                {
                    return NotFound();
                }

                if (advertismentPrice.IsArchieved != null && advertismentPrice.IsArchieved.Value)
                    return BadRequest("السعر مأرشف بالفعل");

                //check if there is relations

                advertismentPrice.IsArchieved = true;
                _unitOfWork.AdvertisementPrice.Edit(advertismentPrice);
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


        // DeArchive: /api/advertismentPrice/dearchive/5
        //[ResponseType(typeof(AdvertismentPrice))]
        [HttpPost]
        [Route("dearchive/{id:int}")]
        public async Task<IHttpActionResult> DeArchive(int id)
        {
            IHttpActionResult response;

            try
            {
                var advertismentPrice = await (_unitOfWork.AdvertisementPrice.GetSingleAsync(id));

                if (advertismentPrice == null)
                {
                    return NotFound();
                }

                if (advertismentPrice.IsArchieved != null && !advertismentPrice.IsArchieved.Value)
                    return BadRequest("السعر غير مأرشف");

                //check if there is relations

                advertismentPrice.IsArchieved = false;
                _unitOfWork.AdvertisementPrice.Edit(advertismentPrice);
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
    }
}