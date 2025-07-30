using System.Collections.ObjectModel;
using Todolist.Services;
using todolist_mvvm.Bussiness_Layer;
using todolist_mvvm.model;
using todolist_mvvm.Helpers;
namespace todolist_mvvm.viewmodel
{
    public class HistoryViewModel : BaseViewModel
    {
        private readonly ITaskService _taskService;

        public ObservableCollection<Tasks> CompletedCriticalTasks { get; set; }
        public ObservableCollection<Tasks> CompletedHighTasks { get; set; }
        public ObservableCollection<Tasks> CompletedMediumTasks { get; set; }
        public ObservableCollection<Tasks> CompletedLowTasks { get; set; }

        public HistoryViewModel(ITaskService taskService)
        {

            CompletedCriticalTasks = new ObservableCollection<Tasks>();
            CompletedHighTasks = new ObservableCollection<Tasks>();
            CompletedMediumTasks = new ObservableCollection<Tasks>();
            CompletedLowTasks = new ObservableCollection<Tasks>();

            _taskService = taskService;
            LoadTasks();
        }

        private void LoadTasks()
        {
            var taskDtos = _taskService.GetTaskHistory(CurrentUser.Id);

            foreach (var dto in taskDtos)
            {
                var t = TaskMapper.FromDto(dto); // Maps TaskDto → Tasks model

                switch (t.Priority)
                {
                    case TaskPriority.Low:
                        CompletedLowTasks.Add(t);
                        break;
                    case TaskPriority.Medium:
                        CompletedMediumTasks.Add(t);
                        break;
                    case TaskPriority.High:
                        CompletedHighTasks.Add(t);
                        break;
                    case TaskPriority.Critical:
                        CompletedCriticalTasks.Add(t);
                        break;
                }
            }
        }
    }
}
