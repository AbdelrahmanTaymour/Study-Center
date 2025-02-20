using Microsoft.AspNetCore.Mvc;
using StudyCenter_BusinessLayer;
using StudyCenter_DataAccessLayer.DTOs.StudentGroupDTOs;

namespace StudyCenter_API.Controllers;

[Route("api/studentsGroups")]
[ApiController]
public class StudentsGroupsController : ControllerBase
{
    [HttpGet(Name = "GetStudentsGroups")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<StudentGroupDto>> GetStudentsGroups()
    {
        var studentsGroups = clsStudentGroup.All();
        if (studentsGroups.Count == 0) return NotFound("No student group found");
        return Ok(studentsGroups);
    }
    
    [HttpGet("{id:int}", Name = "GetStudentGroupById")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<StudentGroupDto> GetStudentGroupById(int id)
    {
        if (id < 1) return BadRequest($"Not accepted ID {id}");
        clsStudentGroup? studentGroup = clsStudentGroup.Find(id);
        if (studentGroup is null) return NotFound($"Student group with id {id} not found");
        return Ok(studentGroup.ToStudentGroupDto());
    }
    
    [HttpGet("exists/{id:int}", Name = "ExistsByStudentGroupId")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult ExistsByStudentGroupId(int id)
    {
        if (id < 1) return BadRequest($"Not accepted data.");
        if (clsStudentGroup.Exists(id)) return Ok(true);
        return NotFound(false);
    }
    
    [HttpGet("exists/{studentId:int}-{groupId:int}", Name = "IsStudentAssignedToGroup")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult IsStudentAssignedToGroup(int studentId, int groupId)
    {
        if (studentId < 1 || groupId < 1) return BadRequest($"Not accepted data.");
        if (clsStudentGroup.IsStudentAssignedToGroup(studentId, groupId)) return Ok(true);
        return NotFound(false);
    }
    
    [HttpPost(Name = "AddStudentGroup")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<StudentGroupDto> AddStudentGroup(StudentGroupCreationDto? newStudentGroup)
    {
        if (newStudentGroup is null) return BadRequest($"Not accepted data.");
        clsStudentGroup studentGroup = new clsStudentGroup
        (
            new StudentGroupDto
            (
                null,
                newStudentGroup.StudentId,
                newStudentGroup.GroupId,
                newStudentGroup.StartDate,
                newStudentGroup.EndDate,
                newStudentGroup.IsActive,
                newStudentGroup.CreatedByUserId
            )
        );

        if (!studentGroup.Save(out string? validationMessage))
        {
            return !string.IsNullOrWhiteSpace(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error adding student group" });
        }

        return CreatedAtRoute("GetStudentGroupById", new { id = studentGroup.StudentGroupID },
            studentGroup.ToStudentGroupDto());
    }
    
    [HttpPut(Name = "UpdateStudentGroup")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<StudentGroupDto> UpdateStudentGroup(int id, StudentGroupDto? updatedStudentGroup)
    {
        if (id < 1 || updatedStudentGroup is null) return BadRequest($"Not accepted data.");
        clsStudentGroup? studentGroup = clsStudentGroup.Find(id);
        if (studentGroup is null) return NotFound($"Student group with id {id} not found");

        studentGroup.StudentID = updatedStudentGroup.StudentId;
        studentGroup.GroupID = updatedStudentGroup.GroupId;
        studentGroup.StartDate = updatedStudentGroup.StartDate;
        studentGroup.EndDate = updatedStudentGroup.EndDate;
        studentGroup.IsActive = updatedStudentGroup.IsActive;
        studentGroup.CreatedByUserID = updatedStudentGroup.CreatedByUserId;

        if (!studentGroup.Save(out string? validationMessage))
        {
            return !string.IsNullOrEmpty(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error updating student group" });
        }

        return Ok(studentGroup.ToStudentGroupDto());
    }
    
    [HttpDelete("{id:int}", Name = "DeleteStudentGroupById")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult DeleteStudentGroupById(int id)
    {
        if (id < 1) return BadRequest($"Not accepted data.");
        if (!clsStudentGroup.Exists(id)) return NotFound($"StudentGroup with id {id} not found");
        return clsStudentGroup.Delete(id)
            ? Ok($"StudentGroup with ID {id} has been deleted.")
            : StatusCode(500, new { message = "Error deleting StudentGroup" });
    }
    
    
    [HttpDelete("{studentId:int}-{groupId:int}", Name = "DeleteStudentGroupByStudentIdAndGroupId")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult DeleteStudentGroupByStudentIdAndGroupId(int studentId, int groupId)
    {
        if (studentId < 1 || groupId < 1) return BadRequest($"Not accepted data.");
        if (!clsStudentGroup.IsStudentAssignedToGroup(studentId, groupId)) return NotFound($"Student group with student id {studentId} and group id {groupId} not found");
        return clsStudentGroup.Delete(studentId, groupId)
            ? Ok($"StudentGroup with student id {studentId} and group id {groupId} has been deleted.")
            : StatusCode(500, new { message = "Error deleting StudentGroup" });
    }
    
}