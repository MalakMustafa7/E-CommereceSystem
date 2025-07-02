using Talabat.Core.Models.Order_Aggeregate;

namespace Talabat.Apis.DTOs
{
    public class OrderToReturnDTO
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } 
        public string Status { get; set; } 
        public Address ShippingAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal DelivaryMethodCost { get; set; } 
        public ICollection<OrderItemDTO> Items { get; set; } = new HashSet<OrderItemDTO>();
        public decimal SubTotal { get; set; }

        public decimal Total { get; set; }

        public string PaymentIntId { get; set; } 
    }
}
