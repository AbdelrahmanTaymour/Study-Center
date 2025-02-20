using Microsoft.AspNetCore.Mvc;
using StudyCenter_BusinessLayer;
using StudyCenter_DataAccessLayer.DTOs.SubjectGradeLevelDTOs;

namespace StudyCenter_API.Controllers;

[Route("api/subjectsGradeLevels")]
[ApiController]
public class SubjectsGradeLevelsController : ControllerBase
{
    [HttpGet(Name = "GetSubjectGradeLevels")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<SubjectGradeLevelDto>> GetSubjectGradeLevels()
    {
        var subjectGradeLevels = clsSubjectGradeLevel.All();
        if (subjectGradeLevels.Count == 0) return NotFound("No SubjectGradeLevels found.");
        return Ok(subjectGradeLevels);
    }

    [HttpGet("{id:int}", Name = "GetSubjectGradeLevelById")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<SubjectGradeLevelDto> GetSubjectGradeLevelById(int id)
    {
        if (id < 1) return BadRequest($"Not accepted ID {id}");
        clsSubjectGradeLevel? subjectGradeLevel = clsSubjectGradeLevel.Find(id);
        if (subjectGradeLevel is null) return NotFound($"SubjectGradeLevel with id {id} not found");
        return Ok(subjectGradeLevel.ToSubjectGradeLevelDto());
    }

    [HttpGet("exists/{id:int}", Name = "ExistsBySubjectGradeLevelId")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult ExistsBySubjectGradeLevelId(int id)
    {
        if (id < 1) return BadRequest($"Not accepted ID {id}");
        return clsSubjectGradeLevel.Exists(id)
            ? Ok(true)
            : NotFound(false);
    }

    [HttpGet("exists/{subjectId:int}-{gradeLevelId:int}", Name = "ExistBySubjectIdAndGradeLevelId")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult ExistsBySubjectGradeLevelId(int subjectId, int gradeLevelId)
    {
        if (subjectId < 1 || gradeLevelId < 1) return BadRequest($"Not accepted data");
        return clsSubjectGradeLevel.Exists(subjectId, gradeLevelId)
            ? Ok(true)
            : NotFound(false);
    }

    [HttpPost(Name = "AddSubjectGradeLevel")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<SubjectGradeLevelDto> AddSubjectGradeLevel(SubjectGradeLevelCreationDto? newSubjectGradeLevel)
    {
        if (newSubjectGradeLevel is null) return BadRequest($"Not accepted data.");
        clsSubjectGradeLevel? subjectGradeLevel = new clsSubjectGradeLevel
        (
            new SubjectGradeLevelDto
            (
                null,
                newSubjectGradeLevel.SubjectId,
                newSubjectGradeLevel.GradeLevelId,
                newSubjectGradeLevel.Fees,
                newSubjectGradeLevel.IsMandatory,
                newSubjectGradeLevel.Description
            )
        );

        if (!subjectGradeLevel.Save(out string? validationMessage))
        {
            return !string.IsNullOrEmpty(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error adding SubjectGradeLevel" });
        }

        return CreatedAtRoute("GetSubjectGradeLevelById", new { id = subjectGradeLevel.SubjectGradeLevelID },
            subjectGradeLevel.ToSubjectGradeLevelDto());
    }

    [HttpPut("{id:int}", Name = "UpdateSubjectGradeLevel")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<SubjectGradeLevelDto> UpdateSubjectGradeLevel(int id,
        SubjectGradeLevelDto updatedSubjectGradeLevel)
    {
        if (id < 1) return BadRequest($"Not accepted data.");
        clsSubjectGradeLevel? subjectGradeLevel = clsSubjectGradeLevel.Find(id);
        if (subjectGradeLevel is null) return NotFound($"SubjectGradeLevel with id {id} not found");

        subjectGradeLevel.SubjectID = updatedSubjectGradeLevel.SubjectId;
        subjectGradeLevel.GradeLevelID = updatedSubjectGradeLevel.GradeLevelId;
        subjectGradeLevel.Fees = updatedSubjectGradeLevel.Fees;
        subjectGradeLevel.IsMandatory = updatedSubjectGradeLevel.IsMandatory;
        subjectGradeLevel.Description = updatedSubjectGradeLevel.Description;

        if (!subjectGradeLevel.Save(out string? validationMessage))
        {
            return !string.IsNullOrEmpty(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error updating SubjectGradeLevel" });
        }

        return Ok(subjectGradeLevel.ToSubjectGradeLevelDto());
    }

    [HttpDelete("{id:int}", Name = "DeleteSubjectGradeLevel")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult DeleteSubjectGradeLevel(int id)
    {
        if (id < 1) return BadRequest($"Not accepted data.");
        if (!clsSubjectGradeLevel.Exists(id)) return NotFound($"SubjectGradeLevel with id {id} not found");
        return clsSubjectGradeLevel.Delete(id)
            ? Ok($"SubjectGradeLevel with ID {id} has been deleted.")
            : StatusCode(500, new { message = "Error deleting SubjectGradeLevel" });
    }
}