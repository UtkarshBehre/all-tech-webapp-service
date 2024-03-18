using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace all_tech_webapp_service.Models.Todo
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RecordType
    {
        TodoItem,
        TodoGroup,
        UserTodo,
        User
    }
}
