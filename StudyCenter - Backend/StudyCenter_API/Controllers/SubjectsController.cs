using Microsoft.AspNetCore.Mvc;
using StudyCenter_BusinessLayer;
using StudyCenter_DataAccessLayer.DTOs.SubjectDTOs;

namespace StudyCenter_API.Controllers;

[Route("api/subject")]
[ApiController]
public class SubjectsController : ControllerBase
{
    [HttpGet(Name = "GetSubjects")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<SubjectDto>> GetSubjects()
    {
        var subjects = clsSubject.All();
        if (subjects.Count == 0) return NotFound("No Subjects found");
        return Ok(subjects);
    }

    [HttpGet("names", Name = "GetSubjectNames")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<SubjectCreationDto>> GetSubjectNames()
    {
        var subjects = clsSubject.AllNames();
        if (subjects.Count() == 0) return NotFound("No Level name found");
        return Ok(subjects);
    }

    [HttpGet("{id:int}", Name = "GetSubjectById")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<SubjectDto> GetSubjectById(int? id)
    {
        if (id is null || id < 1) return BadRequest($"Not accepted ID {id}");
        clsSubject? subject = clsSubject.Find(id);
        if (subject is null) return NotFound($"Subject with id {id} not found");
        return Ok(subject.ToSubjectDto());
    }

    [HttpGet("by-id/{id:int}", Name = "GetSubjectName")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult GetSubjectName(int id)
    {
        if (id < 1) return BadRequest("Not accepted data.");
        var subject = clsSubject.GetSubjectNameBySubjectId(id);
        if (!string.IsNullOrWhiteSpace(subject)) return Ok(subject);
        return NotFound($"Subject with id {id} Not Found");
    }

    [HttpGet("by-name/{subjectName}", Name = "GetSubjectIdByName")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult GetSubjectIdByName(string subjectName)
    {
        if (string.IsNullOrWhiteSpace(subjectName)) return BadRequest("Not accepted data.");
        var subject = clsSubject.GetSubjectId(subjectName);
        if (subject is not null) return Ok(subject);
        return NotFound($"Subject with name ( {subjectName} ) Not Found");
    }

    [HttpGet("exists/id/{id:int}", Name = "ExistsBySubjectId")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult ExistsBySubjectId(int id)
    {
        if (id < 1) return BadRequest($"Not accepted data.");
        if (clsSubject.Exists(id)) return Ok(true);
        return NotFound(false);
    }

    [HttpGet("exists/name/{name}", Name = "ExistsBySubjectName")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult ExistsBySubjectId(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return BadRequest($"Not accepted data.");
        if (clsSubject.Exists(name)) return Ok(true);
        return NotFound(false);
    }

    [HttpPost(Name = "AddSubject")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<SubjectDto> AddSubject(SubjectCreationDto? newSubject)
    {
        if (newSubject is null) return BadRequest($"Not accepted data.");
        clsSubject subject = new clsSubject
        (
            new SubjectDto
            (
                null,
                newSubject.SubjectName
            )
        );

        if (!subject.Save(out string? validationMessage))
        {
            return !string.IsNullOrWhiteSpace(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error adding subject" });
        }

        return CreatedAtRoute("GetSubjectById", new { id = subject.SubjectID },
            subject.ToSubjectDto());
    }

    [HttpPut(Name = "UpdateSubject")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<SubjectDto> UpdateSubject(int id, SubjectDto? updatedSubject)
    {
        if (id < 1 || updatedSubject is null) return BadRequest($"Not accepted data.");
        clsSubject? subject = clsSubject.Find(id);
        if (subject is null) return NotFound($"Subject with id {id} not found");

        subject.SubjectName = updatedSubject.SubjectName;

        if (!subject.Save(out string? validationMessage))
        {
            return !string.IsNullOrEmpty(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error updating subject" });
        }

        return Ok(subject.ToSubjectDto());
    }
    
    [HttpDelete("{id:int}", Name = "DeleteSubject")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult DeleteSubject(int id)
    {
        if (id < 1) return BadRequest($"Not accepted data.");
        if (!clsSubject.Exists(id)) return NotFound($"Subject with id {id} not found");
        return clsSubject.Delete(id)
            ? Ok($"Subject with ID {id} has been deleted.")
            : StatusCode(500, new { message = "Error deleting Subject" });
    }
    
}