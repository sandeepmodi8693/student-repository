using System;

namespace Student.Web.Models
{
    public class SubjectModel
    {
        public Guid SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int Marks { get; set; }
        public string Result { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
