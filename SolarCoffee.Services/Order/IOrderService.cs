using System.Collections.Generic;
using SolarCoffee.Data.Models;

namespace SolarCoffee.Services.Order
{
    public interface IOrderService
    {
         List<SalesOrder> GetOrders();
         ServiceResponse<bool> MarkFulfilled(int id);
        ServiceResponse<bool> GenerateOpenOrder(SalesOrder order);
    }
}