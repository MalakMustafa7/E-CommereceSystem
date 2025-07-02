using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Order_Aggeregate;

namespace Talabat.Core.Specifications.OrderSpec
{
    public class OrderWithPaymentSpec : BaseSpecifications<Order>
    {
        public OrderWithPaymentSpec(string paymentIntentId) : base(O=>O.PaymentInentId==paymentIntentId)
        {

        }
    }
}
