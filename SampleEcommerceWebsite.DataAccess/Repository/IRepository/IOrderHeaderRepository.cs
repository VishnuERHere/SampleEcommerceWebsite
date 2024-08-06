using SampleEcommerceWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleEcommerceWebsite.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader obj);

        void UpdateOrderStatus(int id, string ordersStatus, string? paymentStatus = null);

		void UpdateStripePaymentId(int id, string sessionId, string? paymentIntentId);

	}
}
