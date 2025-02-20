using Microsoft.AspNetCore.Mvc;
using StudyCenter_BusinessLayer;
using StudyCenter_DataAccessLayer.DTOs.TeacherDTOs;

namespace StudyCenter_API.Controllers;

[Route("api/teachers")]
[ApiController]
public class TeachersController : ControllerBase
{
    [HttpGet(Name = "GetTeachers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<TeacherDto>> GetTeachers()
    {
        var teachers = clsTeacher.All();
        if(teachers is null || teachers.Count == 0) return NotFound("No teachers found.");
        return Ok(teachers);
    }

    [HttpGet("{id:int}", Name = "GetTeacherById")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<TeacherDto> GetTeacherById(int id)
    {
        if (id < 1) return BadRequest($"Not accepted ID {id}");
        clsTeacher? teacher = clsTeacher.FindByTeacherId(id);
        if (teacher is null) return NotFound($"Teacher with id {id} not found");
        return Ok(teacher.ToTeacherDto());
    }
    
    [HttpGet("person/{id:int}", Name = "GetTeacherByPersonId")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<TeacherDto> GetTeacherByPersonId(int id)
    {
        if (id < 1) return BadRequest($"Not accepted ID {id}");
        clsTeacher? teacher = clsTeacher.FindByPersonId(id);
        if (teacher is null) return NotFound($"Teacher with id {id} not found");
        return Ok(teacher.ToTeacherDto());
    }

    [HttpGet("fullname/{id:int}", Name = "GetTeacherFullName")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult GetTeacherFullName(int id)
    {
        if (id < 1) return BadRequest($"Not accepted ID {id}");
        var fullName = clsTeacher.GetFullName(id);
        if (fullName is not null) return Ok(fullName);
        return NotFound($"Teacher with id {id} not found");
    }

    [HttpGet("exists/{id:int}", Name = "ExistsByTeacherId")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult ExistsByTeacherId(int id)
    {
        if (id < 1) return BadRequest($"Not accepted ID {id}");
        return clsTeacher.Exists(id) 
            ? Ok(true) 
            : NotFound(false);
    }
    
    
    [HttpPost(Name = "AddTeacher")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<TeacherDto> AddTeacher(TeacherCreationDto? newTeacher)
    {
        if (newTeacher is null) return BadRequest($"Not accepted data.");
        clsTeacher? teacher = new clsTeacher
        (
            new TeacherDto
            (
                null,
                newTeacher.PersonId,
                newTeacher.EducationLevelId,
                newTeacher.TeachingExperience,
                newTeacher.Certifications,
                newTeacher.Status,
                newTeacher.Notes,
                newTeacher.CreatedByUserId
            )
        );

        if (!teacher.Save(out string? validationMessage))
        {
            return !string.IsNullOrEmpty(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error adding teacher" });
        }

        return CreatedAtRoute("GetTeacherById", new { id = teacher.TeacherID },
            teacher.ToTeacherDto());
    }

    [HttpPut("{id:int}",Name = "UpdateTeacher")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<TeacherDto> UpdateTeacher(int id, TeacherUpdateDto updatedTeacher)
    {
        if (id < 1) return BadRequest($"Not accepted data.");
        clsTeacher? teacher = clsTeacher.FindByTeacherId(id);
        if (teacher is null) return NotFound($"Teacher with id {id} not found");

        teacher.PersonID = updatedTeacher.PersonId;
        teacher.EducationLevelID = updatedTeacher.EducationLevelId;
        teacher.TeachingExperience = updatedTeacher.TeachingExperience;
        teacher.Certifications = updatedTeacher.Certifications;
        teacher.Status = updatedTeacher.Status;
        teacher.Notes = updatedTeacher.Notes;

        if (!teacher.Save(out string? validationMessage))
        {
            return !string.IsNullOrEmpty(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error updating education level" });
        }

        return Ok(teacher.ToTeacherDto());
    }

    [HttpDelete("{id:int}-{deletedByUserId}", Name = "DeleteTeacher")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult DeleteTeacher(int id, int deletedByUserId)
    {
        if (id < 1) return BadRequest($"Not accepted data.");
        if (!clsTeacher.Exists(id)) return NotFound($"Teacher with id {id} not found");
        return clsTeacher.Delete(id,deletedByUserId)
            ? Ok($"Teacher with ID {id} has been deleted.")
            : StatusCode(500, new { message = "Error deleting teacher" });
    }
    
    
}