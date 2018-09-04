using Saned.ArousQatar.Data.Core;
using Saned.ArousQatar.Data.Core.Models;
using Saned.ArousQatar.Data.Persistence;
using System;
using System.Web.Http;
using Saned.ArousQatar.Api.Controllers;
using System.IO;
using System.Web.Hosting;
using System.Collections.Generic;
using System.Linq;

namespace Saned.ArousQatar.Api.Infrastructure.Core
{
    public class ApiControllerBase : BasicController
    {

        protected readonly IUnitOfWork _unitOfWork;
        private ApplicationDbContext _context { get; set; }

        public ApiControllerBase()
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);
        }

        public ApiControllerBase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected string SaveImage(string imageFileName, string imageBase64)
        {
            // create random guid to represent image name 
            var randomImage = Guid.NewGuid().ToString() + Path.GetExtension(imageFileName);

            string slogn = "/Uploads/" + randomImage;

            string filePath = (HostingEnvironment.MapPath($"~{slogn}"));

            SaveImageInFileSystem(imageBase64, filePath);

            return randomImage;
        }

        //protected Task<IHttpActionResult> CreateResponse(Func<Task<IHttpActionResult> , IHttpActionResult > function)
        //{
        //    Task<IHttpActionResult> response = null;
        //    try
        //    {
        //        response = function.Invoke();

        //    }
        //    //all possible errors
        //    catch (DbUpdateConcurrencyException ex)
        //    {
        //        LogError(ex);
        //        //response = NotFound();
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        LogError(ex);
        //        //response = BadRequest(ex.InnerException.Message);
        //    }

        //    catch (Exception ex)
        //    {
        //        LogError(ex);
        //        //response = InternalServerError(ex);
        //    }

        //    var tsc = new TaskCompletionSource<IHttpActionResult>();
        //    //tsc.SetResult(response);
        //    return tsc.Task;
        //}

        protected async void LogError(Exception ex)
        {                      
            try
            {
                var error = new Error()
                {
                    Message = ex.Message,
                    DateCreated = DateTime.Now,
                    StackTrace = ex.StackTrace
                };

                await _unitOfWork.Errors.AddAsync(error);
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                // ignored
            }
        }

        protected async void LogError(string method,string ex)

        {
            try
            {
                var error = new Error()
                {
                    Message = ex,
                    DateCreated = DateTime.Now,
                    StackTrace = method
                };

                await _unitOfWork.Errors.AddAsync(error);
                await _unitOfWork.CommitAsync();
            }
            catch 
            {
                //ignored
            }


        }

        protected void SaveImageInFileSystem(string base64, string filePath)
        {
           
                if (base64 == null)
                    return;
                Byte[] bytes = Convert.FromBase64String(base64);
                File.WriteAllBytes(filePath, bytes);
          
          
           
        }

    }

    public static class Helper
    {
        public static IEnumerable<TSource> FromHierarchy<TSource>(
    this TSource source,
    Func<TSource, TSource> nextItem,
    Func<TSource, bool> canContinue)
        {
            for (var current = source; canContinue(current); current = nextItem(current))
            {
                yield return current;
            }
        }

        public static IEnumerable<TSource> FromHierarchy<TSource>(
            this TSource source,
            Func<TSource, TSource> nextItem)
            where TSource : class
        {
            return FromHierarchy(source, nextItem, s => s != null);
        }

        public static string GetaAllMessages(this Exception exception)
        {
            var messages = exception.FromHierarchy(ex => ex.InnerException)
                .Select(ex => ex.Message);
            return String.Join(Environment.NewLine, messages);
        }
    }
}