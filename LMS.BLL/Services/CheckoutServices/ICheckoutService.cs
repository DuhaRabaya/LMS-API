using LMS.DAL.DTO.Response;
using LMS.DAL.DTO.Response.CheckoutResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BLL.Services.CheckoutServices
{
    public interface ICheckoutService
    {
        Task<CheckoutResponse> CreateEnrollmentPaymentSession(string studentId, int courseId);
        Task<BaseResponse> ConfirmEnrollment(string sessionId);
    }
}
