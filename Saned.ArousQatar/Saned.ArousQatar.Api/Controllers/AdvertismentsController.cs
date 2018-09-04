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
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Web.Hosting;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;


namespace Saned.ArousQatar.Api.Controllers
{
    [RoutePrefix("api/advertisement")]
    public class AdvertismentController : ApiControllerBase
    {
        public AdvertismentController(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {

        }

        public AdvertismentController()
        {

        }

        // GET: /api/advertisment/search/1/4?filter=??
        [HttpPost]
        [Route("GetAllFilterd")]
        public async Task<IHttpActionResult> GetAllFilterd(RouteViewModel route)
        {
            IHttpActionResult response = null;
            try
            {
                int currentPage = route.PageNumber ?? 1;
                int currentPageSize = route.PageSize ?? 10;
                int totalCount = 0;


                var alladvertisment = await _unitOfWork.Advertisements.GetAllAsync((currentPage - 1) * currentPageSize, currentPageSize, route.Filter ?? "", false, route.CategoryIdFilter, route.IsPaidedFilter, route.UserIdFilter);

                if (alladvertisment.Count > 0)
                {
                    totalCount = alladvertisment[0].OverAllCount;
                }
                else
                {
                    totalCount = 0;
                }

                PaginationSet<AdvertisementDto> pagedSet = new PaginationSet<AdvertisementDto>()
                {
                    Items = alladvertisment,
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
        [HttpPost]
        [Route("Search")]
        public async Task<IHttpActionResult> GetAll(RouteViewModel route)
        {

            IHttpActionResult response;
            try
            {
                int currentPage = route.PageNumber ?? 1;
                int currentPageSize = route.PageSize ?? 10;

                var alladvertisment =
                   await
                       _unitOfWork.Advertisements.GetAllByCategoryId(null, (currentPage - 1) * currentPageSize,
                           currentPageSize, route.Filter ?? "", false, true);

                response = Ok(alladvertisment);
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
        [Route("archieve/all")]
        public async Task<IHttpActionResult> GetAllArchieve(RouteViewModel route)
        {
            IHttpActionResult response = null;
            try
            {
                int currentPage = route.PageNumber ?? 1;
                int currentPageSize = route.PageSize ?? 10;
                int totalCount = 0;


                var alladvertisment = await _unitOfWork.Advertisements.GetAllArchiveAsync((currentPage - 1) * currentPageSize, currentPageSize, route.Filter ?? "", true);
                totalCount = await _unitOfWork.Advertisements.CountArchieveAsync();

                var alladvertismentVm = Mapper.Map<List<AdvertisementDto>, List<AdvertismentViewModel>>(alladvertisment);


                PaginationSet<AdvertismentViewModel> pagedSet = new PaginationSet<AdvertismentViewModel>()
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

        [HttpPost]
        [Route("{catid:int}/all")]
        public async Task<IHttpActionResult> GetAllByCategoryId(int catid, RouteViewModel route)
        {
            IHttpActionResult response;
            try
            {
                int currentPage = route.PageNumber ?? 1;
                int currentPageSize = route.PageSize ?? 10;
                int totalCount = 0;

                var alladvertisment =
                    await
                        _unitOfWork.Advertisements.GetAllByCategoryId(catid, (currentPage - 1) * currentPageSize,
                            currentPageSize, route.Filter ?? "", false);
                var alladvertismentVm = Mapper.Map<List<AdvertisementSmallDto>, List<AdvertisementSmallViewModel>>(alladvertisment);
                totalCount = alladvertisment[0].OverAllCount;

                PaginationSet<AdvertisementSmallViewModel> pagedSet = new PaginationSet<AdvertisementSmallViewModel>()
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
                response = BadRequest(ex.InnerException?.Message);
            }
            catch (Exception ex)
            {
                LogError(ex);
                response = InternalServerError(ex);
            }

            return response;
        }

        [Authorize]
        [HttpPost]
        [Route("GetUserAds")]
        public async Task<IHttpActionResult> GetAllByUsername(RouteViewModel route)
        {

            string username = "";
            IHttpActionResult response;
            try
            {
                int currentPage = route.PageNumber ?? 1;
                int currentPageSize = route.PageSize ?? 10;
                int totalCount = 0;

                var alladvertisment =
                    await
                        _unitOfWork.Advertisements.GetAllByUsername(username, (currentPage - 1) * currentPageSize,
                            currentPageSize, route.Filter ?? "", false);

                List<MyAdvertisementViewModel> alladvertismentVm = null;

                if (alladvertisment.Count > 0)
                {
                    alladvertismentVm =
                        Mapper.Map<List<MyAdvertisementDto>, List<MyAdvertisementViewModel>>(alladvertisment);
                    totalCount = alladvertisment[0].OverAllCount;
                }

                PaginationSet<MyAdvertisementViewModel> pagedSet = new PaginationSet<MyAdvertisementViewModel>()
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
        [Route("GeAdsByCategoryId")]
        public async Task<IHttpActionResult> GeAdsByCategoryId(RouteViewModel route)
        {
            IHttpActionResult response;
            try
            {
                int currentPage = route.PageNumber ?? 1;
                int currentPageSize = route.PageSize ?? 10;

                var alladvertisment =
                   await
                       _unitOfWork.Advertisements.GetAllByCategoryId(route.CategoryId, (currentPage - 1) * currentPageSize,
                           currentPageSize, route.Filter ?? "", false, true);

                response = Ok(alladvertisment);
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

        // GET: /api/advertisment/5
        //[ResponseType(typeof(Advertisment))]
        [Route("{id:int}")]
        [HttpGet]

        public async Task<IHttpActionResult> GetSingle(int id)
        {
            try
            {
                var advertisment = await (_unitOfWork.Advertisements.GetSingleDtoAsync(id));
                // ReSharper disable once PossibleInvalidOperationException
                if (advertisment == null)
                    return NotFound();

                var advertismentVm = Mapper.Map<AdvertisementDto, AdViewModel>(advertisment);
                advertismentVm.JsonImages=JsonConvert.SerializeObject(await _unitOfWork.AdvertisementImages.GetAllAsync(id));
                //advertismentVm.Images = await _unitOfWork.AdvertisementImages.GetAllAsync(id);
                string userName = User.Identity.GetUserName();
                ApplicationUser u = await GetApplicationUser(userName);
                if (u != null)
                {

                    var likeob = await _unitOfWork.Likes.GetSingleAsyncByUser(advertisment.Id, u.Id);
                    advertismentVm.IsLiked = likeob != null;

                    var faveob = await _unitOfWork.Favorites.GetSingleAsyncByUser(advertisment.Id, u.Id);
                    advertismentVm.IsFavorit = faveob != null;

                    var comob = await _unitOfWork.Complaints.GetSingleAsyncByUser(advertisment.Id, u.Id);
                    advertismentVm.IsReport = comob != null;

                    if (advertisment.ApplicationUserId != u.Id)
                    {
                        advertisment.NumberOfViews = advertisment.NumberOfViews + 1;
                        var advertismentmodel = Mapper.Map<AdvertisementDto, Advertisment>(advertisment);
                        _unitOfWork.Advertisements.Edit(advertismentmodel);
                        await _unitOfWork.CommitAsync();
                        advertismentVm.NumberOfViews = advertisment.NumberOfViews.Value;
                        advertismentVm.IsOwner = false;
                    }
                    else
                    {
                        advertismentVm.IsOwner = true;
                    }
                }

                return Ok(advertismentVm);

            }
            catch (DbUpdateConcurrencyException ex)
            {   
                LogError(ex);
                string msg = ex.GetaAllMessages();
                return BadRequest(msg);
            }
            catch (DbUpdateException ex)
            {
                LogError(ex);
                string msg = ex.GetaAllMessages();
                return BadRequest(msg);
            }
            catch (Exception ex)
            {
                LogError(ex);
                string msg = ex.GetaAllMessages();
                return BadRequest(msg);
            }


        }

        [HttpPost]
        [Route("UserAds")]
        [Attributes.Authorize(Roles = "User")]
        public async Task<IHttpActionResult> GetByUser(PagingViewModel model)
        {
            IHttpActionResult response;
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

                var advertisments =
                    await
                        _unitOfWork.Advertisements.GetByUserId(model.ApplicationUserId, model.PageNumber,
                            model.PageSize, model.Filter, model.isArchieve, true);
                response = Ok(advertisments);
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

        // POST:api/advertisement/save
        [HttpPost]
        [Route("save")]
        [Authorize]
        //[Attributes.Authorize(Roles = "User")]
        public async Task<IHttpActionResult> UpdateAdvertisment(AdvertismentViewModel model)
        {
            IHttpActionResult response;
            Advertisment advertisment;
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

                if (model.Id == 0)
                {
                    advertisment = Mapper.Map<AdvertismentViewModel, Advertisment>(model);
                    _unitOfWork.Advertisements.Add(advertisment);
                    await _unitOfWork.CommitAsync();                   
                    await SaveAdsImages(model.Images, advertisment.Id);
                }
                else
                {
                    advertisment = await (_unitOfWork.Advertisements.GetSingleAsync(model.Id));
                    if (advertisment == null || advertisment.IsArchieved)
                        return NotFound();
                    advertisment.UpdateAds(model);
                    _unitOfWork.Advertisements.Edit(advertisment);
                    await SaveAdsImages(model.Images, advertisment.Id);

                }
                await _unitOfWork.CommitAsync();
                response = Ok(advertisment.Id);

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

        private async Task SaveAdsImages(List<AdImage> modelImagesViewModels, int advId)
        {
            if (modelImagesViewModels.Count > 0)
            {
               

                if (modelImagesViewModels.Count > 12)
                    modelImagesViewModels = modelImagesViewModels.Take(12).ToList();

               
                bool isMainImage = false;
                bool isMainImageSet = false;
                    for (int i = 0; i < modelImagesViewModels.Count; i++)
                    {
                    string image = modelImagesViewModels[i].ImageUrl;
                        int id = modelImagesViewModels[i].Id;
                                       // new image if id equal 0 and image not empty
                         if (id==0 && !string.IsNullOrEmpty(image))
                         {
                             isMainImageSet = SetMainImage(isMainImageSet, ref isMainImage);


                       var randomImage = SaveImage("1.jpg", modelImagesViewModels[i].ImageUrl);
                            AdvertismentImage adImg = new AdvertismentImage()
                            {
                                IsMainImage = isMainImage,
                                ImageUrl = randomImage,
                                AdvertismentId = advId
                            };
                            _unitOfWork.AdvertisementImages.Add(adImg);
                          await _unitOfWork.CommitAsync();
                          }
                    // edit image if id not equal 0 and image not empty
                       else if (id != 0)
                         {
                        isMainImageSet = SetMainImage(isMainImageSet, ref isMainImage);

                        // if not empty add image and delete existing 
                        if (!string.IsNullOrEmpty(image))
                             {
                                AdvertismentImage img = await _unitOfWork.AdvertisementImages.GetImageById(id);
                                Deletefile(img.ImageUrl);
                                var randomImage = SaveImage("1.jpg", modelImagesViewModels[i].ImageUrl);
                                img.Modify(randomImage, advId, isMainImage);                              
                                await _unitOfWork.CommitAsync();
                            }
                            //
                            else
                            {
                            AdvertismentImage img = await _unitOfWork.AdvertisementImages.GetImageById(id);
                                if (img!=null)
                                {
                                    img.IsMainImage = isMainImage;
                                    await _unitOfWork.CommitAsync();
                                }

                        }

                    }
                        
                    }
              
                
          
          }
           
          
        }

        private static bool SetMainImage(bool isMainImageSet, ref bool isMainImage)
        {
            if (!isMainImageSet)
            {
                isMainImage = true;
                isMainImageSet = true;
            }
            else isMainImage = false;
            return isMainImageSet;
        }

        private void Deletefiles(List<AdvertismentImage> images)
        {
            string slogn = "/Uploads/";
            foreach (AdvertismentImage image in images)
            {
                slogn += image.ImageUrl;
                string filePath = (HostingEnvironment.MapPath($"~{slogn}"));
                if (File.Exists(filePath))
                {   try
                    {
                    File.Delete(filePath);
                    }
                    catch (Exception e)
                    {                   

                        Console.WriteLine(e);
                        throw;
                    }
                }
            }
          
        }
        private void Deletefile(string image)
        {
            string slogn = "/Uploads/";          
                slogn += image;
                string filePath = (HostingEnvironment.MapPath($"~{slogn}"));
                if (File.Exists(filePath))
                {
                    try
                    {
                        File.Delete(filePath);
                    }
                    catch (Exception e)
                    {

                        Console.WriteLine(e);
                      throw;                  
                }
            }

        }

        // DELETE: /api/advertisment/delete/5
        //[ResponseType(typeof(Advertisment))]
        [HttpDelete]
        [Route("delete/{id:int}")]
        [Authorize(Roles = "User")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            IHttpActionResult response;
            try
            {
                var advertisment = await (_unitOfWork.Advertisements.GetSingleAsync(id));

                if (advertisment == null)
                {
                    return NotFound();
                }
                string userName = User.Identity.GetUserName();
                ApplicationUser u = await GetApplicationUser(userName);
                if (u.Id != advertisment.ApplicationUserId)
                {
                    return Unauthorized();
                    //ModelState.AddModelError("", "You Need To Login");
                    //return BadRequest(ModelState);
                }

                //check if there is relations

                    int result = await _unitOfWork.Advertisements.DeleteFromAllTables(advertisment.Id);

                await _unitOfWork.CommitAsync();
                response = Ok(result);
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorDescription = "";
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errorDescription+= "dbEx Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage;                      
                    }
                }
                LogError("DeleteAds", errorDescription);
               response = InternalServerError(dbEx);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                LogError(ex);
                string msg = ex.GetaAllMessages();
                return BadRequest(msg);
            }
            catch (DbUpdateException ex)
            {
                LogError(ex);
                string msg = ex.GetaAllMessages();
                return BadRequest(msg);
            }
            catch (Exception ex)
            {
                LogError(ex);
                response = InternalServerError(ex);
            }
            return response;
        }

        // Archive: /api/advertisment/archive/5
        //[ResponseType(typeof(Advertisment))]
        [HttpPost]
        [Route("archieve/{id:int}")]
        public async Task<IHttpActionResult> Archive(int id)
        {
            IHttpActionResult response;

            try
            {
                var advertisment = await (_unitOfWork.Advertisements.GetSingleAsync(id));

                if (advertisment == null)
                {
                    return NotFound();
                }

                if (advertisment.IsArchieved)
                    return BadRequest("التصنيف مأرشف بالفعل");

                //check if there is relations

                advertisment.IsArchieved = true;
                _unitOfWork.Advertisements.Edit(advertisment);
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


        // DeArchive: /api/advertisment/dearchive/5
        //[ResponseType(typeof(Advertisment))]
        [HttpPost]
        [Route("dearchieve/{id:int}")]
        public async Task<IHttpActionResult> DeArchive(int id)
        {
            IHttpActionResult response;

            try
            {
                var advertisment = await (_unitOfWork.Advertisements.GetSingleAsync(id));

                if (advertisment == null)
                {
                    return NotFound();
                }

                if ( !advertisment.IsArchieved)
                    return BadRequest("التصنيف غير مأرشف");

                //check if there is relations

                advertisment.IsArchieved = false;
                _unitOfWork.Advertisements.Edit(advertisment);
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

        [Route("GetHomeAds")]
        [HttpPost]
        public async Task<IHttpActionResult> GetAllFiltered(PagingViewModel viewModel)
        {
            IHttpActionResult response;
            try
            {
                int currentPage = viewModel.PageNumber;
                int? currentPageSize = viewModel.PageSize;

                var allcategory = await _unitOfWork.Advertisements.GetPaginAsync(currentPage, currentPageSize, false, true);
                response = Ok(allcategory);
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

        #region admin area 
        // GET: /api/advertisment/5
        //[ResponseType(typeof(Advertisment))]
        [Route("GetSingleAdmin/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> GetSingleAdmin(int id)
        {
            try
            {
                var advertisment = await (_unitOfWork.Advertisements.GetSingleDtoAsync(id));
                // ReSharper disable once PossibleInvalidOperationException
                if (advertisment == null)
                    return NotFound();

                var advertismentVm = Mapper.Map<AdvertisementDto, AdViewModel>(advertisment);


                List<AdvertismentImage> list= await _unitOfWork.AdvertisementImages.GetAllAsync(id);

                advertismentVm.ImagesList = Mapper.Map<List<AdvertismentImage>, List<AdImage>>(list);
                string userName = User.Identity.GetUserName();
                ApplicationUser u = await GetApplicationUser(userName);
                if (u != null)
                {

                    var likeob = await _unitOfWork.Likes.GetSingleAsyncByUser(advertisment.Id, u.Id);
                    advertismentVm.IsLiked = likeob != null;

                    var faveob = await _unitOfWork.Favorites.GetSingleAsyncByUser(advertisment.Id, u.Id);
                    advertismentVm.IsFavorit = faveob != null;

                    var comob = await _unitOfWork.Complaints.GetSingleAsyncByUser(advertisment.Id, u.Id);
                    advertismentVm.IsReport = comob != null;

                    if (advertisment.ApplicationUserId != u.Id)
                    {
                        advertisment.NumberOfViews = advertisment.NumberOfViews + 1;
                        var advertismentmodel = Mapper.Map<AdvertisementDto, Advertisment>(advertisment);
                        _unitOfWork.Advertisements.Edit(advertismentmodel);
                        await _unitOfWork.CommitAsync();
                        advertismentVm.NumberOfViews = advertisment.NumberOfViews.Value;
                        advertismentVm.IsOwner = false;
                    }
                    else
                    {
                        advertismentVm.IsOwner = true;
                    }
                }

                return Ok(advertismentVm);

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

        [HttpPost]
        [Route("GetAllFilterdAdmin")]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> GetAllFilterdAdmin(RouteViewModel route)
        {
            IHttpActionResult response = null;
            try
            {
                int currentPage = route.PageNumber ?? 1;
                int currentPageSize = route.PageSize ?? 10;
                int totalCount = 0;


                var alladvertisment = await _unitOfWork.Advertisements.GetAllAsync((currentPage - 1) * currentPageSize, currentPageSize, route.Filter ?? "", false, route.CategoryIdFilter, route.IsPaidedFilter, route.UserIdFilter,route.IsExpired);

                if (alladvertisment.Count > 0)
                {
                    totalCount = alladvertisment[0].OverAllCount;
                }
                else
                {
                    totalCount = 0;
                }

                PaginationSet<AdvertisementDto> pagedSet = new PaginationSet<AdvertisementDto>()
                {
                    Items = alladvertisment,
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



        [HttpDelete]
        [Route("Deleteads/{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> Deleteads(int id)
        {
            IHttpActionResult response;
            try
            {
                var advertisment = await (_unitOfWork.Advertisements.GetSingleAsync(id));

                if (advertisment == null)
                {
                    return NotFound();
                }

            
                //check if there is relations

                int result = await _unitOfWork.Advertisements.DeleteFromAllTables(advertisment.Id);

                await _unitOfWork.CommitAsync();
                response = Ok(result);
            }
            catch (Exception ex)
            {
                LogError(ex);
                response = InternalServerError(ex);
            }

            return response;
        }


        [HttpPost]
        [Route("SaveAds")]
        [Authorize(Roles = "Administrator")]
        //[Attributes.Authorize(Roles = "User")]
        public async Task<IHttpActionResult> SaveAdvertisment(AdvertismentViewModel model)
        {
            IHttpActionResult response;
            Advertisment advertisment;
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
                //model.ApplicationUserId = u.Id;

                if (model.Id == 0)
                {
                    advertisment = Mapper.Map<AdvertismentViewModel, Advertisment>(model);
                    _unitOfWork.Advertisements.Add(advertisment);
                    await _unitOfWork.CommitAsync();
                    await SaveAdsImages(model.ImagesList, advertisment.Id);
                }
                else
                {
                    advertisment = await (_unitOfWork.Advertisements.GetSingleAsync(model.Id));
                    if (advertisment == null || advertisment.IsArchieved)
                        return NotFound();
                    advertisment.Update(model);
                    _unitOfWork.Advertisements.Edit(advertisment);
                    await SaveAdsImages(model.ImagesList, advertisment.Id);

                }
                await _unitOfWork.CommitAsync();
                response = Ok(advertisment.Id);

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
        #endregion
    }
}