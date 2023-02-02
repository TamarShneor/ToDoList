using System.Collections.Generic;
using User.Models;
using Task = MyTask.Models.Task;
 using User = User.Models.user;
namespace User.Interfaces
{
    public interface IUserService
    {
        List<user> GetAll();
        user Get(int id);
       void Add(user t); 
        void Delete(int id);
        int Count {get;}
    }

}