using System.Threading.Tasks;
using System.Web.Http;
using Saned.ArousQatar.Data.Core.Models;
using Saned.ArousQatar.Data.Persistence.Repositories;

namespace Saned.ArousQatar.Api.Controllers
{
    public class BasicController : ApiController
    {
        private readonly AuthRepository _repo = null;
        public BasicController()
        {
            _repo = new AuthRepository();
        }
        public async Task<ApplicationUser> GetApplicationUser(string userName)
        {
            return await _repo.FindUserByUserName(userName);
        }
        public async Task<ApplicationUser> GetApplicationUserById(string userId)
        {
            return await _repo.FindUserById(userId);
        }
    }
}