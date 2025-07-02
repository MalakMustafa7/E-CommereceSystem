using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.Apis.DTOs;
using Talabat.Apis.Errors;
using Talabat.Core.Models;
using Talabat.Core.Services;

namespace Talabat.Apis.Controllers
{
     
    public class PaymentsController : ApiBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        private const string endpointSecret = "whsec_2dad688b3d3d0f8c928eeb6c055725c1736f872bc477d2861a5ce8816d86a307";

        public PaymentsController(IPaymentService paymentService, IMapper mapper)
        {
            _paymentService = paymentService;
            _mapper = mapper;
        }
        [Authorize]
        [ProducesResponseType(typeof(CustomerBasketDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDTO>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var CustomerBasket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (CustomerBasket is null) return BadRequest(new ApiResponse(400, "There Is A Problem With Your Basket"));
            var mappedCustomerBasket = _mapper.Map<CustomerBasket, CustomerBasketDTO>(CustomerBasket);
            return Ok(mappedCustomerBasket);
        }
        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    endpointSecret  
                );
                var PaymentIntent = stripeEvent.Data.Object as PaymentIntent;
                if (stripeEvent.Type == "payment_intent.succeeded")
                {
                    await _paymentService.UpdatePaymentIntentSucceedOrFailed(PaymentIntent.Id, true);
                }
                else if (stripeEvent.Type == "payment_intent.payment_failed")
                {
                    await _paymentService.UpdatePaymentIntentSucceedOrFailed(PaymentIntent.Id,false);
                }
                
                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }



    }
}
