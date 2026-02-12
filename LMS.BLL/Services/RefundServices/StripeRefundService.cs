using LMS.DAL.DTO.Response;
using LMS.DAL.DTO.Response.RefundResponse;
using LMS.DAL.Repository.Enrollments;
using Stripe;
using Stripe.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BLL.Services.RefundServices
{
    public class StripeRefundService : IStripeRefundService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public StripeRefundService(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }
        public async Task<Refund> RefundPaymentAsync(string paymentId)
        {
            
                var refundOptions = new RefundCreateOptions
                {
                    PaymentIntent = paymentId, 
                };
                var refundService = new Stripe.RefundService();
                Refund refund = await refundService.CreateAsync(refundOptions);
                return refund;
        }

        public async Task<BaseResponse> RefundEnrollment(string studentId, int courseId)
        {
            var enrollment = await _enrollmentRepository.GetEnrollment(studentId, courseId);

            if (enrollment == null)
                return new BaseResponse { Success = false, Message = "Enrollment not found" };

            //can only be refunded during 3 days after enrollment
            if ((DateTime.UtcNow - enrollment.EnrolledAt).TotalDays > 3)
                return new BaseResponse
                {
                    Success = false,
                    Message = "Refund period expired"
                };

            if (string.IsNullOrEmpty(enrollment.PaymentId))
                return new BaseResponse
                {
                    Success = false,
                    Message = "Payment info missing"
                };

           var refund= await RefundPaymentAsync(enrollment.PaymentId);
            if (refund.Status == "succeeded")
            {
                await _enrollmentRepository.Remove(enrollment);
                return new RefundResponse
                {
                    Success = true,
                    Message = "Refund processed successfully",
                    RefundId = refund.Id,
                    Status = refund.Status,
                    Currency = refund.Currency,
                    Amount = refund.Amount / 100m 
                };
            }
            else return new RefundResponse()
            {
                Success= false,
                Message="refund failed",
                Status = refund.Status
            };
        }
    }
}
