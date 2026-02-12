using LMS.DAL.DTO.Response;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BLL.Services.RefundServices
{
    public interface IStripeRefundService
    {
        Task<Refund> RefundPaymentAsync(string paymentId);
        Task<BaseResponse> RefundEnrollment(string studentId, int courseId);
    }
}
