using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Apis.DTOs;
using Talabat.Apis.Errors;
using Talabat.Core.Models;
using Talabat.Core.Repositories;

namespace Talabat.Apis.Controllers
{
    public class BasketController : ApiBaseController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository , IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        //Get OR ReCreate[after expire date]
        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketAsyns(string BasketId)
        {
            var Basket= await _basketRepository.GetBasketAsync(BasketId);
            return Basket is null ? new CustomerBasket(BasketId) : Basket;
        }

        //update or create newBasket
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasketAsync(CustomerBasketDTO Basket)
        {
            var mappedBasket = _mapper.Map<CustomerBasketDTO,CustomerBasket>(Basket);
            var UpdateOrCreatedBasket= await _basketRepository.UpdateBasketAsync(mappedBasket);
            if (UpdateOrCreatedBasket is null) return BadRequest(new ApiResponse(400));
            return Ok(UpdateOrCreatedBasket);
        }

        //Delete Basket
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasketAsync(string BasketId) { 
            return await _basketRepository.DeleteBasketAsync(BasketId);
        }
    }
}
