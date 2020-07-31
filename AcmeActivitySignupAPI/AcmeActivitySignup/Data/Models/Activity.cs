using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AcmeActivitySignup.Data
{
    /// <summary>
    /// Activity informaiton
    /// </summary>
    public class Activity
    {
        [Key]
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string ImageUrl { get; set; }
        
        public DateTime Date { get; set; }
        
        public virtual List<ActivityAttendee> AttendeesSignedUp { get; set; }
    }
    
    
}