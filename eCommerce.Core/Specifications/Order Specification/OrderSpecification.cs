using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce.Core.Entities.Order_Aggregation;

namespace eCommerce.Core.Specifications.Order_Specification
{
    public class OrderSpecification : BaseSpecifications<Order>
    {
        // Constructor to accept email criteria
        public OrderSpecification(string email) : base(O => O.BuyerEmail == email)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
            SetOrderByDesc(O => O.OrderDate);
        }

        // A constructor to get specific order by id and email.
        public OrderSpecification(string email, int Id) 
        : base(
              O => O.BuyerEmail == email && O.Id == Id
              )
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
    }
}
