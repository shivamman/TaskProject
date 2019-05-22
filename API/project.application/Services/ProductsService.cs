using prject.domain.Models;
using project.application.Interfaces;
using project.persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace project.application.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void CreateProduct(Product product)
        {
            try
            {
                product.CreatedDate = DateTime.Now;
                _unitOfWork.ProductRepository.Create(product);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void DeleteProduct(Guid Id)
        {
            _unitOfWork.ProductRepository.Delete(Id);
            _unitOfWork.Save();
        }

        public Product GetProductsById(Guid Id)
        {
            try
            {
                return _unitOfWork.ProductRepository.Find(Id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Product> GetProductsList()
        {
            try
            {
                return _unitOfWork.ProductRepository.All().ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void UpdateProduct(Product product)
        {
            try
            {
                product.UpdatedDate = DateTime.Now;
                //var dataToUpdate = _unitOfWork.ProductRepository.Find(product.Id);
                _unitOfWork.ProductRepository.Update(product);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
