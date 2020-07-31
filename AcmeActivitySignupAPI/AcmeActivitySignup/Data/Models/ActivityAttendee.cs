using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcmeActivitySignup.Data
{
    /// <summary>
    /// Facilitate many-to-many relation between activities and attendees
    /// </summary>
    public class ActivityAttendee
    {
        [Key]
        public int Id { get; set; }
        public int ActivityId { get; set; }
        
        public int AttendeeId { get; set; }
        
        public string Comments { get; set; }
        
        [ForeignKey("ActivityId")]
        public Activity Activity { get; set; }
        
        [ForeignKey("AttendeeId")]
        public Attendee Attendee { get; set; }
    }
}