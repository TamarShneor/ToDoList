using System.Collections.Generic;
using MyTask.Models;
using Task = MyTask.Models.Task;

namespace MyTask.Interfaces
{
    public interface ITaskService
    {
        List<Task> GetTasksByUserId(int id);
        Task Get(int id);
        void Add(Task t);
        void Delete(int id);
        void Update(Task t);
        int Count {get;}
        
    }


}