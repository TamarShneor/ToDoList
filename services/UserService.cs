using User.Models;
using System.Collections.Generic;
using System.Linq;
//using Task = MyTask.Models.Task;
using user = User.Models.user;
using System.Text.Json;
using User.Interfaces;

namespace User.services
{
    public class UserService:IUserService
    {
        List<user> users { get; }
        private IWebHostEnvironment  webHost;
        private string filePath;
        public UserService(IWebHostEnvironment  webHost)
        {
            this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "data", "User.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                users = JsonSerializer.Deserialize<List<user>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            }
        }

        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(users));
        }
        int nextId = 5;
        public List<user> GetAll() => users;

        public user Get(int id) => users.FirstOrDefault(p => p.Id == id);

        public void Add(user u)
        {
            u.Id = nextId++;
            users.Add(u);
            saveToFile();
        }

        public void Delete(int id)
        {
            var user = Get(id);
 
            if (user is null)
                return;

            users.Remove(user);
            saveToFile();
        }


        public int Count => users.Count();
    }

}