using Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class OrderWithPaymentIntentIdSpecification : Specifications<Order>
    {
        public OrderWithPaymentIntentIdSpecification(string PaymentIntentId) : base(o => o.PaymentintentId == PaymentIntentId)
        {

        }
    }
}
