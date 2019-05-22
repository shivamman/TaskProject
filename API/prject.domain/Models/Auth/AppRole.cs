using Microsoft.AspNetCore.Identity;
using project.domain.DTO;
using System;
using System.Collections.Generic;

namespace project.domain.Models
{
    public class AppRole : IdentityRole<Guid>
    {

    }
    public static class RoleModelExtension
    {
        public static RoleDTO MapToRoleModel(this AppRole input)
        {
            return new RoleDTO()
            {
                Id = input.Id,
                Name = input.Name
            };
        }

        public static IEnumerable<RoleDTO> MapToRoleList(this IEnumerable<AppRole> input)
        {
            foreach (var item in input)
            {
                yield return item.MapToRoleModel();
            }
        }
    }
}
