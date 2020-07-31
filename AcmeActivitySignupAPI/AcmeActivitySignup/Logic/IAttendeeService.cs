using System;
using System.Linq;
using System.Threading.Tasks;
using AcmeActivitySignup.Data;
using AcmeActivitySignup.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AcmeActivitySignup.Logic
{
    public interface IAttendeeService
    {
        /// <summary>
        /// Check to see if datastore already contains an entry for an attendee with a specific email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<bool> CheckIfAttendeeExistsByEmail(string email);
        
        /// <summary>
        /// Create a new attendee in the datastore
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> CreateNewAttendee(AttendeeDTO value);
    }

    public class AttendeeService : IAttendeeService
    {
        private readonly ApplicationDbContext _context;

        public AttendeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Use email to check if a user has already been saved
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> CheckIfAttendeeExistsByEmail(string email)
        {
            //attempt to get user from dbcontext
            Attendee attendee = await _context.Attendees
                .Where(x => string.Equals(x.Email, email, StringComparison.InvariantCultureIgnoreCase))
                .SingleOrDefaultAsync();
            
            if (attendee == null)
            {
                //attendee does not exist
                return false;
            }

            //attendee exists
            return true;
        }

        /// <summary>
        /// Create a new attendee in the context
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> CreateNewAttendee(AttendeeDTO value)
        {
            //map DTO to entity
            Attendee newAttendee = new Attendee()
            {
                Email = value.Email,
                FirstName = value.FirstName,
                LastName = value.LastName
            };

            //save entity
            _context.Attendees.Add(newAttendee);
            await _context.SaveChangesAsync();
            
            return true;
        }
    }
}