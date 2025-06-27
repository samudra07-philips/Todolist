using System.Collections.Generic;
using System.ServiceModel;

[ServiceContract]
public interface IService1
{
    
    [OperationContract]
    List<string> GetTaskSuggestions(string query, int maxResults);
}
