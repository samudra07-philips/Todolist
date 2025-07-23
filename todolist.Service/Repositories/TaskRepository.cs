using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todolist.Services.Contracts;
using Todolist.Services.Data;

namespace Todolist.Services.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _ctx;
        public TaskRepository(AppDbContext ctx) => _ctx = ctx;

        public IEnumerable<TaskDto> GetPending(int userId) =>
            _ctx.Tasks
                .Where(t => t.UserId == userId && !t.IsCompleted)
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    UserId = t.UserId,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                  
                    CompletedAt = t.CompletedAt
                })
                .ToList();

        public void Add(TaskDto dto)
        {
            var entity = new Tasks
            {
                UserId = dto.UserId,
                Title = dto.Title,
                Description = dto.Description,
                IsCompleted = false,
                
                CompletedAt = null
            };
            _ctx.Tasks.Add(entity);
            _ctx.SaveChanges();
        }

        public void Update(TaskDto dto)
        {
            var entity = _ctx.Tasks.Find(dto.Id);
            if (entity == null) return;

            entity.Title = dto.Title;
            entity.Description = dto.Description;
            _ctx.SaveChanges();
        }

        public void Delete(int taskId)  
        {
            var entity = _ctx.Tasks.Find(taskId);
            if (entity == null) return;

            _ctx.Tasks.Remove(entity);
            _ctx.SaveChanges();
        }

        public void MarkComplete(int taskId)
        {
            var entity = _ctx.Tasks.Find(taskId);
            if (entity == null) return;

            entity.IsCompleted = true;
            entity.CompletedAt = DateTime.UtcNow;

            _ctx.SaveChanges();
        }

        public IEnumerable<TaskDto> GetHistory(int userId) =>
            _ctx.Tasks
                .Where(t => t.UserId == userId && t.IsCompleted)
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    UserId = t.UserId,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    
                    CompletedAt = t.CompletedAt
                })
                .ToList();
    }
}
