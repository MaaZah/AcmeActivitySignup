using System;
using AcmeActivitySignup.Data;
using Microsoft.EntityFrameworkCore;

namespace AcmeActivitySignupTests
{
    public class TestBase
    {
        public ApplicationDbContext GetEmptyDbContext()
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            
            return new ApplicationDbContext(builder.Options);
        }
    }
}