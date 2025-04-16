using AutoMapper;
using Core.Contracts;
using Core.Entities.Identity;
using Core.Entities.Order;
using Core.Services;
using E_Commerce_Project.DTOS;
using E_Commerce_Project.Errors;
using E_Commerce_Project.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce_Project.Controllers
{
    public class OrdersController : ApiBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IOrderService orderService , IMapper mapper , IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Order), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]

        public async Task<ActionResult<Order>> CreateOrder (OrderDTO orderDTO)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var MappedAddress = _mapper.Map<AddressDTO, ShippingAddress>(orderDTO.ShippingAddress);

            var Order = await _orderService.CreateOrderAsync(email, orderDTO.BasketId, orderDTO.DeliveryMethodId, MappedAddress);
            if (Order is null) return BadRequest(new ApiResponse(400, "The Problem With Your Order"));
            return Ok(Order);

        }


        [ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDTO>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDTO>>> GetOrderForUsers()
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Orders = await _orderService.GetOrdersForSpecificUserAsync(BuyerEmail);

            if (Orders is null) return NotFound(new ApiResponse(400, "There is no Orders For this Users"));
            var MappedOrder = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDTO>>(Orders);

            return Ok(MappedOrder);
        }


        [ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDTO>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderByIdForUsers(int id)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Order = await _orderService.GetOrderByIdforSpecificUserAsync(BuyerEmail, id);

            if (Order is null) return NotFound(new ApiResponse(400, $"There is no Order with {id} For this Users"));
            var MappedOrder = _mapper.Map<Order, OrderToReturnDTO>(Order);

            return Ok(MappedOrder);
        }



        [CachedAttribute(1000)]
        [Authorize]
        [HttpGet("DeliveryMethod")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethod()
        {
            var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return Ok(DeliveryMethod);
        }




    }
}
