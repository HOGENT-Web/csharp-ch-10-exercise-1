using FluentValidation;

namespace Shared.Users
{
    public static class UserDto
    {
        public class Index
        {
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public string Email { get; set; }
        }

        public class Create
        {
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }

            public class Validator : AbstractValidator<Create>
            {
                public Validator()
                {
                    RuleFor(x => x.Firstname).NotEmpty().Length(1, 100);
                    RuleFor(x => x.Lastname).NotEmpty().Length(1, 100);
                    RuleFor(x => x.Email).NotEmpty().EmailAddress();
                    RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
                }
            }
        }
    }
}
