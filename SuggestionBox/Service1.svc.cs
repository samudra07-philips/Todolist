using System.Collections.Generic;
using System.Linq;
using todolist_mvvm.Bussiness_Layer;
using todolist_mvvm.Data;
namespace SuggestionBox
{
    public class Service1 : IService1
    {
        public List<string> GetSuggestions(string query,int currentUserId)
   {
            using (var context = new AppDbContext()) 
            {
                return context.Tasks
                    .Where(t => t.Name.Contains(query)&& t.UserId == currentUserId)
                    .OrderBy(t => t.Name)
                    .Select(t => t.Name)
                    .Take(10)
                    .ToList();
            }
        }
    }
}
