namespace DeviceApi.Data.Repository.Mongo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using MongoDB.Driver;

    public class MongoContext : IMongoContext
    {
        private IMongoDatabase Database { get; set; }

        public IClientSessionHandle Session { get; set; }

        public MongoClient MongoClient { get; set; }

        private readonly List<Func<Task>> _commands;

        private readonly string mongoConnectionString;

        private readonly string dataBase;

        public MongoContext(string mongoConnectionString, string dataBase)
        {
            this.mongoConnectionString = mongoConnectionString;
            this.dataBase = dataBase;

            // Every command will be stored and it'll be processed at SaveChanges
            _commands = new List<Func<Task>>();
        }



        public async Task<int> SaveChangesAsync()
        {
            ConfigureMongo();

            using (Session = await MongoClient.StartSessionAsync())
            {
                Session.StartTransaction();

                var commandTasks = _commands.Select(c => c());

                await Task.WhenAll(commandTasks);

                await Session.CommitTransactionAsync();
            }

            var commandsCount = _commands.Count;
            _commands.RemoveRange(0, commandsCount);

            return commandsCount;
        }

        private void ConfigureMongo()
        {
            if (MongoClient != null)
                return;

            // Configure mongo (You can inject the config, just to simplify)
            MongoClient = new MongoClient(mongoConnectionString);

            Database = MongoClient.GetDatabase(this.dataBase);

        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            ConfigureMongo();
            return Database.GetCollection<T>(name);
        }

        public void Dispose()
        {
            Session?.Dispose();
            GC.SuppressFinalize(this);
        }

        public void AddCommand(Func<Task> func)
        {
            _commands.Add(func);
        }
    }
}