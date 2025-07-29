using System;
using Todolist.Services.Contracts;
using todolist_mvvm.model;

namespace todolist_mvvm.Helpers
{
    public static class TaskMapper
    {
        /// <summary>
        /// Maps a service-side TaskDto into the client-side Tasks model.
        /// </summary>
        public static Tasks FromDto(TaskDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new Tasks
            {
                Id = dto.Id,
                UserId = dto.UserId,
                Name = dto.Title,
                Description = dto.Description,
                IsCompleted = dto.IsCompleted,
                CompletedAt = dto.CompletedAt,
                Priority = (TaskPriority)Enum.Parse(typeof(TaskPriority), dto.Priority.ToString())
            };
        }
    }
}
