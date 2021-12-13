using Client.Extensions;
using Shared.Users;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Client.Users
{
    public class UserService : IUserService
    {
        private readonly HttpClient authenticatedClient;
        private const string endpoint = "api/user";

        public UserService(HttpClient authenticatedClient)
        {
            this.authenticatedClient = authenticatedClient;
        }
        public async Task<UserResponse.GetIndex> GetIndexAsync(UserRequest.GetIndex request)
        {
            // Add parameters here if needed
            var queryParameters = request.GetQueryString();
            var response = await authenticatedClient.GetFromJsonAsync<UserResponse.GetIndex>($"{endpoint}?{queryParameters}");
            return response;
        }

        public async Task<UserResponse.Create> CreateAsync(UserRequest.Create request)
        {
            var response = await authenticatedClient.PostAsJsonAsync(endpoint, request);
            return await response.Content.ReadFromJsonAsync<UserResponse.Create>();
        }
    }
}
