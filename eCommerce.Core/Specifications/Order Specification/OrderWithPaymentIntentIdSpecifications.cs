using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce.Core.Entities.Order_Aggregation;

namespace eCommerce.Core.Specifications.Order_Specification
{
    public class OrderWithPaymentIntentIdSpecifications : BaseSpecifications<Order>
    {
        public OrderWithPaymentIntentIdSpecifications(string paymentIntent):base(O => O.PaymentIntentId == paymentIntent)
        {
            
        }
    }
}
