using Microsoft.AspNetCore.Mvc;
using StudyCenter_BusinessLayer;
using StudyCenter_DataAccessLayer.DTOs.StudentDTOs;

namespace StudyCenter_API.Controllers;

[Route("api/students")]
[ApiController]
public class StudentsController : ControllerBase
{
    [HttpGet(Name = "GetAllStudents")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<StudentDto> GetAllStudents()
    {
        var students = clsStudent.All();
        if (students is null || students.Count == 0) return NotFound("No students found");
        return Ok(students);
    }

    [HttpGet("{studentId}", Name = "GetStudentById")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<StudentDto> GetStudentById(int studentId)
    {
        if (studentId < 1) return BadRequest($"Not accepted ID {studentId}");
        clsStudent? student = clsStudent.FindById(studentId);
        if (student is null) return NotFound($"Student  with id {studentId} not found");
        return Ok(student.ToStudentDto());
    }

    [HttpGet("person/{personId}", Name = "GetStudentsByPersonId")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<StudentDto> GetStudentsByPersonId(int personId)
    {
        if (personId < 1) return BadRequest($"Not accepted ID {personId}");
        clsStudent? student = clsStudent.FindByPersonId(personId);
        if (student is null) return NotFound($"Student  with person id {personId} not found");
        return Ok(student.ToStudentDto());
    }

    [HttpGet("exists/student-id/{studentId}", Name = "ExistsByStudentId")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult ExistsByStudentId(int studentId)
    {
        if (studentId < 1) return BadRequest($"Not accepted ID {studentId}");
        if (clsStudent.Exists(studentId)) return Ok(true);
        return NotFound(false);
    }

    [HttpGet("check/person/{personId}", Name = "IsStudent")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult IsStudent(int personId)
    {
        if (personId < 1) return BadRequest($"Not accepted ID {personId}");
        if (clsStudent.IsStudent(personId)) return Ok(true);
        return NotFound(false);
    }

    [HttpPost(Name = "AddStudent")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<StudentDto> AddStudent(StudentCreationDto? newStudent)
    {
        if (newStudent is null) return BadRequest($"Not accepted data.");
        clsStudent student = new clsStudent
        (
            new StudentDto
            (
                null,
                newStudent.PersonId,
                newStudent.GradeLevelId,
                newStudent.Status,
                newStudent.Notes,
                newStudent.CreatedByUserId,
                null
            )
        );

        if (!student.Save(out string? validationMessage))
        {
            return !string.IsNullOrEmpty(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error adding student" });
        }

        return CreatedAtRoute("GetStudentById", new {studentId = student.StudentID}, student.ToStudentDto());
    }

    [HttpPut(Name = "UpdateStudent")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<StudentDto> UpdateStudent(int? studentId, StudentUpdateDto? updatedStudent)
    {
        if (updatedStudent is null || studentId < 1) return BadRequest($"Not accepted data.");
        clsStudent? student = clsStudent.FindById(studentId);
        if (student is null) return NotFound($"Student with id {studentId} not found");
        
        student.PersonID = updatedStudent.PersonId;
        student.GradeLevelID = updatedStudent.GradeLevelId;
        student.Status = updatedStudent.Status;
        student.Notes = updatedStudent.Notes;

        if (!student.Save(out string? validationMessage))
        {
            return !string.IsNullOrEmpty(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error updating student" });
        }
        
        return Ok(student.ToStudentDto());
    }

    [HttpDelete("{studentId}-{deletedByUserId}", Name = "DeleteStudent")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult DeleteStudent(int? studentId, int? deletedByUserId)
    {
        if ((studentId is null || deletedByUserId is null)  || (studentId < 1 || deletedByUserId < 1)) 
            return BadRequest("Not accepted data");
        
        if(!clsStudent.Exists(studentId) || !clsUser.ExistByUserID(deletedByUserId)) return NotFound($"Student/User with id {studentId} not found");
        
        return clsStudent.Delete(studentId, deletedByUserId) 
            ? Ok($"Student with ID {studentId} has been deleted.") 
            : StatusCode(500, new { message = "Error deleting user" });
    }
}