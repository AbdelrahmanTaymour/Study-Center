using Microsoft.AspNetCore.Mvc;
using StudyCenter_BusinessLayer;
using StudyCenter_DataAccessLayer.DTOs.ExamResultDTOs;

namespace StudyCenter_API.Controllers;

[Route("api/examResults")]
[ApiController]
public class ExamResultsController : ControllerBase
{
    [HttpGet(Name = "GetExamResults")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<ExamResultDto>> GetExamResults()
    {
        var examResults = clsExamResult.All();
        if (examResults.Count == 0) return NotFound("No exam results found");
        return Ok(examResults);
    }
    
    [HttpGet("all/{studentId:int}", Name = "GetStudentExamResults")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<IEnumerable<ExamResultDto>> GetStudentExamResults(int studentId)
    {
        if (studentId < 1) return BadRequest($"Not accepted ID {studentId}");
        var studentResults = clsExamResult.GetStudentExamResult(studentId);
        if (studentResults.Count == 0) return NotFound("No exam results found");
        return Ok(studentResults);
    }
    
    [HttpGet("{id:int}", Name = "GetExamResultById")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<ExamResultDto> GetExamResultById(int id)
    {
        if (id < 1) return BadRequest($"Not accepted ID {id}");
        clsExamResult? examResult = clsExamResult.Find(id);
        if (examResult is null) return NotFound($"Exam result with id {id} not found");
        return Ok(examResult.ToExamResultDto());
    }
    
    [HttpGet("exists/{id:int}", Name = "ExistsByExamResultId")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult ExistsByExamResultId(int id)
    {
        if (id < 1) return BadRequest($"Not accepted data.");
        if (clsExamResult.Exists(id)) return Ok(true);
        return NotFound(false);
    }
    
    [HttpPost(Name = "AddExamResult")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ExamResultCreationDto> AddExamResult(ExamResultCreationDto? newExamResult)
    {
        if (newExamResult is null) return BadRequest($"Not accepted data.");
        clsExamResult examResult = new clsExamResult
        (
            new ExamResultDto
            (
                null,
                newExamResult.ExamID,
                newExamResult.StudentID,
                newExamResult.MarksObtained
            )
        );

        if (!examResult.Save(out string? validationMessage))
        {
            return !string.IsNullOrWhiteSpace(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error adding exam result" });
        }

        return CreatedAtRoute("GetExamResultById", new { id = examResult.ExamResultID },
            examResult.ToExamResultDto());
    }
    
    [HttpPut(Name = "UpdateExamResult")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ExamResultDto> UpdateExamResult(int id, ExamResultDto? updatedExamResult)
    {
        if (id < 1 || updatedExamResult is null) return BadRequest($"Not accepted data.");
        clsExamResult? examResult = clsExamResult.Find(id);
        if (examResult is null) return NotFound($"Exam result with id {id} not found");

        examResult.ExamID = updatedExamResult.ExamID;
        examResult.StudentID = updatedExamResult.StudentID;
        examResult.MarksObtained = updatedExamResult.MarksObtained;

        if (!examResult.Save(out string? validationMessage))
        {
            return !string.IsNullOrEmpty(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error updating exam result" });
        }

        return Ok(examResult.ToExamResultDto());
    }
    
    [HttpDelete("{id:int}", Name = "DeleteExamResult")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult DeleteExamResult(int id)
    {
        if (id < 1) return BadRequest($"Not accepted data.");
        if (!clsExamResult.Exists(id)) return NotFound($"Exam result with id {id} not found");
        return clsExamResult.Delete(id)
            ? Ok($"Exam result with ID {id} has been deleted.")
            : StatusCode(500, new { message = "Error deleting Exam result" });
    }
}