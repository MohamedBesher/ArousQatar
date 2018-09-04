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

    [RoutePrefix("api/contactUsMessage")]
    public class ContactUsMessageController : ApiControllerBase
    {
        public ContactUsMessageController()
        {

        }

        public ContactUsMessageController(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
        }

        [Route("GetAll")]
        [HttpPost]
        public async Task<IHttpActionResult> GetAll(ViewModel model)
       {
            IHttpActionResult response = null;
            try
            {
                int currentPage = model.PageNumber;
                int currentPageSize = model.PageSize;
                int totalCount = 0;
                var contactUsMessages = await _unitOfWork.ContactUsMessage.GetAllAsync(model.PageNumber, model.PageSize, model.Filter, model.isArchieve);
                if (contactUsMessages.Count > 0)
                    totalCount=contactUsMessages[0].OverAllCount;

                PaginationSet<ContactUsMessageDto> pagedSet = new PaginationSet<ContactUsMessageDto>()
                {
                    Items = contactUsMessages,
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
        [Route("archive/{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> Archive(int id)
        {
            IHttpActionResult response;

            try
            {
                var contactus = await (_unitOfWork.ContactUsMessage.GetSingleAsync(id));

                if (contactus == null)
                {
                    return NotFound();
                }

                if (contactus.IsArchieved)
                    return BadRequest("الرسالة مأرشف بالفعل");

                //check if there is relations

                contactus.Archieve();

                _unitOfWork.ContactUsMessage.Edit(contactus);
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


        [HttpDelete]
        [Route("delete/{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            IHttpActionResult response;

            try
            {
                var contactUsMessage = await (_unitOfWork.ContactUsMessage.GetSingleAsync(id));

                if (contactUsMessage == null)
                {
                    return NotFound();
                }

                //check if there is relations

                _unitOfWork.ContactUsMessage.Delete(contactUsMessage);

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
        [Route("add")]
        public async Task<IHttpActionResult> Add(ContactUsMessageViewModel model)
        {
            IHttpActionResult response = null;
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                ContactUsMessage contactus = new ContactUsMessage { };
                contactus.Modify(model.Name,model.Phone, model.Email, model.Message);
                _unitOfWork.ContactUsMessage.Add(contactus);
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



        
    }
}
