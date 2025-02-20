using Microsoft.AspNetCore.Mvc;
using StudyCenter_BusinessLayer;
using StudyCenter_DataAccessLayer.DTOs.ClassDTOs;

namespace StudyCenter_API.Controllers;

[Route("api/classes")]
[ApiController]
public class ClassesController : ControllerBase
{
    [HttpGet(Name = "GetClasses")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<ClassDto>> GetClasses()
    {
        var classes = clsClass.All();
        if (classes.Count() == 0) return NotFound("No classes found");
        return Ok(classes);
    }

    [HttpGet("{id:int}", Name = "GetClassById")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<ClassDto> GetClass(int id)
    {
        if (id < 1) return BadRequest($"Not accepted ID {id}");
        clsClass? cls = clsClass.Find(id);
        if (cls == null) return NotFound($"Class with id {id} not found");
        return Ok(cls.ToClassDto());
    }

    [HttpGet("exists/id/{id:int}", Name = "ExistsByClassId")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult ExistsByClassId(int id)
    {
        if (id < 1) return BadRequest($"Not accepted data.");
        if (clsClass.Exists(id)) return Ok(true);
        return NotFound(false);
    }

    [HttpGet("exists/name/{name}", Name = "ExistsByClassName")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult ExistsByClassId(string name)
    {
        if (string.IsNullOrEmpty(name)) return BadRequest($"Not accepted data.");
        if (clsClass.Exists(name)) return Ok(true);
        return NotFound(false);
    }

    [HttpPost(Name = "AddClass")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ClassDto> AddClass(ClassCreationDto? newClass)
    {
        if (newClass is null) return BadRequest($"Not accepted data.");
        clsClass? cls = new clsClass
        (
            new ClassDto
            (
                null,
                newClass.ClassName,
                newClass.Capacity,
                newClass.Description
            )
        );

        if (!cls.Save(out string? validationMessage))
        {
            return !string.IsNullOrEmpty(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error adding Education Level" });
        }

        return CreatedAtRoute("GetClassById", new { id = cls.ClassId },
            cls.ToClassDto());
    }

    [HttpPut(Name = "UpdateClass")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ClassDto> UpdateClass(int? id, ClassDto? updatedClass)
    {
        if (id is null || id < 1 || updatedClass is null) return BadRequest($"Not accepted data.");
        clsClass? cls = clsClass.Find(id);
        if (cls is null) return NotFound($"Class with id {id} not found");

        cls.ClassName = updatedClass.ClassName;
        cls.Capacity = updatedClass.Capacity;
        cls.Description = updatedClass.Description;

        if (!cls.Save(out string? validationMessage))
        {
            return !string.IsNullOrEmpty(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error updating class" });
        }

        return Ok(cls.ToClassDto());
    }
    
    [HttpDelete("{id:int}", Name = "DeleteClass")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult DeleteClass(int? id)
    {
        if (id is null || id < 1) return BadRequest($"Not accepted data.");
        if (!clsClass.Exists(id)) return NotFound($"Class with id {id} not found");
        return clsClass.Delete(id)
            ? Ok($"Class with ID {id} has been deleted.")
            : StatusCode(500, new { message = "Error deleting class" });
    }
    
    
}