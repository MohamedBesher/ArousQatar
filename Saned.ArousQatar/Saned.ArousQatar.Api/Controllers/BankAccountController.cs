using AutoMapper;
using Saned.ArousQatar.Api.Infrastructure.Core;
using Saned.ArousQatar.Api.Infrastructure.Extensions;
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
    [RoutePrefix("api/bankAccount")]
    public class BankAccountController : ApiControllerBase
    {
        public BankAccountController(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {

        }

        public BankAccountController()
        {

        }

        // GET: /api/bankAccount/search/1/4?filter=??
        [Route("all")]
        [HttpPost]
        public async Task<IHttpActionResult> GetAll(RouteViewModel route)
        {
            IHttpActionResult response = null;
            try
            {
                int currentPage = route.PageNumber.Value;
                int currentPageSize = route.PageSize.Value;
                int totalCount = 0;


                var allbankAccount = await _unitOfWork.BankAccounts.GetAllAsync((currentPage - 1) * currentPageSize, currentPageSize, route.Filter ?? "");
                totalCount = await _unitOfWork.BankAccounts.CountAsync();

                var allbankAccountVm = Mapper.Map<List<BankAccount>, List<BankAccountViewModel>>(allbankAccount);


                PaginationSet<BankAccountViewModel> pagedSet = new PaginationSet<BankAccountViewModel>()
                {
                    Items = allbankAccountVm,
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

        // GET: /api/bankAccount/5
        //[ResponseType(typeof(BankAccount))]
        [Route("{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSingle(int id)
        {
            try
            {
                var bankAccount = await (_unitOfWork.BankAccounts.GetSingleAsync(id));
                // ReSharper disable once PossibleInvalidOperationException
                if (bankAccount == null)
                    return NotFound();

                var bankAccountVm = Mapper.Map<BankAccount, BankAccountViewModel>(bankAccount);

                return Ok(bankAccountVm);

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



        // POST: /api/bankAccount/update/5?
        //[ResponseType(typeof(void))]
        [HttpPost]
        [Route("update/{id:int}")]
        public async Task<IHttpActionResult> UpdateBankAccount(BankAccountViewModel model)
        {
            IHttpActionResult response;
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (model.Id == 0)
                {

                    #region Adding

                    BankAccount bankAccount = new BankAccount();
                    // the only thing left
                    bankAccount.Update(model);

                    //Don't Miss Here File Upload

                    _unitOfWork.BankAccounts.Add(bankAccount);

                    #endregion
                }
                else
                {
                    #region Update

                    var bankAccount = await (_unitOfWork.BankAccounts.GetSingleAsync(model.Id));

                    // ReSharper disable once PossibleInvalidOperationException
                    if (bankAccount == null)
                    {
                        return NotFound();
                    }

                    bankAccount.Update(model);

                    //Dont Miss Here File Upload

                    _unitOfWork.BankAccounts.Edit(bankAccount);

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

        // DELETE: /api/bankAccount/delete/5
        //[ResponseType(typeof(BankAccount))]
        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            IHttpActionResult response;

            try
            {
                var bankAccount = await (_unitOfWork.BankAccounts.GetSingleAsync(id));

                if (bankAccount == null)
                {
                    return NotFound();
                }

                //check if there is relations

                _unitOfWork.BankAccounts.Delete(bankAccount);

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