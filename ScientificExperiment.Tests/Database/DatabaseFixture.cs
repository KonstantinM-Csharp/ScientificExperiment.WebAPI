using DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
