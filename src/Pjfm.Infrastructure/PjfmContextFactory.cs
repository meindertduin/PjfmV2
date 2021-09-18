using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Pjfm.Infrastructure
{
    public static class PjfmContextFactory
    {
        public static PjfmContext Create(string connectionString)
        {
            var options = new DbContextOptionsBuilder<PjfmContext>()
                .UseSqlServer(new SqlConnection(connectionString))
                .Options;
            
            return new PjfmContext(options);
        }
    }
}