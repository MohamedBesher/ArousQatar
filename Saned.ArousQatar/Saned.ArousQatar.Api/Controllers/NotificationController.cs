using Microsoft.AspNet.Identity;
using Saned.HandByHand.Data.Core;
using Saned.HandByHand.Data.Core.Dtos;
using Saned.HandByHand.Data.Core.Models;
using Saned.HandByHand.Data.Persistence;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using Saned.HandByHand.Data.Persistence.Repositories;
using Saned.ArousQatar.Data.Persistence.Repositories;
using Saned.ArousQatar.Data.Core;
using Saned.ArousQatar.Data.Persistence;
using Saned.ArousQatar.Api.Controllers;
using Saned.ArousQatar.Api.Infrastructure.Core;
using Saned.ArousQatar.Api.Models;
using Saned.ArousQatar.Data.Core.Models;
using Saned.HandByHand.Data.Core.Repositories;

namespace Saned.ArousQatar.Api.Controllers
{
    [RoutePrefix("api/Notification")]
    public class NotificationController : ApiControllerBase
    {
        private new readonly IUnitOfWork _unitOfWork;
        private readonly AuthRepository _repo = null;
        private INotificationRepository INotificationRepository;
        private ApplicationDbContext _context;

        public NotificationController()
        {
            _unitOfWork = new UnitOfWork();
            _repo = new AuthRepository();
            _context = new ApplicationDbContext();
            INotificationRepository = new NotificationRepository(_context);

        }

        public NotificationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repo = new AuthRepository();

        }
        [Attributes.Authorize(Roles = "User")]
        [Route("GetAll")]
        [HttpPost]
        public async Task<IHttpActionResult> GetAll(PagingViewModel viewModel)
        {
            try
            {
                if (viewModel == null)
                    viewModel = new PagingViewModel();

                string userName = User.Identity.GetUserName();
                ApplicationUser u = await _repo.FindUserByUserName(userName);

                var items =
                    await
                        INotificationRepository.GetNotificationList("ar",viewModel.PageNumber, viewModel.PageSize, u.Id);

               
                return Ok(items);
            }
            catch (Exception ex)
            {
                LogError(ex);

                string msg = ex.GetaAllMessages();
                return BadRequest("GetAll --- " + msg);
            }


        }


        [Attributes.Authorize(Roles = "User")]
        [Route("MarkAsRead/{Id}")]
        [HttpPost]
        public async Task<IHttpActionResult> MarkAsRead(string id)
        {
            try
            {
                if (id == null)
                    return BadRequest("EmptyIds");
                var items =await INotificationRepository.MarkAsRead(id);
                return Ok(items);
            }
            catch (Exception ex)
            {
                LogError(ex);
                string msg = ex.GetaAllMessages();
                return BadRequest("GetAll --- " + msg);
            }


        }

    }

   
}