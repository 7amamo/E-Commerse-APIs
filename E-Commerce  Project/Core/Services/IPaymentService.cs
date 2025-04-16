using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IPaymentService
    {
        public Task<CustomerBasket> CreatrOrUpdatePaymentIntent(string BasketId);
    }
}
