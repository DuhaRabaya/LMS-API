using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.DTO.Response.RefundResponse
{
    public class RefundResponse : BaseResponse
    {
        public string RefundId { get; set; }
        public string Status { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
    }
}
