using System;
using System.Runtime.Serialization;
using Todolist.Services.Data;  

namespace Todolist.Services.Contracts
{
    [DataContract]
    public class TaskDto
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public TaskPriority Priority { get; set; }
        [DataMember] public string Title { get; set; }
        [DataMember] public string Description { get; set; }
        [DataMember] public int UserId { get; set; }
        [DataMember] public bool IsCompleted { get; set; }
        [DataMember] public DateTime? CompletedAt { get; set; }
    }
}
