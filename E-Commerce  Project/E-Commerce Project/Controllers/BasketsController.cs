using AutoMapper;
using Core.Contracts;
using Core.Entities;
using E_Commerce_Project.DTOS;
using E_Commerce_Project.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Project.Controllers
{

    public class BasketsController : ApiBaseController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper mapper;

        public BasketsController(IBasketRepository basketRepository , IMapper mapper)
        {
            _basketRepository = basketRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket?>> GetBasketAsync (string basketId)
        {
            var basket =await _basketRepository.GetBasketAsync (basketId);
            return basket is null ? new CustomerBasket(basketId) : basket;
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasketAsync(CustomerBasketDTO basketDto)
        {
            var MappedBasket = mapper.Map<CustomerBasketDTO, CustomerBasket>(basketDto);
            var CreatedorUpdatedBasket = await _basketRepository.UpdateBasketAsync(MappedBasket);
            if (CreatedorUpdatedBasket is null) return BadRequest(new ApiResponse(400));
            return Ok(CreatedorUpdatedBasket);
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasketAsync(string basketId)
            => await _basketRepository.DeleteBasketAsync(basketId);
        
    }
}
