using Microsoft.AspNetCore.Components;
using Shared.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Users
{
    public partial class Index
    {
        private List<UserDto.Index> users;
        [Inject] public IUserService UserService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            UserRequest.GetIndex request = new();
            var response = await UserService.GetIndexAsync(request);
            users = response.Users;
        }
    }
}
