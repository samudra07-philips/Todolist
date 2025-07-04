using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Todolist.Services
{
    public class TaskDto
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public string Description { get; set; }
        [DataMember] public TaskPriority Priority { get; set; }
        [DataMember] public bool IsCompleted { get; set; }
        [DataMember] public DateTime? CompletedAt { get; set; }
        [DataMember] public int UserId { get; set; }
    }
}
