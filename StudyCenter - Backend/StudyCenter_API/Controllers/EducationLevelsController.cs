using Microsoft.AspNetCore.Mvc;
using StudyCenter_BusinessLayer;
using StudyCenter_DataAccessLayer;
using StudyCenter_DataAccessLayer.DTOs.EducationLevelDTOs;

namespace StudyCenter_API.Controllers;

[Route("api/education-levels")]
[ApiController]
public class EducationLevelsController : ControllerBase
{
    [HttpGet(Name = "GetEducationLevels")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<EducationLevelDto>> GetEducationLevels()
    {
        var educationLevels = clsEducationLevel.All();
        if (educationLevels is null || educationLevels.Count == 0) return NotFound("No Education Levels found");
        return Ok(educationLevels);
    }

    [HttpGet("names", Name = "GetEducationLevelNames")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<EducationLevelCreationDto>> GetEducationLevelNames()
    {
        var educationLevels = clsEducationLevel.AllLevelNames();
        if (educationLevels is null) return NotFound("No Level name found");
        return Ok(educationLevels);
    }

    [HttpGet("{id:int}", Name = "GetEducationLevelById")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<EducationLevelDto> GetEducationLevelById(int id)
    {
        if (id < 1) return BadRequest($"Not accepted ID {id}");
        clsEducationLevel? educationLevel = clsEducationLevel.Find(id);
        if (educationLevel is null) return NotFound($"Education Level with id {id} not found");
        return Ok(educationLevel.ToEducationLevelDto());
    }

    [HttpGet("by-id/{id:int}", Name = "GetEducationLevelName")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult GetEducationLevelName(int id)
    {
        if (id < 1) return BadRequest("Not accepted data.");
        var educationLevel = clsEducationLevel.GetEducationLeveName(id);
        if (!string.IsNullOrWhiteSpace(educationLevel)) return Ok(educationLevel);
        return NotFound($"Education Level with id {id} Not Found");
    }

    [HttpGet("by-name/{levelName}", Name = "GetEducationLevelIdByName")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult GetEducationLevelIdByName(string levelName)
    {
        if (string.IsNullOrWhiteSpace(levelName)) return BadRequest("Not accepted data.");
        var educationLevel = clsEducationLevel.GetEducationLeveId(levelName);
        if (educationLevel is not null) return Ok(educationLevel);
        return NotFound($"Education Level with name ( {levelName} ) Not Found");
    }

    [HttpGet("exists/id/{id:int}", Name = "ExistsByEducationLevelId")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult ExistsByEducationLevelId(int id)
    {
        if (id < 1) return BadRequest($"Not accepted data.");
        if (clsEducationLevel.Exists(id)) return Ok(true);
        return NotFound(false);
    }

    [HttpGet("exists/name/{name}", Name = "ExistsByEducationLevelName")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult ExistsByEducationLevelId(string name)
    {
        if (string.IsNullOrEmpty(name)) return BadRequest($"Not accepted data.");
        if (clsEducationLevel.Exists(name)) return Ok(true);
        return NotFound(false);
    }

    [HttpPost(Name = "AddEducationLevel")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<EducationLevelDto> AddEducationLevel(EducationLevelCreationDto? newEducationLevel)
    {
        if (newEducationLevel is null) return BadRequest($"Not accepted data.");
        clsEducationLevel? educationLevel = new clsEducationLevel
        (
            new EducationLevelDto
            (
                null,
                newEducationLevel.LevelName
            )
        );

        if (!educationLevel.Save(out string? validationMessage))
        {
            return !string.IsNullOrEmpty(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error adding Education Level" });
        }

        return CreatedAtRoute("GetEducationLevelById", new { id = educationLevel.EducationLevelID },
            educationLevel.ToEducationLevelDto());
    }

    [HttpPut(Name = "UpdateEducationLevel")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<EducationLevelDto> UpdateEducationLevel(int id, EducationLevelDto? updatedEducationLevel)
    {
        if (id < 1 || updatedEducationLevel is null) return BadRequest($"Not accepted data.");
        clsEducationLevel? educationLevel = clsEducationLevel.Find(id);
        if (educationLevel is null) return NotFound($"Education Level with id {id} not found");

        educationLevel.LevelName = updatedEducationLevel.LevelName;

        if (!educationLevel.Save(out string? validationMessage))
        {
            return !string.IsNullOrEmpty(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error updating education level" });
        }

        return Ok(educationLevel.ToEducationLevelDto());
    }

    [HttpDelete("{id:int}", Name = "DeleteEducationLevel")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult DeleteEducationLevel(int id)
    {
        if ( id < 1) return BadRequest($"Not accepted data.");
        if (!clsEducationLevel.Exists(id)) return NotFound($"Education Level with id {id} not found");
        return clsEducationLevel.Delete(id)
            ? Ok($"Education level with ID {id} has been deleted.")
            : StatusCode(500, new { message = "Error deleting education level" });
    }
    
}