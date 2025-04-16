using Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class OrderSpecifications : Specifications<Order>
    {
        public OrderSpecifications(string BuyerEmail) : base ( o=>o.BuyerEmail == BuyerEmail)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
            AddOrderByDescinding(o => o.OrderDate);
        }

        public OrderSpecifications(string email, int id) : base(o => o.Id == id && o.BuyerEmail == email)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);

        }
    }
}
