using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace todolist_mvvm.model
{
    public enum TaskPriority
    {
        Low,
        Medium,
        High,
        Critical,
    }

    public class Tasks
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public TaskPriority Priority { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime? CompletedAt { get; set; }
    }
}
