using System.Threading.Tasks;

namespace Shared.Users
{
    public interface IUserService
    {
        Task<UserResponse.GetIndex> GetIndexAsync(UserRequest.GetIndex request);

    }
}
