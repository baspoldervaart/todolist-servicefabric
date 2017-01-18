using Microsoft.ServiceFabric.Services.Remoting;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodolistStatefulService
{
    public interface ITodolistStatefulService  : IService
    {
        Task<IEnumerable<TodoItem>> GetTodoItems();
        Task<TodoItem> GetTodoItem(int id);
        Task Create(TodoItem todoItem);
        Task Update(int id, TodoItem todoItem);
        Task Delete(int id);
    }
}
