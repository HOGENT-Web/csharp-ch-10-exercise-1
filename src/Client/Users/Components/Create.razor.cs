using Append.Blazor.Sidepanel;
using Client.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Shared.Products;
using Shared.Users;
using System.Threading.Tasks;

namespace Client.Users.Components
{
    public partial class Create
    {
        private UserDto.Create user = new();
        [Parameter] public EventCallback OnUserCreated { get; set; }

        [Inject] public IUserService UserService { get; set; }
        [Inject] public ISidepanelService Sidepanel { get; set; }
        private async Task CreateUserAsync()
        {
            UserRequest.Create request = new()
            {
                User = user
            };

            await UserService.CreateAsync(request);
            Sidepanel.Close();
            await OnUserCreated.InvokeAsync();
        }
    }
}
