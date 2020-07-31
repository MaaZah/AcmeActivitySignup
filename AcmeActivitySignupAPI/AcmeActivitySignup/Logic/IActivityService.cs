using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AcmeActivitySignup.Data;
using AcmeActivitySignup.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AcmeActivitySignup.Logic
{
    public interface IActivityService
    {
        /// <summary>
        /// Add an attendee to an activity
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> AddAttendeeToActivity(int activityId, AttendeeDTO value);
        /// <summary>
        /// Get a list of all activites from a datastore
        /// </summary>
        /// <returns></returns>
        Task<List<ActivityInformationDTO>> GetActivityList();

        /// <summary>
        /// Add a new activity to the datastore
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        Task<bool> CreateActivity(List<ActivityInformationDTO> values);

        /// <summary>
        /// Get Details & attendees list for a specific activity from a datastore
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        /// <exception cref="ItemNotFoundException"></exception>
        Task<ActivityInformationDTO> GetActivityDetails(int activityId);

        /// <summary>
        /// Get a list of Activities attended by attendee
        /// </summary>
        /// <param name="attendeeId"></param>
        /// <returns></returns>
        Task<List<ActivityInformationDTO>> GetActivitiesForAttendee(int attendeeId);
    }

    public class ActivityService : IActivityService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAttendeeService _attendeeService;

        public ActivityService(ApplicationDbContext context, IAttendeeService attendeeService)
        {
            _context = context;
            _attendeeService = attendeeService;
        }

        /// <summary>
        /// Add an attendee to an activity
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ItemNotFoundException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<bool> AddAttendeeToActivity(int activityId, AttendeeDTO value)
        {
            //get activity from context
            Activity activity = await _context.Activities.Include(x => x.AttendeesSignedUp).ThenInclude(y => y.Attendee)
                .Where(x => x.Id == activityId).SingleOrDefaultAsync();

            //throw if activity doesnt exist
            if (activity == null)
            {
                throw new ItemNotFoundException();
            }

            //check if attendee has already signed up for activity
            if (activity.AttendeesSignedUp.Any(x =>
                String.Equals(x.Attendee.Email, value.Email, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new InvalidOperationException("User has already signed up for this event");
            }

            //check if attendee exists
            if (!await _attendeeService.CheckIfAttendeeExistsByEmail(value.Email))
            {
                await _attendeeService.CreateNewAttendee(value);
            }

            //get attendee from context
            Attendee attendee = await _context.Attendees.Where(x => x.Email == value.Email).SingleOrDefaultAsync();

            //throw if attendee is null for some reason
            if (attendee == null)
            {
                throw new ItemNotFoundException();
            }
            
            //add attendee to list of attendees for activity
            activity.AttendeesSignedUp.Add(new ActivityAttendee()
            {
                AttendeeId = attendee.Id,
                ActivityId = activity.Id,
                Comments = value.Comments
            });
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Get a list of all activities from the DbContext
        /// </summary>
        /// <returns></returns>
        public async Task<List<ActivityInformationDTO>> GetActivityList()
        {
            //get all activities from context
            List<Activity> allActivities =
                await _context.Activities.ToListAsync();

            
            //map entities to DTO
            List<ActivityInformationDTO> result = new List<ActivityInformationDTO>();
            foreach (var act in allActivities)
            {
                result.Add(new ActivityInformationDTO()
                {
                    Title = act.Title,
                    Description = act.Description,
                    Date = act.Date,
                    ActivityId = act.Id,
                    ImageUrl = act.ImageUrl
                });
            }
            
            return result;
        }

        /// <summary>
        /// Get a list of Activities attended by a specific attendee
        /// </summary>
        /// <param name="attendeeId"></param>
        /// <returns></returns>
        public async Task<List<ActivityInformationDTO>> GetActivitiesForAttendee(int attendeeId)
        {
            //get list of activities that this attendee has, well.. attended
            List<Activity> activities =
                await _context.Activities.Where(x => x.AttendeesSignedUp.Any(y => y.Attendee.Id == attendeeId)).ToListAsync();

            //map entity to DTO
            List<ActivityInformationDTO> result = new List<ActivityInformationDTO>();
            foreach (var act in activities)
            {
                result.Add(new ActivityInformationDTO()
                {
                    ActivityId = act.Id,
                    Date = act.Date,
                    Title = act.Title,
                    Description = act.Description,
                    ImageUrl = act.ImageUrl
                });
            }

            return result;
        }

        /// <summary>
        /// Get Details & attendees list for a specific activity from a dbcontext
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        /// <exception cref="ItemNotFoundException"></exception>
        public async Task<ActivityInformationDTO> GetActivityDetails(int activityId)
        {
            //get activity & attendees from context
            Activity activity = await _context.Activities.Include(x => x.AttendeesSignedUp).ThenInclude(y => y.Attendee).SingleOrDefaultAsync(x => x.Id == activityId);

            //throw if not found
            if (activity == null)
            {
                throw new ItemNotFoundException("Activity does not exist");
            }
            
            //map entity to DTO
            ActivityInformationDTO result = new ActivityInformationDTO()
            {
                ActivityId = activity.Id,
                Attendees = new List<AttendeeDTO>(),
                Title = activity.Title,
                Description = activity.Description,
                Date = activity.Date,
                ImageUrl = activity.ImageUrl
            };

            //add attendees
            foreach (var att in activity.AttendeesSignedUp)
            {
                result.Attendees.Add(new AttendeeDTO()
                {
                    FirstName = att.Attendee.FirstName,
                    LastName = att.Attendee.LastName,
                    Email = att.Attendee.Email,
                    Comments = att.Comments
                });
            }

            return result;
        }


        /// <summary>
        /// Add a new activity to the context
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task<bool> CreateActivity(List<ActivityInformationDTO> values)
        {
            foreach (var value in values)
            {
                Activity newActivity = new Activity()
                {
                    Title = value.Title,
                    Date = value.Date,
                    Description = value.Description,
                    ImageUrl = value.ImageUrl
                };

                //add to context
                await _context.Activities.AddAsync(newActivity);
            }
            //save
            await _context.SaveChangesAsync();

            return true;
        }
    }
}