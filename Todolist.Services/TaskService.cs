using System;
using System.Collections.Generic;
using System.Linq;
using todolist_mvvm.Data;
using todolist_mvvm.model;

namespace Todolist.Services
{
    public class TaskService : ITaskService
    {
        public List<TaskDto> GetPendingTasks(int userId)
        {
            //using var db = new AppDbContext();
            using(var db = new AppDbContext())
            {
                return db.Tasks
                    .Where(t => t.UserId == userId && !t.IsCompleted)
                    .Select(t => new TaskDto
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Description = t.Description,
                        taskPriority = t.Priority,
                        IsCompleted = t.IsCompleted,
                        CompletedAt = t.CompletedAt,
                        UserId = t.UserId
                    })
                    .ToList();
            }

        }
        public void AddTask(TaskDto dto)
        {
            using (var db = new AppDbContext())
            {
                db.Tasks.Add(new Tasks
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Priority = dto.taskPriority,
                    UserId = dto.UserId
                });
                db.SaveChanges();
            }
        }
        public void UpdateTask(TaskDto dto)
        {
            using (var db = new AppDbContext())
            {
                var t = db.Tasks.Find(dto.Id);
                if (t != null)
                {
                    t.Name = dto.Name;
                    t.Description = dto.Description;
                    t.Priority = dto.taskPriority;
                    db.SaveChanges();
                }
            }
        }
        public void DeleteTask(int id)
        {
            using (var db = new AppDbContext())
            {
                var t = db.Tasks.Find(id);
                if (t != null)
                {
                    db.Tasks.Remove(t);
                    db.SaveChanges();
                }
            }
        }
        public void MarkCompleted(int id)
        {
            using (var db = new AppDbContext())
            {
                var t = db.Tasks.Find(id);
                if (t != null)
                {
                    t.IsCompleted = true;
                    t.CompletedAt = DateTime.Now;
                    db.SaveChanges();
                }
            }
        }
        public List<TaskDto> GetHistory(int userId)
        {
            using (var db = new AppDbContext())
            {
                return db.Tasks
                    .Where(t => t.UserId == userId && t.IsCompleted)
                    .Select(t => new TaskDto {
                        Id = t.Id,
                        Name = t.Name,
                        Description = t.Description,
                        taskPriority = t.Priority,
                        IsCompleted = t.IsCompleted,
                        CompletedAt = t.CompletedAt,
                        UserId = t.UserId
                    })
                    .ToList();
            }
        }
    }
}
