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
    [RoutePrefix("api/contactInformation")]
    public class ContactInformationController : ApiControllerBase
    {
        public ContactInformationController(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {

        }

        public ContactInformationController()
        {

        }
        // GET: /api/contactInformation/search/1/4?filter=??
        [HttpPost]
        [Route("all")]
        public async Task<IHttpActionResult> GetAll(ViewModel route)
        {
            IHttpActionResult response = null;
            try
            {
                int totalCount = 0;


                var allcontactInformation = await _unitOfWork.ContactInformations.GetAllAsync(route.PageNumber, route.PageSize, route.Filter);
                totalCount = await _unitOfWork.ContactInformations.CountAsync();

                PaginationSet<ContactInformationDto> pagedSet = new PaginationSet<ContactInformationDto>()
                {
                    Items = allcontactInformation,
                    Page = route.PageNumber,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling((decimal)totalCount / route.PageSize)
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
        [Route("GetAll")]
        public async Task<IHttpActionResult> GetAllInfo(ViewModel route)
        {
            IHttpActionResult response = null;
            try
            {
                var allcontactInformation = await _unitOfWork.ContactInformations.GetAllAsync(route.PageNumber, route.PageSize, route.Filter);
                response = Ok(allcontactInformation);
            }
            catch (Exception ex)
            {
                LogError(ex);
                response = InternalServerError(ex);
            }

            return response;
        }



        // GET: /api/contactInformation/5
        //[ResponseType(typeof(ContactInformation))]
        [Route("{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSingle(int id)
        {
            try
            {
                var contactInformation = await (_unitOfWork.ContactInformations.GetSingleAsync(id));
                // ReSharper disable once PossibleInvalidOperationException
                if (contactInformation == null)
                    return NotFound();

                var contactInformationVm = Mapper.Map<ContactInformation, ContactInformationViewModel>(contactInformation);

                return Ok(contactInformationVm);

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



        // POST: /api/contactInformation/update/5?
        //[ResponseType(typeof(void))]
        [HttpPost]
        [Route("update")]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> UpdateContactInformation(ContactInformationViewModel model)
        {
            IHttpActionResult response;
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (model.Id == 0)
                {

                    #region Adding

                    ContactInformation contactInformation = new ContactInformation();
                    // the only thing left
                    contactInformation.Update(model);

                    //Don't Miss Here File Upload

                    _unitOfWork.ContactInformations.Add(contactInformation);

                    #endregion
                }
                else
                {
                    #region Update

                    var contactInformation = await (_unitOfWork.ContactInformations.GetSingleAsync(model.Id));

                    // ReSharper disable once PossibleInvalidOperationException
                    if (contactInformation == null)
                    {
                        return NotFound();
                    }

                    contactInformation.Update(model);

                    //Dont Miss Here File Upload

                    _unitOfWork.ContactInformations.Edit(contactInformation);

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

 


        #region Admin Area 
        [Route("GetSingleAdmin/{id:int}")]
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSingleAdmin(int id)
        {
            try
            {
                var contactInformation = await (_unitOfWork.ContactInformations.GetSingleAsync(id));
                // ReSharper disable once PossibleInvalidOperationException
                if (contactInformation == null)
                    return NotFound();

                var contactInformationVm = Mapper.Map<ContactInformation, ContactInformationViewModel>(contactInformation);

                return Ok(contactInformationVm);

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

        // DELETE: /api/contactInformation/delete/5
        //[ResponseType(typeof(ContactInformation))]
        [HttpDelete]
        [Route("delete/{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            IHttpActionResult response;

            try
            {
                var contactInformation = await (_unitOfWork.ContactInformations.GetSingleAsync(id));

                if (contactInformation == null)
                {
                    return NotFound();
                }


                _unitOfWork.ContactInformations.Delete(contactInformation);

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
        [Route("allAdmin")]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> allAdmin(ViewModel route)
        {
            IHttpActionResult response = null;
            try
            {
                int totalCount = 0;


                var allcontactInformation = await _unitOfWork.ContactInformations.GetAllAsync(route.PageNumber, route.PageSize, route.Filter);
                totalCount = await _unitOfWork.ContactInformations.CountAsync();

                PaginationSet<ContactInformationDto> pagedSet = new PaginationSet<ContactInformationDto>()
                {
                    Items = allcontactInformation,
                    Page = route.PageNumber,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling((decimal)totalCount / route.PageSize)
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