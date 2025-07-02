using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Security.Claims;
using Talabat.Apis.DTOs;
using Talabat.Apis.Errors;
using Talabat.Core.Models.Order_Aggeregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Order = Talabat.Core.Models.Order_Aggeregate.Order;

namespace Talabat.Apis.Controllers
{ 
    public class OrdersController : ApiBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IOrderService orderService, IMapper mapper , IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        [ProducesResponseType(typeof(OrderToReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(OrderDTO orderDTO)
        {
           var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
           var MappedShippingAddress = _mapper.Map<AddressDTO, Address>(orderDTO.shipToAddress);
           var order= await  _orderService.CreateOrderAsync(BuyerEmail, orderDTO.BasketId, orderDTO.DeliveryMethodId,MappedShippingAddress);
           if (order is null) return BadRequest(new ApiResponse(400,"There Is Some Thing Wrong With Your Order"));
           var OrderToReturnDTO = _mapper.Map<Core.Models.Order_Aggeregate.Order, OrderToReturnDTO>(order);
            return Ok(OrderToReturnDTO);

        }

        [ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDTO>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDTO>>> GetOrdersForSpecUser()
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Orders = await _orderService.GetOrdersForSpecificUserAsync(BuyerEmail);
            if (Orders is null) return NotFound(new ApiResponse(404, "There Is No Orders For This User"));
            var OrderToReturnDTO = _mapper.Map<IReadOnlyList<Order>,IReadOnlyList<OrderToReturnDTO>>(Orders);
            return Ok(OrderToReturnDTO);
        }

        [ProducesResponseType(typeof(OrderToReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderToReturnDTO>> GetOrderByIdForSpecUser(int id)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderByIdForSpecificUserAsync(BuyerEmail,id);
            if (order is null) return NotFound(new ApiResponse(404, $"There Is No Order by this id= {id} For This User"));
            var OrderToReturnDTO = _mapper.Map<Core.Models.Order_Aggeregate.Order, OrderToReturnDTO>(order);
            return Ok(OrderToReturnDTO);
        }

        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var DelivaryMethodes = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return Ok(DelivaryMethodes);
        }


    }
}
