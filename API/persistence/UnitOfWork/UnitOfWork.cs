using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using prject.domain.Models;
using project.domain.DTO;
using project.domain.Models;
using project.persistence.Context;
using project.persistence.Repository;

namespace project.persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private CASContext _context;
        private IRepository<AppUser> _userRepository;
        private IRepository<IdentityUserRole<Guid>> _userRoleRepository;
        private IRepository<AppRole> _roleRepository;
        private IRepository<Product> _productRepository;
        private IRepository<Service> _serviceRepository;
        private IRepository<ProductUser> _productUserRepository;
        private IRepository<ServiceUser> _serviceUserRepository;

        public UnitOfWork(CASContext context)
        {
            _context = context;
        }

        public IRepository<AppUser> UserRepository
        {
            get { return _userRepository ?? (_userRepository = new RepositoryBase<AppUser>(_context)); }
        }

        public IRepository<IdentityUserRole<Guid>> UserRoleRepository {
            get { return _userRoleRepository ?? (_userRoleRepository = new RepositoryBase<IdentityUserRole<Guid>>(_context)); }
        }

        public IRepository<AppRole> RoleRepository {
            get { return _roleRepository ?? (_roleRepository = new RepositoryBase<AppRole>(_context)); }
        }

        public IRepository<Product> ProductRepository {
            get { return _productRepository ?? (_productRepository = new RepositoryBase<Product>(_context)); }
        }

        public IRepository<Service> ServiceRepository {
            get { return _serviceRepository ?? (_serviceRepository = new RepositoryBase<Service>(_context)); }
        }

        public IRepository<ProductUser> ProductUserRepository {
            get { return _productUserRepository ?? (_productUserRepository = new RepositoryBase<ProductUser>(_context)); }
        }

        public IRepository<ServiceUser> ServiceUserRepository {
            get { return _serviceUserRepository ?? (_serviceUserRepository = new RepositoryBase<ServiceUser>(_context)); }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
