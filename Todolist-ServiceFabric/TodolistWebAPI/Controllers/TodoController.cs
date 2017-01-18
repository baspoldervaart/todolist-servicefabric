using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TodolistWebAPI.Models;

namespace TodolistWebAPI.Controllers
{
    public class TodoController : ApiController
    {
        // GET api/todo
        public IEnumerable<TodoItem> Get()
        {
            //return new string[] { "value1", "value2" };
            throw new NotImplementedException();
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
