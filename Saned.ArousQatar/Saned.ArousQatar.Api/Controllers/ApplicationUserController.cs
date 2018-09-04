using Microsoft.AspNet.Identity;
using Saned.ArousQatar.Api.Models;
using Saned.ArousQatar.Data.Core;
using Saned.ArousQatar.Data.Core.Models;
using Saned.ArousQatar.Data.Persistence;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Saned.ArousQatar.Api.Controllers
{
    [RoutePrefix("api/User")]
    public class ApplicationUserController : BasicController
    {
        private readonly IUnitOfWork _unitOfWork;
        public ApplicationUserController()
        {
            _unitOfWork = new UnitOfWork();
        }

        public ApplicationUserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Attributes
        [Attributes.Authorize(Roles = "User")]
        [Route("ChangeImage")]
        [HttpPost]
        #endregion
        public async Task<IHttpActionResult> ChangeImage(UserProfileViewModel viewProfile)
        {
            string userName = User.Identity.GetUserName();
            ApplicationUser u = await GetApplicationUser(userName);
            if (u == null)
            {
                ModelState.AddModelError("", "You Need To Login");
                return BadRequest(ModelState);
            }
            int result = await _unitOfWork.User.ChangeImage(u.Id, viewProfile.Picture);
            return Ok(result);
        }

        #region Attributes
        [Attributes.Authorize(Roles = "User")]
        [Route("ChangeInfo")]
        [HttpPost]
        #endregion
        public async Task<IHttpActionResult> ChangeInfo([FromBody] UserProfileViewModel viewProfile)
        {
            try
            {
                if (viewProfile == null)
                {

                    ModelState.AddModelError("", "No Paramter Sent");
                    return BadRequest(ModelState);
                }
                string userName = User.Identity.GetUserName();
                ApplicationUser u = await GetApplicationUser(userName);
                ;
                if (u == null)
                {
                    ModelState.AddModelError("", "You Need To Login");
                    return BadRequest(ModelState);
                }

                if (viewProfile.PhoneNumber != null && _unitOfWork.User.CheckifPhoneAvailable(viewProfile.PhoneNumber))
                {
                    ModelState.AddModelError("PhoneNumber", "Phone Number already Exists");
                    return BadRequest(ModelState);

                }

                int result = await _unitOfWork.User.UpdateInfo(u.Id, viewProfile.Name, viewProfile.PhoneNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                if (viewProfile != null)
                {
                    ModelState.AddModelError("Name", viewProfile.Name);

                }
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

        }



        #region Attributes
        [Attributes.Authorize(Roles = "User")]
        [Route("ChangeInfoEmail")]
        [HttpPost]
        #endregion
        public async Task<IHttpActionResult> ChangeInfo([FromBody] ClientViewModel viewProfile)
        {
            try
            {
                if (viewProfile == null)
                {

                    ModelState.AddModelError("", "No Paramter Sent");
                    return BadRequest(ModelState);
                }
                string userName = User.Identity.GetUserName();
                ApplicationUser u = await GetApplicationUser(userName); ;
                if (u == null)
                {
                    ModelState.AddModelError("", "You Need To Login");
                    return BadRequest(ModelState);
                }
                int result = await _unitOfWork.User.UpdateInfo(u.Id, viewProfile.Name, viewProfile.PhoneNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                if (viewProfile != null)
                {
                    ModelState.AddModelError("Name", viewProfile.Name);

                }
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

        }


        #region Attributes

        [Attributes.Authorize(Roles = "User")]
        [Route("GetUserInfo")]
        [HttpPost]
        #endregion
        public async Task<IHttpActionResult> GetUserInfo()
        {
            string userName = User.Identity.GetUserName();
            return Ok(await _unitOfWork.User.View(userName));
        }

        [Attributes.Authorize(Roles = "Administrator")]
        [Route("GetDetails/{id}")]

        public async Task<IHttpActionResult> GetUserInfo(string id)
        {

            return Ok(await _unitOfWork.User.ViewDetails(id));
        }


        [Attributes.Authorize(Roles = "User")]
        [Route("ChangeStatus/{status}")]
        [HttpPost]
        public async Task<IHttpActionResult> ChangeStatus(bool status)
        {
            try
            {
                string userName = User.Identity.GetUserName();
                ApplicationUser u = await GetApplicationUser(userName); ;
                var user = await _unitOfWork.User.GetUserbyId(u.Id);
                user.Status = status;
                await _unitOfWork.CommitAsync();
                return Ok();
            }
            catch (Exception ex)
            {

                string msg = ex.GetaAllMessages();
                return BadRequest("ChangeStatus --- " + msg);
            }

        }


    }
}