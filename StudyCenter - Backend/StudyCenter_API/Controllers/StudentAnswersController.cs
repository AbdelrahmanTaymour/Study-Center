using Microsoft.AspNetCore.Mvc;
using StudyCenter_BusinessLayer;
using StudyCenter_DataAccessLayer.DTOs.StudentAnswerDTOs;

namespace StudyCenter_API.Controllers;

[Route("api/studentAnswers")]
[ApiController]
public class StudentAnswersController : ControllerBase
{
    [HttpGet(Name = "GetAllStudentAnswers")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<StudentAnswerDto>> GetAllStudentAnswers()
    {
        var studentAnswers = clsStudentAnswer.All();
        if (studentAnswers.Count == 0) return NotFound("No student answers found");
        return Ok(studentAnswers);
    }
    
    [HttpGet("all/{examResultId:int}", Name = "GetAllStudentAnswerPerExamResult")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<IEnumerable<StudentAnswerDto>> GetAllStudentAnswerPerExamResult(int examResultId)
    {
        if (examResultId < 1) return BadRequest($"Not accepted ID {examResultId}");
        var studentAnswers = clsStudentAnswer.AllPerExamResult(examResultId);
        if (studentAnswers.Count == 0) return NotFound("No student answers found");
        return Ok(studentAnswers);
    }
    
    [HttpGet("{id:int}", Name = "GetStudentAnswerById")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<StudentAnswerDto> GetStudentAnswerById(int id)
    {
        if (id < 1) return BadRequest($"Not accepted ID {id}");
        clsStudentAnswer? studentAnswer = clsStudentAnswer.Find(id);
        if (studentAnswer is null) return NotFound($"Student answer with id {id} not found");
        return Ok(studentAnswer.ToStudentAnswerDto());
    }
    
    [HttpGet("exists/{id:int}", Name = "ExistsByStudentAnswerId")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult ExistsByStudentAnswerId(int id)
    {
        if (id < 1) return BadRequest($"Not accepted data.");
        if (clsStudentAnswer.Exists(id)) return Ok(true);
        return NotFound(false);
    }
    
    [HttpGet("check/{examResultId:int}-{questionId:int}", Name = "IsAnswered")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult IsAnswered(int examResultId, int questionId)
    {
        if (examResultId < 1 || questionId < 1) return BadRequest($"Not accepted data.");
        if (clsStudentAnswer.IsAnswered(examResultId, questionId)) return Ok(true);
        return NotFound(false);
    }
    
    [HttpPost(Name = "AddStudentAnswer")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<StudentAnswerCreationDto> AddStudentAnswer(StudentAnswerCreationDto? newStudentAnswer)
    {
        if (newStudentAnswer is null) return BadRequest($"Not accepted data.");
        clsStudentAnswer studentAnswer = new clsStudentAnswer
        (
            new StudentAnswerDto
            (
                null,
                newStudentAnswer.ExamResultID,
                newStudentAnswer.QuestionID,
                newStudentAnswer.AnswerID,
                newStudentAnswer.AnswerText,
                newStudentAnswer.MarksAwarded
            )
        );

        if (!studentAnswer.Save(out string? validationMessage))
        {
            return !string.IsNullOrWhiteSpace(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error adding student answer" });
        }

        return CreatedAtRoute("GetStudentAnswerById", new { id = studentAnswer.StudentAnswerID },
            studentAnswer.ToStudentAnswerDto());
    }
    
    [HttpPut(Name = "UpdateStudentAnswer")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<StudentAnswerDto> UpdateStudentAnswer(int id, StudentAnswerDto? updatedStudentAnswer)
    {
        if (id < 1 || updatedStudentAnswer is null) return BadRequest($"Not accepted data.");
        clsStudentAnswer? studentAnswer = clsStudentAnswer.Find(id);
        if (studentAnswer is null) return NotFound($"Student answer with id {id} not found");

        studentAnswer.ExamResultID = updatedStudentAnswer.ExamResultID;
        studentAnswer.QuestionID = updatedStudentAnswer.QuestionID;
        studentAnswer.AnswerID = updatedStudentAnswer.AnswerID;
        studentAnswer.AnswerText = updatedStudentAnswer.AnswerText;
        studentAnswer.MarksAwarded = updatedStudentAnswer.MarksAwarded;

        if (!studentAnswer.Save(out string? validationMessage))
        {
            return !string.IsNullOrEmpty(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error updating student answer" });
        }

        return Ok(studentAnswer.ToStudentAnswerDto());
    }
    
    [HttpDelete("{id:int}", Name = "DeleteStudentAnswer")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult DeleteStudentAnswer(int id)
    {
        if (id < 1) return BadRequest($"Not accepted data.");
        if (!clsStudentAnswer.Exists(id)) return NotFound($"Student answer with id {id} not found");
        return clsStudentAnswer.Delete(id)
            ? Ok($"Student answer with ID {id} has been deleted.")
            : StatusCode(500, new { message = "Error deleting Student answer" });
    }
}