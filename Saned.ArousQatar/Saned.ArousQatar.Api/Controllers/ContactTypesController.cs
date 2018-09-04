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

namespace Saned.ArousQatar.Api.Controllers
{
    [RoutePrefix("api/contactType")]
    public class ContactTypeController : ApiControllerBase
    {
        public ContactTypeController(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {

        }

        public ContactTypeController()
        {

        }
        [HttpPost]
        [Route("all")]
        public async Task<IHttpActionResult> GetAll(RouteViewModel route)
        {
            IHttpActionResult response = null;
            try
            {
                int currentPage = route.PageNumber.Value;
                int currentPageSize = route.PageSize.Value;
                int totalCount = 0;


                var allcontactType = await _unitOfWork.ContactTypes.GetAllAsync((currentPage - 1) * currentPageSize, currentPageSize);
                totalCount = await _unitOfWork.ContactTypes.CountAsync();

                var allcontactTypeVm = Mapper.Map<List<ContactType>, List<ContactTypeViewModel>>(allcontactType);


                PaginationSet<ContactTypeViewModel> pagedSet = new PaginationSet<ContactTypeViewModel>()
                {
                    Items = allcontactTypeVm,
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

        

        [HttpGet]
        [Route("GetAll")]
        public async Task<IHttpActionResult> GetAllTypes()
        {
            IHttpActionResult response = null;
            try
            {
              
                var allcontactType = await _unitOfWork.ContactTypes.GetAllAsync();

                response = Ok(allcontactType);
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

        // GET: /api/contactType/5
        //[ResponseType(typeof(ContactType))]
        [Route("{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSingle(int id)
        {
            try
            {
                var contactType = await (_unitOfWork.ContactTypes.GetSingleAsync(id));
                // ReSharper disable once PossibleInvalidOperationException
                if (contactType == null)
                    return NotFound();

                var contactTypeVm = Mapper.Map<ContactType, ContactTypeViewModel>(contactType);

                return Ok(contactTypeVm);

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



        // POST: /api/contactType/update/5?
        //[ResponseType(typeof(void))]
        [HttpPost]
        [Route("update")]
        public async Task<IHttpActionResult> UpdateContactType(ContactTypeViewModel model)
        {
            IHttpActionResult response;
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (model.Id == 0)
                {

                    #region Adding

                    ContactType contactType = new ContactType();
                    // the only thing left
                    contactType.Modify(model.Type);

                    //Don't Miss Here File Upload

                    _unitOfWork.ContactTypes.Add(contactType);

                    #endregion
                }
                else
                {
                    #region Update

                    var contactType = await (_unitOfWork.ContactTypes.GetSingleAsync(model.Id));

                    // ReSharper disable once PossibleInvalidOperationException
                    if (contactType == null)
                    {
                        return NotFound();
                    }

                    contactType.Modify(model.Type);

                    //Dont Miss Here File Upload

                    _unitOfWork.ContactTypes.Edit(contactType);

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

        // DELETE: /api/contactType/delete/5
        //[ResponseType(typeof(ContactType))]
        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            IHttpActionResult response;

            try
            {
                var contactType = await (_unitOfWork.ContactTypes.GetSingleAsync(id));

                if (contactType == null)
                {
                    return NotFound();
                }

                //check if there is relations
                if (await _unitOfWork.ContactTypes.IsRelatedToContactInformation(id))
                    return BadRequest("هذا النوع مرتبط بوسائل اتصال");

                _unitOfWork.ContactTypes.Delete(contactType);

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