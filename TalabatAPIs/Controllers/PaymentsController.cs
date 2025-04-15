using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entites;
using Talabat.Core.Services;
using TalabatAPIs.DTOS;
using TalabatAPIs.Errors;

namespace TalabatAPIs.Controllers
{
    [Authorize]
    public class PaymentsController : ApiBaseController
    {
        private readonly IPaymentServiec _paymentServiec;
        private readonly IMapper _mapper;

        public PaymentsController(IPaymentServiec paymentServiec , IMapper mapper)
        {
            _paymentServiec = paymentServiec;
            _mapper = mapper;
        }
        // Create Or update EndPoint
        [ProducesResponseType(typeof(CustomerBasketDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string BasketId)
        {
            var customerBasket =  await _paymentServiec.CreateOrUpdatePaymentIntent(BasketId);
            if (customerBasket is null) return BadRequest(new ApiResponse(400,"There is a problem with your Basket"));
            var MappedBasket = _mapper.Map<CustomerBasket,CustomerBasketDto>(customerBasket);
            return Ok(MappedBasket);
        }


    }
}
