using AutoMapper;
using Microsoft.AspNet.Identity;
using Saned.ArousQatar.Api.Infrastructure.Core;
using Saned.ArousQatar.Api.Models;
using Saned.ArousQatar.Data.Core;
using Saned.ArousQatar.Data.Core.Dtos;
using Saned.ArousQatar.Data.Core.Models;
using Saned.ArousQatar.Data.Persistence;
using Saned.HandByHand.Data.Core.Models.Notifications;
using Saned.HandByHand.Data.Core.Repositories;
using Saned.HandByHand.Data.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using System.Web.Http;

namespace Saned.ArousQatar.Api.Controllers
{
    [RoutePrefix("api/chat")]
    // [Attributes.Authorize]
    public class ChatController : ApiControllerBase
    {
        //public ChatController(IUnitOfWork unitOfWork) : base(unitOfWork)
        //{

        //}

        //public ChatController()
        //{

        //}

        //  private readonly AuthRepository _repo = null;
        readonly new IUnitOfWork _unitOfWork;
        private INotificationRepository INotificationRepository;
        private ApplicationDbContext _context;

        public ChatController()
        {
            _context = new ApplicationDbContext();
            // _repo = new AuthRepository();
            _unitOfWork = new UnitOfWork(_context);
            INotificationRepository = new NotificationRepository(_context);
        }

        [Attributes.Authorize(Roles = "User")]
        [HttpPost]
        [Route("AddRequest")]
        public async Task<IHttpActionResult> AddRequest(RequestViewModel viewModel)
        {
            try
            {
                if (viewModel == null || !ModelState.IsValid)
                    return BadRequest(ModelState);
                string userName = User.Identity.GetUserName();
                ApplicationUser u = await GetApplicationUser(userName);
                viewModel.RequestAuthorId = u.Id;
                var bo = Mapper.Map<RequestViewModel, ChatRequest>(viewModel);
                var result = await _unitOfWork.ChatRequest.ChatRequestAdd(bo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Attributes.Authorize(Roles = "User")]
        [Route("SaveMessage")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveMessage(MessageViewModel viewModel)
        {
            try
            {
                if (ModelState != null && !ModelState.IsValid)
                    return BadRequest(ModelState);
                var bo = Mapper.Map<MessageViewModel, ChatMessage>(viewModel);
                string userName = User.Identity.GetUserName();
                ApplicationUser u = await GetApplicationUser(userName);
                bo.SenderId = u.Id;
              
                var result = await _unitOfWork.ChatRequest.AddMessage(bo);

                ChatHeader header = await _unitOfWork.ChatRequest.GetHeader(viewModel.ChatId);

                string reciever = header.UserTwoId != u.Id ? header.UserTwoId : header.UserOneId;

                if (String.IsNullOrEmpty(reciever))
                    return InternalServerError();

                INotificationRepository.AddNotificaiton(new PushNotification()
                {
                    ChatRequestId = header.RequestId,
                    EnglishMessage = viewModel.MessageContent,
                    Message = viewModel.MessageContent,
                    Notified = false
                },
             reciever, true);

                await _context.SaveChangesAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Attributes.Authorize(Roles = "User")]
        [Route("GetChatByAdvertisment")]
        [HttpPost]
        public async Task<IHttpActionResult> GetChatByAdvertisment(ChatViewModel viewModel)
        {
            try
            {
                string userName = User.Identity.GetUserName();
                ApplicationUser u = await GetApplicationUser(userName);
                var bo = await _unitOfWork.ChatRequest.GetChatByAdvertisment(viewModel.AdvertismentId, viewModel.PageNumber, viewModel.PageSize);
                return Ok(bo);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [Attributes.Authorize(Roles = "User")]
        [HttpPost]
        [Route("GetMessage")]
        public async Task<IHttpActionResult> GetMessage(MessagePagingViewModel viewModel)
        {
            return Ok(await _unitOfWork.ChatRequest.GetMessage(viewModel.ChatId, viewModel.PageNumber, viewModel.PageSize, viewModel.LastSentDate));
        }

        [Attributes.Authorize(Roles = "User")]
        [Route("GetChatByUser")]
        [HttpPost]
        public async Task<IHttpActionResult> GetChatByUser(ChatViewModel viewModel)
        {
            try
            {
                //string userName = User.Identity.GetUserName();
                //ApplicationUser u = await GetApplicationUser(userName);
                //var bo = await _unitOfWork.ChatRequest.GetChatByUserId(u.Id, viewModel.PageNumber, viewModel.PageSize);
                var bo = await _unitOfWork.ChatRequest.GetChatByUserId(viewModel.UserId, viewModel.AdvertismentId, viewModel.PageNumber, viewModel.PageSize);

                return Ok(bo);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [Attributes.Authorize(Roles = "User")]
        [Route("GetHeader/{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetHeader(int id)
        {

            try
            {
                var bo = await _unitOfWork.ChatRequest.GetHeader(id);
                var boRequest = await _unitOfWork.ChatRequest.GetRequest(bo.RequestId);
                var appUserOne = await GetApplicationUserById(bo.UserOneId);
                var appUserTwo = await GetApplicationUserById(bo.UserTwoId);
                ChatHeaderViewModel model = new ChatHeaderViewModel()
                {
                    Id = bo.Id,
                    CreateDate = bo.CreateDate,
                    RequestId = bo.RequestId,
                    UserOne = appUserOne.UserName,
                    UserTwo = appUserTwo.UserName,
                    UserTwoId = bo.UserTwoId,
                    UserOneId = bo.UserOneId,
                    RequestAuthorId = boRequest.RequestAuthorId,
                    AdvertismentId = boRequest.AdvertismentId,
                    PictureUserOne = appUserOne.PhotoUrl,
                    PictureUserTwo = appUserTwo.PhotoUrl,

                };

                return Ok(model);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }


        }

        [Attributes.Authorize(Roles = "User")]
        public async Task<IHttpActionResult> GetHeaderbyChatRequest(long id)
        {

            try
            {
                var bo = await _unitOfWork.ChatRequest.GetHeaderbyChatRequest(id);
                return Ok(bo.Id);

            }
            catch (Exception ex)
            {

                string msg = ex.GetaAllMessages();
                return BadRequest("GetHeader --- " + msg);
            }


        }
    }


}