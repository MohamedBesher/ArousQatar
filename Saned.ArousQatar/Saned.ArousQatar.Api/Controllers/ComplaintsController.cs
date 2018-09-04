using AutoMapper;
using Microsoft.AspNet.Identity;
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
    [RoutePrefix("api/complaint")]
    public class ComplaintController : ApiControllerBase
    {
        public ComplaintController(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {

        }

        public ComplaintController()
        {

        }
        // GET: /api/complaint/search/1/4?filter=??
        [HttpPost]
        [Route("all/users")]
        public async Task<IHttpActionResult> GetAllComplaintsUsers(RouteViewModel route)
        {
            IHttpActionResult response = null;
            try
            {
                int currentPage = route.PageNumber.Value;
                int currentPageSize = route.PageSize.Value;
                int totalCount = 0;


                var allcomplaint = await _unitOfWork.Complaints.GetAllUsersComplaintsAsync((currentPage - 1) * currentPageSize, currentPageSize);
                totalCount = await _unitOfWork.Complaints.CountAsync();

                var allcomplaintVm = Mapper.Map<List<ComplaintDto>, List<ComplaintViewModel>>(allcomplaint);


                PaginationSet<ComplaintViewModel> pagedSet = new PaginationSet<ComplaintViewModel>()
                {
                    Items = allcomplaintVm,
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
        [Route("all/advertisement")]
        public async Task<IHttpActionResult> GetAllComplaintsAdvertisement(RouteViewModel route)
        {
            IHttpActionResult response = null;
            try
            {
                int currentPage = route.PageNumber.Value;
                int currentPageSize = route.PageSize.Value;
                int totalCount = 0;


                var allcomplaint = await _unitOfWork.Complaints.GetAllAdvertisementComplaintsAsync((currentPage - 1) * currentPageSize, currentPageSize, route.Filter);
                totalCount = await _unitOfWork.Complaints.CountAsync();

                // var allcomplaintVm = Mapper.Map<List<ComplaintDto>, List<ComplaintViewModel>>(allcomplaint);


                PaginationSet<ComplaintDto> pagedSet = new PaginationSet<ComplaintDto>()
                {
                    Items = allcomplaint,
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
        [Route("GetAllUserComplaints")]
        public async Task<IHttpActionResult> GetAllUserComplaints(RouteViewModel route)
        {
            IHttpActionResult response = null;
            try
            {
                int currentPage = route.PageNumber.Value;
                int currentPageSize = route.PageSize.Value;
                var allcomplaint = await _unitOfWork.Complaints.GetAllUsersComplaintsAsync((currentPage - 1) * currentPageSize, currentPageSize);
                response = Ok(allcomplaint);
            }
            catch (Exception ex)
            {
                LogError(ex);
                response = InternalServerError(ex);
            }

            return response;
        }

        [HttpPost]
        [Route("GetAllAdsComplaints")]
        public async Task<IHttpActionResult> GetAllAdvertisementComplaints(RouteViewModel route)
        {
            IHttpActionResult response = null;
            try
            {
                int currentPage = route.PageNumber.Value;
                int currentPageSize = route.PageSize.Value;
                var allcomplaint = await _unitOfWork.Complaints.GetAllAdvertisementComplaintsAsync((currentPage - 1) * currentPageSize, currentPageSize, route.Filter);
                response = Ok(allcomplaint);
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
        [Route("archieve/all/advertisement")]
        public async Task<IHttpActionResult> GetAllComplaintsAdvertisementArchieve(RouteViewModel route)
        {
            IHttpActionResult response = null;
            try
            {
                int currentPage = route.PageNumber.Value;
                int currentPageSize = route.PageSize.Value;
                int totalCount = 0;


                var allcomplaint = await _unitOfWork.Complaints.GetAllAdvertisementComplaintsArchieveAsync((currentPage - 1) * currentPageSize, currentPageSize);
                totalCount = await _unitOfWork.Complaints.CountAsync();

                var allcomplaintVm = Mapper.Map<List<ComplaintDto>, List<ComplaintViewModel>>(allcomplaint);


                PaginationSet<ComplaintViewModel> pagedSet = new PaginationSet<ComplaintViewModel>()
                {
                    Items = allcomplaintVm,
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
        [Route("archieve/all/advertisement")]
        public async Task<IHttpActionResult> GetAllComplaintsUserArchieve(RouteViewModel route)
        {
            IHttpActionResult response = null;
            try
            {
                int currentPage = route.PageNumber.Value;
                int currentPageSize = route.PageSize.Value;
                int totalCount = 0;


                var allcomplaint = await _unitOfWork.Complaints.GetAllUsersComplaintsArchieveAsync((currentPage - 1) * currentPageSize, currentPageSize);
                totalCount = await _unitOfWork.Complaints.CountAsync();

                var allcomplaintVm = Mapper.Map<List<ComplaintDto>, List<ComplaintViewModel>>(allcomplaint);


                PaginationSet<ComplaintViewModel> pagedSet = new PaginationSet<ComplaintViewModel>()
                {
                    Items = allcomplaintVm,
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


        // GET: /api/complaint/5
        //[ResponseType(typeof(Complaint))]
        [Route("{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSingle(int id)
        {
            try
            {
                var complaint = await (_unitOfWork.Complaints.GetSingleAsync(id));
                // ReSharper disable once PossibleInvalidOperationException
                if (complaint == null || complaint.IsArchieved.Value)
                    return NotFound();

                var complaintVm = Mapper.Map<Complaint, ComplaintViewModel>(complaint);

                return Ok(complaintVm);

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



        // POST: /api/complaint/save
        [HttpPost]
        [Route("save")]
        [Authorize]
        [Attributes.Authorize(Roles = "User")]
        public async Task<IHttpActionResult> UpdateComplaint(ComplaintViewModel model)
        {
            IHttpActionResult response;
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                string userName = User.Identity.GetUserName();
                ApplicationUser u = await GetApplicationUser(userName);
                model.ApplicationUserId = u.Id;
                if (model.Id == 0)
                {
                    var complaint = Mapper.Map<ComplaintViewModel, Complaint>(model);
                    _unitOfWork.Complaints.Add(complaint);
                    await _unitOfWork.CommitAsync();
                    response = Ok(complaint.Id);
                }
                else
                {
                    var complaint = await (_unitOfWork.Complaints.GetSingleAsync(model.Id));
                    if (complaint == null || complaint.IsArchieved.Value)
                        return NotFound();
                    complaint.Update(model);
                    _unitOfWork.Complaints.Edit(complaint);
                    await _unitOfWork.CommitAsync();
                    response = Ok(complaint.Id);
                }
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

        // DELETE: /api/complaint/delete/5
        //[ResponseType(typeof(Complaint))]
        [HttpDelete]
        [Route("delete/{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            IHttpActionResult response;

            try
            {
                var complaint = await (_unitOfWork.Complaints.GetSingleAsync(id));

                if (complaint == null)
                {
                    return NotFound();
                }

                //check if there is relations

                _unitOfWork.Complaints.Delete(complaint);

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

        // Archive: /api/complaint/archive/5
        //[ResponseType(typeof(Complaint))]
        [HttpPost]
        [Route("archive/{id:int}")]
        public async Task<IHttpActionResult> Archive(int id)
        {
            IHttpActionResult response;

            try
            {
                var complaint = await (_unitOfWork.Complaints.GetSingleAsync(id));

                if (complaint == null)
                {
                    return NotFound();
                }

                if (complaint.IsArchieved != null && complaint.IsArchieved.Value)
                    return BadRequest("الشكوى مأرشف بالفعل");

                //check if there is relations

                complaint.Archieve();

                _unitOfWork.Complaints.Edit(complaint);
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


        // DeArchive: /api/complaint/dearchive/5
        //[ResponseType(typeof(Complaint))]
        [HttpPost]
        [Route("dearchive/{id:int}")]
        public async Task<IHttpActionResult> DeArchive(int id)
        {
            IHttpActionResult response;

            try
            {
                var complaint = await (_unitOfWork.Complaints.GetSingleAsync(id));

                if (complaint == null)
                {
                    return NotFound();
                }

                if (complaint.IsArchieved != null && !complaint.IsArchieved.Value)
                    return BadRequest("الشكوى غير مأرشفه");

                //check if there is relations

                complaint.IsArchieved = false;

                _unitOfWork.Complaints.Edit(complaint);

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


        #region admin 
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [Route("all/advertisement/admin")]
        public async Task<IHttpActionResult> GetAllComplaintsAdvertisementAdmin(RouteViewModel route)
        {
            IHttpActionResult response = null;
            try
            {
                int currentPage = route.PageNumber.Value;
                int currentPageSize = route.PageSize.Value;
                int totalCount = 0;


                var allcomplaint = await _unitOfWork.Complaints.GetAllAdvertisementComplaintsAsync((currentPage - 1) * currentPageSize, currentPageSize , route.Filter);
                totalCount = await _unitOfWork.Complaints.CountAsync();

                // var allcomplaintVm = Mapper.Map<List<ComplaintDto>, List<ComplaintViewModel>>(allcomplaint);


                PaginationSet<ComplaintDto> pagedSet = new PaginationSet<ComplaintDto>()
                {
                    Items = allcomplaint,
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

        #endregion 
    }
}