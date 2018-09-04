using Saned.ArousQatar.Data.Core.Dtos;
using Saned.ArousQatar.Data.Core.Models;
using Saned.ArousQatar.Data.Core.Repositories;
using Saned.ArousQatar.Data.Persistence.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Persistence.Repositories
{
    public class LikeRepository : EntityBaseRepository<Like>, ILikeRepository
    {
        public LikeRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public async Task<List<LikeDto>> GetAllAsync(int advId)
        {
            return
                (await
                    DbContext.Database.SqlQuery<LikeDto>("LikeGetAll @id",
                        new SqlParameter("id", SqlDbType.Int) { Value = advId }).ToListAsync());
        }

        public async Task<Like> GetSingleAsync(int id)
        {
            return (await GetSingleAsync("Like", id));
        }

        public void Save(Like bo)
        {
            DbContext.Likes.Add(bo);
        }

        public async Task<Like> GetSingleAsyncByUser(int id, string userId)
        {
            return await DbContext.Likes.SingleOrDefaultAsync(l => l.AdvertismentId == id && l.ApplicationUserId == userId);
        }

    }

    public class ApplicationUserRepositoryAsync : IApplicationUserRepositoryAsync
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUserRepositoryAsync(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfileDetails> ViewDetails(string userId)
        {
            SqlParameter idParameter = new SqlParameter("@UserId", userId);
            return
                (await
                    _context.Database.SqlQuery<UserProfileDetails>("AspNetUsers_Select_Details @UserId", idParameter)
                        .FirstOrDefaultAsync());
        }


        public async Task<int> UpdateInfo(string userId, string name, string phoneNumber)
        {
            SqlParameter idParameter = new SqlParameter("@Id", userId);
            SqlParameter nameParameter = new SqlParameter("@Name", name);
            SqlParameter phoneNumberParameter = Getparamter(phoneNumber, "Phone");

            return
                (await
                    _context.Database.ExecuteSqlCommandAsync("AspNetUsers_Update_Info @Id,@Name,@Phone",
                        idParameter, nameParameter, phoneNumberParameter));
        }

        public async Task<int> ChangeImage(string userId, string imgUrl)
        {
            SqlParameter idParameter = new SqlParameter("@Id", userId);
            SqlParameter imgUrlParameter = new SqlParameter("@PhotoUrl", imgUrl);
            return
                (await
                    _context.Database.ExecuteSqlCommandAsync("AspNetUsers_Update_ImageUrl @Id,@PhotoUrl", idParameter,
                        imgUrlParameter));
        }

        public async Task<UserProfileDetails> View(string userName)
        {
            SqlParameter idParameter = new SqlParameter("@UserName", userName);
            return
                (await
                    _context.Database.SqlQuery<UserProfileDetails>("AspNetUsers_Select_Info @UserName", idParameter)
                        .FirstOrDefaultAsync());

        }

        private static SqlParameter Getparamter(object id, string parameterName)
        {
            var accountTypeIdParameter = id == null
                ? new SqlParameter("@" + parameterName, DBNull.Value)
                : new SqlParameter("@" + parameterName, id);
            return accountTypeIdParameter;
        }

        public async Task<List<UserData>> GetAllAsync()
        {
            return (await _context.Database.SqlQuery<UserData>("EXEC [dbo].[AspNetUsers_GetAll]").ToListAsync());

        }

        public async Task<UserData> GetSingleAsync(string id)
        {
            var res =
                  (await _context.Database.SqlQuery<UserData>("EXEC [dbo].[AspNetUsers_GetSingle] @Id",
                          new SqlParameter("Id", SqlDbType.NVarChar) { Value = id }).FirstAsync());
            return res;
        }

        ////public async Task<List<AppUserData>> GetWithFilterAsync(int pageNumber = 1,
        ////    int? pageSize = null, string filter = null, bool? isDelete = null,
        ////    int? cityId = null, bool? isApprove = null,
        ////    bool? gender = null, bool? emailConfirm = null, string role = null)
        ////{
        ////    return (await _context.Database.SqlQuery<AppUserData>
        ////        ("AspNetUsers_SelectAllPaging @PageNumber,@PageSize,@Filter,@IsDeleted,@IsApprove,@EmailConfirm,@Role",
        ////            new SqlParameter("@PageNumber", pageNumber),
        ////            Getparamter(pageSize, "PageSize"),
        ////            Getparamter(filter, "Filter"),
        ////            Getparamter(isDelete, "IsDeleted"),
        ////            Getparamter(isApprove, "IsApprove"),
        ////            Getparamter(emailConfirm, "EmailConfirm"),
        ////             Getparamter(role, "Role")
        ////        ).ToListAsync());
        ////}


        public async Task<int> DeleteUser(string id)
        {

            SqlParameter idParameter = new SqlParameter("UserId", SqlDbType.NVarChar) { Value = id };
            return
                (await
                    _context.Database.SqlQuery<int>("AspNetUsers_Delete_ById @UserId", idParameter)
                        .FirstOrDefaultAsync());
           
            //var user = _context.Users.Find(id);
            //_context.Users.Remove(user);
            //return await _context.SaveChangesAsync();
        }

        public async Task<List<UserData>> GetWithFilterAsync(int pageNumber = 1, int pageSize = 8, string Filter = null, string role = "User")
        {
            return (await _context.Database.SqlQuery<UserData>("EXEC [dbo].[AspNetUsers_GetAllFilterd] @PageNumber,@PageSize,@Filter ,@Role",
                new SqlParameter("PageNumber", SqlDbType.Int) { Value = pageNumber }
              , new SqlParameter("PageSize", SqlDbType.Int) { Value = pageSize }, Getparamter(Filter, "Filter"), Getparamter(role, "Role")).ToListAsync());
        }

        public bool CheckifPhoneAvailable(string phoneNumber)
        {
            return _context.Users.Any(u => u.PhoneNumber == phoneNumber);
        }


        public async Task<ApplicationUser> GetUserbyId(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

    }
}
