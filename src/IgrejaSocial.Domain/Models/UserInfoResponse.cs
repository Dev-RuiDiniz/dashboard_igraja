using System;

namespace IgrejaSocial.Domain.Models
{
    public class UserInfoResponse
    {
        public string Email { get; set; } = string.Empty;
        public string[] Roles { get; set; } = Array.Empty<string>();
    }
}
