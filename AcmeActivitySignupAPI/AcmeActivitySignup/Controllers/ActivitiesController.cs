using System.Collections.Generic;
using System.Threading.Tasks;
using AcmeActivitySignup.DTOs;
using AcmeActivitySignup.Logic;
using Microsoft.AspNetCore.Mvc;

namespace AcmeActivitySignup.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivityService _service;

        public ActivitiesController(IActivityService service)
        {
            _service = service;
        }

        
        [HttpPost("Attend")]
        public async Task<IActionResult> AddAttendeeToActivity([FromQuery]int activityId, [FromBody]AttendeeDTO value)
        {
            var result = await _service.AddAttendeeToActivity(activityId, value);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewActivity([FromBody]List<ActivityInformationDTO> value)
        {
            var result = await _service.CreateActivity(value);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllActivities()
        {
            var result = await _service.GetActivityList();

            return Ok(result);
        }

        [HttpGet("{activityId}")]
        public async Task<IActionResult> GetActivityDetails([FromRoute]int activityId)
        {
            var result = await _service.GetActivityDetails(activityId);

            return Ok(result);
        }

        [HttpGet("Attendee/{attendeeId}")]
        public async Task<IActionResult> GetActivitiesForAttendee([FromRoute]int attendeeId)
        {
            var result = await _service.GetActivitiesForAttendee(attendeeId);

            return Ok(result);
        }
    }
}