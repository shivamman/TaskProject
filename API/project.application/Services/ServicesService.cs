using prject.domain.Models;
using project.application.Interfaces;
using project.domain.Models;
using project.persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace project.application.Services
{
    public class ServicesService : IServicesService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServicesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void CreateService(Service service)
        {
            try
            {
                service.CreatedDate = DateTime.Now;
                _unitOfWork.ServiceRepository.Create(service);
                _unitOfWork.Save();
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void DeleteService(Guid Id)
        {
            try
            {
                _unitOfWork.ServiceRepository.Delete(Id);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Service GetServicesById(Guid Id)
        {
            try
            {
                return _unitOfWork.ServiceRepository.Find(Id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Service> GetServicesList()
        {
            try
            {
                return _unitOfWork.ServiceRepository.All().ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void UpdateService(Service service)
        {
            try
            {
                service.UpdatedDate = DateTime.Now;
                _unitOfWork.ServiceRepository.Update(service);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
