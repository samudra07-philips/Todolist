using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Todolist.Services.Contracts;
using Todolist.Services.Repositories;

namespace Todolist.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repo;
        public TaskService(ITaskRepository repo) => _repo = repo;

        public List<TaskDto> GetPendingTasks(int userId) =>
            _repo.GetPending(userId).ToList();
        public void AddTask(TaskDto dto) => _repo.Add(dto);
        public void UpdateTask(TaskDto dto) => _repo.Update(dto);
        public void DeleteTask(int taskId) => _repo.Delete(taskId);
        public void MarkTaskComplete(int taskId) => _repo.MarkComplete(taskId);
        public List<TaskDto> GetTaskHistory(int userId) =>
            _repo.GetHistory(userId).ToList();  

       

    }
}
