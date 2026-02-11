using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.DTO.Request.CoursesRequests
{
    public class CourseRequest
    {
        public List<CourseTranslationRequest> Translations { get; set; }
        public decimal Price { get; set; }
        public IFormFile Thumbnail { get; set; }
        public decimal DiscountPercentage { get; set; } = 0;
        public DateTime? DiscountStartAt { get; set; }
        public DateTime? DiscountEndAt { get; set; }

    }
}
