using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entites;
using Talabat.Core.Repositories;
using TalabatAPIs.DTOS;
using TalabatAPIs.Errors;

namespace TalabatAPIs.Controllers
{
    public class basketController : ApiBaseController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public basketController(IBasketRepository _BasketRepository , IMapper mapper)
        {
            _basketRepository = _BasketRepository;
            _mapper = mapper;
        }
        // "https://localhost:7060/api/basket"
        // Get or Recreate
        //[HttpGet("{BasketId}")]
        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket( string BasketId)
        {
            var Basket = await _basketRepository.GetBasketAsync(BasketId);
            //if (Basket == null) return new CustomerBasket(BasketId);           
            return Basket is null ? new CustomerBasket(BasketId) : Ok(Basket);
        }

        
        // Update or Create 
        [HttpPost("BasketId")]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var MappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
            var CreatedOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(MappedBasket);
            if (CreatedOrUpdatedBasket is null) return BadRequest(new ApiResponse(400));
            return Ok(CreatedOrUpdatedBasket);
        }



        // Delete
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket (string BasketId)
        {
           return  await _basketRepository.DeleteBasketAsync(BasketId);
        }
       
    } 
}
