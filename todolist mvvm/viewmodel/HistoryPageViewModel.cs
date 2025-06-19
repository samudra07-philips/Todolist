using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using todolist_mvvm.Data;
using todolist_mvvm.model;

namespace todolist_mvvm.viewmodel
{
    public class HistoryViewModel : BaseViewModel
    {   
        public ObservableCollection<Tasks> CompletedCriticalTasks { get; set; }
        public ObservableCollection<Tasks> CompletedHighTasks { get; set; }
        public ObservableCollection<Tasks> CompletedMediumTasks { get; set; }
        public ObservableCollection<Tasks> CompletedLowTasks { get; set; }

        public HistoryViewModel()
        {
           
            CompletedCriticalTasks = new ObservableCollection<Tasks>();
            CompletedHighTasks = new ObservableCollection<Tasks>();
            CompletedMediumTasks = new ObservableCollection<Tasks>();
            CompletedLowTasks = new ObservableCollection<Tasks>();
            LoadTasks();
        }
        private void LoadTasks()
        {
            using (var context = new AppDbContext())
            {
                var tasks = context
                    .Tasks.Where(t => t.IsCompleted) 
                    .ToList();

                foreach (var t in tasks)
                {
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
}
