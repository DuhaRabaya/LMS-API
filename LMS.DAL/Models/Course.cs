using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Models
{
    public class Course : BaseModel
    {
        public string? InstructorId { get; set; }
        public decimal Price { get; set; }
        public bool IsPublished { get; set; } = false;
        public double AverageRating { get; set; } = 0;
        public List<CourseTranslation> Translations { get; set; }   
        public ApplicationUser Instructor { get; set; }
        public string Thumbnail { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public decimal DiscountPercentage { get; set; } = 0; 
        public DateTime? DiscountStartAt { get; set; }
        public DateTime? DiscountEndAt { get; set; }

        [NotMapped]
        public decimal FinalPrice
        {
            get
            {
                if (DiscountPercentage <= 0 || DiscountPercentage > 100 ||
                  DiscountStartAt == null || DiscountEndAt == null ||
                  DiscountEndAt <= DiscountStartAt)
                     return Price;


                var now = DateTime.UtcNow;

                if (now < DiscountStartAt || now > DiscountEndAt)
                    return Price;

                return Price - (Price * DiscountPercentage/100);
            }
        }

    }
}
