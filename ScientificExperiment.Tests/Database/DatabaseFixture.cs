using DAL;
using Microsoft.EntityFrameworkCore;

namespace ScientificExperiment.Tests.Database
{
    public class DatabaseFixture : IDisposable
    {
        public DataContext Context { get; private set; }

        public DatabaseFixture()
        {
            var connectionString = $"DataSource=file:{Guid.NewGuid()}.db?mode=memory&cache=shared";
            var options = new DbContextOptionsBuilder<DataContext>()
                            .UseSqlite(connectionString)
                            .Options;

            Context = new DataContext(options);
            Context.Database.Migrate();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
