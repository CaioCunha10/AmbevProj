using MongoDB.Bson;  
using MongoDB.Driver;  
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ambev.DeveloperEvaluation.WebApi.Services
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;

        public MongoDbService()
        {
            var connectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING");
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("developer_evaluation_db");  
        }

        public IMongoCollection<EventLog> EventLogs => _database.GetCollection<EventLog>("EventLogs");

        public IMongoDatabase GetDatabase()
        {
            return _database;
        }

    }

    public class EventLog
    {
        public ObjectId Id { get; set; }
        public string Event { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
