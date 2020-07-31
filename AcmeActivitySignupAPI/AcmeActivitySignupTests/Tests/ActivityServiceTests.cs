using System;
using System.Collections.Generic;
using System.Linq;
using AcmeActivitySignup;
using AcmeActivitySignup.Data;
using AcmeActivitySignup.DTOs;
using AcmeActivitySignup.Logic;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AcmeActivitySignupTests.Tests
{
    public class ActivityServiceTests : TestBase
    {
        [Fact]
        public async void ShouldCreateNewActivity()
        {
            //Arrange
            await using var context = this.GetEmptyDbContext();
            
            var attendeeService = new AttendeeService(context);
            var activityService = new ActivityService(context, attendeeService);
            
            //Act
            await activityService.CreateActivity(new List<ActivityInformationDTO>()
            {new ActivityInformationDTO()
                {
                    Title = "test activity",
                    Description = "this is a test",
                    Date = DateTime.UtcNow
                }
            });
            
            //Assert
            Assert.Equal(1, context.Activities.Count());
        }

        [Fact]
        public async void ShouldGetTwoActivities()
        {
            //Arrange
            await using var context = this.GetEmptyDbContext();
            
            var attendeeService = new AttendeeService(context);
            var activityService = new ActivityService(context, attendeeService);

            await context.Activities.AddAsync(new Activity()
            {
                Title = "test activity",
                Description = "this is a test",
                Date = DateTime.UtcNow
            });
            
            await context.Activities.AddAsync(new Activity()
            {
                Title = "Second test activity",
                Description = "this is another test",
                Date = DateTime.UtcNow
            });

            await context.SaveChangesAsync();
            

            //Act
            var result = await activityService.GetActivityList();
            
            //Assert
            Assert.Equal(2, result.Count());
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public async void ShouldGetMatchingActivityId(int activityId)
        {
            //Arrange
            await using var context = this.GetEmptyDbContext();
            
            var attendeeService = new AttendeeService(context);
            var activityService = new ActivityService(context, attendeeService);

            await context.Activities.AddAsync(new Activity()
            {
                Title = "test activity",
                Description = "this is a test",
                Date = DateTime.UtcNow,
                Id = activityId
            });

            await context.SaveChangesAsync();
            

            //Act
            var result = await activityService.GetActivityDetails(activityId);
            
            //Assert
            Assert.Equal(activityId, result.ActivityId);
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public async void GetActivityDetailsShouldThrowItemNotFoundException(int activityId)
        {
            //Arrange
            await using var context = this.GetEmptyDbContext();
            
            var attendeeService = new AttendeeService(context);
            var activityService = new ActivityService(context, attendeeService);

            await context.Activities.AddAsync(new Activity()
            {
                Title = "test activity",
                Description = "this is a test",
                Date = DateTime.UtcNow,
                Id = 99
            });

            await context.SaveChangesAsync();

            //Act & Assert
            await Assert.ThrowsAsync<ItemNotFoundException>(() => activityService.GetActivityDetails(activityId));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void ShouldAddAttendeeToActivity(int activityCount)
        {
            //Arrange
            await using var context = this.GetEmptyDbContext();
            
            var attendeeService = new AttendeeService(context);
            var activityService = new ActivityService(context, attendeeService);

            for (int i = 1; i <= activityCount; i++)
            {
                await context.Activities.AddAsync(new Activity()
                {
                    Title = "test activity",
                    Description = "this is a test",
                    Date = DateTime.UtcNow,
                    Id = i
                });
            }
            
            await context.SaveChangesAsync();
            
            //Act
            string attendeeEmail = "testEmail@test.com";

            for (int i = 1; i <= activityCount; i++)
            {
                await activityService.AddAttendeeToActivity(i, new AttendeeDTO()
                {
                    Email = attendeeEmail,
                    FirstName = "John",
                    LastName = "Doe"
                });
            }

           
            
            //Assert

            Assert.Equal(activityCount, context.ActivityAttendees
                .Include(x => x.Attendee).Count(x => string.Equals(x.Attendee.Email, attendeeEmail, StringComparison.InvariantCultureIgnoreCase)));
        }
        
        
        [Fact]
        public async void AddAttendeeToActivityShouldThrowItemNotFoundException()
        {
            //Arrange
            int activityId = 1;
            await using var context = this.GetEmptyDbContext();
            
            var attendeeService = new AttendeeService(context);
            var activityService = new ActivityService(context, attendeeService);

            await context.Activities.AddAsync(new Activity()
            {
                Title = "test activity",
                Description = "this is a test",
                Date = DateTime.UtcNow,
                Id = activityId
            });
            await context.SaveChangesAsync();
            
            //Act & Assert
            await Assert.ThrowsAsync<ItemNotFoundException>( () => activityService.AddAttendeeToActivity(99, new AttendeeDTO()
            {
                Email = "testEmail@test.com",
                FirstName = "John",
                LastName = "Doe"
            }));
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void ShouldGetAllActivitiesForAttendee(int activityCount)
        {
            //Arrange
            await using var context = this.GetEmptyDbContext();
            
            var attendeeService = new AttendeeService(context);
            var activityService = new ActivityService(context, attendeeService);

            await context.Attendees.AddAsync(new Attendee()
            {
                Email = "testEmail@test.com",
                FirstName = "John",
                LastName = "Doe",
                Id = 1
            });

            for (int i = 1; i <= activityCount; i++)
            {
                await context.Activities.AddAsync(new Activity()
                {
                    Title = "test activity",
                    Description = "this is a test",
                    Date = DateTime.UtcNow,
                    Id = i
                });

                context.ActivityAttendees.Add(new ActivityAttendee()
                {
                    AttendeeId = 1,
                    ActivityId = i
                });
            }
            
            await context.SaveChangesAsync();
            
            //Act
            var result = await activityService.GetActivitiesForAttendee(1);


            //Assert
            Assert.Equal(activityCount, result.Count());
        }
    }
}