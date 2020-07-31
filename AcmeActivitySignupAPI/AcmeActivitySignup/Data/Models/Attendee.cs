using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AcmeActivitySignup.Data
{
    /// <summary>
    /// Employee information
    /// </summary>
    public class Attendee
    {
        [Key]
        public int Id { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string Email { get; set; }
        
        public virtual List<ActivityAttendee> ActivitiesSignedUpFor { get; set; }
    }
}