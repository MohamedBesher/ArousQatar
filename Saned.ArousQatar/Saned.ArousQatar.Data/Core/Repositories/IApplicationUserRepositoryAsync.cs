using Saned.ArousQatar.Data.Core.Dtos;
using Saned.ArousQatar.Data.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Core.Repositories
{
    public interface IApplicationUserRepositoryAsync
    {
        Task<UserProfileDetails> ViewDetails(string userId);
        Task<int> UpdateInfo(string userId, string name, string phoneNumber);

        bool CheckifPhoneAvailable(string phoneNumber);

        Task<int> ChangeImage(string userId, string imgUrl);
        Task<UserProfileDetails> View(string userName);

        Task<List<UserData>> GetAllAsync();
        Task<List<UserData>> GetWithFilterAsync(int pageNumber = 1, int pageSize = 8, string Filter = null, string role = "User");
        Task<UserData> GetSingleAsync(string id);

        Task<int> DeleteUser(string id);
        //Task<List<AppUserData>> GetWithFilterAsync(
        //   int pageNumber = 1,
        //   int? pageSize = null,
        //   string filter = null,
        //   bool? isDelete = null,
        //   int? cityId = null,
        //   bool? isApprove = null,
        //   bool? gender = null,
        //   bool? emailConfirm = null,
        //   string role = null
        //   );
        Task<ApplicationUser> GetUserbyId(string uId);
    }
}