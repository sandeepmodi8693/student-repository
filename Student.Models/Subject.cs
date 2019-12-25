using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Student.Models
{
    public class Subject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int Marks { get; set; }
        public string Result { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        [ForeignKey("Students")]
        [Required]
        public Guid StudentId { get; set; }
        public virtual Student Students { get; set; }
    }
}
