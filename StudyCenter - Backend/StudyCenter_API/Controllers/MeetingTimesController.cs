using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using StudyCenter_BusinessLayer;
using StudyCenter_DataAccessLayer.DTOs.MeetingTimeDTOs;

namespace StudyCenter_API.Controllers;

[Route("api/meetingTimes")]
[ApiController]
public class MeetingTimesController : ControllerBase
{
    [HttpGet(Name = "GetMeetingTimes")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<MeetingTimeDto>> GetMeetingTimes()
    {
        var meetingTimes = clsMeetingTime.All();
        if (meetingTimes.Count == 0) return NotFound("No Meeting Times found");
        
        return Ok(meetingTimes);
    }
    
    [HttpGet("{id:int}", Name = "GetMeetingTimeById")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<MeetingTimeDto> GetMeetingTimeById(int id)
    {
        if (id < 1) return BadRequest($"Not accepted ID {id}");
        clsMeetingTime? meetingTime = clsMeetingTime.Find(id);
        if (meetingTime is null) return NotFound($"MeetingTime with id {id} not found");
        return Ok(meetingTime.ToMeetingTimeDto());
    }
    
    [HttpGet("exists/{id:int}", Name = "ExistsByMeetingTimeId")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult ExistsByMeetingTimeId(int id)
    {
        if (id < 1) return BadRequest($"Not accepted data.");
        if (clsMeetingTime.Exists(id)) return Ok(true);
        return NotFound(false);
    }
    
    [HttpGet("exists{startTime:datetime}-{meetingDays:int}", Name = "ExistsByMeetingTimeStartTime")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult ExistsByStartTimeAndMeetingDays(DateTime startTime, byte meetingDays)
    {
        if (meetingDays < 0) return BadRequest($"Not accepted data.");
        if (clsMeetingTime.Exists(startTime.TimeOfDay, meetingDays)) return Ok(true);
        return NotFound(false);
    }
    
    [HttpPost(Name = "AddMeetingTime")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<MeetingTimeDto> AddMeetingTime(MeetingTimeCreationDto? newMeetingTime)
    {
        if (newMeetingTime is null) return BadRequest($"Not accepted data.");
        clsMeetingTime meetingTime = new clsMeetingTime
        (
            new MeetingTimeDto
            (
                null,
                newMeetingTime.StartTime,
                newMeetingTime.EndTime,
                newMeetingTime.MeetingDays
            )
        );

        if (!meetingTime.Save(out string? validationMessage))
        {
            return !string.IsNullOrWhiteSpace(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error adding Meeting Time" });
        }

        return CreatedAtRoute("GetMeetingTimeById", new { id = meetingTime.MeetingTimeID },
            meetingTime.ToMeetingTimeDto());
    }
    
    [HttpPut(Name = "UpdateMeetingTime")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<MeetingTimeDto> UpdateMeetingTime(int id, MeetingTimeDto? updatedMeetingTime)
    {
        if (id < 1 || updatedMeetingTime is null) return BadRequest($"Not accepted data.");
        clsMeetingTime? meetingTime = clsMeetingTime.Find(id);
        if (meetingTime is null) return NotFound($"MeetingTime with id {id} not found");

        meetingTime.StartTime = updatedMeetingTime.StartTime;
        meetingTime.EndTime = updatedMeetingTime.EndTime;
        meetingTime.MeetingDays = updatedMeetingTime.MeetingDays;

        if (!meetingTime.Save(out string? validationMessage))
        {
            return !string.IsNullOrEmpty(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error updating Meeting Time" });
        }

        return Ok(meetingTime.ToMeetingTimeDto());
    }
    
    [HttpDelete("{id:int}", Name = "DeleteMeetingTime")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult DeleteMeetingTime(int id)
    {
        if (id < 1) return BadRequest($"Not accepted data.");
        if (!clsMeetingTime.Exists(id)) return NotFound($"Meeting Time with id {id} not found");
        return clsMeetingTime.Delete(id)
            ? Ok($"MeetingTime with ID {id} has been deleted.")
            : StatusCode(500, new { message = "Error deleting Meeting Time" });
    }
    
}