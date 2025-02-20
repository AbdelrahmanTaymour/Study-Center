using Microsoft.AspNetCore.Mvc;
using StudyCenter_BusinessLayer;
using StudyCenter_DataAccessLayer.DTOs.GroupDTOs;

namespace StudyCenter_API.Controllers;

[Route("api/groups")]
[ApiController]
public class GroupsController : ControllerBase
{
    [HttpGet(Name = "GetGroups")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<GroupDto>> GetGroups()
    {
        var groups = clsGroup.All();
        if (groups.Count == 0) return NotFound("No Groups found");
        return Ok(groups);
    }
    
    [HttpGet("{id:int}", Name = "GetGroupById")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<GroupDto> GetGroupById(int id)
    {
        if (id < 1) return BadRequest($"Not accepted ID {id}");
        clsGroup? group = clsGroup.Find(id);
        if (group is null) return NotFound($"Group with id {id} not found");
        return Ok(group.ToGroupDto());
    }
    
    [HttpGet("exists/{id:int}", Name = "ExistsByGroupId")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult ExistsByGroupId(int id)
    {
        if (id < 1) return BadRequest($"Not accepted data.");
        if (clsGroup.Exists(id)) return Ok(true);
        return NotFound(false);
    }
    
    [HttpPost(Name = "AddGroup")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<GroupCreationDto> AddGroup(GroupCreationDto? newGroup)
    {
        if (newGroup is null) return BadRequest($"Not accepted data.");
        clsGroup group = new clsGroup
        (
            new GroupDto
            (
                null,
                newGroup.GroupName,
                newGroup.ClassId,
                newGroup.TeacherId,
                newGroup.SubjectTeacherId,
                newGroup.MeetingTimeId,
                newGroup.Description,
                newGroup.CreatedByUserId,
                DateTime.MinValue,
                DateTime.MinValue, 
                newGroup.IsActive,
                null
            )
        );

        if (!group.Save(out string? validationMessage))
        {
            return !string.IsNullOrWhiteSpace(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error adding Group" });
        }

        return CreatedAtRoute("GetGroupById", new { id = group.GroupID },
            group.ToGroupDto());
    }
    
    [HttpPut(Name = "UpdateGroup")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<GroupDto> UpdateGroup(int id, GroupUpdateDto? updatedGroup)
    {
        if (id < 1 || updatedGroup is null) return BadRequest($"Not accepted data.");
        clsGroup? group = clsGroup.Find(id);
        if (group is null) return NotFound($"Group with id {id} not found");

        group.GroupName = updatedGroup.GroupName;
        group.ClassID = updatedGroup.ClassId;
        group.TeacherID = updatedGroup.TeacherId;
        group.SubjectTeacherID = updatedGroup.SubjectTeacherId;
        group.MeetingTimeID = updatedGroup.MeetingTimeId;
        group.Description = updatedGroup.Description;
        group.CreatedByUserID = updatedGroup.CreatedByUserId;
        group.IsActive = updatedGroup.IsActive;

        if (!group.Save(out string? validationMessage))
        {
            return !string.IsNullOrEmpty(validationMessage)
                ? BadRequest(new { message = validationMessage })
                : StatusCode(500, new { message = "Error updating Group" });
        }

        return Ok(group.ToGroupDto());
    }
    
    [HttpDelete("{id:int}", Name = "DeleteGroup")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult DeleteGroup(int id)
    {
        if (id < 1) return BadRequest($"Not accepted data.");
        if (!clsGroup.Exists(id)) return NotFound($"Group with id {id} not found");
        return clsGroup.Delete(id)
            ? Ok($"Group with ID {id} has been deleted.")
            : StatusCode(500, new { message = "Error deleting Group" });
    }
    
}