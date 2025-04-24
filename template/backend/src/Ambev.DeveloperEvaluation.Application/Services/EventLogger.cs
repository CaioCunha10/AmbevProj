using System.Threading.Tasks;
using MongoDB.Bson;
using Ambev.DeveloperEvaluation.WebApi.Services;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.WebApi.Services
{
    public class EventLogger
    {
        private readonly IMongoCollection<EventLog> _eventCollection;

        public EventLogger(MongoDbService mongoDbService)
        {
            _eventCollection = mongoDbService.EventLogs;
        }

        public async Task LogEventAsync(string eventType, string message)
        {
            var log = new EventLog
            {
                Event = eventType,
                Message = message,
                Timestamp = DateTime.UtcNow
            };

            await _eventCollection.InsertOneAsync(log);
        }
    }
}
