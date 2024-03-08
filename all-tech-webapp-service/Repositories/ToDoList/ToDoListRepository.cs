using all_tech_webapp_service.Connectors;
using all_tech_webapp_service.Models.ToDoItem;
using AutoMapper;

namespace all_tech_webapp_service.Repositories.ToDoList
{
    public class ToDoListRepository : IToDoListRepository
    {
        private readonly ICosmosDbConnector _cosmosDbConnector;

        public ToDoListRepository(ICosmosDbConnector cosmosDbConnector)
        {
            _cosmosDbConnector = cosmosDbConnector ?? throw new ArgumentNullException(nameof(cosmosDbConnector));
        }

        /// <summary>
        /// Creates new To Do Item
        /// </summary>
        /// <param name="toDoItemRecord">To Do Item Record</param>
        /// <returns></returns>
        public async Task<ToDoItemRecord> CreateToDoItem(ToDoItemRecord toDoItemRecord)
        {
            return await _cosmosDbConnector.CreateItemAsync(toDoItemRecord);
        }

        /// <summary>
        /// Gets all To Do Items
        /// </summary>
        /// <returns></returns>
        public async Task<List<ToDoItemRecord>> GetAllToDoItems()
        {
            var allToDoItems =  await _cosmosDbConnector.ReadItemsAsync<ToDoItemRecord>();
            return allToDoItems.FindAll(x => !x.IsDeleted);
        }

        /// <summary>
        /// Gets a To Do Item by Id
        /// </summary>
        /// <param name="id">To Do Item Id</param>
        /// <returns></returns>
        public async Task<ToDoItemRecord> GetToDoItem(Guid id)
        {
            var toDoItemRecord = await _cosmosDbConnector.ReadItemAsync<ToDoItemRecord>(id.ToString());
            if (toDoItemRecord == null || toDoItemRecord.IsDeleted)
            {
                throw new FileNotFoundException($"No Record Found with the given Id: {id}");
            }
            return toDoItemRecord;
        }

        /// <summary>
        /// Updates a To Do Item
        /// </summary>
        /// <param name="toDoItemRecord">To Do Item Record</param>
        /// <returns></returns>
        public async Task<ToDoItemRecord> UpdateToDoItem(ToDoItemRecord toDoItemRecord)
        {
            var existingToDoItem = await GetToDoItem(toDoItemRecord.Id);
            if (existingToDoItem == null || existingToDoItem.IsDeleted)
            {
                throw new FileNotFoundException($"No Record Found with the given Id: {toDoItemRecord.Id}");
            }
            return await _cosmosDbConnector.UpdateItemAsync(toDoItemRecord, toDoItemRecord.Id.ToString());
        }

        /// <summary>
        /// Deletes a To Do Item
        /// </summary>
        /// <param name="id">To Do Item Id</param>
        /// <returns></returns>
        public async Task<bool> DeleteToDoItem(Guid id)
        {
            var toDoItemRecord = await GetToDoItem(id);
            if (toDoItemRecord == null || toDoItemRecord.IsDeleted)
            {
                throw new FileNotFoundException($"No Record Found with the given Id: {id}");
            }
            toDoItemRecord.IsDeleted = true;
            await UpdateToDoItem(toDoItemRecord);
            return true;
        }
    }
}
