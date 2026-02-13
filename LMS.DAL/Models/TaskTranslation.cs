using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Models
{
    public class TaskTranslation : BaseModel
    {
        public string Language { get; set; }   
        public string Title { get; set; }
        public string Description { get; set; }
        public int TaskItemId { get; set; }
        public TaskItem Task { get; set; }
    }
}
