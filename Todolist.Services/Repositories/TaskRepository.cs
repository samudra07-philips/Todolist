using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todolist.Services.Contracts;

using todolist_mvvm.Data;
using todolist_mvvm.model;

namespace Todolist.Services.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _ctx;

        public TaskRepository(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<TaskDto> GetPending(int userId)
        {
            return _ctx.Tasks
                .Where(t => t.UserId == userId && !t.IsCompleted)
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    UserId = t.UserId,
                    Name = t.Name,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                  
                    CompletedAt = t.CompletedAt
                })
                .ToList();
        }

        public void Add(TaskDto dto)
        { 
            _ctx.Tasks.Add(new Tasks
            {
                Id = dto.UserId,
                Name = dto.Name,
                Description = dto.Description,
                
            });
            _ctx.SaveChanges();
        }

        public void Update(TaskDto dto)
        {
            var entity = _ctx.Tasks.Find(dto.Id);
            if (entity != null)
            {
                entity.Name = dto.Name;
                entity.Description = dto.Description;
                entity.IsCompleted = dto.IsCompleted;
                entity.CompletedAt = dto.CompletedAt;
                _ctx.SaveChanges();
            }
        }

        public void Delete(int taskId)
        {
            var entity = _ctx.Tasks.Find(taskId);
            if (entity != null)
            {
                _ctx.Tasks.Remove(entity);
                _ctx.SaveChanges();
            }
        }

        public void MarkComplete(int taskId)
        {
            var entity = _ctx.Tasks.Find(taskId);
            if (entity != null)
            {
                entity.IsCompleted = true;
                entity.CompletedAt = DateTime.UtcNow;
                _ctx.SaveChanges();
            }
        }

        public IEnumerable<TaskDto> GetHistory(int userId)
        {
            return _ctx.Tasks
                .Where(t => t.UserId == userId && t.IsCompleted)
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    UserId = t.UserId,
                    Name = t.Name,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                   
                    CompletedAt = t.CompletedAt
                })
                .ToList();
        }

       
    }
}
