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
using Saned.ArousQatar.Api.Validators;

namespace Saned.ArousQatar.Api.Controllers
{
    [RoutePrefix("api/advertismentImage")]
    public class AdvertismentImageController : ApiControllerBase
    {
        public AdvertismentImageController(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {

        }

        // /api/advertismentImage/id
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetAll(int id)
        {
            IHttpActionResult response;
            try
            {
                var alladvertismentImage = await _unitOfWork.AdvertisementImages.GetAllAsync(id);
                var alladvertismentImageVm = Mapper.Map<List<AdvertismentImage>, List<AdvertisementImageViewModel>>(alladvertismentImage);

                response = Ok(alladvertismentImageVm);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                LogError(ex);
                response = NotFound();
            }
            catch (DbUpdateException ex)
            {
                LogError(ex);
                response = BadRequest(ex.InnerException?.Message);
            }
            catch (Exception ex)
            {
                LogError(ex);
                response = InternalServerError(ex);
            }

            return response;
        }
        

        // GET: /api/advertismentImage/5
        //[ResponseType(typeof(AdvertismentImage))]
        [Route("{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSingle(int id)
        {
            try
            {
                var advertismentImage = await (_unitOfWork.AdvertisementImages.GetSingleAsync(id));
                // ReSharper disable once PossibleInvalidOperationException
                if (advertismentImage == null)
                    return NotFound();

                var advertismentImageVm = Mapper.Map<AdvertismentImage, AdvertisementImageViewModel>(advertismentImage);

                return Ok(advertismentImageVm);

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

        [MimeMultipart]
        [Route("image/upload")]
        public IHttpActionResult Post(HttpRequestMessage request, AdvertisementImageViewModel model)
        {
            var adv = _unitOfWork.Advertisements.GetSingleAsync(model.AdvertismentId);

            if (adv == null)
                return NotFound();

            else
            {
                var uploadPath = HttpContext.Current.Server.MapPath("~/Uploads/Images/Advertisements");

                //Read the MIME multipart asynchronously
                var multipartFormDataStreamProvider = new UploadMultipartFormProvider(uploadPath);

                Request.Content.ReadAsMultipartAsync(multipartFormDataStreamProvider);

                string _localFileName =
                        multipartFormDataStreamProvider.FileData.Select(multiPartData => multiPartData.LocalFileName)
                            .FirstOrDefault();
                
                //Create response 
                FileUploadResult fileUploadResult = new FileUploadResult
                {
                    LocalFilePath = _localFileName,
                    FileLength = new FileInfo(_localFileName).Length,
                    FileName = Path.GetFileName(_localFileName)
                };

                // update the database
                var advImage = new AdvertismentImage()
                {
                    AdvertismentId = model.AdvertismentId,
                    ImageUrl = fileUploadResult.FileName
                };

                if (advImage.IsMainImage != null && advImage.IsMainImage.Value)
                {
                    _unitOfWork.AdvertisementImages.ResetMainImage(model.AdvertismentId);
                }

                _unitOfWork.AdvertisementImages.AddAsync(advImage);
                _unitOfWork.Commit();

                return Ok(fileUploadResult);
            }
        }


        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            IHttpActionResult response;

            try
            {
                var advertismentImage = await (_unitOfWork.AdvertisementImages.GetSingleAsync(id));

                if (advertismentImage == null)
                {
                    return NotFound();
                }
                //check if there is relations

                _unitOfWork.AdvertisementImages.Delete(advertismentImage);

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
        [Route("save")]
        public async Task<IHttpActionResult> SaveImage(AdvertisementImageViewModel model)
        {
            IHttpActionResult response;
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (model.Id == 0)
                {
                    AdvertismentImage AdImage = new AdvertismentImage();
                    AdImage.Update(model);
                   _unitOfWork.AdvertisementImages.Add(AdImage);
                }
                else
                {
                    var image = await (_unitOfWork.AdvertisementImages.GetSingleAsync(model.Id));
                    if (image == null)
                        return NotFound();                  
                    image.Update(model);
                    _unitOfWork.AdvertisementImages.Edit(image);
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

        [HttpPost]
        [Route("SaveAdImageList")]
        public async Task<IHttpActionResult> SaveListOfImages(List<AdvertisementImageViewModel> model)
        {
            IHttpActionResult response;
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {

                foreach (AdvertisementImageViewModel img in model)
                {
                    var bo = Mapper.Map<AdvertisementImageViewModel, AdvertismentImage>(img);
                    _unitOfWork.AdvertisementImages.Add(bo);
                    await _unitOfWork.CommitAsync();
                }
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
    }
}