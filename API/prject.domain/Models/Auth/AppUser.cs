using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace project.domain.DTO
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    public static class UserResponseModelExtension
    {
        public static UserResponseDTO MapToUserModel(this AppUser input)
        {
            return new UserResponseDTO()
            {
                UserId = input.Id,
                Email = input.Email,
                FirstName = input.FirstName,
                LastName = input.LastName,
                UserName = input.UserName
            };
        }

        public static IEnumerable<UserResponseDTO> MapToUserList(this IEnumerable<AppUser> input)
        {
            foreach (var item in input)
            {
                yield return item.MapToUserModel();
            }
        }
    }
}
