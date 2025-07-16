using System.Collections.Generic;
using Todolist.Services.

namespace Todolist.Services.Repositories
{
    public interface ITaskRepository
    {
        IEnumerable<TaskDto> GetPending(int userId);
        void Add(TaskDto dto);
        void Update(TaskDto dto);
        void Delete(int taskId);
        void MarkComplete(int taskId);
        IEnumerable<TaskDto> GetHistory(int userId);
    }
}
