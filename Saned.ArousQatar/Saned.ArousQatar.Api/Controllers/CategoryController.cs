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

// ReSharper disable All

namespace Saned.ArousQatar.Api.Controllers
{

    [RoutePrefix("api/category")]
    public class CategoryController : ApiControllerBase
    {
        public CategoryController()
        {

        }

        public CategoryController(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
        }

        // /api/category/all
        [Route("GetAll")]
        [HttpPost]
        public async Task<IHttpActionResult> GetAll(RouteViewModel route)
        {
            IHttpActionResult response = null;
            try
            {
                int currentPage = route.PageNumber.Value;
                int currentPageSize = route.PageSize.Value;
                int totalCount = 0;

                var categories = await _unitOfWork.Categories.GetAllAsync((currentPage - 1) * currentPageSize, currentPageSize, route.Filter ?? "", false);
                totalCount = categories[0].OverAllCount;

                var categoriesVm = Mapper.Map<List<CategoryAllDto>, List<CategoryViewModel>>(categories);


                PaginationSet<CategoryViewModel> pagedSet = new PaginationSet<CategoryViewModel>()
                {
                    Items = categoriesVm,
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


        


        [Route("GetAllAdmin")]
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> GetAllAdmin(RouteViewModel route)
        {
            IHttpActionResult response = null;
            try
            {
                int currentPage = route.PageNumber.Value;
                int currentPageSize = route.PageSize.Value;
                int totalCount = 0;

                var categories = await _unitOfWork.Categories.GetAllAsync((currentPage - 1) * currentPageSize, currentPageSize, route.Filter ?? "", false);
                totalCount = categories[0].OverAllCount;

                var categoriesVm = Mapper.Map<List<CategoryAllDto>, List<CategoryViewModel>>(categories);


                PaginationSet<CategoryViewModel> pagedSet = new PaginationSet<CategoryViewModel>()
                {
                    Items = categoriesVm,
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

        [Route("GetCategories")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllPaging()
        {
            IHttpActionResult response = null;
            try
            {

                var categories = await _unitOfWork.Categories.GetAllNameAsync(false);

                // var categoriesVm = Mapper.Map<List<CategoryDto>, List<CategoryViewModel>>(categories);

                response = Ok(categories);
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


                var alladvertisment = await _unitOfWork.Categories.GetAllAsync((currentPage - 1) * currentPageSize, currentPageSize, route.Filter ?? "", true);
                totalCount = alladvertisment[0].OverAllCount;

                var alladvertismentVm = Mapper.Map<List<CategoryAllDto>, List<CategoryViewModel>>(alladvertisment);


                PaginationSet<CategoryViewModel> pagedSet = new PaginationSet<CategoryViewModel>()
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

        // api/category/id
        [Route("{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSingle(int id)
        {
            try
            {
                var category = await (_unitOfWork.Categories.GetSingleAsync(id));
                if (category == null || category.IsArchieved.Value)
                    return NotFound();

                var categoryVm = Mapper.Map<Category, CategoryViewModel>(category);

                return Ok(categoryVm);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                LogError(ex);
                return NotFound();
            }
            catch (DbUpdateException ex)
            {
                LogError(ex);
                return BadRequest(ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return InternalServerError(ex);
            }
        }

        // /api/category/add
        [HttpPost]
        [Route("add")]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> AddCategory(CategoryViewModel model)
        {
            IHttpActionResult response = null;
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                Category category = new Category { };


                if (!string.IsNullOrEmpty(model.ImageFilename) && !string.IsNullOrEmpty(model.ImageBase64))
                {
                    var randomImage = SaveImage(model.ImageFilename, model.ImageBase64);

                    model.ImageUrl = randomImage;
                }
                else
                {
                    return BadRequest();
                }

                /// the only thing left
                category.Modify(model.Name, model.IconName, model.ImageUrl);

                //Dont Miss Here File Upload

                _unitOfWork.Categories.Add(category);

                await _unitOfWork.CommitAsync();

                response = Ok(model);
            }
            catch (Exception ex)
            {
                LogError(ex);
                response = InternalServerError(ex);
            }
            return response;
        }

        //  /api/category/edit/id
        [HttpPost]
        [Route("edit/{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> UpdateCategory(CategoryViewModel model, int id)
        {
            IHttpActionResult response = null;
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != model.Id)
                return BadRequest();

            try
            {
                var category = await (_unitOfWork.Categories.GetSingleAsync(id));

                if (category == null || category.IsArchieved.Value)
                {
                    return NotFound();
                }

                if (!string.IsNullOrEmpty(model.ImageFilename) && !string.IsNullOrEmpty(model.ImageBase64))
                {
                    var randomImage = SaveImage(model.ImageFilename, model.ImageBase64);

                    model.ImageUrl = randomImage;
                }
                else
                {
                    model.ImageUrl = category.ImageUrl;
                }

                category.Modify(model.Name, model.IconName, model.ImageUrl);

                //Dont Miss Here File Upload

                _unitOfWork.Categories.Edit(category);
                await _unitOfWork.CommitAsync();
                response = Ok();
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

        [HttpDelete]
        [Route("delete/{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            IHttpActionResult response = null;

            try
            {
                var category = await (_unitOfWork.Categories.GetSingleAsync(id));

                if (category == null)
                {
                    return NotFound();
                }

                //check if there is relations
                if (await _unitOfWork.Categories.IsRelatedWithAdvertisements(id))
                    return Ok(new { isDeleted = false, errorMessage = "هذاالصنف مرتبط باعلانات" });

                _unitOfWork.Categories.Delete(category);

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

        [HttpPost]
        [Route("archive/{id:int}")]
        public async Task<IHttpActionResult> Archive(int id)
        {
            IHttpActionResult response = null;

            try
            {
                var category = await (_unitOfWork.Categories.GetSingleAsync(id));

                if (category == null)
                {
                    return NotFound();
                }

                if (category.IsArchieved.Value)
                    return BadRequest("التصنيف مأرشف بالفعل");

                //check if there is relations

                category.IsArchieved = true;
                _unitOfWork.Categories.Edit(category);
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

        [Route("GetHomeCategory")]
        [HttpPost]
        public async Task<IHttpActionResult> GetAllFiltered(PagingViewModel viewModel)
        {
            IHttpActionResult response;
            try
            {
                int currentPage = viewModel.PageNumber;
                int? currentPageSize = viewModel.PageSize;

                var allcategory = await _unitOfWork.Categories.GetPaginAsync(currentPage, currentPageSize, viewModel.Filter, false);
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
    }
}
