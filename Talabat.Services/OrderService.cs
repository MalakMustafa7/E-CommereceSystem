using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Models.Order_Aggeregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications.OrderSpec;

namespace Talabat.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepo , IUnitOfWork unitOfWork , IPaymentService paymentService  )
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int DelivaryMethodId, Address ShippingAddress)
        {
            var basket =await _basketRepo.GetBasketAsync(basketId);
            var orderedItems = new List<OrderItem>();
            if(basket?.Items.Count() > 0)
            {
                foreach (var item in basket.Items)
                {
                    var Product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var ProductItemOrdered = new ProductItemOrdered(Product.Id, Product.Name, Product.PictureUrl);
                    var OrderItem = new OrderItem(ProductItemOrdered, Product.Price, item.Quantity);
                    orderedItems.Add(OrderItem);
                }  
            }
            var SubTotal = orderedItems.Sum(item => item.Price * item.Quantity);
            var DeliveryMethode = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DelivaryMethodId);
            var Spec = new OrderWithPaymentSpec(basket.PaymentIntentId);
            var ExOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(Spec);
            if (ExOrder is not null) {
                _unitOfWork.Repository<Order>().Delete(ExOrder);
               await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            }
            var Order = new Order(buyerEmail, ShippingAddress, DeliveryMethode, orderedItems, SubTotal,basket.PaymentIntentId);
            await _unitOfWork.Repository<Order>().AddAsync(Order);
            var Result =await _unitOfWork.CompleteAsync();
            if (Result <= 0) return null;
            return Order;
        }

        public async Task<Order> GetOrderByIdForSpecificUserAsync(string buyerEmail, int orderId)
        {
             var Spec = new OrderSpecification(buyerEmail, orderId);
             var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(Spec);
             return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string buyerEmail)
        {
            var Spec = new OrderSpecification(buyerEmail);
            var orders=  await  _unitOfWork.Repository<Order>().GetAllWithSpecAsync(Spec);
            return orders;
        }
    }
}
