using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Talabat.Apis.DTOs
{
    public class OrderDTO
    {
        [Required]
        public string BasketId {  get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressDTO shipToAddress { get; set; }
    }
}
