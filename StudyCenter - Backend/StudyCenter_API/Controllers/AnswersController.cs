using Microsoft.AspNetCore.Mvc;
using StudyCenter_BusinessLayer;
using StudyCenter_DataAccessLayer.DTOs.AnswerDTOs;

namespace StudyCenter_API.Controllers;

[Route("api/answers")]
[ApiController]
public class AnswersController : ControllerBase
{
    [HttpGet(Name = "GetAnswers")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<AnswerDto>> GetAnswers()
    {
        var answers = clsAnswer.AllAnswers();
        if (answers.Count == 0) return NotFound("No answers found");
        return Ok(answers);
    }
    
    [HttpGet("all/{questionId:int}", Name = "GetExamAnswers")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<IEnumerable<AnswerDto>> GetExamAnswers(int questionId)
    {
        if (questionId < 1) return BadRequest($"Not accepted ID {questionId}");
        var answers = clsAnswer.AllQuestionAnswers(questionId);
        if (answers.Count == 0) return NotFound("No answers found");
        return Ok(answers);
    }
    
    [HttpGet("{id:int}", Name = "GetAnswerById")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<AnswerDto> GetAnswerById(int id)
    {
        if (id < 1) return BadRequest($"Not accepted ID {id}");
        clsAnswer? answer = clsAnswer.Find(id);
        if (answer is null) return NotFound($"Answer with id {id} not found");
        return Ok(answer.ToAnswerDto());
    }
    
    [HttpGet("exists/{id:int}", Name = "ExistsByAnswerId")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult ExistsByAnswerId(int id)
    {
        if (id < 1) return BadRequest($"Not accepted data.");
        if (clsAnswer.Exists(id)) return Ok(true);
        return NotFound(false);
    }
    
    [HttpPost(Name = "AddAnswer")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<AnswerCreationDto> AddAnswer(AnswerCreationDto? newAnswer)
    {
        if (newAnswer is null) return BadRequest($"Not accepted data.");
        clsAnswer answer = new clsAnswer
        (
            new AnswerDto
            (
                null,
                newAnswer.QuestionID,
                newAnswer.AnswerText,
                newAnswer.IsCorrect
            )
        );

        if (!answer.Save(out string? validationMessage))
        {
            return !string.IsNullOrWhiteSpace(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error adding answer" });
        }

        return CreatedAtRoute("GetAnswerById", new { id = answer.AnswerID },
            answer.ToAnswerDto());
    }
    
    [HttpPut(Name = "UpdateAnswer")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<AnswerDto> UpdateAnswer(int id, AnswerDto? updatedAnswer)
    {
        if (id < 1 || updatedAnswer is null) return BadRequest($"Not accepted data.");
        clsAnswer? answer = clsAnswer.Find(id);
        if (answer is null) return NotFound($"Answer with id {id} not found");

        answer.QuestionID =updatedAnswer.QuestionID;
        answer.AnswerText =updatedAnswer.AnswerText;
        answer.IsCorrect =updatedAnswer.IsCorrect;

        if (!answer.Save(out string? validationMessage))
        {
            return !string.IsNullOrEmpty(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error updating answer" });
        }

        return Ok(answer.ToAnswerDto());
    }
    
    [HttpDelete("{id:int}", Name = "DeleteAnswer")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult DeleteAnswer(int id)
    {
        if (id < 1) return BadRequest($"Not accepted data.");
        if (!clsAnswer.Exists(id)) return NotFound($"Answer with id {id} not found");
        return clsAnswer.Delete(id)
            ? Ok($"Answer with ID {id} has been deleted.")
            : StatusCode(500, new { message = "Error deleting answer" });
    }
}