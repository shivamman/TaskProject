using project.domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace project.domain.DTO
{
    public class RoleDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public static class RoleDTOModelExtension
    {
        public static AppRole MapToProjectModel(this RoleDTO input)
        {
            return new AppRole()
            {
                Id = input.Id,
                Name = input.Name
            };
        }
    }
}
