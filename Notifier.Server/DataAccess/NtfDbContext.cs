using Microsoft.EntityFrameworkCore;
using Notifier.Server.Entities;

namespace Notifier.Server.DataAccess
{
    public class NtfDbContext : DbContext
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="options"></param>
        public NtfDbContext(DbContextOptions<NtfDbContext> options) : base(options)
        { }

        /// <summary>
        /// NtfUser entity
        /// </summary>
        public DbSet<NtfUser> User { get; set; }
    }
}
