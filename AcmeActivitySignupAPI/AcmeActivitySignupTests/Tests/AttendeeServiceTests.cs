using System.Linq;
using AcmeActivitySignup.Data;
using AcmeActivitySignup.DTOs;
using AcmeActivitySignup.Logic;
using Xunit;

namespace AcmeActivitySignupTests
{
    public class AttendeeServiceTests : TestBase
    {

        [Fact]
        public async void ShouldCreateNewAttendee()
        {
            await using var context = this.GetEmptyDbContext();
            //Arrange
            var service = new AttendeeService(context);
                
            //Act
            var result = await service.CreateNewAttendee(new AttendeeDTO()
            {
                Email = "testEmail@test.com",
                FirstName = "John",
                LastName = "Doe",
            });
                
            //Assert
            Assert.Equal(1, context.Attendees.Count());
        }


        [Fact]
        public async void ShouldAddDuplicateAttendees()
        {
            await using var context = this.GetEmptyDbContext();
            //Arrange
            context.Attendees.Add(new Attendee()
            {
                Email = "testEmail@test.com",
                FirstName = "John",
                LastName = "Doe",
                Id = 1
            });
            await context.SaveChangesAsync();
            var service = new AttendeeService(context);
                
                
            //Act
            var result = await service.CreateNewAttendee(new AttendeeDTO()
            {
                Email = "testEmail@test.com",
                FirstName = "John",
                LastName = "Doe",
            });
                
            //Assert
            Assert.Equal(2, context.Attendees.Count());
        }
        
        [Fact]
        public async void ShouldReturnUserDoesExist()
        {
            await using var context = this.GetEmptyDbContext();
            //Arrange
            await context.Attendees.AddAsync(new Attendee()
            {
                Email = "testEmail@test.com",
                FirstName = "John",
                LastName = "Doe",
                Id = 1
            });
            await context.SaveChangesAsync();
            var service = new AttendeeService(context);
                
                
            //Act
            var result = await service.CheckIfAttendeeExistsByEmail("testEmail@test.com");
                
            //Assert
            Assert.True(result);
        }
        
        [Fact]
        public async void ShouldReturnUserDoesNotExist()
        {
            await using var context = this.GetEmptyDbContext();
            //Arrange
            await context.Attendees.AddAsync(new Attendee()
            {
                Email = "testEmail@test.com",
                FirstName = "John",
                LastName = "Doe",
                Id = 1
            });
            await context.SaveChangesAsync();
            var service = new AttendeeService(context);
                
                
            //Act
            var result = await service.CheckIfAttendeeExistsByEmail("differentTestEmail@test.com");
                
            //Assert
            Assert.False(result);
        }
    }
}