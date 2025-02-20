using Microsoft.AspNetCore.Mvc;
using StudyCenter_BusinessLayer;
using StudyCenter_DataAccessLayer.DTOs.ExamDTOs;

namespace StudyCenter_API.Controllers;

[Route("api/exams")]
[ApiController]
public class ExamsController : ControllerBase
{
    [HttpGet(Name = "GetExams")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<ExamDto>> GetExams()
    {
        var exams = clsExam.All();
        if (exams.Count == 0) return NotFound("No exams found");
        return Ok(exams);
    }
    
    [HttpGet("{id:int}", Name = "GetExamById")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<ExamDto> GetExamById(int id)
    {
        if (id < 1) return BadRequest($"Not accepted ID {id}");
        clsExam? exam = clsExam.Find(id);
        if (exam is null) return NotFound($"Exam with id {id} not found");
        return Ok(exam.ToExamDto());
    }
    
    [HttpGet("exists/{id:int}", Name = "ExistsByExamId")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult ExistsByExamId(int id)
    {
        if (id < 1) return BadRequest($"Not accepted data.");
        if (clsExam.Exists(id)) return Ok(true);
        return NotFound(false);
    }
    
    [HttpGet("exists/{examName}", Name = "ExistsByExamName")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult ExistsByExamName(string examName)
    {
        if (string.IsNullOrWhiteSpace(examName)) return BadRequest($"Not accepted data.");
        if (clsExam.Exists(examName)) return Ok(true);
        return NotFound(false);
    }
    
    [HttpPost(Name = "AddExam")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ExamDto> AddExam(ExamCreationDto? newExam)
    {
        if (newExam is null) return BadRequest($"Not accepted data.");
        clsExam exam = new clsExam
        (
            new ExamDto
            (
                null,
                newExam.SubjectGradeLevelID,
                newExam.ExamName,
                newExam.ExamDate,
                newExam.TotalMarks,
                newExam.PassingMarks
            )
        );

        if (!exam.Save(out string? validationMessage))
        {
            return !string.IsNullOrWhiteSpace(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error adding exam" });
        }

        return CreatedAtRoute("GetExamById", new { id = exam.ExamID },
            exam.ToExamDto());
    }
    
    [HttpPut(Name = "UpdateExam")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ExamDto> UpdateExam(int id, ExamDto? updatedExam)
    {
        if (id < 1 || updatedExam is null) return BadRequest($"Not accepted data.");
        clsExam? exam = clsExam.Find(id);
        if (exam is null) return NotFound($"Exam with id {id} not found");

        exam.SubjectGradeLevelID = updatedExam.SubjectGradeLevelID;
        exam.ExamName = updatedExam.ExamName;
        exam.ExamDate = updatedExam.ExamDate;
        exam.TotalMarks = updatedExam.TotalMarks;
        exam.PassingMarks = updatedExam.PassingMarks;

        if (!exam.Save(out string? validationMessage))
        {
            return !string.IsNullOrEmpty(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error updating exam" });
        }

        return Ok(exam.ToExamDto());
    }
    
    [HttpDelete("{id:int}", Name = "DeleteExam")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult DeleteExam(int id)
    {
        if (id < 1) return BadRequest($"Not accepted data.");
        if (!clsExam.Exists(id)) return NotFound($"Exam with id {id} not found");
        return clsExam.Delete(id)
            ? Ok($"Exam with ID {id} has been deleted.")
            : StatusCode(500, new { message = "Error deleting exam" });
    }
}