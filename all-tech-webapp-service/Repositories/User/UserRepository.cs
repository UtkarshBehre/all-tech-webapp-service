using all_tech_webapp_service.Connectors;
using System.Linq.Expressions;
using all_tech_webapp_service.Models.User;
using all_tech_webapp_service.Models.Todo;
using all_tech_webapp_service.Providers.Cache;
using all_tech_webapp_service.Properties;

namespace all_tech_webapp_service.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly ICosmosDbConnector _cosmosDbConnector;
        private readonly ICacheProvider _cacheProvider;

        public UserRepository(ICosmosDbConnector cosmosDbConnector, ICacheProvider cacheProvider)
        {
            _cosmosDbConnector = cosmosDbConnector ?? throw new ArgumentNullException(nameof(cosmosDbConnector));
            _cacheProvider = cacheProvider ?? throw new ArgumentNullException(nameof(cacheProvider));
        }

        public async Task<UserRecord> CreateUser(UserRecord user)
        {
            await _cacheProvider.Delete(CacheKeysConstants.KEY_USER_ALL);
            var userRecord = await _cosmosDbConnector.CreateItemAsync(user, user.RecordType);
            return userRecord;
        }

        public async Task<List<UserRecord>> GetAllUsers()
        {
            var key = CacheKeysConstants.KEY_USER_ALL;
            var allUsers = await _cacheProvider.Get<List<UserRecord>>(key);

            if (allUsers != null && allUsers.Count != 0)
            {
                return allUsers;
            }

            Expression<Func<UserRecord, bool>> predicate = x
                => x.RecordType == RecordType.User &&
                   !x.IsDeleted;
            
            allUsers = await _cosmosDbConnector.ReadItemsAsync<UserRecord>(RecordType.User, predicate);
            await _cacheProvider.Set(key, allUsers);
            return allUsers;
        }

        public async Task<UserRecord> GetUser(Guid id)
        {
            var key = string.Format(CacheKeysConstants.KEY_USER_ID, id);
            var userRecord = await _cacheProvider.Get<UserRecord>(key);
            if (userRecord != null)
            {
                return userRecord;
            }

            userRecord = await _cosmosDbConnector.ReadItemAsync<UserRecord>(id.ToString(), id.ToString(), RecordType.User);
            if (userRecord == null || userRecord.IsDeleted)
            {
                throw new FileNotFoundException($"No User Record Found with the given Id: {id}");
            }

            await _cacheProvider.Set(key, userRecord);
            return userRecord;
        }

        public async Task<UserRecord> GetUserByEmailId(string email)
        {
            var key = string.Format(CacheKeysConstants.KEY_USER_EMAIL, email);
            var userRecord = await _cacheProvider.Get<UserRecord>(key);

            if (userRecord != null)
            {
                return userRecord;
            }

            Expression<Func<UserRecord, bool>> predicate = x
                => x.RecordType == RecordType.User &&
                   x.Email == email.ToLowerInvariant() &&
                   !x.IsDeleted;
            ;
            var userRecords = await _cosmosDbConnector.ReadItemsAsync<UserRecord>(RecordType.User, predicate);
            userRecord = userRecords?.FirstOrDefault();
            if (userRecord == null)
            {
                throw new FileNotFoundException($"No User Record Found with the given Email: {email}");
            }
            await _cacheProvider.Set(key, userRecord);
            return userRecord;
        }

        public async Task<UserRecord> GetUserByGoogleId(string id)
        {
            var key = string.Format(CacheKeysConstants.KEY_USER_ID, id);
            var userRecord = await _cacheProvider.Get<UserRecord>(key);

            if (userRecord != null)
            {
                return userRecord;
            }

            Expression<Func<UserRecord, bool>> predicate = x
                => x.RecordType == RecordType.User &&
                   x.GoogleId == id &&
                   !x.IsDeleted;

            var userRecords = await _cosmosDbConnector.ReadItemsAsync<UserRecord>(RecordType.User, predicate);

            userRecord = userRecords.FirstOrDefault();
            if (userRecord == null || userRecord.IsDeleted)
            {
                throw new FileNotFoundException($"No User Record Found with the given Id: {id}");
            }
            await _cacheProvider.Set(key, userRecord);
            return userRecord;
        }

        public async Task<UserRecord> UpdateUser(UserRecord user)
        {
            await _cacheProvider.Delete(string.Format(CacheKeysConstants.KEY_USER_ID, user.Id));
            await _cacheProvider.Delete(string.Format(CacheKeysConstants.KEY_USER_EMAIL, user.Email));
            await _cacheProvider.Delete(string.Format(CacheKeysConstants.KEY_USER_ID, user.GoogleId));
            await _cacheProvider.Delete(CacheKeysConstants.KEY_USER_ALL);
            var existingUser = await GetUser(user.Id);
            if (existingUser == null || existingUser.IsDeleted)
            {
                throw new FileNotFoundException($"No User Record Found with the given Id: {user.Id}");
            }
            return await _cosmosDbConnector.UpdateItemAsync(user, user.Id.ToString(), user.RecordType, user.Etag);
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            var userRecord = await GetUser(id);
            if (userRecord == null || userRecord.IsDeleted)
            {
                throw new FileNotFoundException($"No User  Record Found with the given Id: {id}");
            }

            userRecord.IsDeleted = true;
            await UpdateUser(userRecord);
            return true;
        }
    }
}
