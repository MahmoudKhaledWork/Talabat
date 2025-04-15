using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Core;
using Talabat.Core.Entites.Order_Aggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Services;
using TalabatAPIs.DTOS;
using TalabatAPIs.Errors;

namespace TalabatAPIs.Controllers
{
    public class OrdersController : ApiBaseController
    {
        private readonly IOrderServices _orderServices;
        private readonly IMapper _mapper;
        public OrdersController(IOrderServices orderServices, IMapper mapper)
        {
            _orderServices = orderServices;
            _mapper = mapper;
        }
        #region Create Order 
        // Create Order 
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var MappedAddress = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);
            var Order = await _orderServices.CreateOrderAsync(BuyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, MappedAddress);
            return Order is null ? BadRequest(new ApiResponse(400, "There is a Problem with your order")) : Ok(Order);
        }

        #endregion


        [ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<OrderToReturnDto>> GetOrdersForUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderServices.GetOrdersForSpecificUserAsync(Email);
            if (orders is null) return NotFound(new ApiResponse(404, "Theres No Orders For Thos User"));
            var MappedOrder = _mapper.Map<IReadOnlyList<Order>,IReadOnlyList<OrderToReturnDto>>(orders);    
            return Ok(MappedOrder);
        }

        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{Id}")]
        [Authorize]
        public async Task<ActionResult<OrderToReturnDto>> GetOrdersForUserById(int OrderId)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderServices.GetOrderByIdForSpecificUserAsync(Email,OrderId);
            if (orders is null) return NotFound(new ApiResponse(404, "Theres No Orders For This User"));
            var MappedOrder = _mapper.Map<Order, OrderToReturnDto>(orders);
            return Ok(MappedOrder);
        }

        [HttpGet("DeliveryMethod")]        
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetAllDeliveryMethod()
        {
            var DeliveryMethod = await _orderServices.GetAllDeliveryMethodAsync();
            return Ok(DeliveryMethod);
        }


















    }
}
