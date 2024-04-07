using all_tech_webapp_service.Connectors;
using System.Linq.Expressions;
using all_tech_webapp_service.Models.User;
using all_tech_webapp_service.Models.Todo;

namespace all_tech_webapp_service.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly ICosmosDbConnector _cosmosDbConnector;

        public UserRepository(ICosmosDbConnector cosmosDbConnector)
        {
            _cosmosDbConnector = cosmosDbConnector ?? throw new ArgumentNullException(nameof(cosmosDbConnector));
        }

        public async Task<UserRecord> CreateUser(UserRecord user)
        {
            return await _cosmosDbConnector.CreateItemAsync(user, user.RecordType);
        }

        public async Task<List<UserRecord>> GetAllUsers()
        {
            Expression<Func<UserRecord, bool>> predicate = x
                => x.RecordType == RecordType.User &&
                   !x.IsDeleted;
            ;
            var allUsers = await _cosmosDbConnector.ReadItemsAsync<UserRecord>(RecordType.User, predicate);
            return allUsers;
        }

        public async Task<UserRecord> GetUser(Guid id)
        {
            var userRecord = await _cosmosDbConnector.ReadItemAsync<UserRecord>(id.ToString(), id.ToString(), RecordType.User);
            if (userRecord == null || userRecord.IsDeleted)
            {
                throw new FileNotFoundException($"No User Record Found with the given Id: {id}");
            }
            return userRecord;
        }

        public async Task<UserRecord> GetUserByEmailId(string email)
        {
            Expression<Func<UserRecord, bool>> predicate = x
                => x.RecordType == RecordType.User &&
                   x.Email == email &&
                   !x.IsDeleted;
            ;
            var userRecords = await _cosmosDbConnector.ReadItemsAsync<UserRecord>(RecordType.User, predicate);
            var userRecord = userRecords?.FirstOrDefault();
            if (userRecord == null)
            {
                throw new FileNotFoundException($"No User Record Found with the given Email: {email}");
            }
            return userRecord;
        }

        public async Task<UserRecord> GetUserByGoogleId(string id)
        {
            Expression<Func<UserRecord, bool>> predicate = x
                => x.RecordType == RecordType.User &&
                   x.GoogleId == id &&
                   !x.IsDeleted;

            var userRecords = await _cosmosDbConnector.ReadItemsAsync<UserRecord>(RecordType.User, predicate);

            var userRecord = userRecords.FirstOrDefault();
            if (userRecord == null || userRecord.IsDeleted)
            {
                throw new FileNotFoundException($"No User Record Found with the given Id: {id}");
            }
            return userRecord;
        }

        public async Task<UserRecord> UpdateUser(UserRecord user)
        {
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
