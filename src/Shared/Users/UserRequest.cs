namespace Shared.Users
{
    public static class UserRequest
    {
        public class GetIndex
        {
            // Filter stuff here if you want to.
        }

        public class Create
        {
            public UserDto.Create User { get; set; }
        }
    }
}
