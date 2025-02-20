using Microsoft.AspNetCore.Mvc;
using StudyCenter_BusinessLayer;
using StudyCenter_DataAccessLayer.DTOs.SubjectTeacherDTOs;

namespace StudyCenter_API.Controllers;

[Route("api/subject-teachers")]
[ApiController]
public class SubjectTeachersController : ControllerBase
{
    [HttpGet(Name = "GetSubjectTeachers")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<SubjectTeacherDto>> GetSubjectTeachers()
    {
        var subjectTeachers = clsSubjectTeacher.All();
        if (subjectTeachers.Count == 0) return NotFound("No SubjectTeachers found");
        return Ok(subjectTeachers);
    }
    
    [HttpGet("{id:int}", Name = "GetSubjectTeacherById")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<SubjectTeacherDto> GetSubjectTeacherById(int id)
    {
        if (id < 1) return BadRequest($"Not accepted ID {id}");
        clsSubjectTeacher? subjectTeacher = clsSubjectTeacher.Find(id);
        if (subjectTeacher is null) return NotFound($"SubjectTeacher with id {id} not found");
        return Ok(subjectTeacher.ToSubjectTeacherDto());
    }
    
    [HttpGet("check/{teacherId::int}-{subjectGradeLevelId:int}", Name = "IsTeachingSubject")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult IsTeachingSubject(int teacherId, int subjectGradeLevelId)
    {
        if (teacherId < 1 || subjectGradeLevelId < 1) return BadRequest("Not accepted data");
        if (clsSubjectTeacher.IsTeachingSubject(teacherId,subjectGradeLevelId)) return Ok(true);
        return NotFound(false);
    }
    
    [HttpPost(Name = "AddSubjectTeacher")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<SubjectTeacherDto> AddSubjectTeacher(SubjectTeacherCreationDto? newSubjectTeacher)
    {
        if (newSubjectTeacher is null) return BadRequest($"Not accepted data.");
        clsSubjectTeacher subjectTeacher = new clsSubjectTeacher
        (
            new SubjectTeacherDto
            (
                null,
                newSubjectTeacher.SubjectGradeLevelId,
                newSubjectTeacher.TeacherId,
                DateTime.MinValue, 
                DateTime.Today, 
                newSubjectTeacher.IsActive
            )
        );

        if (!subjectTeacher.Save(out string? validationMessage))
        {
            return !string.IsNullOrWhiteSpace(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error adding SubjectTeacher" });
        }

        return CreatedAtRoute("GetSubjectTeacherById", new { id = subjectTeacher.SubjectTeacherID },
            subjectTeacher.ToSubjectTeacherCreationDto());
    }
    
    [HttpPut(Name = "UpdateSubjectTeacher")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<SubjectTeacherUpdateDto> UpdateSubjectTeacher(int id, SubjectTeacherUpdateDto? updatedSubjectTeacher)
    {
        if (id < 1 || updatedSubjectTeacher is null) return BadRequest($"Not accepted data.");
        clsSubjectTeacher? subjectTeacher = clsSubjectTeacher.Find(id);
        if (subjectTeacher is null) return NotFound($"SubjectTeacher with id {id} not found");

        subjectTeacher.SubjectGradeLevelID = updatedSubjectTeacher.SubjectGradeLevelId;
        subjectTeacher.TeacherID = updatedSubjectTeacher.TeacherId;
        subjectTeacher.IsActive = updatedSubjectTeacher.IsActive;

        if (!subjectTeacher.Save(out string? validationMessage))
        {
            return !string.IsNullOrEmpty(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error updating SubjectTeacher" });
        }

        return Ok(subjectTeacher.ToSubjectTeacherUpdateDto());
    }
    
    [HttpDelete("{id:int}", Name = "DeleteSubjectTeacher")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult DeleteSubjectTeacher(int id)
    {
        if (id < 1) return BadRequest($"Not accepted data.");
        if (!clsSubjectTeacher.Exists(id)) return NotFound($"SubjectTeacher with id {id} not found");
        return clsSubjectTeacher.Delete(id)
            ? Ok($"SubjectTeacher with ID {id} has been deleted.")
            : StatusCode(500, new { message = "Error deleting SubjectTeacher" });
    }
    
    
}