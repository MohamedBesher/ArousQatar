using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Saned.ArousQatar.Api.Infrastructure.Core;
using Saned.ArousQatar.Api.Models;
using Saned.ArousQatar.Data.Core;
using Saned.ArousQatar.Data.Core.Models;

namespace Saned.ArousQatar.Api.Controllers {
	[RoutePrefix("/api/advertismentImage")]
	public class AdvertismentImageController : ApiControllerBase
	{
		public AdvertismentImageController( IUnitOfWork unitOfWork ) :
			base( unitOfWork ) {

		}

		// GET: /api/advertismentImage/search/1/4?filter=??
		[Route("search/{page:int=0}/{pageSize=4}/{filter?}")]
		public async Task<IHttpActionResult> GetAll(int? page, int? pageSize, string filter = "")
        {
            IHttpActionResult response = null;
            try
            {
                int currentPage = page.Value;
                int currentPageSize = pageSize.Value;
                int totalCount = 0;


                var alladvertismentImage = await _unitOfWork.AdvertismentImages.GetAllAsync((currentPage-1) * currentPageSize, currentPageSize, filter);
                totalCount = await _unitOfWork.AdvertismentImages.CountAsync();

                var alladvertismentImageVm = Mapper.Map<List<AdvertismentImage>, List<AdvertismentImageViewModel>>(alladvertismentImage);


                PaginationSet<AdvertismentImageViewModel> pagedSet = new PaginationSet<AdvertismentImageViewModel>()
                {
                    Items = alladvertismentImageVm,
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

		public async Task<IHttpActionResult> GetAll( ) {
			IHttpActionResult response;
			try {
				var alladvertismentImage = await _unitOfWork.AdvertismentImages.GetAllAsync( );
				var alladvertismentImageVm = Mapper.Map<List<AdvertismentImage> , List<AdvertismentImageViewModel>>( alladvertismentImage );

				response = Ok( alladvertismentImageVm );
			} catch ( DbUpdateConcurrencyException ex ) {
				LogError( ex );
				response = NotFound( );
			} catch ( DbUpdateException ex ) {
				LogError( ex );
				response = BadRequest( ex.InnerException?.Message );
			} catch ( Exception ex ) {
				LogError( ex );
				response = InternalServerError( ex );
			}

			return response;
		}
		


		// GET: /api/advertismentImage/5
		//[ResponseType(typeof(AdvertismentImage))]
		[Route( "{id:int}" )]
		[HttpGet]
		public async Task<IHttpActionResult> GetSingle( int id ) {
			try {
				var advertismentImage = await ( _unitOfWork.AdvertismentImages.GetSingleAsync( id ) );
				// ReSharper disable once PossibleInvalidOperationException
				if ( advertismentImage == null || advertismentImage.IsArchieved.Value )
					return NotFound( );

				var advertismentImageVm = Mapper.Map<AdvertismentImage , AdvertismentImageViewModel>( advertismentImage );

				return Ok( advertismentImageVm );

			} catch ( DbUpdateConcurrencyException ex ) {
				LogError( ex );
				return NotFound( );
			} catch ( DbUpdateException ex ) {
				LogError( ex );
				return BadRequest( ex.InnerException?.Message );
			} catch ( Exception ex ) {
				LogError( ex );
				return InternalServerError( ex );
			}


		}



		// POST: /api/advertismentImage/update/5?
		//[ResponseType(typeof(void))]
		[HttpPost]
		[Route( "update/{id:int}" )]
		public async Task<IHttpActionResult> UpdateAdvertismentImage( AdvertismentImageViewModel model ) {
			IHttpActionResult response;
			if ( !ModelState.IsValid )
				return BadRequest( ModelState );

			try {
				if ( model.Id == 0 ) {

					#region Adding

					AdvertismentImage advertismentImage = new AdvertismentImage();
					// the only thing left
					advertismentImage.Update( model );

					//Don't Miss Here File Upload

					_unitOfWork.AdvertismentImages.Add( advertismentImage );

					#endregion
				} else {
					#region Update

					var advertismentImage = await ( _unitOfWork.AdvertismentImages.GetSingleAsync( model.Id ) );

					// ReSharper disable once PossibleInvalidOperationException
					if ( advertismentImage == null || advertismentImage.IsArchieved.Value ) {
						return NotFound( );
					}

					advertismentImage.Update( model );

					//Dont Miss Here File Upload

					_unitOfWork.AdvertismentImages.Edit( advertismentImage );

					#endregion

				}
				await _unitOfWork.CommitAsync( );

				response = Ok( model );
			} catch ( DbUpdateConcurrencyException ex ) {
				LogError( ex );
				response = NotFound( );
			} catch ( Exception ex ) {
				LogError( ex );
				response = InternalServerError( ex );
			}
			return response;
		}

		// DELETE: /api/advertismentImage/delete/5
		//[ResponseType(typeof(AdvertismentImage))]
		[HttpPost]
		[Route( "delete/{id:int}" )]
		public async Task<IHttpActionResult> Delete( int id ) {
			IHttpActionResult response;

			try {
				var advertismentImage = await ( _unitOfWork.AdvertismentImages.GetSingleAsync( id ) );

				if ( advertismentImage == null ) {
					return NotFound( );
				}

				//check if there is relations

				_unitOfWork.AdvertismentImages.Delete( advertismentImage );

				await _unitOfWork.CommitAsync( );
				response = Ok( );
			} catch ( Exception ex ) {
				LogError( ex );
				response = InternalServerError( ex );
			}

			return response;
		}
		
		// Archive: /api/advertismentImage/archive/5
		//[ResponseType(typeof(AdvertismentImage))]
		[HttpPost]
		[Route( "archive/{id:int}" )]
		public async Task<IHttpActionResult> Archive( int id ) {
			IHttpActionResult response;

			try {
				var advertismentImage = await ( _unitOfWork.AdvertismentImages.GetSingleAsync( id ) );

				if ( advertismentImage == null ) {
					return NotFound( );
				}

				if ( advertismentImage.IsArchieved != null && advertismentImage.IsArchieved.Value )
					return BadRequest( "التصنيف مأرشف بالفعل" );

				//check if there is relations

				advertismentImage.IsArchieved = true;
				_unitOfWork.AdvertismentImages.Edit( advertismentImage );
				await _unitOfWork.CommitAsync( );
				response = Ok( );
			} catch ( Exception ex ) {
				LogError( ex );
				response = InternalServerError( ex );
			}

			return response;
		}

		
		// DeArchive: /api/advertismentImage/dearchive/5
		//[ResponseType(typeof(AdvertismentImage))]
		[HttpPost]
		[Route( "dearchive/{id:int}" )]
		public async Task<IHttpActionResult> DeArchive( int id ) {
			IHttpActionResult response;

			try {
				var advertismentImage = await ( _unitOfWork.AdvertismentImages.GetSingleAsync( id ) );

				if ( advertismentImage == null ) {
					return NotFound( );
				}

				if ( advertismentImage.IsArchieved != null && !advertismentImage.IsArchieved.Value )
					return BadRequest( "التصنيف غير مأرشف" );

				//check if there is relations
				
				advertismentImage.IsArchieved = false;
				_unitOfWork.AdvertismentImages.Edit( advertismentImage );
				await _unitOfWork.CommitAsync( );
				response = Ok( );
			} catch ( Exception ex ) {
				LogError( ex );
				response = InternalServerError( ex );
			}

			return response;
		}
	}
}