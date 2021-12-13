using Append.Blazor.Sidepanel;
using Client.Users.Components;
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
        [Inject] public ISidepanelService Sidepanel { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetUsersAsync();
        }

        private void OpenCreateForm()
        {
            var callback = EventCallback.Factory.Create(this, GetUsersAsync);
            Sidepanel.Open<Create>("Gebruiker", "Toevoegen", (nameof(Create.OnUserCreated), callback));
        }

        public async Task GetUsersAsync()
        {
            UserRequest.GetIndex request = new();
            var response = await UserService.GetIndexAsync(request);
            users = response.Users;
        }
    }
}
