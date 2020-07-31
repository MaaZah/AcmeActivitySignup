using System;
using System.Collections.Generic;
using AcmeActivitySignup.Data;

namespace AcmeActivitySignup.DTOs
{
    
    public class ActivityInformationDTO
    {
        public int ActivityId { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string ImageUrl { get; set; }
        
        public DateTime Date { get; set; }

        public List<AttendeeDTO> Attendees { get; set; }
    }
}