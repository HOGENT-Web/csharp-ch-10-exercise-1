﻿using System.Collections.Generic;

namespace Shared.Users
{
    public static class UserResponse
    {
        public class GetIndex
        {
            public List<UserDto.Index> Users { get; set; } = new();
        }
    }
}
