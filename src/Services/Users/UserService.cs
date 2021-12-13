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

        public async Task<UserResponse.Create> CreateAsync(UserRequest.Create request)
        {
            UserResponse.Create response = new();

            var auth0Request = new UserCreateRequest
            {
                Email = request.User.Email,
                FirstName = request.User.Firstname,
                LastName = request.User.Lastname,
                Password = request.User.Password,
                Connection = "Username-Password-Authentication" // Name of the Database connection
            };

            var createdUser = await _managementApiClient.Users.CreateAsync(auth0Request);

            // Caching might be nice here
            var allRoles = await _managementApiClient.Roles.GetAllAsync(new GetRolesRequest());
            var adminRole = allRoles.First(x => x.Name == "Administrator");

            var assignRoleRequest = new AssignRolesRequest
            {
                Roles = new string[] { adminRole.Id }
            };
            await _managementApiClient.Users.AssignRolesAsync(createdUser?.UserId, assignRoleRequest);

            response.Auth0UserId = createdUser.UserId;

            return response;

        }
    }
}
