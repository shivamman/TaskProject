using prject.domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace project.application.Interfaces
{
    public interface IProductsService
    {
        Product GetProductsById(Guid Id);
        List<Product> GetProductsList();
        void CreateProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Guid Id);
    }
}
