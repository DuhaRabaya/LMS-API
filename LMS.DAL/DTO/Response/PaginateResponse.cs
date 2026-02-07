using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.DTO.Response
{
    public class PaginateResponse<T>
    {
        public int Page { get; set; }
        public int Total { get; set; }
        public int Limit { get; set; }
        public List<T> Data { get; set; }
    }
}
