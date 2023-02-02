using MyTask.Models;
using System.Collections.Generic;
using System.Linq;
using MyTask.Interfaces;
using Task = MyTask.Models.Task;
using System.Text.Json;

namespace MyTask.services
{
    public class TaskService:ITaskService
    {
        List<Task>? Tasks { get; }
        private IWebHostEnvironment  webHost;
        private string filePath;
        public TaskService(IWebHostEnvironment  webHost)
        {
               this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "data", "Task.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                Tasks = JsonSerializer.Deserialize<List<Task>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            }
        }

        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(Tasks));
        }
        int nextId = 4;
        // public List<Task> GetAll() => Tasks;

        public Task Get(int id) => Tasks.FirstOrDefault(p => p.Id == id);

        public void Add(Task task)
        {
            task.Id = nextId++;
            
            Tasks.Add(task);
            saveToFile();
            
        }

        public void Delete(int id)
        {
            var task = Get(id);
            if (task is null)
                return;

            Tasks.Remove(task);
            saveToFile();
        }
        

        public void Update(Task task)
        {
            var index = Tasks.FindIndex(p => p.Id == task.Id);
            if (index == -1)
                return;

            Tasks[index] = task;
            saveToFile();
        }

        public int Count => Tasks.Count();

        public List<Task> GetTasksByUserId(int id){
            return Tasks.FindAll(t=>t.UserId==id); 
        }

    }

}