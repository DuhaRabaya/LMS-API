using LMS.BLL.Services.CheckoutServices;
using LMS.BLL.Services.CourseServices;
using LMS.BLL.Services.EnrollmentsServices;
using LMS.BLL.Services.RefundServices;
using LMS.DAL.DTO.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.PL.Areas.Student
{
    [Route("api/student/[controller]")]
    [ApiController]
    [Authorize(Roles ="Student")]
    
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IEnrollmentService _enrollmentService;
        private readonly ICheckoutService _checkoutService;
        private readonly IStripeRefundService _stripeRefundService;

        public CoursesController(ICourseService courseService,
            IEnrollmentService enrollmentService,
            ICheckoutService checkoutService,
            IStripeRefundService stripeRefundService)
        {
            _courseService = courseService;
            _enrollmentService = enrollmentService;
            _checkoutService = checkoutService;
            _stripeRefundService = stripeRefundService;
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetCourses([FromQuery] string lang = "en",
            [FromQuery]int page = 1, [FromQuery]int limit = 1, [FromQuery]string? search=null
            , [FromQuery] string? instructorId = null, [FromQuery]decimal? price=null,
             [FromQuery] decimal? minPrice = null,
             [FromQuery] decimal? maxPrice = null
            , [FromQuery] double? minRating = null, [FromQuery] double? maxRating = null
            , [FromQuery]string? sortBy = null,[FromQuery]bool asc = true)
        {
            var result = await _courseService.GetCoursesForStudent(lang, page,limit,search,instructorId,price,minPrice,maxPrice,minRating,maxRating,sortBy,asc);
            return Ok(result);
        }

        [HttpPost("enroll/{courseId}")]
        public async Task<IActionResult> Enroll([FromRoute] int courseId)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = await _checkoutService.CreateEnrollmentPaymentSession(studentId, courseId);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("refund/{courseId}")]
        public async Task<IActionResult> Refund([FromRoute]int courseId)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _stripeRefundService.RefundEnrollment(studentId, courseId);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }


        [HttpGet("success")]
        [AllowAnonymous]
        public async Task<IActionResult> Success([FromQuery] string session_id)
        {
            var response = await _checkoutService.ConfirmEnrollment(session_id);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("cancel")]
        public ActionResult<BaseResponse> Cancel()
        {
            return Ok(new BaseResponse
            {
                Success = false,
                Message = "Payment canceled, enrollment failed"
            });
        }

        [HttpGet("enrollments")]
        public async Task<IActionResult> GetEnrollments([FromQuery] string lang="en")
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var enrollments = await _enrollmentService.GetStudentEnrollments(studentId, lang);

            return Ok(enrollments);
        }
    }
}
