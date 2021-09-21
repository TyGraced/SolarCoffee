using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SolarCoffee.Data;

namespace SolarCoffee.Services.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly SolarDbContext _db;
        public CustomerService(SolarDbContext dbContext)
        {
            _db = dbContext;

        }

        /// <summary>
        /// Adds a new Customer record
        /// </summary>
        /// <param name="customer">Customer Instance</param>
        /// <returns>ServiceResponse<Customer></returns>
        public ServiceResponse<Data.Models.Customer> CreateCustomer(Data.Models.Customer customer)
        {
            try
            {
                 _db.Customers.Add(customer);
                 _db.SaveChanges();
                 return new ServiceResponse<Data.Models.Customer>
                 {
                     IsSuccess = true,
                     Message = "New Customer added",
                     Time = DateTime.UtcNow,
                     Data = customer
                 };
            }
            catch (Exception ex)
            {
                
                return new ServiceResponse<Data.Models.Customer>
                 {
                     IsSuccess = false,
                     Message = ex.StackTrace,
                     Time = DateTime.UtcNow,
                     Data = customer
                 };
            }
        }

        /// <summary>
        /// Delete a customer record
        /// </summary>
        /// <param name="id">int customer primary key</param>
        /// <returns>ServiceResponse<bool></returns>
        public ServiceResponse<bool> DeleteCustomer(int id)
        {
            var customer = _db.Customers.Find(id);
            var now = DateTime.UtcNow;

            if (customer == null)
            {
                return new ServiceResponse<bool>
                {
                    Time = now,
                    IsSuccess = false,
                    Message = "Customer to delete not found",
                    Data = false
                };
            }

            try
            {
                 _db.Customers.Remove(customer);
                 _db.SaveChanges();

                 return new ServiceResponse<bool>
                 {
                     Time = now,
                     IsSuccess = true,
                     Message = "Customer deleted",
                     Data = true
                 };
            }
            catch (Exception ex)
            {
                
                return new ServiceResponse<bool>
                {
                    Time = now,
                    IsSuccess = false,
                    Message = ex.StackTrace,
                    Data = false
                };
            }
        }

        /// <summary>
        /// Returns a list of Customers from the database
        /// </summary>
        /// <returns>List<Customer></returns>
        public List<Data.Models.Customer> GetAllCustomers()
        {
            return _db.Customers
                .Include(customer => customer.PrimaryAddress)
                .OrderBy(customer => customer.LastName)
                .ToList();
        }

        /// <summary>
        /// Get a customer record by primary key
        /// </summary>
        /// <param name="id">int customer primary key</param>
        /// <returns>Customer</returns>
        public Data.Models.Customer GetById(int id)
        {
            return _db.Customers.Find(id);
        }
    }
}