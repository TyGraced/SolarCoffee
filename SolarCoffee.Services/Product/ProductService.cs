using System;
using System.Collections.Generic;
using System.Linq;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;

namespace SolarCoffee.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly SolarDbContext _db;
        public ProductService(SolarDbContext dbContext)
        {
            _db = dbContext;

        }

        /// <summary>
        /// Archives a Product by setting boolean IsArchived to true
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ServiceResponse<Data.Models.Product> ArchiveProduct(int id)
        {
            try
            {
                 var product = _db.Products.Find(id);
                 product.IsArchived = true;
                 _db.SaveChanges();

                 return new ServiceResponse<Data.Models.Product>
                 {
                     Data = product,
                     Time = DateTime.UtcNow,
                     Message = "Archived Product",
                     IsSuccess = true
                 };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Data.Models.Product>
                {
                    Data = null,
                    Time = DateTime.UtcNow,
                    Message = ex.StackTrace,
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        /// Adds a new product to the database
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public ServiceResponse<Data.Models.Product> CreateProduct(Data.Models.Product product)
        {
            try
            {
                _db.Products.Add(product);

                var newInventory = new ProductInventory
                {
                    Product = product,
                    QuantityOnHand = 0,
                    IdealQuantity = 10
                };

                _db.ProductInventories.Add(newInventory);

                _db.SaveChanges();

                return new ServiceResponse<Data.Models.Product>
                {
                    Data = product,
                    Time = DateTime.UtcNow,
                    Message = "Saved new product",
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {

                return new ServiceResponse<Data.Models.Product>
                {
                    Data = product,
                    Time = DateTime.UtcNow,
                    Message = ex.StackTrace,
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        /// Retrieves all Products from the databse
        /// </summary>
        /// <returns></returns>
        public List<Data.Models.Product> GetAllProducts()
        {
            return _db.Products.ToList();
        }

        /// <summary>
        /// Retrieves a Product from the database by primary key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Data.Models.Product GetProductById(int id)
        {
            return _db.Products.Find(id);
        }
    }
}