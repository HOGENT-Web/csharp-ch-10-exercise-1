using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;
using Shared.Users;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Users
{
    public class UserService : IUserService
    {
        private readonly ManagementApiClient _managementApiClient;

        public UserService(ManagementApiClient managementApiClient)
        {
            _managementApiClient = managementApiClient;
        }
        public async Task<UserResponse.GetIndex> GetIndexAsync(UserRequest.GetIndex request)
        {
            UserResponse.GetIndex response = new();
            var users = await _managementApiClient.Users.GetAllAsync(new GetUsersRequest(), new PaginationInfo());
            response.Users = users.Select(x => new UserDto.Index
            {
                Email = x.Email,
                Firstname = x.FirstName,
                Lastname = x.LastName,
            }).ToList();

            return response;
        }
    }
}
