using IEnge.Controllers.Api;
using IEnge.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IEnge.Database
{
    public class DatabaseContext : DbContext
    {

        private readonly ILoggerFactory _loggerFactory;

        public DatabaseContext(DbContextOptions<DatabaseContext> options, ILoggerFactory loggerFactory) : base(options)
        {
            _loggerFactory = loggerFactory;
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<AutomationProject> AutomationProjects{ get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
