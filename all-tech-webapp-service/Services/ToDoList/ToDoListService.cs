using all_tech_webapp_service.Models.ToDoItem;
using all_tech_webapp_service.Providers;
using all_tech_webapp_service.Repositories.ToDoList;

namespace all_tech_webapp_service.Services.ToDoList
{
    public class ToDoListService : IToDoListService
    {
        private readonly IToDoListRepository _toDoListRepository;
        private readonly IAutoMapperProvider _autoMapperProvider;

        /// <summary>
        /// Constructor for ToDoListService
        /// </summary>
        /// <param name="todoListRepository"></param>
        /// <param name="autoMapperProvider"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ToDoListService(IToDoListRepository todoListRepository, IAutoMapperProvider autoMapperProvider)
        {
            _toDoListRepository = todoListRepository ?? throw new ArgumentNullException(nameof(todoListRepository));
            _autoMapperProvider = autoMapperProvider ?? throw new ArgumentNullException(nameof(autoMapperProvider));
        }

        /// <summary>
        /// Creates a new ToDoItem
        /// </summary>
        /// <param name="toDoItemCreateRequest">toDo Item Create Request</param>
        /// <returns></returns>
        public async Task<ToDoItemResponse> CreateToDoItem(ToDoItemCreateRequest toDoItemCreateRequest)
        {
            var toDoItemRecord = _autoMapperProvider.Mapper.Map<ToDoItemRecord>(toDoItemCreateRequest);
            toDoItemRecord = await _toDoListRepository.CreateToDoItem(toDoItemRecord);
            var toDoItemResponse = _autoMapperProvider.Mapper.Map<ToDoItemResponse>(toDoItemRecord);
            return toDoItemResponse;
        }

        /// <summary>
        /// Gets all ToDoItems
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ToDoItemResponse>> GetAllToDoItems()
        {
            var toDoItemRecords = await _toDoListRepository.GetAllToDoItems();
            var toDoItemResponses = _autoMapperProvider.Mapper.Map<IEnumerable<ToDoItemResponse>>(toDoItemRecords);
            return toDoItemResponses;
        }

        /// <summary>
        /// Gets a ToDoItem by Id
        /// </summary>
        /// <param name="id">ToDo Item Id</param>
        /// <returns></returns>
        public async Task<ToDoItemResponse> GetToDoItem(Guid id)
        {
            var toDoItemRecord = await _toDoListRepository.GetToDoItem(id);
            var toDoItemResponse = _autoMapperProvider.Mapper.Map<ToDoItemResponse>(toDoItemRecord);
            return toDoItemResponse;
        }

        /// <summary>
        /// Updates a ToDoItem
        /// </summary>
        /// <param name="toDoItemUpdateRequest">ToDo Item Update Request</param>
        /// <returns></returns>
        public async Task<ToDoItemResponse> UpdateToDoItem(ToDoItemUpdateRequest toDoItemUpdateRequest)
        {
            var toDoItemRecord = _autoMapperProvider.Mapper.Map<ToDoItemRecord>(toDoItemUpdateRequest);
            toDoItemRecord = await _toDoListRepository.UpdateToDoItem(toDoItemRecord);
            var toDoItemResponse = _autoMapperProvider.Mapper.Map<ToDoItemResponse>(toDoItemRecord);
            return toDoItemResponse;
        }

        /// <summary>
        /// Deletes a ToDoItem by Id
        /// </summary>
        /// <param name="id">ToDo Item Id</param>
        /// <returns></returns>
        public async Task<bool> DeleteToDoItem(Guid id)
        {
            return await _toDoListRepository.DeleteToDoItem(id);
        }
    }
}
