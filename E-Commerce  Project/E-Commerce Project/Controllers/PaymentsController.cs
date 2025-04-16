using AutoMapper;
using Core.Services;
using E_Commerce_Project.DTOS;
using E_Commerce_Project.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Project.Controllers
{

    public class PaymentsController : ApiBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;

        public PaymentsController(IPaymentService paymentService, IMapper mapper)
        {
            _paymentService = paymentService;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<ActionResult<CustomerBasketDTO>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var Basket = await _paymentService.CreatrOrUpdatePaymentIntent(basketId);
            if (Basket == null) return BadRequest(new ApiResponse(400, "The Problem in Your Basket"));
            var mappedBasket = _mapper.Map<CustomerBasketDTO>(Basket);
            return Ok(Basket);

        }

    }   

}
