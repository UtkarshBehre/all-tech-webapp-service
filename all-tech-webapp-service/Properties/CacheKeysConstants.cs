namespace all_tech_webapp_service.Properties
{
    public class CacheKeysConstants
    {
        // user cache keys
        public const string KEY_USER_ALL = "user/all";
        public const string KEY_USER_ID = "user/id/{0}";
        public const string KEY_USER_EMAIL = "user/email/{0}";

        // user todo cache keys
        public const string KEY_ALL_USER_TODOS = "user_todo/all_user_todos";
        public const string KEY_USER_TODO_ID = "user_todo/id/{0}";

        // todo group cache keys
        public const string KEY_TODO_GROUP_ID = "todo_group/id/{0}";

        // todo item cache keys
        public const string KEY_TODO_ITEM_ALL = "todo_item/all";
        public const string KEY_TODO_ITEM_ID = "todo_item/id/{0}";
    }
}
