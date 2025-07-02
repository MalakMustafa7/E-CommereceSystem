using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Order_Aggeregate;

namespace Talabat.Core.Services
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int DelivaryMethodId, Address ShippingAddress );

        Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string buyerEmail);

        Task<Order> GetOrderByIdForSpecificUserAsync(string buyerEmail, int orderId);
    }
}
