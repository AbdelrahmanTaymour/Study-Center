using Microsoft.AspNetCore.Mvc;
using StudyCenter_BusinessLayer;
using StudyCenter_DataAccessLayer.DTOs.AttendanceDTOs;

namespace StudyCenter_API.Controllers;

[Route("api/Attendance")]
[ApiController]
public class AttendanceController : ControllerBase
{
    [HttpGet(Name = "GetAttendances")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<AttendanceDto>> GetAttendances()
    {
        var attendances = clsAttendance.All();
        if (attendances.Count == 0) return NotFound("No Attendances found");
        return Ok(attendances);
    }
    
    [HttpGet("{id:int}", Name = "GetAttendanceById")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<AttendanceDto> GetAttendanceById(int id)
    {
        if (id < 1) return BadRequest($"Not accepted ID {id}");
        clsAttendance? attendance = clsAttendance.Find(id);
        if (attendance is null) return NotFound($"Attendance with id {id} not found");
        return Ok(attendance.ToAttendanceDto());
    }
    
    [HttpGet("exists/{id:int}", Name = "ExistsByAttendanceId")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult ExistsByAttendanceId(int id)
    {
        if (id < 1) return BadRequest($"Not accepted data.");
        if (clsAttendance.Exists(id)) return Ok(true);
        return NotFound(false);
    }
    
    [HttpPost(Name = "AddAttendance")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<AttendanceDto> AddAttendance(AttendanceCreationDto? newAttendance)
    {
        if (newAttendance is null) return BadRequest($"Not accepted data.");
        clsAttendance attendance = new clsAttendance
        (
            new AttendanceDto
            (
                null,
                newAttendance.StudentGroupId,
                DateTime.MinValue, 
                newAttendance.Notes
            )
        );

        if (!attendance.Save(out string? validationMessage))
        {
            return !string.IsNullOrWhiteSpace(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error adding Attendance" });
        }

        return CreatedAtRoute("GetAttendanceById", new { id = attendance.AttendanceID },
            attendance.ToAttendanceDto());
    }
    
    [HttpPut(Name = "UpdateAttendance")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<AttendanceDto> UpdateAttendance(int id, AttendanceDto? updatedAttendance)
    {
        if (id < 1 || updatedAttendance is null) return BadRequest($"Not accepted data.");
        clsAttendance? attendance = clsAttendance.Find(id);
        if (attendance is null) return NotFound($"Attendance with id {id} not found");

        attendance.StudentGroupID = updatedAttendance.StudentGroupId;
        attendance.Notes = updatedAttendance.Notes;

        if (!attendance.Save(out string? validationMessage))
        {
            return !string.IsNullOrEmpty(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error updating Attendance" });
        }

        return Ok(attendance.ToAttendanceDto());
    }
    
    [HttpDelete("{id:int}", Name = "DeleteAttendance")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult DeleteAttendance(int id)
    {
        if (id < 1) return BadRequest($"Not accepted data.");
        if (!clsAttendance.Exists(id)) return NotFound($"Attendance with id {id} not found");
        return clsAttendance.Delete(id)
            ? Ok($"Attendance with ID {id} has been deleted.")
            : StatusCode(500, new { message = "Error deleting Attendance" });
    }
}