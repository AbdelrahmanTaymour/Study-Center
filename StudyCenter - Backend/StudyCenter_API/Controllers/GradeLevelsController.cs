using Microsoft.AspNetCore.Mvc;
using StudyCenter_BusinessLayer;
using StudyCenter_DataAccessLayer.DTOs.GradeLevelDTOs;

namespace StudyCenter_API.Controllers;

[Route("api/GradeLevels")]
[ApiController]
public class GradeLevelsController : ControllerBase
{
    [HttpGet(Name = "GetAllGradeLevels")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<GradeLevelDto>> GetAllGradeLevels()
    {
        List<GradeLevelDto> gradeLevels = clsGradeLevel.GetAllGradeLevels();
        if(gradeLevels is null || gradeLevels.Count == 0) return NotFound("No grade levels found");
        return Ok(gradeLevels);
    }

    [HttpGet("all/names", Name = "GetAllGradeLevelsName")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<GradeLevelCreationDto> GetAllGradeLevelsName()
    {
        List<GradeLevelCreationDto> gradeLevels = clsGradeLevel.GetAllGradeLevelsName();
        if(gradeLevels is null || gradeLevels.Count == 0) return NotFound("No grade levels found");
        return Ok(gradeLevels);
    }

    [HttpGet("{gradeId}", Name = "GetGradeLevelById")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<GradeLevelDto> GetGradeLevelById(int? gradeId)
    {
        if(gradeId < 1) return BadRequest($"Not accepted ID {gradeId}");
        clsGradeLevel? gradeLevel = clsGradeLevel.Find(gradeId);
        if(gradeLevel is null) return NotFound($"Grade level {gradeId} not found");
        return Ok(gradeLevel.ToGradeLevelDto());
    }

    [HttpGet("grades/{gradeName}", Name = "GetGradeLevelId")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult GetGradeLevelId(string gradeName)
    {
        if(string.IsNullOrWhiteSpace(gradeName)) return BadRequest($"Not accepted grade name {gradeName}");
        var gradeLevelId = clsGradeLevel.GetGradeLevelId(gradeName);
        if(gradeLevelId is null) return NotFound($"Grade level {gradeName} not found");
        return Ok(gradeLevelId.Value);
    }

    [HttpDelete("{gradeId}", Name = "DeleteGradeLevel")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult DeleteGradeLevel(int? gradeId)
    {
        if (gradeId is null || gradeId < 1) return BadRequest("Not accepted data");
        if(!clsGradeLevel.Exists(gradeId)) return NotFound($"User with ID {gradeId} is not found.");
        if(clsGradeLevel.Delete(gradeId)) return Ok($"Person with ID {gradeId} has been deleted.");
        return StatusCode(500, new { message = "Error deleting person" });
    }

    [HttpPost(Name = "AddGradeLevel")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult AddGradeLevel(GradeLevelCreationDto newGradeLevel)
    {
        if(newGradeLevel is null) return BadRequest($"Not accepted data");
        clsGradeLevel gradeLevel = new clsGradeLevel(new GradeLevelDto(null, newGradeLevel.GradeName));

        if (!gradeLevel.Save(out string? validationMessage))
        {
            return !string.IsNullOrEmpty(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error creating grade level" });
        }

        return CreatedAtRoute("GetGradeLevelById", new { gradeId = gradeLevel.GradeLevelID }, gradeLevel.ToGradeLevelDto());
    }

    [HttpPut("{gradeId}", Name = "UpdateGradeLevel")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<GradeLevelDto> UpdateGradeLevel(int? gradeId, GradeLevelDto? updatedGradeLevel)
    {
        if(updatedGradeLevel is null || gradeId < 1) return BadRequest("Not accepted data");
        
        clsGradeLevel? gradeLevel = clsGradeLevel.Find(gradeId);
        if(gradeLevel is null) return NotFound($"Grade level {gradeId} not found");
        
        gradeLevel.GradeName = updatedGradeLevel.GradeName;
        if (!gradeLevel.Save(out string? validationMessage))
        {
            return !string.IsNullOrEmpty(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error updating grade level" });
        }
        
        return Ok(gradeLevel.ToGradeLevelDto());
    }
    
}