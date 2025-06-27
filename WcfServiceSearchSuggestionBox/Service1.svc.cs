using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using todolist_mvvm.Data;

namespace WcfServiceSearchSuggestionBox
{
    public class Service1 : IService1
    {
        public List<string> GetTaskSuggestions(string query, int maxResults)
        {
            if (string.IsNullOrWhiteSpace(query) || maxResults <= 0)
            {
                return new List<string>();
            }

            using (var context = new AppDbContext())
            {
                return context
                    .Tasks.Where(t => t.Name.Contains(query))
                    .Select(t => t.Name)
                    .ToList();
            }
        }
    }
}
