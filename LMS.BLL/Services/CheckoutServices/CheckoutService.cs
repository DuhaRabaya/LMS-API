using LMS.BLL.Services.EmailServices;
using LMS.DAL.DTO.Response;
using LMS.DAL.DTO.Response.CheckoutResponses;
using LMS.DAL.Models;
using LMS.DAL.Repository.Courses;
using LMS.DAL.Repository.Enrollments;
using Microsoft.AspNetCore.Identity;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.BLL.Services.CheckoutServices
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICheckoutRepository _enrollmentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public CheckoutService(ICheckoutRepository enrollmentRepository, ICourseRepository courseRepository,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender)
        {
            _enrollmentRepository = enrollmentRepository;
            _courseRepository = courseRepository;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task<CheckoutResponse> CreateEnrollmentPaymentSession(string studentId, int courseId)
        {
            var course = await _courseRepository.Get(courseId);
            if (course is null)
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Message = "Course not found!"
                };
            }

            var alreadyEnrolled = await _enrollmentRepository.IsEnrolled(studentId, courseId);
            if (alreadyEnrolled)
                return new CheckoutResponse
                {
                    Success = false,
                    Message = "Already enrolled in this course!"
                };

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "payment",
                SuccessUrl = $"http://localhost:5165/api/student/courses/success?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"http://localhost:5165/api/student/courses/cancel",
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            UnitAmountDecimal = course.FinalPrice * 100, 
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = course.Translations.FirstOrDefault()?.Name ?? "Course",
                                Description = course.Translations.FirstOrDefault()?.Description ?? "No description"
                            }
                        },
                        Quantity = 1
                    }
                },
                Metadata = new Dictionary<string, string>
                {
                    { "UserId", studentId },
                    { "CourseId", courseId.ToString() }
                }
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return new CheckoutResponse
            {
                Success = true,
                Message = "Stripe checkout session created",
                Url = session.Url,
                PaymentId = session.PaymentIntentId
            };
        }

        public async Task<BaseResponse> ConfirmEnrollment(string sessionId)
        {
            var service = new SessionService();
            var session = await service.GetAsync(sessionId); 
            var studentId = session.Metadata["UserId"];
            var courseId = int.Parse(session.Metadata["CourseId"]);

            var alreadyEnrolled = await _enrollmentRepository.IsEnrolled(studentId, courseId);
            if (alreadyEnrolled)
                return new BaseResponse
                {
                    Success = false,
                    Message = "Already enrolled!"
                };

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                EnrolledAt = DateTime.UtcNow
            };
            await _enrollmentRepository.Add(enrollment);

            var user = await _userManager.FindByIdAsync(studentId);
            await _emailSender.SendEmail(user.Email, "Successful payment",
                "<h1>Payment completed successfully!<br>You are now enrolled in the course.</h1>");

            return new BaseResponse
            {
                Success = true,
                Message = "Enrollment successful!"
            };
        }
    }
}
