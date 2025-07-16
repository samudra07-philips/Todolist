using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Todolist.Services.Contracts;
namespace Todolist.Services
{
    public interface ITaskService
    {
        [OperationContract] List<TaskDto> GetPendingTasks(int userId);
        [OperationContract] void AddTask(TaskDto task);
        [OperationContract] void UpdateTask(TaskDto task);
        [OperationContract] void DeleteTask(int taskId);
        [OperationContract] void MarkCompleted(int taskId);
        [OperationContract] List<TaskDto> GetHistory(int userId);
    }
}
