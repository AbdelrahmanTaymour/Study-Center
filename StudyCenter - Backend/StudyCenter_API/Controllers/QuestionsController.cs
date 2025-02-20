using Microsoft.AspNetCore.Mvc;
using StudyCenter_BusinessLayer;
using StudyCenter_DataAccessLayer.DTOs.QuestionDTOs;

namespace StudyCenter_API.Controllers;

[Route("api/questions")]
[ApiController]
public class QuestionsController : ControllerBase
{
    [HttpGet(Name = "GetQuestions")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<QuestionDto>> GetQuestions()
    {
        var questions = clsQuestion.AllQuestions();
        if (questions.Count == 0) return NotFound("No questions found");
        return Ok(questions);
    }
    
    [HttpGet("all/{examId:int}", Name = "GetExamQuestions")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<IEnumerable<QuestionDto>> GetExamQuestions(int examId)
    {
        if (examId < 1) return BadRequest($"Not accepted ID {examId}");
        var questions = clsQuestion.AllExamQuetions(examId);
        if (questions.Count == 0) return NotFound("No questions found");
        return Ok(questions);
    }
    
    
    [HttpGet("{id:int}", Name = "GetQuestionById")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<QuestionDto> GetQuestionById(int id)
    {
        if (id < 1) return BadRequest($"Not accepted ID {id}");
        clsQuestion? question = clsQuestion.Find(id);
        if (question is null) return NotFound($"Question with id {id} not found");
        return Ok(question.ToQuestionDto());
    }
    
    [HttpGet("exists/{id:int}", Name = "ExistsByQuestionId")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult ExistsByQuestionId(int id)
    {
        if (id < 1) return BadRequest($"Not accepted data.");
        if (clsQuestion.Exists(id)) return Ok(true);
        return NotFound(false);
    }
    
    [HttpPost(Name = "AddQuestion")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<QuestionCreationDto> AddQuestion(QuestionCreationDto? newQuestion)
    {
        if (newQuestion is null) return BadRequest($"Not accepted data.");
        clsQuestion question = new clsQuestion
        (
            new QuestionDto
            (
                null,
                newQuestion.ExamID,
                newQuestion.QuestionText,
                newQuestion.QuestionType,
                newQuestion.Marks
            )
        );

        if (!question.Save(out string? validationMessage))
        {
            return !string.IsNullOrWhiteSpace(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error adding question" });
        }

        return CreatedAtRoute("GetQuestionById", new { id = question.QuestionID },
            question.ToQuestionDto());
    }
    
    [HttpPut(Name = "UpdateQuestion")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<QuestionDto> UpdateQuestion(int id, QuestionDto? updatedQuestion)
    {
        if (id < 1 || updatedQuestion is null) return BadRequest($"Not accepted data.");
        clsQuestion? question = clsQuestion.Find(id);
        if (question is null) return NotFound($"Question with id {id} not found");

        question.ExamID = updatedQuestion.ExamID;
        question.QuestionText = updatedQuestion.QuestionText;
        question.QuestionType = (clsQuestion.enQuestionType)updatedQuestion.QuestionType;
        question.Marks = updatedQuestion.Marks;

        if (!question.Save(out string? validationMessage))
        {
            return !string.IsNullOrEmpty(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error updating question" });
        }

        return Ok(question.ToQuestionDto());
    }
    
    [HttpDelete("{id:int}", Name = "DeleteQuestion")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult DeleteQuestion(int id)
    {
        if (id < 1) return BadRequest($"Not accepted data.");
        if (!clsQuestion.Exists(id)) return NotFound($"Question with id {id} not found");
        return clsQuestion.Delete(id)
            ? Ok($"Question with ID {id} has been deleted.")
            : StatusCode(500, new { message = "Error deleting question" });
    }
}