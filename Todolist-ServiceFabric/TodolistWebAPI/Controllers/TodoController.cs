using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using TodolistStatefulService;

namespace TodolistWebAPI.Controllers
{
    public class TodoController : ApiController
    {
        // GET api/todo
        public async Task<IEnumerable<TodoItem>> Get()
        {
            // Deze partitionkey laten staan, is nodig om het te laten werken.
            var partition = new ServicePartitionKey(1); //provide the partitionKey for stateful services. for stateless services, you can just comment this out
            ITodolistStatefulService todolistStatefulServiceClient = ServiceProxy.Create<ITodolistStatefulService>(new Uri("fabric:/Todolist_ServiceFabric/TodolistStatefulService"), partition);

            var todoItems = await todolistStatefulServiceClient.GetTodoItems();

            return todoItems;
        }

        // GET api/todo/5
        public TodoItem Get(int id)
        {
            throw new NotImplementedException();
        }

        // POST api/todo
        public void Post([FromBody]TodoItem value)
        {
        }

        // PUT api/todo/5
        public void Put(int id, [FromBody]TodoItem value)
        {
        }

        // DELETE api/todo/5
        public void Delete(int id)
        {
        }
    }
}