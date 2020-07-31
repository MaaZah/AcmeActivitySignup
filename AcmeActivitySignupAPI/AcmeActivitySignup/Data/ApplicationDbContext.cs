using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AcmeActivitySignup.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        public DbSet<Activity> Activities { get; set; }
        
        public DbSet<Attendee> Attendees { get; set; }
        
        public DbSet<ActivityAttendee> ActivityAttendees { get; set; }
    }
    
    /// <summary>
    /// Designtime dbcontext factory for code-first migrations
    /// </summary>
    public class BloggingContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySql("CONNECTION_STRING_GOES_HERE");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}