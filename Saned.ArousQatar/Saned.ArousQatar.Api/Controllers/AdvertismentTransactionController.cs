using AutoMapper;
using Saned.ArousQatar.Api.Infrastructure.Core;
using Saned.ArousQatar.Api.Infrastructure.Extensions;
using Saned.ArousQatar.Api.Models;
using Saned.ArousQatar.Data.Core;
using Saned.ArousQatar.Data.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using PayPal.Api;
using Saned.ArousQatar.Api.Utilities;
using Saned.ArousQatar.Api.Validators;

namespace Saned.ArousQatar.Api.Controllers
{
    [RoutePrefix("api/advertismentTransaction")]
    public class AdvertismentTransactionController : ApiControllerBase
    {
        public AdvertismentTransactionController(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {

        }


        public AdvertismentTransactionController()
        {
            
        }
        [HttpPost]
        [Route("save")]
        [Attributes.Authorize(Roles = "User")]

        public async Task<IHttpActionResult> SaveTransaction(AdvertismentTransactionViewModel model)
        {
            IHttpActionResult response;
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                
               

            var advertisment = await (_unitOfWork.Advertisements.GetSingleAsync(model.AdvertismentId));
                if (advertisment == null || !advertisment.IsExpired)
                    return NotFound();



                //try
                //{
                //   Payment  data =GetpayMentDetails(model.PaymentId);

                //    return BadRequest(data.state);
                //    //if (!data)
                //    //{
                //    //    ModelState.AddModelError("", "Invalid Payment Id-" + model.PaymentId + "-A7A -" + GetpayMentDetails(model.PaymentId));
                //    //    return BadRequest(ModelState);
                //    //}
                //}
                //catch (Exception e)
                //{
                //    return BadRequest(e.Message);
                //}
                if (!GetpayMentDetails(model.PaymentId))
                {
                    ModelState.AddModelError("", "Invalid Payment Id-" + model.PaymentId);
                    return BadRequest(ModelState);
                }


                string userName = User.Identity.GetUserName();
                ApplicationUser u = await GetApplicationUser(userName);
                if (u.Id != advertisment.ApplicationUserId)
                {
                    return Unauthorized();
                }

                #region AddTransaction

                AdvertismentTransaction transaction = new AdvertismentTransaction()
                {
                    AdvertismentId = model.AdvertismentId,
                    PaymentId = model.PaymentId
                };
                    _unitOfWork.AdvertisementTransactions.Add(transaction);            
                    await _unitOfWork.CommitAsync();

                #endregion

                #region UpdateAdvertisment

                if (transaction.Id != 0)
                {

                    int days;
                    AdvertismentPrice price = (await _unitOfWork.AdvertisementPrice.GetSingle(advertisment.AdvertismentPriceId));
                    if (int.TryParse(price.Period, out days))
                    {
                        advertisment.SetExpired(false, DateTime.Now, DateTime.Now.AddDays(days));
                       _unitOfWork.Advertisements.Edit(advertisment);

                    }

                    await _unitOfWork.CommitAsync();
                }

                #endregion
                response = Ok(transaction.Id);
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



        public bool GetpayMentDetails(string paymentId)
        {

            // ### Api Context
            // Pass in a `APIContext` object to authenticate 
            // the call and to send a unique request id 
            // (that ensures idempotency). The SDK generates
            // a request id if you do not pass one explicitly. 
            // See [Configuration.cs](/Source/Configuration.html) to know more about APIContext.
            var apiContext = Saned.ArousQatar.Api.Models.Configuration.GetAPIContext();

            // Specify a Payment ID to retrieve.  For demonstration purposes, we'll be using a previously-executed payment that used a PayPal account.

            if (string.IsNullOrEmpty(paymentId))
                return false;

            // ^ Ignore workflow code segment
            // Retrieve the details of the payment.
            var payment = Payment.Get(apiContext, paymentId);

          

            if (payment != null && payment.state == "approved")
            {
                return true;
            }
            else
                return false;

        }

    }
}