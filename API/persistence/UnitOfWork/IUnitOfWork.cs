using Microsoft.AspNetCore.Identity;
using prject.domain.Models;
using project.domain.DTO;
using project.domain.Models;
using project.persistence.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace project.persistence.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<AppUser> UserRepository { get; }
        IRepository<IdentityUserRole<Guid>> UserRoleRepository { get; }
        IRepository<AppRole> RoleRepository { get; }
        IRepository<Product> ProductRepository { get; }
        IRepository<Service> ServiceRepository { get; }
        IRepository<ProductUser> ProductUserRepository { get; }
        IRepository<ServiceUser> ServiceUserRepository { get; }
        void Save();
    }
}
