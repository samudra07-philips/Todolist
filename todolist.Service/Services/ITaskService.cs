using System.Collections.Generic;
using System.ServiceModel;
using Todolist.Services.Contracts;

namespace Todolist.Services
{
    [ServiceContract]
    public interface ITaskService
    {
        [OperationContract] List<TaskDto> GetPendingTasks(int userId);
        [OperationContract] void AddTask(TaskDto dto);
        [OperationContract] void UpdateTask(TaskDto dto);
        [OperationContract] void DeleteTask(int taskId);
        [OperationContract] void MarkTaskComplete(int taskId);
        [OperationContract] List<TaskDto> GetTaskHistory(int userId);


    }
}
