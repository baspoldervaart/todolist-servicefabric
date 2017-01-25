using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Shared.Models;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;

namespace TodolistStatefulService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class TodolistStatefulService : StatefulService, ITodolistStatefulService
    {
        public TodolistStatefulService(StatefulServiceContext context)
            : base(context)
        { }

        internal async Task<IReliableDictionary<int, TodoItem>> GetTodoListItemsDictionairy()
        {
            return await this.StateManager.GetOrAddAsync<IReliableDictionary<int, TodoItem>>("TodoListItems");
        }

        public async Task Create(TodoItem todoItem)
        {
            var myDictionary = await GetTodoListItemsDictionairy();
            using (var tx = this.StateManager.CreateTransaction())
            {
                await myDictionary.AddAsync(tx, todoItem.Id, todoItem);

                // If an exception is thrown before calling CommitAsync, the transaction aborts, all changes are
                // discarded, and nothing is saved to the secondary replicas.
                await tx.CommitAsync();
            }
        }

        public async Task Delete(int id)
        {
            var myDictionary = await GetTodoListItemsDictionairy();
            using (var tx = this.StateManager.CreateTransaction())
            {
                await myDictionary.TryRemoveAsync(tx, id);

                // If an exception is thrown before calling CommitAsync, the transaction aborts, all changes are
                // discarded, and nothing is saved to the secondary replicas.
                await tx.CommitAsync();
            }
        }

        public async Task<TodoItem> GetTodoItem(int id)
        {
            var myDictionary = await GetTodoListItemsDictionairy();
            using (var tx = this.StateManager.CreateTransaction())
            {
                var item = await myDictionary.TryGetValueAsync(tx, id);
                return item.Value;
            }
        }

        public async Task<IEnumerable<TodoItem>> GetTodoItems()
        {
            var myDictionary = await GetTodoListItemsDictionairy();
            using (var tx = this.StateManager.CreateTransaction())
            {
                var todoItems = new List<TodoItem>();
                var enumerable = await myDictionary.CreateEnumerableAsync(tx);
                var enumerator = enumerable.GetAsyncEnumerator();

                while (await enumerator.MoveNextAsync(CancellationToken.None))
                {
                    todoItems.Add(enumerator.Current.Value);
                }

                return todoItems;
            }
        }

        public async Task Update(int id, TodoItem todoItem)
        {
            var myDictionary = await GetTodoListItemsDictionairy();
            using (var tx = this.StateManager.CreateTransaction())
            {
                if (await myDictionary.ContainsKeyAsync(tx, id))
                {
                    var oldTodoItem = await myDictionary.TryGetValueAsync(tx, id);
                    await myDictionary.TryUpdateAsync(tx, id, todoItem, oldTodoItem.Value);
                }

                // If an exception is thrown before calling CommitAsync, the transaction aborts, all changes are
                // discarded, and nothing is saved to the secondary replicas.
                await tx.CommitAsync();
            }
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            // The CreateServiceRemotingListener is being used to accept remote requests from other services (eg. WebAPI)
            return new[] { new ServiceReplicaListener(context => this.CreateServiceRemotingListener(context)) };
        }
    }
}