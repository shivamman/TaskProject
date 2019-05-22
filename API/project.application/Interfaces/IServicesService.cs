using project.domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace project.application.Interfaces
{
    public interface IServicesService
    {
        Service GetServicesById(Guid Id);
        List<Service> GetServicesList();
        void DeleteService(Guid Id);
        void CreateService(Service Service);
        void UpdateService(Service Service);
    }
}
